using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellRenderer
    {
        void SetIsHighlighted(bool value);
        void SetIsSelected(bool value);
        void SetMainTexture(Texture texture);
        void SetIsWalkable(bool value);
    }
}