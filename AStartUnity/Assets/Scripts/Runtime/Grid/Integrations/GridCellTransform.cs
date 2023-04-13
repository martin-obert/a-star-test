using System;
using UnityEngine;

namespace Runtime.Grid.Integrations
{
    public sealed class GridCellTransform : IGridCellTransform
    {
        private readonly Transform _transform;

        public GridCellTransform(Transform transform)
        {
            _transform = transform ? transform : throw new ArgumentNullException(nameof(transform));
        }

        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }
    }
}