using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileType", menuName = "TileData/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public Texture2D texture = null;

    [Header("Travel Cost")]
    [Tooltip("If true, this tile is traversable")]
    public bool canTravel = true;

    [Tooltip("How much does it cost to travel trough this tile")]
    [Min(1f)]
    public uint travelTime = 1;
}
