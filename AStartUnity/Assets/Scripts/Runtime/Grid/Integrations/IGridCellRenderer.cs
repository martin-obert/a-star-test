using UnityEngine;

namespace Runtime.Grid.Integrations
{
    /// <summary>
    /// Exposes required functionality for grid cell, decouples from Unity API and allows better testing
    /// </summary>
    public interface IGridCellRenderer
    {
        void SetIsHighlighted(bool value);
        void SetIsSelected(bool value);
        void SetMainTexture(Texture texture);
        void SetIsWalkable(bool value);
    }
}