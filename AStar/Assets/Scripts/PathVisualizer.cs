using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
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

    private Tile startingTile = null;
    private Tile endingTile = null;

    private void Update()
    {
        // Select a tile when clicked one of th emouse buttons
        if (Input.GetMouseButtonDown(0)) SelectTile(ref startingTile, startingTileColor);
        if (Input.GetMouseButtonDown(1)) SelectTile(ref endingTile, endingTileColor);
    }

    private void SelectTile(ref Tile tile, Color color)
    {
        // Get new tile and if the can is traversable, update the tile
        Tile newTile = GetTileInWorld();
        if(newTile.tileType.canTravel)
        {
            // Reset the color of the last selected tile
            if (tile) tile.SetTileColor(Color.white);

            tile = newTile;
            tile.SetTileColor(color);
        }
    }

    /// <summary>
    /// Casts a ray to get the tile component of the object you clicked on
    /// </summary>
    /// <returns> Tile component of selected object </returns>
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
}
