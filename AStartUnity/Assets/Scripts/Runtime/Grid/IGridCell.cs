using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface IGridCell
    {
        Vector2Int GridPosition { get; set; }
    }
}