using System.Linq;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Terrains;

namespace Runtime.Grid
{
    public static class GridGenerator
    {
        public static IGridCell[] GenerateGrid(int rowCount, int colCount, ITerrainVariantRepository repository)
        {
            // TODO: check args for negative values

            var result = new IGridCell[rowCount * colCount];

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    result[row * colCount + col] = new GridCell
                    {
                        RowIndex = row,
                        ColIndex = col,
                        WorldPosition = GridCellHelpers.ToWorldCoords(row, col),
                        HeightHalf = GridDefinitions.HeightRadius,
                        WidthHalf = GridDefinitions.WidthRadius,
                        TerrainVariant = repository.GetRandomTerrainVariant(row, col)
                    };
                }
            }

            foreach (var gridCell in result)
            {
                var neighbours = CollectNeighbours(gridCell, result);
                gridCell.SetNeighbours(neighbours);
            }

            return result;
        }

        public static IGridCell[] CollectNeighbours(IGridCell currentCell, IGridCell[] grid)
        {
            var shift = currentCell.IsOddRow ? 0 : -1;
            var result = new[]
            {
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex, currentCell.ColIndex + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex, currentCell.ColIndex - 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex - 1,
                    currentCell.ColIndex + shift),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex - 1,
                    currentCell.ColIndex + shift + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex + 1,
                    currentCell.ColIndex + shift),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex + 1,
                    currentCell.ColIndex + shift + 1),
            };
            return result.Where(x => x != null).Distinct().ToArray();
        }
    }
}