using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellSelectable : IGridCell
    {
        bool IsBoxCastHit(Vector2 cursor);
        bool IsCircleCastHit(Vector2 cursor);
    }
}