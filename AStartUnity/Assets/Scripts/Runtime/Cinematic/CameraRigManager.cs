using System;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Messaging;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Cinematic
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRigManager : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed = 1;
        [SerializeField] private Transform root;

        private IGridManager _gridManager;
        private IUserInputManager _userInputManager;
        
        private void Awake()
        {
            Assert.IsNotNull(root, "root != null");
        }

        private void Start()
        {
            _gridManager = UnitOfWork.Instance.GridManager;
            _userInputManager = UnitOfWork.Instance.UserInputManager;
            
            UnitOfWork.Instance.EventSubscriber
                .OnGridInstantiated()
                .Where(_ => _gridManager != null)
                .Subscribe(x =>
                {
                    var center = UnitOfWork.Instance.GridManager.Center;
                    root.transform.position = new Vector3(center.x, 0, center.y);
                }).AddTo(this);
        }

        private void Update()
        {
            var currentPosition = root.position;

            var movementVector = _userInputManager.AxisMovementVector;

            if (movementVector == Vector3.zero) return;
            
            var newPosition = currentPosition + movementVector * (cameraSpeed * Time.deltaTime);
            
            if (!_gridManager.IsPointOnGrid(new Vector2(newPosition.x, root.transform.position.z)))
            {
                newPosition.x = root.position.x;
            }

            if (!_gridManager.IsPointOnGrid(new Vector2(root.position.x, newPosition.z)))
            {
                newPosition.z = root.position.z;
            }

            root.position = newPosition;
        }
    }
}