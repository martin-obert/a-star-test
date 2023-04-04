using UnityEngine;

namespace Runtime.Grid.Services
{
    public interface IGridRaycaster
    {
        Ray GetRayFromMousePosition(Vector2 mousePosition);
        bool TryGetHitOnGrid(Ray ray, out Vector3 point);
    }
}