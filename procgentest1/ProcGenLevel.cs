namespace procgentest1;

public class ProcGenLevel
{

    private Random random = new();

    public ProcGenLevel()
    {
        Density = 50;
        NumberOfSteps = 20;
    }

    private int Density { get; set; }
    private int NumberOfSteps { get; set; }

    public Level2D Generate(Level2D StartingMap)
    {
        Level2D CelluarMap = GenerateNoiseGrid(StartingMap);
        Level2D lastSolvableMap = CelluarMap;
        for (int currentStep = 0; currentStep < NumberOfSteps; currentStep++)
        {
            CelluarMap = CelluarAutomata(CelluarMap);
            if (IsSolvable(CelluarMap))
            {
                lastSolvableMap = CelluarMap;
            }
        }
        return lastSolvableMap;
    }

    private static Level2D CelluarAutomata(Level2D NoiseGrid)
    {
        Level2D CurrentGrid = new(NoiseGrid);
        int deathLimit = NoiseGrid.GetLength(1) == 1 ? 8 : 4;
        int birthLimit = NoiseGrid.GetLength(1) == 1 ? 6 : 7;

        for (int x = 0; x < NoiseGrid.GetLength(0); x++)
        {
            for (int y = 0; y < NoiseGrid.GetLength(1); y++)
            {
                int neighbourCount = CountAliveNeighbours(NoiseGrid, x, y);
                if (NoiseGrid.IsFloor(x, y))
                {
                    if (neighbourCount < deathLimit)
                    {
                        CurrentGrid.SetEmpty(x, y);
                    }
                    else
                    {
                        CurrentGrid.SetFloor(x, y);
                    }
                }
                else
                {
                    if (neighbourCount > birthLimit)
                    {
                        CurrentGrid.SetFloor(x, y);
                    }
                    else
                    {
                        CurrentGrid.SetEmpty(x, y);
                    }
                }
            }
        }

        return CurrentGrid;
    }

    private static int CountAliveNeighbours(Level2D noiseGrid, int x, int y)
    {
        int count = 0;
        for (int i=-1; i<2; i++)
        {
            for (int j=-1; j<2; j++)
            {
                int neighbourX = x+i;
                int neighbourY = y+j;
                if (i==0 && j==0) continue;
                if (neighbourX < 0 || neighbourY < 0 || neighbourX >= noiseGrid.GetLength(0) || neighbourY >= noiseGrid.GetLength(1))
                {
                    count++;
                }
                else if (noiseGrid.IsFloor(neighbourX, neighbourY))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool IsSolvable(Level2D CurrentGrid)
    {
        return new AStar().FindPath(CurrentGrid);
    }

    private static bool IsTheJumpLongerThanPlayerCanJump(Level2D NoiseGrid, int x, int y)
    {
        return x > 1 && NoiseGrid.IsEmpty(x - 1, y)
            && NoiseGrid.IsEmpty(x, y);
    }

    private Level2D GenerateNoiseGrid(Level2D startingMap)
    {
        Level2D NoiseGrid = new(startingMap);
        for (int x = 0; x < startingMap.GetLength(0); x++)
        {
            for (int y = 0; y < startingMap.GetLength(1); y++)
            {
                if (NoiseGrid.IsStartOrEnd(x, y))
                {
                    continue;
                }
                int noise = random.Next(0, 99);
                if (noise > Density)
                {
                    NoiseGrid.SetEmpty(x, y);
                }
                else
                {
                    NoiseGrid.SetFloor(x, y);
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