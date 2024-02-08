using System.Diagnostics.CodeAnalysis;

namespace procgentest1
{

    public sealed class Node : IEquatable<Node>
    {
        private readonly int cost = 1;
        public readonly Vector2 Position;

        public int Cost { get; set; }
        public Node? Parent { get; set; }
        private readonly int x;
        private readonly int y;

        public Node(int x, int y)
        {
            Position = new Vector2(x, y);
            this.x = x;
            this.y = y;
        }

        [ExcludeFromCodeCoverageAttribute]
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals((Node)obj);
        }

        public bool Equals(Node? other)
        {
            return other != null && this.x == other.x && this.y == other.y;
        }

        [ExcludeFromCodeCoverageAttribute]
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + cost;
            hash = hash * 23 + Position.Y;
            return hash * 23 + Position.X;
        }
    }

    public class AStar
    {
        private Level2D level = new("");
        private int height;
        private int width;

        private string[] neighbourMap;

        public bool HasPath(Level2D level)
        {
            Node start = new(level.StartPosition().X, level.StartPosition().Y);
            Node end = new(level.EndPosition().X, level.EndPosition().Y);
            return HasPath(level, start, end);
        }

        public bool HasPath(Level2D level, Node start, Node end)
        {
            Stack<Node> path = FindPath(level, start, end);
            if (path == null)
            {
                return false;
            }
            return true;
        }

        public Stack<Node>? FindPath(Level2D level, Node start, Node end)
        {
            if (level.Cache[start.Position.X + start.Position.Y * width].available[end.Position.X + end.Position.Y * width])
            {
                return level.Cache[start.Position.X + start.Position.Y * width].Paths[end.Position.X + end.Position.Y * width];
            }
            this.level = level;
            height = level.GetHeight();
            width = level.GetWidth();
            PriorityQueue<Node, float> OpenList = new(width * height);
            List<Node> ClosedList = new(width * height);
            List<Node> neighbours;
            neighbourMap = new string[width * height];
            Node current;

            OpenList.Enqueue(start, start.Cost);
            while (OpenList.Count != 0 && !ClosedList.Contains(end))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                neighbourMap = new string[width * height];
                neighbours = GetNeighbouringNodes(current);

                foreach (Node n in neighbours)
                {
                    CheckNeighbours(ClosedList, OpenList, n, current);
                }
            }

            if (!ClosedList.Contains(end))
            {
                return null;
            }
            Stack<Node> Path = new();
            Node? temp = ClosedList.Find(x => x.Equals(end));
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != null);
            Stack<Node> returnPath = new(Path);
            while (Path.Count != 0)
            {
                current = Path.Pop();
                Stack<Node> tempPath = new();
                Node currentStart = current;
                while (currentStart != null)
                {
                    tempPath.Push(currentStart);
                    level.Cache[currentStart.Position.X + currentStart.Position.Y * width].available[current.Position.X + current.Position.Y * width] = true;
                    level.Cache[currentStart.Position.X + currentStart.Position.Y * width].Paths[current.Position.X + current.Position.Y * width] = tempPath;
                    currentStart = currentStart.Parent;
                }
                
            }
            return returnPath;
        }

        private void CheckNeighbours(List<Node> ClosedList, PriorityQueue<Node, float> OpenList, Node n, Node current)
        {
            if (!ClosedList.Contains(n) && !level.IsEmpty(n.Position))
            {
                bool isFound = false;
                foreach (var (Element, Priority) in OpenList.UnorderedItems)
                {
                    if (Element.Equals(n))
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    n.Cost = current.Cost + 1;
                    OpenList.Enqueue(n, n.Cost);
                    n.Parent = current;
                }
            }
        }

        private List<Node> GetNeighbouringNodes(Node current)
        {
            if (level.Cache[current.Position.X + current.Position.Y * width].neighbours != null)
            {
                return level.Cache[current.Position.X + current.Position.Y * width].neighbours;
            }
            List<Node> neighbours = new(width * height);
            WalkableNeighbours(current, neighbours);
            HorizontalJumps(current, neighbours);
            VerticalJumps(current, neighbours);
            Falling(current, neighbours);
            level.Cache[current.Position.X + current.Position.Y * width].neighbours = neighbours;
            return neighbours;
        }

        private void NeighboursAdd(Node neighbour, List<Node> neighbours)
        {
            if (neighbourMap[neighbour.Position.X + neighbour.Position.Y * width] != "X")
            {
                neighbourMap[neighbour.Position.X + neighbour.Position.Y * width] = "X";
                neighbours.Add(neighbour);
            }
        }

        private void WalkableNeighbours(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 1 < width)
            {
                NeighboursAdd(new(current.Position.X + 1, current.Position.Y), neighbours);
            }
            if (current.Position.X - 1 >= 0)
            {
                NeighboursAdd(new(current.Position.X - 1, current.Position.Y), neighbours);
            }
        }

        private void Falling(Node current, List<Node> neighbours)
        {
            string[] fallChecksMap = new string[width * height];
            if (current.Position.X + 1 < width)
            {
                Node FallFromRightJump = new(current.Position.X + 1, current.Position.Y);
                if (!level.IsFloor(FallFromRightJump.Position))
                {
                    KeepFalling(FallFromRightJump, neighbours, fallChecksMap);
                }
            }
            Node FallFromCurrentPosition = new(current.Position.X, current.Position.Y);
            if (!level.IsFloor(FallFromCurrentPosition.Position))
            {
                KeepFalling(FallFromCurrentPosition, neighbours, fallChecksMap);
            }
            if (current.Position.X - 1 >= 0)
            {
                Node FallFromLeftJump = new(current.Position.X - 1, current.Position.Y);
                if (!level.IsFloor(FallFromLeftJump.Position))
                {
                    KeepFalling(FallFromLeftJump, neighbours, fallChecksMap);
                }
            }
        }

        private void KeepFalling(Node current, List<Node> neighbours, string[] fallChecksMap)
        {
            if (!level.IsWithinBounds(current.Position) || !level.IsEmpty(current.Position) || fallChecksMap[current.Position.X + current.Position.Y * width] == "X")
            {
                return;
            }

            if (current.Position.Y + 1 < height)
            {
                if (current.Position.X + 1 < width)
                {
                    Node DownAndRight = new(current.Position.X + 1, current.Position.Y + 1);
                    if (level.IsFloor(DownAndRight.Position))
                    {
                        NeighboursAdd(DownAndRight, neighbours);
                    }
                    else
                    {
                        KeepFalling(DownAndRight, neighbours, fallChecksMap);
                        fallChecksMap[DownAndRight.Position.X + DownAndRight.Position.Y * width] = "X";
                    }
                }
                if (current.Position.X - 1 >= 0)
                {
                    Node DownAndLeft = new(current.Position.X - 1, current.Position.Y + 1);
                    if (level.IsFloor(DownAndLeft.Position))
                    {
                        NeighboursAdd(DownAndLeft, neighbours);
                    }
                    else
                    {
                        KeepFalling(DownAndLeft, neighbours, fallChecksMap);
                        fallChecksMap[DownAndLeft.Position.X + DownAndLeft.Position.Y * width] = "X";
                    }
                }
                Node StraightDown = new(current.Position.X, current.Position.Y + 1);
                if (level.IsFloor(StraightDown.Position))
                {
                    NeighboursAdd(StraightDown, neighbours);
                }
                else
                {
                    KeepFalling(StraightDown, neighbours, fallChecksMap);
                    fallChecksMap[StraightDown.Position.X + StraightDown.Position.Y * width] = "X";
                }
            }
        }

        private void VerticalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.Y - 1 >= 0)
            {
                if (current.Position.X + 1 < width)
                {
                    NeighboursAdd(new(current.Position.X + 1, current.Position.Y - 1), neighbours);
                }
                if (current.Position.X - 1 >= 0)
                {
                    NeighboursAdd(new(current.Position.X - 1, current.Position.Y - 1), neighbours);
                }
                NeighboursAdd(new(current.Position.X, current.Position.Y - 1), neighbours);
            }
        }

        private void HorizontalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 2 < width)
            {
                NeighboursAdd(new(current.Position.X + 2, current.Position.Y), neighbours);
            }
            if (current.Position.X - 2 >= 0)
            {
                NeighboursAdd(new(current.Position.X - 2, current.Position.Y), neighbours);
            }
        }

        public bool FindPathFromAll(Level2D level)
        {
            Node end = new(level.EndPosition().X, level.EndPosition().Y);
            for (int x = 0; x < level.GetWidth(); x++)
            {
                for (int y = 0; y < level.GetHeight(); y++)
                {
                    Node currentStart = new(x, y);
                    if (level.IsFloor(x, y) && !HasPath(level, currentStart, end))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool FindPathToAll(Level2D level)
        {
            Node start = new(level.StartPosition().X, level.StartPosition().Y);
            for (int x = 0; x < level.GetWidth(); x++)
            {
                for (int y = 0; y < level.GetHeight(); y++)
                {
                    Node currentEnd = new(x, y);
                    if (level.IsFloor(x, y) && !HasPath(level, start, currentEnd))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}