namespace procgentest1;

public class ProcGenLevel
{
    private Random random = new Random();

    public string[] Generate(int length)
    {
        List<string> map = ["S", .. Enumerable.Repeat("F", length), "E"];
        return [.. map];
    }

    public void SetSeed(int seed) {
        random = new Random(seed);
    }

    public string[] GenerateRandom(int max) {
        return Generate(random.Next(max));
    }
}