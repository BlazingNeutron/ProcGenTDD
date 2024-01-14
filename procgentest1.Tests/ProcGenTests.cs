namespace procgentest1.Tests;

using procgentest1;

public class ProcGenTests
{
    private const int LENGTH_ONE_FLOOR_SEED = 1;
    private const int LENGTH_TWO_FLOOR_SEED = 9;
    private const int LENGTH_TWO_FLOOR_JUMP_SEED = 2;

    ProcGenLevel levelGenerator = new ProcGenLevel();

    [Fact]
    public void SimplestLevel()
    {
        Assert.Equal(["S", "E"], levelGenerator.Generate(0));
    }

    [Fact]
    public void AddOneFloor() {
        levelGenerator.SetSeed(LENGTH_ONE_FLOOR_SEED);
        Assert.Equal(["S", "F", "E" ], levelGenerator.Generate(1));
    }

    [Fact]
    public void RandomMaxLengthAllFloor() {
        levelGenerator.SetSeed(LENGTH_TWO_FLOOR_SEED);
        Assert.Equal(["S", "F", "F", "E" ], levelGenerator.GenerateRandom(5));
    }

    [Fact]
    public void LevelWithAJump() {
        levelGenerator.SetSeed(LENGTH_TWO_FLOOR_JUMP_SEED);
        Assert.Equal(["S", "F", "F", "", "E" ], levelGenerator.GenerateRandom(5));
    }
}