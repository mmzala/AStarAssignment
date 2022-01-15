using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;

public class Tile : MonoBehaviour, IAStarNode
{
    public TileType tileType { get; private set; }
    private new Renderer renderer;

    public void Initialize(TileType tileType)
    {
        this.tileType = tileType;
        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", tileType.texture);
    }

    public void SetNeighbours(List<IAStarNode> neighbours)
    {
        Neighbours = neighbours;
    }

    #region AStar
    public IEnumerable<IAStarNode> Neighbours { get; private set; }

    public float CostTo(IAStarNode neighbour)
    {
        return 1f;
    }

    public float EstimatedCostTo(IAStarNode goal)
    {
        return 1f;
    }
    #endregion // AStar
}
