using System;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public static class GridRaycaster 
    {
        public static Ray GetRayFromMousePosition(Camera mainCamera, Vector2 mousePosition)
        {
            if (!mainCamera)
            {
                throw new NullReferenceException("No main camera");
            }

            var cameraPosition = mainCamera.transform.position;

            var cursor = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
                mainCamera.nearClipPlane));
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