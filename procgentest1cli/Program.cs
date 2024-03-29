﻿using System.Text;
using System.Text.RegularExpressions;
using procgentest1;

namespace procgentest1;
public class Program
{

    private readonly ProcGenLevel levelGenerator = new();
    static void Main(string[] args)
    {
        Program program = new();
        program.LargeMap();
    }

    public void LargeMap()
    {
        Level2D template = new(GenerateEmptyLevel(26, 26));
        AStar aStar = new();
        levelGenerator.SetSeed(0);
        Level2D actualMap = levelGenerator.Generate(template);
        Console.WriteLine(actualMap.ToString());
    }

    public void Falling()
    {
        AssertMapCanExist(
        "SF FFF\r\n" +
        "  F   \r\n" +
        "    FE",
        GenerateEmptyLevel(6, 3, 0, 17));
    }

    private int FindMap(string expected, string template)
    {
        for (int i = 0; i < 1; i++)
        {
            levelGenerator.SetSeed(i);
            Level2D actualMap = levelGenerator.Generate(new Level2D(template));
            if (expected == actualMap.ToString())
            {
                Console.WriteLine("i = " + i + actualMap.ToString());
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
        Console.WriteLine(actualMap.ToString());
    }
}