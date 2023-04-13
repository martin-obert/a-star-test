using Runtime.Grid.Models;

namespace Runtime.Grid.Mappers
{
    public static class GridCellMapper
    {
        public static GridCellDataModel ToGridCellSave(IGridCellViewModel value)
        {
            return new GridCellDataModel
            {
                ColIndex = value.ColIndex,
                TerrainType = value.TerrainType,
                RowIndex = value.RowIndex
            };
        }
    }
}