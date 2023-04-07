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
        public static IGridCell Create(int row, int col, ITerrainVariant terrainVariant)
        {
            return new GridCell
            {
                RowIndex = row,
                ColIndex = col,
                WorldPosition = GridCellHelpers.ToWorldCoords(row, col),
                HeightHalf = GridDefinitions.HeightRadius,
                WidthHalf = GridDefinitions.WidthRadius,
                TerrainVariant = terrainVariant,
                IsWalkable = terrainVariant.IsWalkable
            };
        }
        
        public static IGridCell Create(int row, int col, TerrainType type, IEnumerable<ITerrainVariant> terrainVariants)
        {
            var terrainVariant = terrainVariants.First(x => x.Type == type);
            return new GridCell
            {
                RowIndex = row,
                ColIndex = col,
                WorldPosition = GridCellHelpers.ToWorldCoords(row, col),
                HeightHalf = GridDefinitions.HeightRadius,
                WidthHalf = GridDefinitions.WidthRadius,
                TerrainVariant = terrainVariant,
                IsWalkable = terrainVariant.IsWalkable
            };
        }
    }
}