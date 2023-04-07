using System.Collections.Generic;
using System.Linq;
using Runtime.Definitions;
using Runtime.Grid.Data;
using Runtime.Grid.Services;
using Runtime.Terrains;

namespace Runtime.Grid.Mappers
{
    public static class GridCellMapper
    {
        public static IGridCell GridCellFromSave(GridCellSave value, IEnumerable<ITerrainVariant> terrainVariants)
        {
            return GridCellFactory.Create(value.RowIndex, value.ColIndex,
                terrainVariants.First(x => x.Type == value.TerrainType));
        }

        public static GridCellSave ToGridCellSave(IGridCell value)
        {
            return new GridCellSave
            {
                ColIndex = value.ColIndex,
                TerrainType = value.TerrainType,
                RowIndex = value.RowIndex
            };
        }
    }
}