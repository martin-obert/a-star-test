using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public sealed class GridCellPresenter : MonoBehaviour
    {
        private IGridCell _cell;

        public void SetDataModel(IGridCell cell)
        {
            _cell = cell;
            name = $"row: {cell.RowIndex}, col: {cell.ColIndex}";
            transform.position = cell.WorldPosition;

        }

        private Bounds _bounds;

        public bool IsBoxCastHit(Vector2 cursor)
        {
            var position = transform.position;
            var v = cursor.y <= position.z + _cell.HeightHalf &&
                    cursor.y >= position.z - _cell.HeightHalf;

            var h = cursor.x <= position.x + _cell.WidthHalf &&
                    cursor.x >= position.x - _cell.WidthHalf;

            return v && h;
        }

        public bool IsCircleCastHit(Vector2 cursor)
        {
            var position = transform.position;

            return IsPointInsideEllipse(cursor.x, cursor.y, position.x, position.z, _cell.WidthHalf,
                _cell.HeightHalf);
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