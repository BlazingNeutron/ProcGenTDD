namespace procgentest1;

public class ProcGenLevel
{

    private Random random = new();

    public ProcGenLevel()
    {
        Density = 40;
        NumberOfSteps = 10;
    }

    private int Density { get; set; }
    private int NumberOfSteps { get; set; }

    public Level2D Generate(Level2D StartingMap)
    {
        Level2D lastSolvableMap = new("");
        Level2D CelluarMap = GenerateNoiseGrid(StartingMap);
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

    private Level2D CelluarAutomata(Level2D NoiseGrid)
    {
        Level2D CurrentGrid = new(NoiseGrid);
        for (int x = 0; x < NoiseGrid.GetLength(0); x++)
        {
            for (int y = 0; y < NoiseGrid.GetLength(1); y++)
            {
                if (random.Next(0, 99) < ProbabilityOfFloor(NoiseGrid, x, y))
                {
                    CurrentGrid.SetFloor(x, y);
                }
                else
                {
                    CurrentGrid.SetEmpty(x, y);
                }
            }
        }

        return CurrentGrid;
    }

    private static int ProbabilityOfFloor(Level2D noiseGrid, int x, int y)
    {
        if (noiseGrid.IsStartOrEnd(x, y))
        {
            return 100;
        }
        int count = 0;
        for (int i = -2; i < 3; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbourX = x + i;
                int neighbourY = y + j;
                if (i == 0 && j == 0) continue;
                if (!(neighbourX < 0 || neighbourY < 0 || neighbourX >= noiseGrid.GetLength(0) || neighbourY >= noiseGrid.GetLength(1)) && noiseGrid.IsFloor(neighbourX, neighbourY))
                {
                    count++;
                }
            }
        }

        return Math.Max(0, 100 - (count * 25));
    }

    private static bool IsSolvable(Level2D CurrentGrid)
    {
        return new AStar().FindPath(CurrentGrid);
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