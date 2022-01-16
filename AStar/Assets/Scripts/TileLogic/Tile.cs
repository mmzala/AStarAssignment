using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;

public class Tile : MonoBehaviour, IAStarNode
{
    public TileType tileType { get; private set; }
    private new Renderer renderer;

    /// <summary>
    /// Sets the tile type and applies the texture
    /// </summary>
    /// <param name="tileType"> Tile settings </param>
    public void Initialize(TileType tileType)
    {
        this.tileType = tileType;
        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", tileType.texture);
    }

    #region Setters
    public void SetNeighbours(List<IAStarNode> neighbours)
    {
        Neighbours = neighbours;
    }

    public void SetTileColor(Color color)
    {
        renderer.material.color = color;
    }
    #endregion // Setters

    #region AStar
    public IEnumerable<IAStarNode> Neighbours { get; private set; }

    /// <summary>
    /// Gets the travel time to a neighbour tile
    /// </summary>
    /// <param name="neighbour"> Tile  </param>
    /// <returns> Travel time to a neighbour tile </returns>
    public float CostTo(IAStarNode neighbour)
    {
        Tile neighbourTile = neighbour as Tile;
        return neighbourTile.tileType.travelTime;
    }

    /// <summary>
    /// Gets estimated travel cost to the given goal
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    public float EstimatedCostTo(IAStarNode goal)
    {
        Tile goalTile = goal as Tile;
        Vector3 direction = goalTile.transform.position - transform.position;
        return direction.magnitude;
    }
    #endregion // AStar
}
