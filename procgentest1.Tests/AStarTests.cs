namespace procgentest1.Tests;
using procgentest1;

public class AStarTests()
{

    private readonly AStar PathFinder = new();

    [Fact]
    public void PlayerJumpsAndThenFalls()
    {
        Assert.True(PathFinder.HasPath(new(
            "SF   \n" +
            "   FE")));
    }

    [Fact]
    public void PlayerCannotFinish()
    {
        Assert.False(PathFinder.HasPath(new(
            "S    \n" +
            "   FE")));
    }

    [Fact]
    public void PlayerCannotFinishSimpleLevel()
    {
        Assert.False(PathFinder.HasPath(new("S  E")));
    }

    [Fact]
    public void PlayerExtraFall()
    {
        Assert.True(PathFinder.HasPath(new(
            "SFF   \n" +
            "      \n" +
            "  FFFE")));
    }

    [Fact]
    public void IsEscapeable()
    {
        Assert.True(PathFinder.FindPathFromAll(new(
            "SFF   \n" +
            "      \n" +
            "  FFFE")));
    }

    [Fact]
    public void HasInescapeableArea()
    {
        Assert.False(PathFinder.FindPathFromAll(new(
            "SF    \n" +
            "   FFE\n" +
            "FF    ")));
    }

    [Fact]
    public void HasUnreachableArea()
    {
        Assert.False(PathFinder.FindPathToAll(new(
            "SF  FF\n" +
            "      \n" +
            "FFFFFE")));
    }

    [Fact]
    public void SimplePath()
    {
        Level2D level = new("SFFFFE");
        Node start = new(0, 0);
        Node end = new(5, 0);
        Stack<Node> Path = PathFinder.FindPath(level, start, end);
        Assert.Equal(4, Path.Count);
    }

    [Fact]
    public void PathContainsExpectedSteps()
    {
        Level2D level = new("SFF   \n  FFFF\nFFFFFE");
        //XFX   
        //  XFFF
        //FFFXFX
        Node start = new(0,0);
        Node end = new(5,2);
        Stack<Node> Path = PathFinder.FindPath(level, start, end);
        Assert.Equal(new(5,2), Path.Pop());
        Assert.Equal(new(3,2), Path.Pop());
        Assert.Equal(new(1,2), Path.Pop());
        Assert.Equal(new(2,1), Path.Pop());
        Assert.Equal(new(2,0), Path.Pop());
        Assert.Equal(new(0,0), Path.Pop());
        Assert.Equal(0, Path.Count);
    }

    [Fact]
    public void PopulateCacheStartToEnd()
    {
        Level2D level = new("SF  FF\n  FF  \nFFFFFE");
        PathFinder.HasPath(level);
        Assert.True(level.Cache[0].available[17]);
    }

    [Fact]
    public void PopulateCacheStartToNext()
    {
        Level2D level = new("SFF   \n  FFFF\nFFFFFE");
        PathFinder.HasPath(level);
        Assert.True(level.Cache[0].available[2]);
        Assert.True(level.Cache[2].available[17]);
        Assert.True(level.Cache[8].available[17]);
    }
}