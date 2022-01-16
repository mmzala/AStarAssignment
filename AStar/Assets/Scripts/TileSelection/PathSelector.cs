using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;

public class PathSelector : MonoBehaviour
{
    [Header("Path Selecting")]
    public new Camera camera = null;
    public LayerMask selectionMask = 1;

    [Min(10f)]
    public float maxRayDistance = 10f;

    [Header("Path Colors")]
    [SerializeField]
    private Color pathColor = Color.blue;

    [SerializeField]
    private Color startingTileColor = Color.green;

    [SerializeField]
    private Color endingTileColor = Color.red;

    private readonly Color defaultColor = Color.white;

    // Path variables
    private IList<IAStarNode> path;
    private Tile startingTile = null;
    private Tile endingTile = null;

    private void Update()
    {
        // Select a tile when clicked left/right mouse button
        if (Input.GetMouseButtonDown(0)) SelectTile(ref startingTile, startingTileColor);
        if (Input.GetMouseButtonDown(1)) SelectTile(ref endingTile, endingTileColor);
    }

    #region TileSelection
    /// <summary>
    /// Selects tile by setting it's color and tries to show the path
    /// </summary>
    /// <param name="tile"> What tile to select </param>
    /// <param name="color"> With what color to highlight the tile </param>
    private void SelectTile(ref Tile tile, Color color)
    {
        // Get new tile, if it is a tile already in use or is null, then return
        Tile newTile = GetTileInWorld();
        if (newTile == startingTile || newTile == endingTile || newTile == null) return;

        // If the tile is traversable, update the tile
        if (newTile.tileType.canTravel)
        {
            ClearPathColors();

            // Reset the color of the last selected tile
            if (tile) tile.SetTileColor(defaultColor);

            tile = newTile;
            tile.SetTileColor(color);

            ShowPath();
        }
    }

    /// <summary>
    /// Casts a ray to get the tile component of the object you clicked on
    /// </summary>
    /// <returns> Tile component of object the user clicked on </returns>
    private Tile GetTileInWorld()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxRayDistance, selectionMask))
        {
            return hit.transform.GetComponent<Tile>();
        }

        return null;
    }
    #endregion // TileSelection

    #region PathVisualization
    /// <summary>
    /// Gets the path from the AStar class and shows it by changing the color of the tiles in the path
    /// </summary>
    private void ShowPath()
    {
        if (!startingTile || !endingTile) return;

        path = AStar.GetPath(startingTile, endingTile);
        if (path == null) return; // If path not found, then return

        // Change color of all tiles in path, but skip the first and last tile, because they have different colors
        for (int i = 1; i < path.Count - 1; i++)
        {
            Tile tile = path[i] as Tile;
            tile.SetTileColor(pathColor);
        }
    }

    /// <summary>
    /// Clears the old path by changing the tiles to default color
    /// </summary>
    private void ClearPathColors()
    {
        if (path == null) return;

        // Change color of all tiles in path, but skip the first and last tile, because they have different colors
        for (int i = 1; i < path.Count - 1; i++)
        {
            Tile tile = path[i] as Tile;
            tile.SetTileColor(defaultColor);
        }
    }
    #endregion // PathVisualization
}
