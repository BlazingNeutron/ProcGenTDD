namespace procgentest1.Tests;

using System.Text;
using System.Text.RegularExpressions;
using procgentest1;
using Xunit.Abstractions;

public class AStarTests()
{

    [Fact]
    public void PlayerJumpsAndThenFalls()
    {
        AStar aStar = new();
        Assert.True(aStar.FindPath(new("SF   \n" + 
                                       "   FE")));
    }
}