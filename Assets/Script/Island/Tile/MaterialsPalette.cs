using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMaterialsPalette", menuName = "Palette/Materials")]
public class MaterialsPalette : ScriptableObject
{
    [SerializeField] private Material[] m_materialsPalette;
    public Material[] MaterialPalette => m_materialsPalette;

    private int gridSize = 100;
    public int width = 100;
    public int height = 100;
    public float scale = 25f;
    public int maxGridValue = 4;

    public void ChangeMaterialAt(MeshRenderer renderer, float x, float y)
    {
        return;
        Material[] mats = renderer.materials;

        mats[0] =  m_materialsPalette[0];
        renderer.materials = mats;
    }

    float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[100, 100];

        for (int y = 0; y < 100; y++) {
            for (int x = 0; x < 100; x++) {
                float sampleX = x / 25;
                float sampleY = y / 25;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    int[,] GenerateGrid(float[,] noiseMap, int maxGridValue)
    {
        int[,] grid = new int[width, height];

        float maxValue = float.MinValue;
        float minValue = float.MaxValue;

        // Find the max and min values in the noise map
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (noiseMap[x, y] > maxValue) {
                    maxValue = noiseMap[x, y];
                }
                if (noiseMap[x, y] < minValue) {
                    minValue = noiseMap[x, y];
                }
            }
        }

        // Normalize the noise map values between 0 and 1
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minValue, maxValue, noiseMap[x, y]);
            }
        }

        // Convert the normalized noise map values to grid values between 0 and maxGridValue
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                grid[x, y] = Mathf.RoundToInt(Mathf.Lerp(0, maxGridValue, noiseMap[x, y]));
            }
        }

        return grid;
    }
}
