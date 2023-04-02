﻿using Runtime.Grid;
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
                        WorldPosition = GridCellCoordsHelpers.ToWorldCoords(row, col)
                    };
                }
            }

            return result;
        }
    }
}