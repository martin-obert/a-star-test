using System.Collections.Generic;
using System.ComponentModel;
using PathFinding;
using UnityEngine;

namespace Runtime.Grid.Models
{
    /// <summary>
    /// Observable for outer objects, that exposes available operations over the Grid Cell
    /// </summary>
    public interface IGridCellViewModel : IAStarNode, INotifyPropertyChanged
    {
        bool IsSelected { get; }
      
        bool IsPinned { get; }
        
        void ToggleSelected(bool? value = null);
        
        bool IsHighlighted { get; }
        
        void ToggleHighlighted(bool? value = null);
        
        void TogglePinned(bool? value = null);
        
        int RowIndex { get; }
        
        int ColIndex { get; }
        
        /// <summary>
        /// Is on the shifted row
        /// </summary>
        bool IsOddRow { get; }
        Vector3 WorldPosition { get; }
        
        /// <summary>
        /// Rectangle around the cell for cursor collision
        /// </summary>
        Rect Bounds { get; }

        public float HeightHalf { get; }
        
        public float WidthHalf { get; }
        
        void SetNeighbours(IEnumerable<IGridCellViewModel> neighbours);
        
        bool IsWalkable { get; }
        
        TerrainType TerrainType { get; }
    }
}