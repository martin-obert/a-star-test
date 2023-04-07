using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Terrains;

namespace Runtime.Grid
{
    public static class GridGenerator
    {
        public static IGridCell[] GenerateGrid(int rowCount, int colCount, ITerrainVariantRepository terrainVariantRepository)
        {
            if (rowCount <= 0) throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, "must be greater than 0");
            if (colCount <= 0) throw new ArgumentOutOfRangeException(nameof(colCount), colCount, "must be greater than 0");

            var result = new IGridCell[rowCount * colCount];

            Parallel.For(0, rowCount, row =>
            {
                Parallel.For(0, colCount, col =>
                {
                    var terrainVariant = terrainVariantRepository.GetRandomTerrainVariant();
                    result[row * colCount + col] = GridCellFactory.Create(row, col, terrainVariant);
                });
            });

           
            return result;
        }

        public static void PopulateNeighbours(IGridCell[] grid)
        {
            Parallel.ForEach(grid, gridCell =>
            {
                var neighbours = GridGenerator.CollectNeighbours(gridCell, grid);
                gridCell.SetNeighbours(neighbours);
            });

        }

        public static IEnumerable<IGridCell> CollectNeighbours(IGridCell currentCell, IGridCell[] grid)
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