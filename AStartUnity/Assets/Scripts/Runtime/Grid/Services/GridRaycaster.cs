using System;
using UnityEngine;

namespace Runtime.Grid.Services
{

    public interface IGridRaycastCamera
    {
        Vector3 Position { get; }
        Vector3 ScreenToWorldPoint(Vector2 mousePosition);
    }

    internal sealed class GridRaycastCamera : IGridRaycastCamera
    {
        private readonly Camera _camera;

        public GridRaycastCamera(Camera camera)
        {
            _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
        }

        public Vector3 Position => _camera.transform.position;
        
        public Vector3 ScreenToWorldPoint(Vector2 mousePosition)
        {
            return _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.nearClipPlane));
        }
    }

    public static class GridRaycaster 
    {
        public static Ray GetRayFromMousePosition(IGridRaycastCamera mainCamera, Vector2 mousePosition)
        {
            if (mainCamera == null)
            {
                throw new NullReferenceException("No main camera");
            }

            var cameraPosition = mainCamera.Position;

            var cursor = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
            var direction = cursor - cameraPosition;
            return new Ray(cameraPosition, direction);
        }

        public static bool TryGetHitOnGrid(Ray ray, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            var plane = new Plane(Vector3.up, Vector3.zero);

            if (!plane.Raycast(ray, out var enter)) return false;
            hitPoint = ray.origin + ray.direction * enter;
            return true;
        }
    }
}