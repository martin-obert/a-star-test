using System;
using UnityEngine;

namespace Runtime.Grid.Integrations
{
    public sealed class GridRaycastCamera : IGridRaycastCamera
    {
        private readonly UnityEngine.Camera _camera;

        public GridRaycastCamera(UnityEngine.Camera camera)
        {
            _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
        }

        public Vector3 Position => _camera.transform.position;
        
        public Vector3 ScreenToWorldPoint(Vector2 mousePosition)
        {
            return _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.nearClipPlane));
        }
    }
}