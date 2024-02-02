using System.Numerics;
using System.Text;

namespace procgentest1;

public class Level2D
{
    private const string EMPTY_SPACE = " ";
    private const string FLOOR_SPACE = "F";
    private const string START_SPACE = "S";
    private const string END_SPACE = "E";
    public Level2D(Level2D otherLevel)
    {
        LevelArray = new string[otherLevel.GetWidth(), otherLevel.GetHeight()];
        Array.Copy(otherLevel.LevelArray, LevelArray, otherLevel.GetWidth() * otherLevel.GetHeight());
    }

    public Level2D(string levelAsString) => ConvertStringMapTo2DArray(levelAsString);

    public int GetWidth() => LevelArray.GetLength(0);
    public int GetHeight() => LevelArray.GetLength(1);

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
        for (int y = 0; y < LevelArray.GetLength(1); y++)
        {
            for (int x = 0; x < LevelArray.GetLength(0); x++)
            {
                if (LevelArray[x, y] == value)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return new();
    }

    private string[,] LevelArray { get; set; } = new string[0, 0];

    private void ConvertStringMapTo2DArray(string startingMap)
    {
        string[] rows = startingMap.Split("\n");
        int width = rows.Max(x => x.Length);
        int height = rows.Length;

        LevelArray = new string[width, height];

        for (int y = 0; y < height; y++)
        {
            string row = rows[y];
            string[] chars = row.Select(c => c.ToString()).ToArray();
            for (int x = 0; x < width; x++)
            {
                LevelArray[x, y] = chars[x];
            }
        }
    }

    private string Convert2DArrayToStringMap()
    {
        StringBuilder stringMapBuilder = new();
        for (int y = 0; y < LevelArray.GetLength(1); y++)
        {
            for (int x = 0; x < LevelArray.GetLength(0); x++)
            {
                stringMapBuilder.Append(LevelArray[x, y]);
            }
            if (y + 1 < LevelArray.GetLength(1))
            {
                stringMapBuilder.Append("\r\n");
            }
        }
        return stringMapBuilder.ToString();
    }

    internal bool IsFloor(int x, int y)
    {
        return LevelArray[x, y] == FLOOR_SPACE || IsStartOrEnd(x, y);
    }

    internal void SetFloor(int x, int y)
    {
        if (!IsStartOrEnd(x, y))
        {
            LevelArray[x, y] = FLOOR_SPACE;
        }
    }

    internal bool IsEmpty(int x, int y)
    {
        return LevelArray[x, y] == EMPTY_SPACE;
    }

    internal bool IsEmpty(Vector2 position) => IsEmpty((int)position.X, (int)position.Y);

    internal bool IsStartOrEnd(int x, int y)
    {
        return LevelArray[x, y] == START_SPACE || LevelArray[x, y] == END_SPACE;
    }

    internal void SetEmpty(int x, int y)
    {
        if (!IsStartOrEnd(x, y))
        {
            LevelArray[x, y] = EMPTY_SPACE;
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < LevelArray.GetLength(0) && y < LevelArray.GetLength(1);
    }

    public bool IsWithinBounds(Vector2 Position)
    {
        return IsWithinBounds((int)Position.X, (int)Position.Y);
    }
}