using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;

public class Map : MonoBehaviour
{
    [Header("Tiles")]
    public List<TileType> tileTypes;

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

    // All hexagon tiles in the map
    private Tile[,] tiles;

    // Used to get the right z coordinate for the tile
    private const float hexagonHeightDiffrence = 0.75f;

    private void Start()
    {
        CreateMap();
    }

    private void CreateMap()
    {
        int[,] map = MapGenerator.GenerateMap(tileTypes.Count, width, height);
        tiles = new Tile[width, height];

        // Create hexagon map
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                CreateHexagon(map[x, y], x, y);
            }
        }

        SetTileNeighbours();
    }

    /// <summary>
    /// Creates the hexagon, initializes the tile and adds it to the tiles list
    /// </summary>
    /// <param name="tileType"> What tile type it's going to use from the tileTypes list </param>
    /// <param name="x"> X position </param>
    /// <param name="y"> Y position </param>
    private void CreateHexagon(int tileType, int x, int y)
    {
        Transform hexagon = Instantiate(hexagonPrefab, transform);
        hexagon.position = CalculateHexagonWorldPosition(x, y);

        Tile tile = hexagon.GetComponent<Tile>();
        tile.Initialize(tileTypes[tileType]);
        tiles[x, y] = tile;
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

    private void SetTileNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y].SetNeighbours(GetTileNeighbours(x, y));
            }
        }
    }

    // TODO: Make sure the Water tileType doesn't get added, because it's not travelable
    private List<IAStarNode> GetTileNeighbours(int x, int y)
    {
        List<IAStarNode> neighbours = new List<IAStarNode>();

        // Left
        if (x > 0) neighbours.Add(tiles[x - 1, y]);

        // Right
        if (x < width - 1) neighbours.Add(tiles[x + 1, y]);

        if (y % 2 == 0)
        {
            // Top Left
            if (y > 0 && x > 0) neighbours.Add(tiles[x - 1, y - 1]);

            // Top Right
            if (y > 0) neighbours.Add(tiles[x, y - 1]);

            // Bottom Left
            if (y < height - 1 && x > 0) neighbours.Add(tiles[x - 1, y + 1]);

            // Bottom Right
            if (y < height - 1) neighbours.Add(tiles[x, y + 1]);
        }
        else
        {
            // Top Left
            if (y > 0) neighbours.Add(tiles[x, y - 1]);

            // Top Right
            if (y > 0 && x < width - 1) neighbours.Add(tiles[x + 1, y - 1]);

            // Bottom Left
            if (y < height - 1) neighbours.Add(tiles[x, y + 1]);

            // Bottom Right
            if (y < height - 1 && x < width - 1) neighbours.Add(tiles[x + 1, y + 1]);
        }

        return neighbours;
    }
}
