namespace procgentest1.Tests;

using System;
using System.Numerics;
using System.Text;
using procgentest1;
using Xunit.Abstractions;

public class ProcGenTests(ITestOutputHelper output)
{
    private const int ALL_FLOOR_SEED = 1;
    private const int SHORT_JUMP = 16;
    private const int TOO_LONG_JUMP = 30;
    private const int NO_FLOOR_ADD = 0;
    private const int SIMPLE_HIGH_FLOOR = 174;

    private readonly ProcGenLevel levelGenerator = new();
    private readonly ITestOutputHelper output = output;

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
        AssertMap(SHORT_JUMP, "S F E", "S   E");
    }

    [Fact]
    public void LevelWithALongJump()
    {
        AssertMap(TOO_LONG_JUMP, "SF FF E", "S     E");
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
    public void LargerLevel30X30()
    {
        AssertMap(0,
            "SF   FF F FFF   FFFF   FFF  F \n" +
            " F  F FFFF    FF    F F  FFFFF\n" +
            " FFFFFF   F  FF   F FFFF FF FF\n" +
            " F     F     F     FF FFFFF FF\n" +
            "    F FFF FFFFFF  FF  F  FFFFF\n" +
            "F FFFF F  F     F FFF FFFF  FF\n" +
            "   F FF  FFF  FF  FFF FF  FF F\n" +
            " FF   FFFFF F     FF FFF FFFF \n" +
            "FF F   F   FFF  F FFF F  F  FF\n" +
            " F FF  FF  F FFF  FF  F F  F F\n" +
            "F FF  F FF F F      F F     F \n" +
            "FF  F  FF  F F       F   F  FF\n" +
            "F F FFF FFF   FFFFF F  FF FF F\n" +
            " F   F          F   F   FFF  F\n" +
            "FF  FF  F F F F F    F FFFFFFF\n" +
            "   FF  F    FF    FFF FF FF   \n" +
            "F F F FF  FF F F F  FFF   FFF \n" +
            " F F  FF  F  F FFFFF  F F     \n" +
            " F      FFFFFFFF    F F  FFF F\n" +
            " FF F      F F F   F   F F   F\n" +
            "FF F         F   F  FFF F F FF\n" +
            " FFFFFF FFFFF  F F F  FFF F  F\n" +
            "  FF FFF FF FFFFF FF FF F FF F\n" +
            "    FF F  F F     F F F FFFFFF\n" +
            "FFF  FFFFFF FFF  F   FF F   F \n" +
            " F F F FF F  F  FFFF  FFFF FFF\n" +
            "  FF F   F FFFFFF  FFF F FFFF \n" +
            "     F FFF FF F FF FF        F\n" +
            "     FF  F FF  FFFFF F FFF   F\n" +
            "   FFFFF   F FFF  FFFFFFFFF  E",
            GenerateEmptyLevel(30, 30, 0, 928));
    }

    private string GenerateEmptyLevel(int width, int height, int startIndex, int endIndex)
    {
        string emptyLevel = string.Concat(Enumerable.Repeat(new string(' ', width) + '\n', height));
        emptyLevel = emptyLevel.Remove(emptyLevel.Length - 1, 1);
        StringBuilder sb = new(emptyLevel);
        sb[startIndex] = 'S';
        sb[endIndex] = 'E';
        return sb.ToString();
    }

    private void AssertMap(int Seed, string Expected, string startingMap)
    {
        levelGenerator.SetSeed(Seed);
        Level2D actualMap = levelGenerator.Generate(new Level2D(startingMap));
        output.WriteLine(actualMap.ToString());
        Assert.Equal(Expected, actualMap.ToString());
    }
}