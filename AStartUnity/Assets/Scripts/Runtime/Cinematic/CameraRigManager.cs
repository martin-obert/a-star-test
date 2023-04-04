using Runtime.Grid.Services;
using Runtime.Inputs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Cinematic
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRigManager : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private float cameraSpeed = 1;
        [SerializeField] private Transform root;

        private void Awake()
        {
            Assert.IsNotNull(root, "root != null");
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            var currentPosition = root.position;

            var movementVector = UserInputManager.Instance.AxisMovement;

            if (movementVector != Vector3.zero)
            {
                var newPosition = currentPosition + movementVector * (cameraSpeed * Time.deltaTime);
                if (!GridManager.Instance.IsPointOnGrid(new Vector2(newPosition.x, root.transform.position.z)))
                {
                    newPosition.x = root.position.x;
                }
                if (!GridManager.Instance.IsPointOnGrid(new Vector2(root.position.x, newPosition.z)))
                {
                    newPosition.z = root.position.z;
                }

                root.position = newPosition;
            }
        }
    }
}