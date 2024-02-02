using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace procgentest1
{

    public class Node(Vector2 pos)
    {
        public readonly float Cost = 1;
        public readonly Vector2 Position = pos;

        public override bool Equals(object? obj)
        {
            Node? other = obj as Node;
            return this.Position.Equals(other.Position);
        }

        [ExcludeFromCodeCoverageAttribute]
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (int)Cost;
            return hash * 23 + Position.GetHashCode();
        }
    }

    public class AStar
    {
        private int height;
        private int width;

        public bool FindPath(Level2D level)
        {
            Node start = new(level.StartPosition());
            Node end = new(level.EndPosition());
            return FindPath(level, start, end);
        }

        public bool FindPath(Level2D level, Node start, Node end)
        {
            height = level.GetHeight();
            width = level.GetWidth();
            PriorityQueue<Node, float> OpenList = new();
            List<Node> ClosedList = [];
            List<Node> neighbours;
            Node current;

            OpenList.Enqueue(start, start.Cost);
            while (OpenList.Count != 0 && !ClosedList.Contains(end))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                neighbours = GetNeighbouringNodes(current, level);

                foreach (Node n in neighbours)
                {
                    if (!ClosedList.Contains(n) && !level.IsEmpty(n.Position))
                    {
                        foreach (var (Element, Priority) in OpenList.UnorderedItems)
                        {
                            if (Element.Equals(n))
                            {
                                continue;
                            }
                        }
                        OpenList.Enqueue(n, n.Cost + 1);
                    }
                }
            }

            if (!ClosedList.Contains(end))
            {
                return false;
            }

            return true;
        }

        private List<Node> GetNeighbouringNodes(Node current, Level2D level)
        {
            List<Node> neighbours = [];
            WalkableNeighbours(current, neighbours);
            HorizontalJumps(current, neighbours);
            VerticalJumps(current, neighbours);
            Falling(current, neighbours, level);
            return neighbours;
        }

        private void WalkableNeighbours(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 1 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 1, current.Position.Y)));
            }
            if (current.Position.X - 1 >= 0)
            {
                neighbours.Add(new(new Vector2(current.Position.X - 1, current.Position.Y)));
            }
        }

        private void Falling(Node current, List<Node> neighbours, Level2D level)
        {
            if (current.Position.X + 1 < width)
            {
                KeepFalling(new(new(current.Position.X + 1, current.Position.Y)), neighbours, level);
            }
            KeepFalling(new(new(current.Position.X, current.Position.Y)), neighbours, level);
            if (current.Position.X - 1 >= 0)
            {
                KeepFalling(new(new(current.Position.X - 1, current.Position.Y)), neighbours, level);
            }
        }

        private void KeepFalling(Node current, List<Node> neighbours, Level2D level)
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
                    neighbours.Add(new(new Vector2(current.Position.X + 1, current.Position.Y + 1)));
                    KeepFalling(new(new(current.Position.X + 1, current.Position.Y + 1)), neighbours, level);
                }
                if (current.Position.X - 1 >= 0)
                {
                    neighbours.Add(new(new Vector2(current.Position.X - 1, current.Position.Y + 1)));
                    KeepFalling(new(new(current.Position.X - 1, current.Position.Y + 1)), neighbours, level);
                }
                neighbours.Add(new(new Vector2(current.Position.X, current.Position.Y + 1)));
                KeepFalling(new(new(current.Position.X, current.Position.Y + 1)), neighbours, level);
            }
        }

        private void VerticalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.Y - 1 >= 0)
            {
                if (current.Position.X + 1 < width)
                {
                    neighbours.Add(new(new Vector2(current.Position.X + 1, current.Position.Y - 1)));
                }
                if (current.Position.X - 1 >= 0)
                {
                    neighbours.Add(new(new Vector2(current.Position.X - 1, current.Position.Y - 1)));
                }
                neighbours.Add(new(new Vector2(current.Position.X, current.Position.Y - 1)));
            }
        }

        private void HorizontalJumps(Node current, List<Node> neighbours)
        {
            if (current.Position.X + 2 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 2, current.Position.Y)));
            }
            if (current.Position.X - 2 >= 0)
            {
                neighbours.Add(new(new Vector2(current.Position.X - 2, current.Position.Y)));
            }
        }

        public bool FindPathFromAll(Level2D level)
        {
            Node end = new(level.EndPosition());
            for (int x = 0; x < level.GetWidth(); x++)
            {
                for (int y = 0; y < level.GetHeight(); y++)
                {
                    Node currentStart = new(new(x, y));
                    if (level.IsFloor(x, y) && !FindPath(level, currentStart, end))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}