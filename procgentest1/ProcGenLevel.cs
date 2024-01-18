namespace procgentest1;

public class ProcGenLevel
{
    private const string EMPTY_SPACE = " ";
    private const string FLOOR_SPACE = "F";
    private Random random = new();

    public string[,] Generate(string[,] startingMap)
    {
        string[,] noise_grid = GenerateNoiseGrid(startingMap);
        return CelluarAutomaton(noise_grid);
    }

    private static string[,] CelluarAutomaton(string[,] noiseGrid)
    {
        string[,] currentGrid = new string[noiseGrid.GetLength(0), noiseGrid.GetLength(1)];
        Array.Copy(noiseGrid, currentGrid, noiseGrid.GetLength(0) * noiseGrid.GetLength(1));
        for (int x = 0; x < noiseGrid.GetLength(0); x++)
        {
            for (int y = 0; y < noiseGrid.GetLength(1); y++)
            {
                if (IsTheJumpLongerThan3Spaces(noiseGrid, x, y))
                {
                    currentGrid[x - 3, y] = FLOOR_SPACE;
                }
            }
        }

        return currentGrid;
    }

    private static bool IsTheJumpLongerThan3Spaces(string[,] noise_grid, int x, int y)
    {
        return x > 3 && noise_grid[x - 3, y] == EMPTY_SPACE
            && noise_grid[x - 2, y] == EMPTY_SPACE
            && noise_grid[x - 1, y] == EMPTY_SPACE
            && noise_grid[x, y] == EMPTY_SPACE;
    }

    private string[,] GenerateNoiseGrid(string[,] startingMap)
    {
        string[,] noise_grid = new string[startingMap.GetLength(0),startingMap.GetLength(1)];
        Array.Copy(startingMap, noise_grid, startingMap.GetLength(0) * startingMap.GetLength(1));
        for (int x = 0; x < startingMap.GetLength(0); x++)
        {
            for (int y = 0; y < startingMap.GetLength(1); y++) 
            {
                if (IsThisTheStartOrEnd(noise_grid[x, y]))
                {
                    continue;
                }
                int noise = random.Next(0, 99);
                if (noise > 50)
                {
                    noise_grid[x, y] = EMPTY_SPACE;
                }
                else
                {
                    noise_grid[x, y] = FLOOR_SPACE;
                }
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