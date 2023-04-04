using UnityEngine;

namespace Runtime.Terrains
{
    public interface ITerrainVariant
    {
        Texture2D ColorTexture { get; }

        int DaysTravelCost { get; }
        
        bool IsWalkable { get; }
    }
}