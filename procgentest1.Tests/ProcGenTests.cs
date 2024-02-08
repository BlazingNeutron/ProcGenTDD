using System.Text;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace procgentest1.Tests;
public class ProcGenTests(ITestOutputHelper output)
{
    private readonly ProcGenLevel levelGenerator = new();
    private readonly ITestOutputHelper output = output;

    [Fact]
    public void SimplestLevel()
    {
        AssertMapCanExist("SE", GenerateEmptyLevel(2, 1));
    }

    [Fact]
    public void OneFloor()
    {
        AssertMapCanExist("SFE", GenerateEmptyLevel(3, 1));
    }

    [Fact]
    public void LongerAllFloorMap()
    {
        AssertMapCanExist("SFFE", GenerateEmptyLevel(4, 1));
    }

    [Fact]
    public void LevelWithAJump()
    {
        AssertMapCanExist("SFF E", GenerateEmptyLevel(5, 1));
    }

    [Fact]
    public void TooLongJumpCleanedUp()
    {
        AssertMapCanExist("SFFF FE", GenerateEmptyLevel(7, 1));
    }

    [Fact]
    public void SimpleTwoDimensionalLevel()
    {
        AssertMapCanExist("  \r\nSE", GenerateEmptyLevel(2, 2, 2, 3));
    }

    [Fact]
    public void HighJump()
    {
        AssertMapCanExist(" F  \r\nS  E", GenerateEmptyLevel(4, 2, 4, 7));
    }

    [Fact]
    public void StartAndEndAreDifferentHeights()
    {
        AssertMapCanExist("SF \r\n   \r\n  E", GenerateEmptyLevel(3, 3));
    }

    [Fact]

    public void Falling()
    {
        AssertMapCanExist(
        "SF FFF\r\n" +
        "  F   \r\n" +
        "    FE",
        GenerateEmptyLevel(6, 3));
    }

    [Fact]
    public void FallingJump()
    {
        AssertMapCanExist(
            "SF   \r\n" +
            "   FE",
            GenerateEmptyLevel(5, 2));
    }

    [Fact]
    public void SeveralPlatforms()
    {
        AssertMapCanExist(
            "SFF \r\n" +
            "  FF\r\n" +
            "FF E",
            GenerateEmptyLevel(4, 3));
    }

    [Fact]
    public void SomeInescapeableAreasAreRemoved()
    {
        AssertMapCannotExist(
            "SF    \r\n" +
            "   FFE\r\n" +
            "FF    ",
            GenerateEmptyLevel(6, 3, 0, 11));
    }

    [Fact]
    public void SomeUnreachableAreasAreRemoved()
    {
        AssertMapCannotExist(
            "SF  FF\r\n" +
            "      \r\n" +
            "FFFFFE",
            GenerateEmptyLevel(6, 3, 0, 11));
    }

    [Fact]
    public void GeneratedMapsAreSolvable()
    {
        Level2D template = new Level2D(GenerateEmptyLevel(6, 3));
        AStar aStar = new();
        for (int i = 0; i < 10000; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(template);
            if (!aStar.HasPath(actualMap)){
                Assert.Fail("Did not find a path.");
            }
        }
    }

    [Fact]
    public void GeneratedMapsAreAllEscapeable()
    {
        Level2D template = new Level2D(GenerateEmptyLevel(6, 3));
        AStar aStar = new();
        for (int i = 0; i < 10000; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(template);
            if (!aStar.FindPathFromAll(actualMap)){
                Assert.Fail("Did not find a path.");
            }
        }
    }

    [Fact]
    public void GeneratedMapsAreAllReachable()
    {
        Level2D template = new Level2D(GenerateEmptyLevel(6, 3));
        AStar aStar = new();
        for (int i = 0; i < 10000; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(template);
            if (!aStar.FindPathToAll(actualMap)){
                Assert.Fail("Did not find a path.");
            }
        }
    }

    [Fact]
    public void LargeMap()
    {
        Level2D template = new(GenerateEmptyLevel(25, 25));
        AStar aStar = new();
        levelGenerator.SetSeed(0);
        Level2D actualMap = levelGenerator.Generate(template);
    }

    /**
     * FindMap - finds a random seed with the expected map, if possible to generate.
     *   This is a "temporary" method while new proc gen rules are introduced and tweaked.
     */
    private int FindMap(string expected, string template)
    {
        for (int i = 0; i < 10000; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(new Level2D(template));
            if (expected == actualMap.ToString())
            {
                output.WriteLine("i = " + i + actualMap.ToString());
                return i;
            }
        }
        return -1;
    }

    private string GenerateEmptyLevel(int width, int height)
    {
        return GenerateEmptyLevel(width, height, 0, width * height -1);
    }

    private string GenerateEmptyLevel(int width, int height, int startIndex, int endIndex)
    {
        string emptyLevel = new(' ', width * height);
        StringBuilder sb = new(emptyLevel);
        sb[startIndex] = 'S';
        sb[endIndex] = 'E';
        emptyLevel = Regex.Replace(sb.ToString(), ".{" + width + "}", "$0\n");
        return emptyLevel.Remove(emptyLevel.Length - 1, 1);
    }

    private void AssertMapCanExist(string Expected, string startingMap)
    {
        levelGenerator.SetSeed(FindMap(Expected, startingMap));
        Level2D actualMap = levelGenerator.Generate(new Level2D(startingMap));
        output.WriteLine(actualMap.ToString());
        Assert.Equal(Expected, actualMap.ToString());
    }

    private void AssertMapCannotExist(String expected, string startingMap)
    {
        Assert.Equal(-1, FindMap(expected, startingMap));
    }
}