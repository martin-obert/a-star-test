using UnityEngine;

namespace Grid
{
    public interface IGridCell
    {
        Vector2Int GridPosition { get; set; }
    }
}