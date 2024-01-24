using System.Numerics;

namespace procgentest1
{

    public class Node
    {
        public float Cost;
        public Vector2 Position;

        public Node(Vector2 pos)
        {
            this.Position = pos;
            this.Cost = 1;
        }

        public override bool Equals(object? obj)
        {
            // if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            // {
            //     return false;
            // }
            // else
            // {
                Node other = (Node)obj;
                return other.Position.Equals(this.Position);
            // }
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
                    if (!ClosedList.Contains(n) && level.Get((int)n.Position.X, (int)n.Position.Y) != " ")
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
            List<Node> neighbours = new();
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
            if (current.Position.X + 2 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 2, current.Position.Y)));
            }
            if (current.Position.Y - 1 >= 0 && current.Position.X + 1 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 1, current.Position.Y - 1)));
            }
            if (current.Position.Y - 1 >= 0 && current.Position.X - 1 >= 0)
            {
                neighbours.Add(new(new Vector2(current.Position.X - 1, current.Position.Y - 1)));
            }
            if (current.Position.Y + 1 < height && current.Position.X + 2 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 2, current.Position.Y + 1)));
            }
            if (current.Position.Y + 2 < height && current.Position.X + 2 < width)
            {
                neighbours.Add(new(new Vector2(current.Position.X + 2, current.Position.Y + 2)));
            }
            return neighbours;
        }
    }
}