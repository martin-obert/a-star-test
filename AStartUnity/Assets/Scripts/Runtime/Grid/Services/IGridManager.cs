using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public interface IGridManager
    {
        IGridCell HoverCell { get; }
        bool IsPointOnGrid(Vector2 point);
        Vector2 Center { get; }
    }
}