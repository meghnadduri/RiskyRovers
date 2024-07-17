using UnityEngine;
using UnityEngine.UI;

public class DiamondSquareTerrainGenerator : MonoBehaviour
{
    public int mapSize = 2049; // Size of the heightmap grid (must be 2^n + 1, e.g., 65, 129, 257, ...)
    public static int lowResMapSize = 16; // Size of the low res map ((2049-1)/64)
    public int lowResScale = 2048/lowResMapSize ;
    public float roughness = 0.7f; // Roughness factor for terrain generation
    public float heightScale = 10f; // Scale factor for terrain height
    public int terrainCount = 1; // Number of terrains to generate
    public int gridWidth = 3; // Number of terrains per row
    public float spacing = -1.0f; // Spacing between terrains (use negative or zero for minimal spacing)
    public Vector3 terrainScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale for each terrain tile
    public Vector3 mapPositionOffset = new Vector3(500f, 500f, 0f);
    public RawImage rawImage; // Reference to the RawImage component to display the map
    public Transform rover; // Rover to track
    public float rawX; // X coordinate of rover position
    public float rawY; // Y coordinate of rover position
    public int convertedX; // Rover X position as a value in the array
    public int convertedY; // Rover Y position as a vlue in the array
    public bool[,] boolArray = new bool[16, 16]; // Stores values of the mini map that have been explored
    public float[,] heightMap; // The main terrain map
    public float [,] lowResMap; // The low res layer of the mini map

    void Start()
    {
        // Initialize every value in the bool array as false bc nothing has been explored yet
        for (int i = 0; i < 16; i++) 
        {
            for (int j = 0; j < 16; j++)
            {
                boolArray[i, j] = false;
            }
        }

        // Generate the appropriate amount of terrains (1 in this case)
        for (int i = 0; i < terrainCount; i++)
        {
            GenerateTerrain(i);
        }
    }

    void Update() {

        // Get the world coordinates of the rover
        Vector3 roverWorldPosition = rover.transform.position;

        // Get just the raw X and Y coordinates from the vector
        rawX = (float) roverWorldPosition.x;
        rawY = (float) roverWorldPosition.y;

        // Convert it to 16 x 16
        // The raw coordinates are on a scale from -10 to 10. To get them to a scale of 0 to 15, we have to add 10, multiply by 8, and round down to the nearest int
        convertedX = (int) ((rawX + 10.0f) * 0.8f);
        convertedY = (int) ((rawY + 10.0f) * 0.8f);

        // Change the appropriate value in the bool array
        if (convertedX < 16 && convertedY < 16) { // Check to make sure the new coords are within the array bounds
            boolArray[convertedX, convertedY] = true; // Change rover's location to true
            
            // If they exist, change the values directly around the rover's location to true
            if (convertedX > 0) {
                boolArray[convertedX - 1, convertedY] = true;
            }
            if (convertedY > 0) {
                boolArray[convertedX, convertedY - 1] = true;
            }
            if (convertedX < 15) {
                boolArray[convertedX + 1, convertedY] = true;
            }
            if (convertedY < 15) {
                boolArray[convertedX, convertedY + 1] = true;
            }
            if (convertedX > 0 && convertedY > 0) {
                boolArray[convertedX - 1, convertedY - 1] = true;
            }
            if (convertedX > 0 && convertedY < 15) {
                boolArray[convertedX - 1, convertedY + 1] = true;
            }
            if (convertedX < 15 && convertedY < 15) {
                boolArray[convertedX + 1, convertedY + 1] = true;
            }
            if (convertedX < 15 && convertedY > 0) {
                boolArray[convertedX + 1, convertedY - 1] = true;
            }
        }
        
        // Display the lowResMap
        DisplayLowResMap(lowResMap, 1, boolArray);
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

                // Find tempXCoord and tempYCoord on the 2049 x 2049 relative to where x and y are on the 16 x 16
                int tempXCoord = lowResScale * x;
                int tempYCoord = lowResScale * y;
               
                // Add up all the values in that area 
                for (int a = 0; a < lowResScale; a++) {
                    for (int b = 0; b < lowResScale; b++) {
                        avg += heightMap[tempXCoord + a, tempYCoord + b];
                    }
                }
               
                // Divide by lowResScale squared to find the average
                avg = avg / (lowResScale*lowResScale);

                // Assign value to its respective index on the lowResMap
                lowResMap[x,y] = avg;
            }
        }
    }

    void DisplayHeightMap(float[,] heightMap, int index)
    {
        // Create a new texture
        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.RGBA32, false);
        // Smoothing the texture
        texture.filterMode = FilterMode.Bilinear;
        // Create a new array for colors
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

        // Create a new sprite and assign the color values (texture) and dimensions (mapSize)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), Vector2.one * 0.5f);
        // Create a new GameObject and label it
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        // Add a sprite renderer component
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        // Add the sprite created above as the sprite
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

    void DisplayLowResMap(float[,] lowResMap, int index, bool[,] boolArray)
    {
        // Create a new texture
        Texture2D texture = new Texture2D(lowResMapSize, lowResMapSize, TextureFormat.RGBA32, false);
        // Smoothing the texture
        texture.filterMode = FilterMode.Bilinear;
        // Create new array for the colors
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

        

        // Make the color change
        for (int m = 0; m < 16; m++) {
            for (int n = 0; n < 16; n++) {
                if (boolArray[m,n] == true) { 
                    // If a value is set to 'true' (has been explored by player), set that unit on the lowResMap to transparent to reveal the high res map below it
                    colors[n * lowResMapSize + m ] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        // Create a new sprite and assign the color values (texture) and dimensions (lowResMapSize)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, lowResMapSize, lowResMapSize), Vector2.one * 0.5f);
        // Create a new GameObject and label it
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        // Add a sprite renderer component
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        // Add the sprite created above as the sprite
        renderer.sprite = sprite;

        // Calculate position for the terrain with minimal spacing
        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (lowResMapSize - 1 + spacing);
        float yOffset = row * (lowResMapSize - 1 + spacing);
        terrain.transform.position = new Vector3(xOffset, yOffset, 0);

        // Apply the scaling transform
        terrain.transform.localScale = new Vector3(10f, 10f, 10f);

        //Apply the position transform
        terrain.transform.position = mapPositionOffset;

        // Assign the low res map to a rawImage in order to make it a UI element
        if (rawImage != null) {
            rawImage.texture = texture;
        }

        // Since we are generating a new low res map every frame, destroy each one to avoid crashing the game
        Destroy(terrain,0.1f);
    }
}