using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    /// <summary>
    /// Generates a 2D int array can can be used to create a map of hexagons with textures
    /// </summary>
    /// <param name="tileTypesNum"> Amount of tileTypes </param>
    /// <param name="width"> Width of map </param>
    /// <param name="height"> Height of map </param>
    /// <returns> 2D int array of value between 0 and tileTypesNum </returns>
    public static int[,] GenerateMap(int tileTypesNum, uint width, uint height)
    {
        int[,] map = new int[width, height];

        // Fill the array
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                map[i, j] = Random.Range(0, tileTypesNum);
            }
        }

        return map;
    }
}
