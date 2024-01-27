namespace procgentest1.Tests;

using System;
using System.Text;
using procgentest1;
using Xunit.Abstractions;

public class ProcGenTests()
{
    private const int ALL_FLOOR_SEED = 1;
    private const int SHORT_JUMP = 16;
    private const int TOO_LONG_JUMP = 30;
    private const int NO_FLOOR_ADD = 0;
    private const int SIMPLE_HIGH_FLOOR = 174;

    readonly ProcGenLevel levelGenerator = new();

    [Fact]
    public void SimplestLevel()
    {
        AssertMap(ALL_FLOOR_SEED, "SE", "SE");
    }

    [Fact]
    public void OneFloor()
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
        AssertMap(TOO_LONG_JUMP, "SFFFF E", "S     E");
    }

    [Fact]
    public void SimpleTwoDimensionalLevel()
    {
        AssertMap(NO_FLOOR_ADD, "  \nSE", "  \nSE");
    }

    [Fact]
    public void HighJump()
    {
        AssertMap(4954, " F  \nS  E", "    \nS  E");
    }

    [Fact]
    public void StartAndEndAreDifferentHeights()
    {
        AssertMap(SIMPLE_HIGH_FLOOR, "SF \n   \n  E", "S  \n   \n  E");
    }

    [Fact]
    public void EmptyLevel()
    {
        AssertMap(SIMPLE_HIGH_FLOOR, "", "");
    }

    [Fact]
    public void LargerLevel10Squared()
    {
        AssertMap(0, 
            "SFFF F F  \n"+
            " F FFF  F \n"+
            " F F  FFFF\n"+
            "   FF     \n"+
            " FF FF  F \n"+
            "F    FF  F\n"+
            " F     FFF\n"+
            "   FF F   \n"+
            "F  FF    F\n"+
            "   FF  F E",
            "S         \n          \n          \n          \n          \n          \n          \n          \n          \n         E");
    }

    private void AssertMap(int Seed, string Expected, string startingMap)
    {
        levelGenerator.SetSeed(Seed);
        Level2D actualMap = levelGenerator.Generate(new Level2D(startingMap));
        Assert.Equal(Expected, actualMap.ToString());
    }
}