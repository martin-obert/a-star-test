using System;
using Runtime.Grid.Services;
using UnityEngine;

namespace Runtime.Cinematic
{
    public  class CameraRigService
    {
        public void UpdatePosition(
            Transform root,
            float cameraSpeed,
            Vector3 movementVector,
            Func<Vector2, bool> isPointOnGrid)
        {
            var currentPosition = root.position;

            if (movementVector == Vector3.zero) return;

            var newPosition = currentPosition + movementVector * (cameraSpeed * Time.deltaTime);

            if (!isPointOnGrid(new Vector2(newPosition.x, root.transform.position.z)))
            {
                newPosition.x = root.position.x;
            }

            if (!isPointOnGrid(new Vector2(root.position.x, newPosition.z)))
            {
                newPosition.z = root.position.z;
            }

            root.position = newPosition;
        }
    }
}