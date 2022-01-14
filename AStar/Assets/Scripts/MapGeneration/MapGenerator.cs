using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    /// <summary>
    /// Generates a 2D uint array can can be used to create a map of hexagons
    /// </summary>
    /// <param name="texturesNum"> Amount of textures </param>
    /// <param name="width"> Width of map </param>
    /// <param name="height"> Height of map </param>
    /// <returns> 2D uint array </returns>
    public static int[,] GenerateMap(int texturesNum, uint width, uint height)
    {
        int[,] map = new int[width, height];

        // Fill the array
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                map[i, j] = Random.Range(0, texturesNum);
            }
        }

        return map;
    }
}
