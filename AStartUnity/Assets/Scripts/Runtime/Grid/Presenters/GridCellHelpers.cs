using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Definitions;
using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public static class GridCellHelpers
    {
        public static bool IsCellOdd(int rowIndex) => rowIndex % 2 != 0;

        public static IGridCell GetCellByCoords(IEnumerable<IGridCell> source, int rowIndex, int colIndex)
        {
            return source.FirstOrDefault(x => x.RowIndex == rowIndex && x.ColIndex == colIndex);
        }

        public static Vector3 ToWorldCoords(int rowIndex, int colIndex)
        {
            var colShift = 0f;
            const float rowShift = GridDefinitions.HeightRadius * 2 * .75f;

            if (IsCellOdd(rowIndex))
            {
                colShift = GridDefinitions.WidthRadius;
            }

            var result = new Vector3
            {
                x = colIndex * (GridDefinitions.WidthRadius * 2) + colShift,
                y = 0,
                z = rowIndex * (GridDefinitions.HeightRadius * 2) * rowShift
            };

            return result;
        }

        public static IGridCell GetCellByWorldPoint(Vector2 cursorPosition,
            IEnumerable<IGridCell> cells)
        {
            var boxHitCastResults = cells.Where(x => IsBoxCastHit(x, cursorPosition)).ToArray();

            if (boxHitCastResults.Length == 0) return null;
            if (boxHitCastResults.Length <= 1) return boxHitCastResults[0];

            var circleCastResult = boxHitCastResults.Where(x => IsCircleCastHit(x, cursorPosition)).ToArray();
            if (circleCastResult.Length == 1)
            {
                return circleCastResult[0];
            }

            if (circleCastResult.Length > 1)
            {
                // TODO: hexagon shape cast result
            }
            return null;

        }


        public static bool IsBoxCastHit(IGridCell cell, Vector2 cursor)
        {
            var position = cell.WorldPosition;
            var v = cursor.y <= position.z + cell.HeightHalf &&
                    cursor.y >= position.z - cell.HeightHalf;

            var h = cursor.x <= position.x + cell.WidthHalf &&
                    cursor.x >= position.x - cell.WidthHalf;

            return v && h;
        }

        public static bool IsCircleCastHit(IGridCell cell, Vector2 cursor)
        {
            var position = cell.WorldPosition;
            return IsPointInsideEllipse(cursor.x, cursor.y, position.x, position.z, cell.WidthHalf,
                cell.HeightHalf);
        }

        private static bool IsPointInsideEllipse(double x, double y, double cx, double cy, double a, double b)
        {
            double dx = x - cx;
            double dy = y - cy;
            double value = (dx * dx) / (a * a) + (dy * dy) / (b * b);
            return value <= 1;
        }
    }
}