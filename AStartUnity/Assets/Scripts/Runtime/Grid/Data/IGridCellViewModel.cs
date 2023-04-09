using System.Collections.Generic;
using System.ComponentModel;
using PathFinding;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Data
{
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
        bool IsOddRow { get; }
        Vector3 WorldPosition { get; }
        public float HeightHalf { get; }
        public float WidthHalf { get; }
        void SetNeighbours(IEnumerable<IGridCellViewModel> neighbours);
        bool IsWalkable { get; }
        TerrainType TerrainType { get; }
    }
}