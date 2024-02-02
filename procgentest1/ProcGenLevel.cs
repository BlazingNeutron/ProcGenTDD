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
            if (IsSolvable(CelluarMap) && IsEscapeable(CelluarMap))
            {
                lastSolvableMap = CelluarMap;
            }
            else
            {
                currentStep--;
            }
        }
        return lastSolvableMap;
    }

    private bool IsEscapeable(Level2D celluarMap)
    {
        return new AStar().FindPathFromAll(celluarMap);
    }

    private Level2D CelluarAutomata(Level2D NoiseGrid)
    {
        Level2D CurrentGrid = new(NoiseGrid);
        for (int x = 0; x < NoiseGrid.GetWidth(); x++)
        {
            for (int y = 0; y < NoiseGrid.GetHeight(); y++)
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

    private static int ProbabilityOfFloor(Level2D NoiseGrid, int x, int y)
    {
        if (NoiseGrid.IsStartOrEnd(x, y))
        {
            return 100;
        }
        if (NoiseGrid.IsFloor(x, y))
        {
            if (DirectlyAboveOrBelowFloor(NoiseGrid, x, y))
            {
                return 20;
            }
            if (BetweenFloors(NoiseGrid, x, y))
            {
                return 50;
            }
            if (BesideOneFloor(NoiseGrid, x, y))
            {
                return 25;
            }
            AStar aStar = new();
            Node end = new(NoiseGrid.EndPosition());
            Node start = new(new System.Numerics.Vector2(x, y));
            if (!aStar.FindPath(NoiseGrid, start, end))
            {
                return 0;
            }
        }

        return 50;
    }

    private static bool BesideOneFloor(Level2D noiseGrid, int x, int y)
    {
        return (noiseGrid.IsWithinBounds(x + 1, y) && noiseGrid.IsFloor(x + 1, y)) ||
            (noiseGrid.IsWithinBounds(x - 1, y) && noiseGrid.IsFloor(x - 1, y));
    }

    private static bool BetweenFloors(Level2D noiseGrid, int x, int y)
    {
        return noiseGrid.IsWithinBounds(x + 1, y) && noiseGrid.IsFloor(x + 1, y) &&
            noiseGrid.IsWithinBounds(x - 1, y) && noiseGrid.IsFloor(x - 1, y);
    }

    private static bool DirectlyAboveOrBelowFloor(Level2D NoiseGrid, int x, int y)
    {
        return (NoiseGrid.IsWithinBounds(x, y - 1) && NoiseGrid.IsFloor(x, y - 1)) ||
            (NoiseGrid.IsWithinBounds(x, y + 1) && NoiseGrid.IsFloor(x, y + 1));
    }

    private static bool IsSolvable(Level2D CurrentGrid)
    {
        return new AStar().FindPath(CurrentGrid);
    }

    private Level2D GenerateNoiseGrid(Level2D startingMap)
    {
        Level2D NoiseGrid = new(startingMap);
        for (int x = 0; x < startingMap.GetWidth(); x++)
        {
            for (int y = 0; y < startingMap.GetHeight(); y++)
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