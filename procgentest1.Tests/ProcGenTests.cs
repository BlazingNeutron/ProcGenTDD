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
        AssertMapCanExist("SE", GenerateEmptyLevel(2, 1, 0, 1));
    }

    [Fact]
    public void OneFloor()
    {
        AssertMapCanExist("SFE", GenerateEmptyLevel(3, 1, 0, 2));
    }

    [Fact]
    public void LongerAllFloorMap()
    {
        AssertMapCanExist("SFFE", GenerateEmptyLevel(4, 1, 0, 3));
    }

    [Fact]
    public void LevelWithAJump()
    {
        AssertMapCanExist("SFF E", GenerateEmptyLevel(5, 1, 0, 4));
    }

    [Fact]
    public void TooLongJumpCleanedUp()
    {
        AssertMapCanExist("SFFF FE", GenerateEmptyLevel(7, 1, 0, 6));
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
        AssertMapCanExist("SF \r\n   \r\n  E", GenerateEmptyLevel(3, 3, 0, 8));
    }

    [Fact]
    public void EmptyLevel()
    {
        AssertMapCanExist("", "");
    }

    [Fact]
    public void Falling()
    {
        AssertMapCanExist(
            "SF FFF\r\n" +
            "F F F \r\n" +
            "FFFF E",
            GenerateEmptyLevel(6, 3, 0, 17));
    }

    [Fact]
    public void FallingJump()
    {
        AssertMapCanExist(
            "SF   \r\n" +
            "   FE",
            GenerateEmptyLevel(5, 2, 0, 9));
    }

    // [Fact]
    // public void SeveralPlatforms()
    // {
    //     AssertMapCanExist(
    //         "SFF \r\n" +
    //         "  FF\r\n" +
    //         "FF E",
    //         GenerateEmptyLevel(4, 3, 0, 11));
    // }

    /**
     * FindMap - finds a random seed with the expected map, if possible to generate.
     *   This is a "temporary" method while new proc gen rules are introduced and tweaked.
     */
    private int FindMap(string expected, string template)
    {
        for (int i = 0; i < 1000; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(new Level2D(template));
            if (expected == actualMap.ToString())
            {
                output.WriteLine("i = " + i + actualMap.ToString());
                return i;
            }
        }
        Assert.Fail("Map Cannot be generated");
        return 0;
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
}