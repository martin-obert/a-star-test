using System;
using UnityEngine;

namespace Runtime.Terrains
{
    [Serializable]
    public sealed class TerrainVariant : ITerrainVariant
    {
        [SerializeField] private Texture2D colorTexture;

        [SerializeField] private int daysTravelCost = 1;

        public Texture2D ColorTexture => colorTexture;

        public int DaysTravelCost => daysTravelCost;
    }
}