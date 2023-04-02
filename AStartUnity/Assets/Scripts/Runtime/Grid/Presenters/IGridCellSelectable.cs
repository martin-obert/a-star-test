using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellSelectable
    {
        bool IsBoxCastHit(Vector2 cursor);
        bool IsCircleCastHit(Vector2 cursor);
    }
}