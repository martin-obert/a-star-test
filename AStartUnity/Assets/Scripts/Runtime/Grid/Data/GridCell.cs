using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using PathFinding;
using Runtime.Grid.Presenters;
using UnityEngine;

namespace Runtime.Grid.Data
{
    /// <summary>
    /// Implementation of a <see cref="IAStarNode"/>
    /// </summary>
    public sealed class GridCell : IGridCell
    {
        private bool _isSelected;
        private bool _isHighlighted;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public void ToggleSelected(bool? value = null)
        {
            if (value.HasValue)
            {
                IsSelected = value.Value;
                return;
            }

            IsSelected = !IsSelected;
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

        public void ToggleHighlighted(bool? value = null, bool includeNeighbours = false)
        {
            value ??= !IsHighlighted;
            IsHighlighted = value.Value;
            if (!includeNeighbours) return;
            
            foreach (var neighbour in Neighbours.OfType<IGridCell>())
            {
                neighbour.ToggleHighlighted(value);
            }
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

        public void SetNeighbours(IGridCell[] neighbours)
        {
            Neighbours = neighbours;
        }

        public IEnumerable<IAStarNode> Neighbours { get; private set; }


        public float CostTo(IAStarNode neighbour)
        {
            throw new System.NotImplementedException();
        }

        public float EstimatedCostTo(IAStarNode target)
        {
            throw new System.NotImplementedException();
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