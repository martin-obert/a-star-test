using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;

namespace Runtime.Grid
{
    public static class GridGenerator
    {
        public static GridCellSave[] GenerateGridCellDataModels(int rowCount, int colCount)
        {
            if (rowCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, "must be greater than 0");
            if (colCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(colCount), colCount, "must be greater than 0");

            var result = new GridCellSave[rowCount * colCount];
            Parallel.For(0, rowCount,
                row =>
                {
                    Parallel.For(0, colCount,
                        col =>
                        {
                            result[row * colCount + col] =
                                GridCellFactory.Create(row, col, TerrainTypeHelpers.GetRandomTerrainType());
                        });
                });


            return result;
        }

        public static void PopulateNeighbours(IGridCellViewModel[] grid)
        {
            Parallel.ForEach(grid, gridCell =>
            {
                var neighbours = CollectNeighbours(gridCell, grid);
                gridCell.SetNeighbours(neighbours);
            });
        }

        public static IEnumerable<IGridCellViewModel> CollectNeighbours(IGridCellViewModel currentCellViewModel,
            IGridCellViewModel[] grid)
        {
            var shift = currentCellViewModel.IsOddRow ? 0 : -1;
            var result = new[]
            {
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex, currentCellViewModel.ColIndex + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex, currentCellViewModel.ColIndex - 1),
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex - 1,
                    currentCellViewModel.ColIndex + shift),
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex - 1,
                    currentCellViewModel.ColIndex + shift + 1),
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex + 1,
                    currentCellViewModel.ColIndex + shift),
                GridCellHelpers.GetCellByCoords(grid, currentCellViewModel.RowIndex + 1,
                    currentCellViewModel.ColIndex + shift + 1),
            };
            return result.Where(x => x != null).Distinct().ToArray();
        }
    }
}