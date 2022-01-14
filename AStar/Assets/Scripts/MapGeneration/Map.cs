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
        int[,] map = MapGenerator.GenerateMap(textures.Count, width, height);

        // Create hexagon map
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                CreateHexagon(map[x, y], x, y);
            }
        }
    }

    /// <summary>
    /// Creates the hexagon, sets it's position in the map and sets it's material
    /// </summary>
    /// <param name="hexagonType"> What texture it's going to use from the textures list </param>
    /// <param name="x"> X position </param>
    /// <param name="y"> Y position </param>
    private void CreateHexagon(int hexagonType, int x, int y)
    {
        Transform hexagon = Instantiate(hexagonPrefab, transform);

        hexagon.position = CalculateHexagonWorldPosition(x, y);
        hexagon.eulerAngles = new Vector3(0, 180, 0); // Rotate the hexagon so the texture looks right

        hexagon.GetComponent<Renderer>().material.SetTexture("_MainTex", textures[hexagonType]);
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
