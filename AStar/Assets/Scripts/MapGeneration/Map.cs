using System.Collections;
using System.Collections.Generic;
using System;
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

    #region MapCreation
    public void CreateMap()
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
    #endregion // MapCreation

    #region NeighbourSelection
    /// <summary>
    /// Sets neighbours for all tiles in the map
    /// </summary>
    private void SetTileNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // There os no need to give the non treversable tiles neighbours
                if (!tiles[x, y].tileType.canTravel) continue;

                tiles[x, y].SetNeighbours(GetTileNeighbours(x, y));
            }
        }
    }

    /// <summary>
    /// Gets all neighbour tiles that are traversable of the tile selected by given coordinates
    /// </summary>
    /// <param name="x"> X position of the tile in the map </param>
    /// <param name="y"> Y position of the tile in the ma </param>
    /// <returns> All neighbour tiles that are traversable </returns>
    private List<IAStarNode> GetTileNeighbours(int x, int y)
    {
        List<IAStarNode> neighbours = new List<IAStarNode>();
        // This allows to add neighbours, only when they are treversable
        Action<Tile> Add = neighbour => { if (neighbour.tileType.canTravel) neighbours.Add(neighbour); };

        // Left tile
        if (x > 0) Add(tiles[x - 1, y]);

        // Right tile
        if (x < width - 1) Add(tiles[x + 1, y]);

        // Is even
        if (y % 2 == 0)
        {
            // Top Left tile
            if (y > 0 && x > 0) Add(tiles[x - 1, y - 1]);

            // Top Right tile
            if (y > 0) Add(tiles[x, y - 1]);

            // Bottom Left tile
            if (y < height - 1 && x > 0) Add(tiles[x - 1, y + 1]);

            // Bottom Right tile
            if (y < height - 1) Add(tiles[x, y + 1]);
        }
        // Not even
        else
        {
            // Top Left tile
            if (y > 0) Add(tiles[x, y - 1]);

            // Top Right tile
            if (y > 0 && x < width - 1) Add(tiles[x + 1, y - 1]);

            // Bottom Left tile
            if (y < height - 1) Add(tiles[x, y + 1]);

            // Bottom Right tile
            if (y < height - 1 && x < width - 1) Add(tiles[x + 1, y + 1]);
        }

        return neighbours;
    }
    #endregion // NeighbourSelection
}
