using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<Texture2D> textures;

    [Header("Map Size")]
    [SerializeField, Min(5)]
    private uint width = 5;

    [SerializeField, Min(5)]
    private uint height = 5;

    [Header("Hexagon Settings")]
    [SerializeField]
    private Transform hexagonPrefab = null;

    [Tooltip("Can create a gap between hexagons")]
    [SerializeField, Min(0f)]
    private float haxagonGap = 0f;

    // Used to get the right z coordinate for the tile
    private const float hexagonHeightDiffrence = 0.75f;

    private void Start()
    {
        CreateMap();
    }

    private void CreateMap()
    {
        uint[,] map = MapGenerator.GenerateMap(textures.Count, width, height);

        // Create hexagon map
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Transform hexagon = Instantiate(hexagonPrefab, transform);
                hexagon.transform.position = CalculateHexagonWorldPosition(x, y);
            }
        }
    }

    private Vector3 CalculateHexagonWorldPosition(int x, int y)
    {
        // Get x coordinate offset every second row, so the hexagon goes in the right place
        float Xoffset = 0;
        if(y % 2 != 0)
        {
            Xoffset = hexagonPrefab.localScale.y / 2;
        }

        // Calculate the hexagon position
        Vector2 hexagonPosition = new Vector2(-width / 2 + x, -height / 2 + y);
        hexagonPosition.x = hexagonPosition.x * (hexagonPrefab.localScale.x + haxagonGap) + Xoffset;
        hexagonPosition.y = hexagonPosition.y * (hexagonPrefab.localScale.y + haxagonGap) * hexagonHeightDiffrence;

        return new Vector3(hexagonPosition.x, 0, hexagonPosition.y);
    }
}
