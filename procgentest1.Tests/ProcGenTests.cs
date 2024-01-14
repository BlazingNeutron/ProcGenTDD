namespace procgentest1.Tests;

using procgentest1;

public class ProcGenTests
{
    ProcGenLevel levelGenerator = new ProcGenLevel();

    [Fact]
    public void SimplestLevel()
    {
        Assert.Equal(levelGenerator.Generate(0), ["S", "E"]);
    }

    [Fact]
    public void AddOneFloor() {
        Assert.Equal(levelGenerator.Generate(1), ["S", "F", "E" ]);
    }

    [Fact]
    public void RandomMaxLengthAllFloor() {
        levelGenerator.SetSeed(122);
        Assert.Equal(levelGenerator.GenerateRandom(5), ["S", "F", "F", "E" ]);
    }
}