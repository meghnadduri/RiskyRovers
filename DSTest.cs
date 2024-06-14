using UnityEngine;

public class DSTest : MonoBehaviour
{
    public int mapSize = 2049; // Size of the heightmap grid (must be 2^n + 1, e.g., 65, 129, 257, ...)
    public int lowResMapSize = 683; // Size of the low res map (2049/3)
    public float roughness = 0.7f; // Roughness factor for terrain generation
    public float heightScale = 10f; // Scale factor for terrain height

    private float[,] heightMap;
    public float [,] lowResMap;

    void Start()
    {
        // Initialize the heightmap
        heightMap = new float[mapSize, mapSize];

        // Initialize the lowResMap
        lowResMap = new float [lowResMapSize, lowResMapSize];

        // Perform diamond-square algorithm to generate terrain
        DiamondSquare(0, 0, mapSize - 1, mapSize - 1, heightScale);

        // Generate the lowResMap
        lowResMapCreate();

        // Display the heightmap visually
        DisplayHeightMap();

        // Display the lowResMap visually
        DisplayLowResMap();
    }

    void DiamondSquare(int x1, int y1, int x2, int y2, float scale)
    {
        // Base case: stop when the range is 1x1
        if (x2 - x1 <= 1 || y2 - y1 <= 1)
            return;

        // Diamond step
        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                float avg = (heightMap[x1, y1] + heightMap[x2, y1] + heightMap[x1, y2] + heightMap[x2, y2]) / 4.0f;
                heightMap[x, y] = avg + Random.Range(-1f, 1f) * scale;
            }
        }

        // Square step
        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                if (x == x1 + (x2 - x1) / 2 || y == y1 + (y2 - y1) / 2)
                    continue;

                float sum = 0;
                int count = 0;

                if (x > x1) { sum += heightMap[x - (x2 - x1) / 2, y]; count++; }
                if (x < x2) { sum += heightMap[x + (x2 - x1) / 2, y]; count++; }
                if (y > y1) { sum += heightMap[x, y - (y2 - y1) / 2]; count++; }
                if (y < y2) { sum += heightMap[x, y + (y2 - y1) / 2]; count++; }

                heightMap[x, y] = sum / count + Random.Range(-1f, 1f) * scale;
            }
        }

        // Recursively call DiamondSquare for each quadrant
        DiamondSquare(x1, y1, x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(x1 + (x2 - x1) / 2, y1, x2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(x1, y1 + (y2 - y1) / 2, x1 + (x2 - x1) / 2, y2, scale * roughness);
        DiamondSquare(x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, x2, y2, scale * roughness);
    }

    void lowResMapCreate() {
        for (int x = 0; x < lowResMapSize; x++) {
            for (int y = 0; y < lowResMapSize; y++) {
                float avg = 0.0f;

                // Multiply both coords by 3
                int tempXCoord = 3 * x;
                int tempYCoord = 3 * y;
                
                // Add up the 9 values
                avg += heightMap[tempXCoord,tempYCoord];
                avg += heightMap[tempXCoord,tempYCoord+1];
                avg += heightMap[tempXCoord,tempYCoord+2];

                avg += heightMap[tempXCoord+1,tempYCoord];
                avg += heightMap[tempXCoord+1,tempYCoord+1];
                avg += heightMap[tempXCoord+1,tempYCoord+2];

                avg += heightMap[tempXCoord+2,tempYCoord];
                avg += heightMap[tempXCoord+2,tempYCoord+1];
                avg += heightMap[tempXCoord+2,tempYCoord+2];
                
                // Divide by 9
                avg = avg / 9;

                // Assign value to lowResMap
                lowResMap[x,y] = avg;
            }
        }
    }

    void DisplayHeightMap()
    {
        Texture2D texture = new Texture2D(mapSize, mapSize);
        Color[] colors = new Color[mapSize * mapSize];

        // Convert heightmap values to grayscale colors
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float heightValue = heightMap[x, y];
                colors[y * mapSize + x] = Color.Lerp(Color.black, Color.white, heightValue);
            }
        }

        // Apply colors to the texture
        texture.SetPixels(colors);
        texture.Apply();

        // Create a sprite and display it
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain");
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    void DisplayLowResMap() {
        Texture2D texture = new Texture2D(lowResMapSize, lowResMapSize);
        Color[] colors = new Color[lowResMapSize * lowResMapSize];

        // Convert heightmap values to grayscale colors
        for (int y = 0; y < lowResMapSize; y++)
        {
            for (int x = 0; x < lowResMapSize; x++)
            {
                float heightValue = lowResMap[x, y];
                colors[y * lowResMapSize + x] = Color.Lerp(Color.black, Color.white, heightValue);
            }
        }

        // Apply colors to the texture
        texture.SetPixels(colors);
        texture.Apply();

        // Create a sprite and display it
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, lowResMapSize, lowResMapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("LowResMap");
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}