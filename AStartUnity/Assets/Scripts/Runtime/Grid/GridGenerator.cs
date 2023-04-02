using System.Linq;
using Runtime.Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;

namespace Grid
{
    public static class GridGenerator
    {
        public static IGridCell[] GenerateGrid(int rowCount, int colCount)
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
                        WidthHalf = GridDefinitions.WidthRadius
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
            var result = new[]
            {
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex, currentCell.ColIndex + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex, currentCell.ColIndex - 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex - 1, currentCell.ColIndex),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex - 1, currentCell.ColIndex + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex + 1, currentCell.ColIndex),
                GridCellHelpers.GetCellByCoords(grid, currentCell.RowIndex + 1, currentCell.ColIndex + 1),
            };
            return result.Where(x => x != null).ToArray();
        }
    }
}