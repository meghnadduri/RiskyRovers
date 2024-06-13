using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiamondSquareTerrain : MonoBehaviour
{
    float[,] map;
    public int SizeXY;
    public Tilemap tilemap;
    public Tile[] tiles;
    public float[] levels;


    void DiamondSquareAlgorithm(int size) {
        int half = size / 2;
        if (half < 1) return;
        for (int y = half; y < SizeXY; y += size)
        {
            for (int x = half; x < SizeXY; x += size) {
                DiamondStep(x, y, half);
            }
        }
    }

    void DiamondStep(int x , int y, int half)
    {
        float value = 0.0f;
        value += map[x + half, y - half];
        value += map[x - half, y + half];
        value += map[x + half, y + half];
        value += map[x - half, y - half];

        value += Random.Range(0, half * 2) - half;
        value /= 4;
        map[x, y] = value;

    }

    void SquareStep(int x, int y, int half)
    {
        float value = 0.0f;
        int cont = 0;
        if (x - half >= 0)
        {
            value += map[]
        }
    }


}