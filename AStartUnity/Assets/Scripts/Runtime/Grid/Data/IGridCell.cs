using System.ComponentModel;
using PathFinding;
using UnityEngine;

namespace Runtime.Grid.Data
{
    public interface IGridCell : IAStarNode, INotifyPropertyChanged
    {
        bool IsSelected { get; }
        void ToggleSelected(bool? value = null);
        bool IsHighlighted { get; }
        void ToggleHighlighted(bool? value = null, bool includeNeighbours = false);
        int RowIndex { get; }
        int ColIndex { get; }
        bool IsOddRow { get; }
        Vector3 WorldPosition { get; }
        public float HeightHalf { get; }
        public float WidthHalf { get; }
        void SetNeighbours(IGridCell[] neighbours);
    }
}