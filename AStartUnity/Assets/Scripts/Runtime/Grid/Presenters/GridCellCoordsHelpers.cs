using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public static class GridCellCoordsHelpers
    {
         
        private const float WidthRadius = .5f;
        private const float HeightRadius = .5f;

        public static bool IsCellOdd(int rowIndex) => rowIndex % 2 != 0;

        public static IGridCell GetCellByCoords(IEnumerable<IGridCell> source, int rowIndex, int colIndex)
        {
            return source.FirstOrDefault(x => x.RowIndex == rowIndex && x.ColIndex == colIndex);
        }
        
        public static Vector3 ToWorldCoords(int rowIndex, int colIndex)
        {
            var colShift = 0f;
            const float rowShift = HeightRadius * 2 * .75f;
            
            if (IsCellOdd(rowIndex))
            {
                colShift = WidthRadius;
            }

            var result = new Vector3
            {
                x = colIndex * (WidthRadius * 2) + colShift,
                y = 0,
                z = rowIndex * (HeightRadius * 2) * rowShift
            };

            return result;
        }

        public static IGridCell GetCellPointingAt(Vector2 cursorPosition, IEnumerable<IGridCellSelectable> cells)
        {
            var boxHitCastResults = cells.Where(x => x.IsBoxCastHit(cursorPosition)).ToArray();

            if (boxHitCastResults.Length == 0) return null;
            if (boxHitCastResults.Length <= 1) return boxHitCastResults[0];
            
            var circleCastResult = boxHitCastResults.Where(x => x.IsCircleCastHit(cursorPosition)).ToArray();
            if (circleCastResult.Length == 0)
            {
                throw new Exception("This should not happen");
            }

            return circleCastResult[0];
        }
    }
}