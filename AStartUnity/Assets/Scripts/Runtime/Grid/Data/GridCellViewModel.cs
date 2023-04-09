using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using PathFinding;
using Runtime.Grid.Presenters;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Data
{
    /// <summary>
    /// Implementation of a <see cref="IAStarNode"/>
    /// </summary>
    public sealed class GridCellViewModel : IGridCellViewModel
    {
        private bool _isSelected;
        private bool _isHighlighted;
        private bool _isPinned;

        public bool IsPinned
        {
            get => _isPinned;
            private set
            {
                if (value == _isPinned) return;
                _isPinned = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            private set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            private set
            {
                if (value == _isHighlighted) return;
                _isHighlighted = value;

                OnPropertyChanged();
            }
        }


        public void ToggleHighlighted(bool? value = null)
        {
            value ??= !IsHighlighted;

            IsHighlighted = value.Value;
        }

        public void TogglePinned(bool? value = null)
        {
            value ??= !IsPinned;
            IsPinned = value.Value;
        }

        public void ToggleSelected(bool? value = null)
        {
            value ??= !IsSelected;
            IsSelected = value.Value;
        }

        /// <summary>
        /// Row position on the grid
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Column position on the grid
        /// </summary>
        public int ColIndex { get; set; }

        public bool IsOddRow => GridCellHelpers.IsCellOdd(RowIndex);
        public Vector3 WorldPosition { get; set; }
        public float HeightHalf { get; set; }
        public float WidthHalf { get; set; }

        public void SetNeighbours(IEnumerable<IGridCellViewModel> neighbours)
        {
            Neighbours = neighbours;
        }

        public bool IsWalkable { get; set; }
        public TerrainType TerrainType { get; set; }

        public IEnumerable<IAStarNode> Neighbours { get; private set; }
        public ITerrainVariant TerrainVariant { get; set; }


        public float CostTo(IAStarNode neighbour)
        {
            if (neighbour == null)
                throw new ArgumentNullException(nameof(neighbour));
            if (!Neighbours.Contains(neighbour))
                throw new Exception($"Not a neighbour of cell (r: {RowIndex} - c: {ColIndex})");

            // TODO: until we introduce terrains this will be constant
            return IsWalkable ? DaysTravelCost : float.MaxValue;
        }

        public float DaysTravelCost { get; set; }

        public float EstimatedCostTo(IAStarNode target)
        {
            if (target is not IGridCellViewModel gridCell) throw new Exception("Must be a grid cell for pathfinding est.");
            if (!gridCell.IsWalkable)
                return float.MaxValue;
            
            return Math.Abs((gridCell.WorldPosition - WorldPosition).magnitude);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}