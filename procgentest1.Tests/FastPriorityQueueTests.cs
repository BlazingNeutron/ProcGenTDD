namespace procgentest1.Tests;

public class FastPriorityQueueTests()
{

    private FastPriorityQueue fastPriorityQueue = new(30);

    [Fact]
    public void CanInstatiate()
    {
        Assert.NotNull(fastPriorityQueue);
    }

    [Fact]
    public void EnqueueItem()
    {
        Assert.True(fastPriorityQueue.Enqueue(new Node(1, 1), 1));
    }

    [Fact]
    public void Count()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        Assert.Equal(1, fastPriorityQueue.Count);
    }

    [Fact]
    public void Dequeue()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        Assert.Equal(new Node(1, 1), fastPriorityQueue.Dequeue());
    }

    [Fact]
    public void Contains()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        Assert.True(fastPriorityQueue.Contains(new Node(1, 1)));
    }

    [Fact]
    public void DoesNotContains()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        Assert.False(fastPriorityQueue.Contains(new Node(2, 2)));
    }

    [Fact]
    public void PriorityMatters()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        fastPriorityQueue.Enqueue(new Node(2, 2), 2);
        Assert.Equal(new Node(1,1), fastPriorityQueue.Dequeue());
    }

    [Fact]
    public void SecondaryDequeues()
    {
        fastPriorityQueue.Enqueue(new Node(1, 1), 1);
        fastPriorityQueue.Enqueue(new Node(2, 2), 2);
        fastPriorityQueue.Dequeue();
        Assert.Equal(new Node(2,2), fastPriorityQueue.Dequeue());
    }
}