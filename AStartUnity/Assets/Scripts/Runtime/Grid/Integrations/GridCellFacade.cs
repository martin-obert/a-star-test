using System;
using Runtime.Grid.Models;
using UnityEngine;

namespace Runtime.Grid.Integrations
{
    /// <summary>
    /// Exposes the <see cref="IGridCellViewModel"/> to Unity API and editor
    /// </summary>
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