using System;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellTransform
    {
        float IsLifted { get; set; }
    }
    
    internal sealed class GridCellTransform : IGridCellTransform
    {
        private readonly Transform _transform;
        private const float LiftAmount = .5f;
        public GridCellTransform(Transform transform)
        {
            _transform = transform ? transform : throw new ArgumentNullException(nameof(transform));
        }
        

        public float IsLifted { get; set; }
    }
}