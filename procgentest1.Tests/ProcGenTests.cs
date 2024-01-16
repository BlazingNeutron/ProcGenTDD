namespace procgentest1.Tests;

using procgentest1;
using Xunit.Abstractions;

public class ProcGenTests
{
    private const int ALL_FLOOR_SEED = 1;
    private const int SHORT_JUMP = 16;
    private const int TOO_LONG_JUMP = 30;

    readonly ProcGenLevel levelGenerator = new();

    [Fact]
    public void SimplestLevel()
    {
        AssertMap(ALL_FLOOR_SEED, "SE", "SE");
    }

    [Fact]
    public void AddOneFloor()
    {
        AssertMap(ALL_FLOOR_SEED, "SFE", "S E");
    }

    [Fact]
    public void LongerAllFloorMap()
    {
        AssertMap(ALL_FLOOR_SEED, "SFFE", "S  E");
    }

    [Fact]
    public void LevelWithAJump()
    {
        AssertMap(SHORT_JUMP, "SFF E", "S   E");
    }

    [Fact]
    public void LevelWithALongJump()
    {
        AssertMap(TOO_LONG_JUMP, "SFF   E", "S     E");
    }

    private void AssertMap(int Seed, string Expected, string startingMap)
    {
        levelGenerator.SetSeed(Seed);
        Assert.Equal(Expected, string.Join("", levelGenerator.Generate(startingMap.Select(x => x.ToString()).ToArray())));
    }
}