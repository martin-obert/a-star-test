using System;
using UnityEngine;

namespace Runtime.Terrains
{
    
    [CreateAssetMenu(menuName = "Terrain Variant", fileName = "TerrainVariant", order = 0)]
    public sealed class TerrainVariant : ScriptableObject, ITerrainVariant
    {
        [SerializeField] private int daysTravelCost = 1;
        [SerializeField] private bool isWalkable = true;
        [SerializeField] private TerrainType type;
        [SerializeField] private Texture2D texture;

        public int DaysTravelCost => daysTravelCost;

        public bool IsWalkable => isWalkable;

        public TerrainType Type => type;
        public Texture TextureOverride => texture;
    }
}