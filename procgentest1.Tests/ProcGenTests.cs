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

    [Fact]
    public void SimpleTwoDimensionalLevel()
    {
       AssertMap(0, "  \nSE", "  \nSE");
    }

    private void AssertMap(int Seed, string Expected, string startingMap)
    {
        levelGenerator.SetSeed(Seed);
        string[,] actualMap = levelGenerator.Generate(ConvertStringMapTo2DArray(startingMap));
        Assert.Equal(Expected, Convert2DArrayToStringMap(actualMap));
    }

    private static string Convert2DArrayToStringMap(string[,] actualMap)
    {
        StringBuilder stringMapBuilder = new();
        for (int y = 0; y < actualMap.GetLength(1); y++) 
        {
            for (int x = 0; x < actualMap.GetLength(0); x++)
            {
                stringMapBuilder.Append(actualMap[x ,y]);
            }
            if (y + 1 < actualMap.GetLength(1))
            {
                stringMapBuilder.Append('\n');
            }
        }
        return stringMapBuilder.ToString();
    }

    private static string[,] ConvertStringMapTo2DArray(string startingMap)
    {
        string[] rows = startingMap.Split("\n");
        int width = rows.Max(x => x.Length);
        int height = rows.Length;

        string[,] startingMap2D = new string[width, height];

        for (int y = 0; y < height; y++)
        {
            string row = rows[y];
            string[] chars = row.Select(c => c.ToString()).ToArray();
            for (int x = 0; x < width; x++)
            {
                startingMap2D[x, y] = chars[x];
            }
        }

        return startingMap2D;
    }
}