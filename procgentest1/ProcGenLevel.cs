namespace procgentest1;

public class ProcGenLevel
{

    private Random random = new();

    public ProcGenLevel()
    {
        Density = 50;
    }

    private int Density { get; set; }

    public Level2D Generate(Level2D StartingMap)
    {
        return CelluarAutomata(GenerateNoiseGrid(StartingMap));
    }

    private static Level2D CelluarAutomata(Level2D NoiseGrid)
    {
        Level2D CurrentGrid = new(NoiseGrid);

        while (!IsSolvable(CurrentGrid))
        {
            for (int x = 0; x < NoiseGrid.GetLength(0); x++)
            {
                for (int y = 0; y < NoiseGrid.GetLength(1); y++)
                {
                    if (IsTheJumpLongerThanPlayerCanJump(NoiseGrid, x, y))
                    {
                        CurrentGrid.SetFloor(new(x - 1, y));
                    }
                }
            }
        }

        return CurrentGrid;
    }

    private static bool IsSolvable(Level2D CurrentGrid)
    {
        return new AStar().FindPath(CurrentGrid);
    }

    private static bool IsTheJumpLongerThanPlayerCanJump(Level2D NoiseGrid, int x, int y)
    {
        return x > 1 && NoiseGrid.IsEmpty(new(x - 1, y))
            && NoiseGrid.IsEmpty(new(x, y));
    }

    private Level2D GenerateNoiseGrid(Level2D startingMap)
    {
        Level2D NoiseGrid = new(startingMap);
        for (int x = 0; x < startingMap.GetLength(0); x++)
        {
            for (int y = 0; y < startingMap.GetLength(1); y++)
            {
                if (NoiseGrid.IsStartOrEnd(new(x, y)))
                {
                    continue;
                }
                int noise = random.Next(0, 99);
                if (noise > Density)
                {
                    NoiseGrid.SetEmpty(new(x, y));
                }
                else
                {
                    NoiseGrid.SetFloor(new(x, y));
                }
            }
        }

        return NoiseGrid;
    }

    public void SetSeed(int seed)
    {
        random = new Random(seed);
    }
}