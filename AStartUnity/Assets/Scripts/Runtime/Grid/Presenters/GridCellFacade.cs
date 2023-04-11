using System;
using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Renderer))]
    public class GridCellFacade : MonoBehaviour
    {
        public IGridCellViewModel ViewModel { get; set; }

        public void SetViewModel(IGridCellViewModel value)
        {
            if (ViewModel != null) throw new Exception("ViewModel already set");

            ViewModel = value;

            name = $"row: {value.RowIndex}, col: {value.ColIndex}";
            transform.position = value.WorldPosition;
        }
    }
}