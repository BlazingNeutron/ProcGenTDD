namespace procgentest1;

public class ProcGenLevel
{
    private const string EMPTY_SPACE = " ";
    private const string FLOOR_SPACE = "F";
    private Random random = new();

    public string[] Generate(string[] startingMap)
    {
        string[] noise_grid = GenerateNoiseGrid(startingMap);
        return CelluarAutomaton(startingMap, noise_grid);
    }

    private static string[] CelluarAutomaton(string[] startingMap, string[] noise_grid)
    {
        string[] tmp_grid = new string[startingMap.Length];
        noise_grid.CopyTo(tmp_grid, 0);
        for (int i = 0; i < startingMap.Length; i++)
        {
            if (IsTheJumpLongerThan3Spaces(noise_grid, i))
            {
                tmp_grid[i - 3] = FLOOR_SPACE;
            }
        }

        return tmp_grid;
    }

    private static bool IsTheJumpLongerThan3Spaces(string[] noise_grid, int i)
    {
        return i > 3 && noise_grid[i - 3] == EMPTY_SPACE
            && noise_grid[i - 2] == EMPTY_SPACE
            && noise_grid[i - 1] == EMPTY_SPACE
            && noise_grid[i] == EMPTY_SPACE;
    }

    private string[] GenerateNoiseGrid(string[] startingMap)
    {
        string[] noise_grid = new string[startingMap.Length];
        startingMap.CopyTo(noise_grid, 0);
        for (int i = 0; i < startingMap.Length; i++)
        {
            if (IsThisTheStartOrEnd(noise_grid[i]))
            {
                continue;
            }
            int noise = random.Next(0, 99);
            if (noise > 50)
            {
                noise_grid[i] = EMPTY_SPACE;
            }
            else
            {
                noise_grid[i] = FLOOR_SPACE;
            }
        }

        return noise_grid;
    }

    private static bool IsThisTheStartOrEnd(string gridValue)
    {
        return gridValue == "S" || gridValue == "E";
    }

    public void SetSeed(int seed) {
        random = new Random(seed);
    }
}