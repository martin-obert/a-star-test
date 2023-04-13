using System;
using Runtime.Grid.Integrations;
using UnityEngine;

namespace Runtime.Grid.Services
{
    /// <summary>
    /// Static methods for getting raycast hit on the plane that should be on same level as the grid
    /// </summary>
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