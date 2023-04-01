using Runtime.Grid;
using UnityEngine;

namespace Grid
{
    public static class GridGenerator
    {
        public static IGridCell[] GenerateGrid(int rowCount, int colCount)
        {
            var result = new IGridCell[rowCount * colCount];
            
            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    result[row * colCount + col] = new GridCell
                    {
                        GridPosition = new Vector2Int(col, row)
                    };
                }
            }

            return result;
        }
    }
}