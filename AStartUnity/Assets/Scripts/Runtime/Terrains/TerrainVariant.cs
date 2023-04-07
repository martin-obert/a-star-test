using System;
using UnityEngine;

namespace Runtime.Terrains
{
    public sealed class TerrainVariant : MonoBehaviour, ITerrainVariant
    {
        [SerializeField] private int daysTravelCost = 1;
        [SerializeField] private bool isWalkable = true;
        [SerializeField] private TerrainType type;

        public int DaysTravelCost => daysTravelCost;

        public bool IsWalkable => isWalkable;

        public TerrainType Type => type;
    }
}