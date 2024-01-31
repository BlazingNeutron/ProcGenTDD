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
            height = level.GetLength(1);
            width = level.GetLength(0);
            Node start = new(level.StartPosition());
            Node end = new(level.EndPosition());
            PriorityQueue<Node, float> OpenList = new();
            List<Node> ClosedList = [];
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

        private List<Node> GetNeighbouringNodes(Node current)
        {
            List<Node> neighbours = [];
            WalkableNeighbours(current, neighbours);
            HorizontalJumps(current, neighbours);
            VerticalJumps(current, neighbours);
            Falling(current, neighbours);
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
            if (current.Position.Y + 1 < height)
            {
                neighbours.Add(new(new Vector2(current.Position.X, current.Position.Y + 1)));
            }
            if (current.Position.Y - 1 >= 0)
            {
                neighbours.Add(new(new Vector2(current.Position.X, current.Position.Y - 1)));
            }
        }

        private void Falling(Node current, List<Node> neighbours)
        {
            int StartOfFallY = (int)(current.Position.Y + 1);
            int maxY = (height - 1) - StartOfFallY;
            for (int y = StartOfFallY; y < maxY; y++)
            {
                float diff = Math.Abs(current.Position.Y - y);
                int maxX = Math.Min(width - 1, (int)((diff/2f) + 2f));
                int minX = Math.Max(0, (int)((diff/-2f) - 2f));
                for (int x = minX; x < maxX; x++)
                {
                    if (current.Position.X + x < width && current.Position.Y + y < height)
                    {
                        neighbours.Add(new(new Vector2(current.Position.X + x, current.Position.Y + y)));
                    }
                }
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
    }
}