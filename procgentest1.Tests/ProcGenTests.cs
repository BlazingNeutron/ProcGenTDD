namespace procgentest1.Tests;

using procgentest1;
using Xunit.Abstractions;

public class ProcGenTests
{
    private const int ALL_FLOOR_SEED = 1;
    private const int SHORT_JUMP = 16;
    private const int TOO_LONG_JUMP = 30;

    readonly ProcGenLevel levelGenerator = new ProcGenLevel();

    [Fact]
    public void SimplestLevel()
    {
        Assert.Equal(["S", "E"], levelGenerator.Generate(["S", "E"]));
    }

    [Fact]
    public void AddOneFloor()
    {
        levelGenerator.SetSeed(ALL_FLOOR_SEED);
        Assert.Equal(["S", "F", "E"], levelGenerator.Generate(["S", "", "E"]));
    }

    [Fact]
    public void LongerAllFloorMap()
    {
        levelGenerator.SetSeed(ALL_FLOOR_SEED);
        Assert.Equal(["S", "F", "F", "E"], levelGenerator.Generate(["S", "", "", "E"]));
    }

    [Fact]
    public void LevelWithAJump()
    {
        levelGenerator.SetSeed(SHORT_JUMP);
        Assert.Equal(["S", "F", "F", " ", "E"], levelGenerator.Generate(["S", "", "", "", "E"]));
    }

    [Fact]
    public void LevelWithALongJump()
    {
        levelGenerator.SetSeed(TOO_LONG_JUMP);
        Assert.Equal(["S", "F", "F", " ", " ", " ", "E" ], levelGenerator.Generate(["S", "", "", "", "", "", "E" ]));
    }
}