using System.Numerics;
using System.Text;

namespace procgentest1;

public class Level2D
{
    public class CachedAStarResults
    {
        public bool[] available = null;
        public Stack<Node>[] Paths = [];
        public List<Node> neighbours = null;
    }

    private const string EMPTY_SPACE = " ";
    private const string FLOOR_SPACE = "F";
    private const string START_SPACE = "S";
    private const string END_SPACE = "E";

    private int width;
    private int height;
    public CachedAStarResults[] Cache { get; set; }
    public Level2D(Level2D otherLevel)
    {
        width = otherLevel.GetWidth();
        height = otherLevel.GetHeight();
        LevelArray = new string[width * height];
        Array.Copy(otherLevel.LevelArray, LevelArray, width * height);
        PrepareCache();
    }

    public Level2D(string levelAsString)
    {
        ConvertStringMapTo2DArray(levelAsString);
        PrepareCache();
    }

    private void PrepareCache()
    {
        this.Cache = new CachedAStarResults[width * height];
        for (int i = 0; i < Cache.Length; i++)
        {
            Cache[i] = new();
            Cache[i].Paths = new Stack<Node>[width * height];
            Cache[i].available = new bool[width * height];
            for (int j = 0; j < Cache[i].available.Length; j++)
            {
                Cache[i].available[j] = false;
                Cache[i].Paths[j] = null;
            }
        }
    }


    public int GetWidth() => width;
    public int GetHeight() => height;

    override public string ToString() => Convert2DArrayToStringMap();

    public Vector2 StartPosition()
    {
        return FindPositionOf(START_SPACE);
    }

    public Vector2 EndPosition()
    {
        return FindPositionOf(END_SPACE);
    }

    private Vector2 FindPositionOf(string value)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (LevelArray[x + y * width] == value)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return new(-1, -1);
    }

    private string[] LevelArray { get; set; } = [];

    private void ConvertStringMapTo2DArray(string startingMap)
    {
        string[] rows = startingMap.Split("\n");
        width = rows.Max(x => x.Length);
        height = rows.Length;

        LevelArray = new string[width * height];

        for (int y = 0; y < height; y++)
        {
            string row = rows[y];
            string[] chars = row.Select(c => c.ToString()).ToArray();
            for (int x = 0; x < width; x++)
            {
                LevelArray[x + y * width] = chars[x];
            }
        }
    }

    private string Convert2DArrayToStringMap()
    {
        StringBuilder stringMapBuilder = new();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                stringMapBuilder.Append(LevelArray[x + y * width]);
            }
            if (y + 1 < height)
            {
                stringMapBuilder.Append("\r\n");
            }
        }
        return stringMapBuilder.ToString();
    }

    internal void SetFloor(int x, int y)
    {
        if (!IsStartOrEnd(x, y))
        {
            LevelArray[x + y * width] = FLOOR_SPACE;
        }
    }

    internal bool IsEmpty(int x, int y)
    {
        return LevelArray[x + y * width] == EMPTY_SPACE;
    }

    internal bool IsEmpty(Vector2 position) => IsEmpty(position.X, position.Y);

    internal bool IsStartOrEnd(int x, int y)
    {
        return LevelArray[x + y * width] == START_SPACE || LevelArray[x + y * width] == END_SPACE;
    }

    internal void SetEmpty(int x, int y)
    {
        if (!IsStartOrEnd(x, y))
        {
            LevelArray[x + y * width] = EMPTY_SPACE;
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool IsWithinBounds(Vector2 Position)
    {
        return IsWithinBounds(Position.X, Position.Y);
    }

    internal bool IsFloor(int x, int y)
    {
        return LevelArray[x + y * width] == FLOOR_SPACE || IsStartOrEnd(x, y);
    }

    internal bool IsFloor(Vector2 position)
    {
        return IsFloor(position.X, position.Y);
    }
}