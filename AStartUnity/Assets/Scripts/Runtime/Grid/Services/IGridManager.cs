using System.Collections.Generic;
using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public interface IGridService
    {
        bool IsPointOnGrid(Vector2 point);
        Vector2 Center { get; }
        IEnumerable<IGridCell> Cells { get; }
    }
}