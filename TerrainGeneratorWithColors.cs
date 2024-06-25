using UnityEngine;

public class DiamondSquareTerrainGenerator : MonoBehaviour
{
    public int mapSize = 2049; // Size of the heightmap grid (must be 2^n + 1, e.g., 65, 129, 257, ...)
    public int lowResMapSize = 683; // Size of the low res map (2049/3)
    public float roughness = 0.7f; // Roughness factor for terrain generation
    public float heightScale = 10f; // Scale factor for terrain height
    public int terrainCount = 5; // Number of terrains to generate
    public int gridWidth = 3; // Number of terrains per row
    public float spacing = -1.0f; // Spacing between terrains (use negative or zero for minimal spacing)
    public Vector3 terrainScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale for each terrain tile
    public float[,] heightMap;
    public float [,] lowResMap;

    void Start()
    {
        for (int i = 0; i < terrainCount; i++)
        {
            GenerateTerrain(i);
        }
    }

    void GenerateTerrain(int index)
    {
        // Initialize the heightmap
        heightMap = new float[mapSize, mapSize];

        // Initialize the lowResMap
        lowResMap = new float [lowResMapSize, lowResMapSize];

        // Perform diamond-square algorithm to generate terrain
        DiamondSquare(heightMap, 0, 0, mapSize - 1, mapSize - 1, heightScale);

        // Generate the lowResMap
        lowResMapCreate();

        // Display the heightmap visually
        DisplayHeightMap(heightMap, index);

        // Display the lowResMap visually
        DisplayLowResMap(lowResMap, index);
    }

    void DiamondSquare(float[,] heightMap, int x1, int y1, int x2, int y2, float scale)
    {
        if (x2 - x1 <= 1 || y2 - y1 <= 1)
            return;

        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                float avg = (heightMap[x1, y1] + heightMap[x2, y1] + heightMap[x1, y2] + heightMap[x2, y2]) / 4.0f;
                heightMap[x, y] = avg + Random.Range(-1f, 1f) * scale;
            }
        }

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

        DiamondSquare(heightMap, x1, y1, x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(heightMap, x1 + (x2 - x1) / 2, y1, x2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(heightMap, x1, y1 + (y2 - y1) / 2, x1 + (x2 - x1) / 2, y2, scale * roughness);
        DiamondSquare(heightMap, x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, x2, y2, scale * roughness);
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

    void DisplayHeightMap(float[,] heightMap, int index)
    {
        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear; // Smoothing the texture
        Color[] colors = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float heightValue = heightMap[x, y];
                // Mars-like color gradient from dark brown to light brown
                colors[y * mapSize + x] = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Calculate position for the terrain with minimal spacing
        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (mapSize - 1 + spacing);
        float yOffset = row * (mapSize - 1 + spacing);
        terrain.transform.position = new Vector3(xOffset, yOffset, 0);

        // Apply the scaling transform
        terrain.transform.localScale = terrainScale;
    }

    void DisplayLowResMap(float[,] lowResMap, int index)
    {
        Texture2D texture = new Texture2D(lowResMapSize, lowResMapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear; // Smoothing the texture
        Color[] colors = new Color[lowResMapSize * lowResMapSize];

        for (int y = 0; y < lowResMapSize; y++)
        {
            for (int x = 0; x < lowResMapSize; x++)
            {
                float heightValue = lowResMap[x, y];
                // Mars-like color gradient from dark brown to light brown
                colors[y * lowResMapSize + x] = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, lowResMapSize, lowResMapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Calculate position for the terrain with minimal spacing
        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (lowResMapSize - 1 + spacing);
        float yOffset = row * (lowResMapSize - 1 + spacing);
        terrain.transform.position = new Vector3(xOffset, yOffset, 0);

        // Apply the scaling transform
        terrain.transform.localScale = terrainScale;
    }
}
