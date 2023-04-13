using PathFinding;
using UnityEngine;

namespace Runtime.Grid.Models
{
    /// <summary>
    /// Type of terrain that will be rendered and taken into algorithm by <see cref="AStar.GetPath"/>
    /// </summary>
    public interface ITerrainVariant
    {
        int DaysTravelCost { get; }

        bool IsWalkable { get; }
        
        TerrainType Type { get; }
        
        Texture TextureOverride { get; }
    }
}