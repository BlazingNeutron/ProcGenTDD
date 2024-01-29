namespace procgentest1;

public class ProcGenLevel
{
    private const string EMPTY_SPACE = " ";
    private const string FLOOR_SPACE = "F";
    private Random random = new();

    public Level2D Generate(Level2D startingMap)
    {
        Level2D noise_grid = GenerateNoiseGrid(startingMap);
        return CelluarAutomata(noise_grid);
    }

    private static Level2D CelluarAutomata(Level2D noiseGrid)
    {
        var currentGrid = new Level2D(noiseGrid);

        while (!IsSolvable(currentGrid))
        {
            for (int x = 0; x < noiseGrid.GetLength(0); x++)
            {
                for (int y = 0; y < noiseGrid.GetLength(1); y++)
                {
                    if (IsTheJumpLongerThanPlayerCanJump(noiseGrid, x, y))
                    {
                        currentGrid.Set(x - 1, y, FLOOR_SPACE);
                    }
                }
            }
        }

        return currentGrid;
    }

    private static bool IsSolvable(Level2D currentGrid)
    {
        return new AStar().FindPath(currentGrid);
    }

    private static bool IsTheJumpLongerThanPlayerCanJump(Level2D noise_grid, int x, int y)
    {
        return x > 1 && noise_grid.Get(x - 1, y) == EMPTY_SPACE
            && noise_grid.Get(x, y) == EMPTY_SPACE;
    }

    private Level2D GenerateNoiseGrid(Level2D startingMap)
    {
        Level2D noise_grid = new(startingMap);
        for (int x = 0; x < startingMap.GetLength(0); x++)
        {
            for (int y = 0; y < startingMap.GetLength(1); y++)
            {
                if (IsThisTheStartOrEnd(noise_grid.Get(x, y)))
                {
                    continue;
                }
                int noise = random.Next(0, 99);
                if (noise > 50)
                {
                    noise_grid.Set(x, y, EMPTY_SPACE);
                }
                else
                {
                    noise_grid.Set(x, y, FLOOR_SPACE);
                }
            }
        }

        return noise_grid;
    }

    private static bool IsThisTheStartOrEnd(string gridValue)
    {
        return gridValue == "S" || gridValue == "E";
    }

    public void SetSeed(int seed)
    {
        random = new Random(seed);
    }
}