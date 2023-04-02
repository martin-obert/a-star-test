
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellHoverable
    {
        bool CanHover { get; }
        void OnCursorEnter();
        void OnCursorExit();

        bool IsBoxCastHit(Vector2 cursor);
        bool IsCircleCastHit(Vector2 cursor);
    }
}