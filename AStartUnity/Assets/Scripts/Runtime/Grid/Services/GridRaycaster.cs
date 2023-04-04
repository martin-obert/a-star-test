using System;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public sealed class GridRaycaster : IGridRaycaster
    {
        private Camera _mainCamera;

        public Ray GetRayFromMousePosition(Vector2 mousePosition)
        {
            if (!_mainCamera)
            {
                _mainCamera = Camera.main;
            }

            if (!_mainCamera)
            {
                throw new NullReferenceException("No main camera");
            }

            var cameraPosition = _mainCamera.transform.position;

            var cursor = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
                _mainCamera.nearClipPlane));
            var direction = cursor - cameraPosition;
            return new Ray(cameraPosition, direction);
        }

        public bool TryGetHitOnGrid(Ray ray, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            var plane = new Plane(Vector3.up, Vector3.zero);

            if (!plane.Raycast(ray, out var enter)) return false;
            hitPoint = ray.origin + ray.direction * enter;
            return true;
        }
    }
}