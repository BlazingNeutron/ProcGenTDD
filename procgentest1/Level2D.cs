using System.Text;

namespace procgentest1;

public class Level2D {
    public Level2D(Level2D otherLevel)
    {
        LevelArray = new string[otherLevel.GetLength(0), otherLevel.GetLength(1)];
        Array.Copy(otherLevel.LevelArray, LevelArray, otherLevel.GetLength(0) * otherLevel.GetLength(1));
    }

    public Level2D(string levelAsString) => ConvertStringMapTo2DArray(levelAsString);

    public string Get(int x, int y) => LevelArray[x, y];

    public void Set(int x, int y, string value) => LevelArray[x, y] = value;

    public int GetLength(int direction = 0) => LevelArray.GetLength(direction);

    override public string ToString() => Convert2DArrayToStringMap();

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
                stringMapBuilder.Append(LevelArray[x ,y]);
            }
            if (y + 1 < LevelArray.GetLength(1))
            {
                stringMapBuilder.Append('\n');
            }
        }
        return stringMapBuilder.ToString();
    }
}