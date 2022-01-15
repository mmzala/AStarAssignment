using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileType", menuName = "TileData/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public Texture2D texture = null;

    [Header("Travel Cost")]
    [Tooltip("If true, you can travel trough this tile type")]
    public bool canTravel = true;

    [Min(1f)]
    public uint travelTime = 1;
}
