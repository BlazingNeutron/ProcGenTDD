using System.Diagnostics.CodeAnalysis;

namespace procgentest1
{

    public sealed class Node : IEquatable<Node>
    {
        private readonly int cost = 1;
        public readonly Vector2 Position;
        
        public int Cost { get; set; }
        public Node? Parent { get; set; }

        public Node(int x, int y)
        {
            Position = new Vector2(x, y);
            Parent = null;
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
            return other != null && this.Position.X == other.Position.X && this.Position.Y == other.Position.Y;
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

        public bool HasPath(Level2D level)
        {
            Node start = new(level.StartPosition().X, level.StartPosition().Y);
            Node end = new(level.EndPosition().X, level.EndPosition().Y);
            return FindPath(level, start, end) != null;
        }

        public bool HasPath(Level2D level, Node start, Node end)
        {
            return FindPath(level, start, end) != null;
        }

        public Stack<Node>? FindPath(Level2D level, Node start, Node end)
        {
            this.level = level;
            height = level.GetHeight();
            width = level.GetWidth();
            PriorityQueue<Node, float> OpenList = new(width * height);
            List<Node> ClosedList = new(width * height);
            List<Node> neighbours;
            Node current;

            OpenList.Enqueue(start, start.Cost);
            while (OpenList.Count != 0 && !ClosedList.Contains(end))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
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
            Node? temp = end;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);
            return Path;
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
            List<Node> neighbours = new(width * height);
            WalkableNeighbours(current, neighbours);
            HorizontalJumps(current, neighbours);
            VerticalJumps(current, neighbours);
            Falling(current, neighbours);
            return neighbours;
        }

        private void neighboursAdd(Node neighbour, List<Node> neighbours)
        {
            if (!neighbours.Contains(neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        private void WalkableNeighbours(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 1 < width)
            {
                neighboursAdd(new(current.Position.X + 1, current.Position.Y), neighbours);
            }
            if (current.Position.X - 1 >= 0)
            {
                neighboursAdd(new(current.Position.X - 1, current.Position.Y), neighbours);
            }
        }

        private void Falling(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 1 < width)
            {
                KeepFalling(new(current.Position.X + 1, current.Position.Y), neighbours);
            }
            KeepFalling(new(current.Position.X, current.Position.Y), neighbours);
            if (current.Position.X - 1 >= 0)
            {
                KeepFalling(new(current.Position.X - 1, current.Position.Y), neighbours);
            }
        }

        private void KeepFalling(Node current, List<Node> neighbours)
        {
            if (!level.IsWithinBounds(current.Position))
            {
                return;
            }

            if (!level.IsEmpty(current.Position))
            {
                return;
            }

            if (current.Position.Y + 1 < height)
            {
                if (current.Position.X + 1 < width)
                {
                    neighboursAdd(new(current.Position.X + 1, current.Position.Y + 1), neighbours);
                    KeepFalling(new(current.Position.X + 1, current.Position.Y + 1), neighbours);
                }
                if (current.Position.X - 1 >= 0)
                {
                    neighboursAdd(new(current.Position.X - 1, current.Position.Y + 1), neighbours);
                    KeepFalling(new(current.Position.X - 1, current.Position.Y + 1), neighbours);
                }
                neighboursAdd(new(current.Position.X, current.Position.Y + 1), neighbours);
                KeepFalling(new(current.Position.X, current.Position.Y + 1), neighbours);
            }
        }

        private void VerticalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.Y - 1 >= 0)
            {
                if (current.Position.X + 1 < width)
                {
                    neighboursAdd(new(current.Position.X + 1, current.Position.Y - 1), neighbours);
                }
                if (current.Position.X - 1 >= 0)
                {
                    neighboursAdd(new(current.Position.X - 1, current.Position.Y - 1), neighbours);
                }
                neighboursAdd(new(current.Position.X, current.Position.Y - 1), neighbours);
            }
        }

        private void HorizontalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 2 < width)
            {
                neighboursAdd(new(current.Position.X + 2, current.Position.Y), neighbours);
            }
            if (current.Position.X - 2 >= 0)
            {
                neighboursAdd(new(current.Position.X - 2, current.Position.Y), neighbours);
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