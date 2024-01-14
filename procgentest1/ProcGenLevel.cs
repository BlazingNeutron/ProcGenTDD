namespace procgentest1;

public class ProcGenLevel
{
    private Random random = new Random();

    public string[] Generate(int length)
    {
        //generate noise grid
        string[] noise_grid = new string[length];
        for (int i = 0; i < length; i++) {
            int noise = random.Next(0, 100);
            if (noise > 50) {
                noise_grid[i] = "";
            } else {
                noise_grid[i] = "F";
            }
        }
        List<string> map = ["S", .. noise_grid, "E"];
        return [.. map];
    }

    public void SetSeed(int seed) {
        random = new Random(seed);
    }

    public string[] GenerateRandom(int max) {
        return Generate(random.Next(max));
    }
}