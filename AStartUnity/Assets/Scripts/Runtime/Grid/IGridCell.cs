using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface IGridCell
    {
        int RowIndex { get; set; }
        int ColIndex { get; set; }
    }
}