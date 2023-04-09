using System.Collections.Generic;
using System.Linq;
using Runtime.Definitions;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Terrains;

namespace Runtime.Grid
{
    public static class GridCellFactory
    {
        public static IGridCellViewModel Create(int row, int col, ITerrainVariant terrainVariant)
        {
            return new GridCellViewModel
            {
                RowIndex = row,
                ColIndex = col,
                WorldPosition = GridCellHelpers.ToWorldCoords(row, col),
                HeightHalf = GridDefinitions.HeightRadius,
                WidthHalf = GridDefinitions.WidthRadius,
                TerrainVariant = terrainVariant,
                IsWalkable = terrainVariant.IsWalkable,
                TerrainType = terrainVariant.Type,
                DaysTravelCost = terrainVariant.DaysTravelCost
            };
        }
        
        public static IGridCellViewModel Create(int row, int col, TerrainType type, IEnumerable<ITerrainVariant> terrainVariants)
        {
            var terrainVariant = terrainVariants.First(x => x.Type == type);
            return Create(row, col, terrainVariant);
        }
    }
}