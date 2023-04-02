using UnityEngine;

namespace Runtime.Grid.Services
{
    public interface IGridRaycaster
    {
        Ray GetRayFromMousePosition();
        bool TryGetHitOnGrid(Ray ray, out Vector3 point);
    }
}