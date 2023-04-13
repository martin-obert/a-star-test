using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.Definitions;
using Runtime.Grid.Models;
using UnityEngine;

namespace Runtime.Grid
{
    public static class GridCellHelpers
    {
        public static bool IsCellOdd(int rowIndex) => rowIndex % 2 != 0;

        public static IGridCellViewModel GetCellByCoords(IEnumerable<IGridCellViewModel> source, int rowIndex, int colIndex)
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

        public static IGridCellViewModel GetCellByWorldPoint(Vector2 cursorPosition,
            IEnumerable<IGridCellViewModel> cells)
        {
            var gridCells = cells.ToArray();
            var result = new ConcurrentBag<IGridCellViewModel>();
            Parallel.ForEach(gridCells, c =>
            {
                if (!IsBoxCastHit(c, cursorPosition)) return;
                result.Add(c);
            });
            if (result.IsEmpty) return null;
            if (result.Count == 1)
            {
                return result.First();
            }

            var circleCastResult = result.Where(x => IsCircleCastHit(x, cursorPosition)).ToArray();
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


        public static bool IsBoxCastHit(IGridCellViewModel cellViewModel, Vector2 cursor)
        {
            var isBoxCastHit = cellViewModel.Bounds.Contains(cursor);
            return isBoxCastHit;
        }

        public static bool IsCircleCastHit(IGridCellViewModel cellViewModel, Vector2 cursor)
        {
            var position = cellViewModel.WorldPosition;
            return IsPointInsideEllipse(cursor.x, cursor.y, position.x, position.z, cellViewModel.WidthHalf,
                cellViewModel.HeightHalf);
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