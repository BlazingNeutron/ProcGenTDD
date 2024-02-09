namespace procgentest1
{
    public class FastPriorityQueue
    {

        private int numNodes;
        private Node[] topPriority;
        private Node[] secondaryPriorities;
        private double[] queue;
        private int topHead;
        private int topTail;
        private int otherHead;
        private int otherTail;
        private int queueHead;
        private int queueTail;

        public FastPriorityQueue(int maxSize)
        {
            numNodes = 0;
            topPriority = new Node[maxSize];
            secondaryPriorities = new Node[maxSize];
            queue = new double[maxSize];
            topHead = 0;
            topTail = 0;
            otherHead = 0;
            otherTail = 0;
            queueHead = 0;
            queueTail = 0;
        }

        public int Count
        {
            get
            {
                return numNodes;
            }
        }

        public bool Enqueue(Node node, double priority)
        {
            if (priority == 1)
            {
                topPriority[topHead] = node;
                topHead++;
            }
            else
            {
                secondaryPriorities[otherHead] = node;
                otherHead++;
            }
            queue[queueHead] = priority;
            queueHead++;
            numNodes++;
            return true;
        }

        public Node Dequeue()
        {
            Node returnNode;
            if (queue.Contains(1))
            {
                queue[Array.IndexOf(queue, 1)] = 0;
                returnNode = topPriority[topTail];
                topPriority[topTail] = null;
                topTail++;
            }
            else
            {
                int index = Array.FindIndex(queue, x => x != 0);
                if (index < 0 || index >= queue.Length)
                {
                    return null;
                }
                queue[index] = 0;
                returnNode = secondaryPriorities[otherTail];
                secondaryPriorities[otherTail] = null;
                otherTail++;
            }
            numNodes--;
            return returnNode;
        }

        public bool Contains(Node node)
        {
            return topPriority.Contains(node) || secondaryPriorities.Contains(node);
        }
    }
}