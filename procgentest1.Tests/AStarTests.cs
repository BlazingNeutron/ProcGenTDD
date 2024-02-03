namespace procgentest1.Tests;
using procgentest1;

public class AStarTests()
{

    private readonly AStar PathFinder = new();

    [Fact]
    public void PlayerJumpsAndThenFalls()
    {
        Assert.True(PathFinder.FindPath(new(
            "SF   \n" +
            "   FE")));
    }

    [Fact]
    public void PlayerCannotFinish()
    {
        Assert.False(PathFinder.FindPath(new(
            "S    \n" +
            "   FE")));
    }

    [Fact]
    public void PlayerCannotFinishSimpleLevel()
    {
        Assert.False(PathFinder.FindPath(new("S  E")));
    }

    [Fact]
    public void PlayerExtraFall()
    {
        Assert.True(PathFinder.FindPath(new(
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
}