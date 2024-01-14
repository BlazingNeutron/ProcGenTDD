namespace procgentest1;

public class ProcGenLevel
{
    private Random random = new Random();

    public string[] Generate(int length)
    {
        //generate noise grid
        string[] noise_grid = new string[length];
        for (int i = 0; i < length; i++) {
            int noise = random.Next(0, 99);
            if (noise > 50) {
                noise_grid[i] = " ";
            } else {
                noise_grid[i] = "F";
            }
        }
        string[] tmp_grid = new string[length];
        noise_grid.CopyTo(tmp_grid, 0);
        for (int i = 0; i < length; i++) {
            if (i > 3 && noise_grid[i - 3] == " " 
                && noise_grid[i - 2] == " " && noise_grid[i - 1] == " " 
                && noise_grid[i] == " ") {
                tmp_grid[i-3] = "F";
            }
        }
        List<string> map = ["S", .. tmp_grid, "E"];
        return [.. map];
    }

    public void SetSeed(int seed) {
        random = new Random(seed);
    }
}