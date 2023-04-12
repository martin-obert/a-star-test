using System;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Messaging;
using UniRx;
using UnityEngine;

namespace Runtime.Cinematic
{
    /// <summary>
    /// Service that manipulates with player POV camera and keeps it within the grid bounds
    /// </summary>
    public class CameraRigService : IDisposable
    {
        private readonly IGridService _gridService;
        private readonly IUserInputService _userInputService;
        private readonly Transform _cameraRoot;
        private readonly CompositeDisposable _disposable = new();

        public CameraRigService(
            IGridService gridService, 
            IUserInputService userInputService,
            Transform cameraRoot)
        {
            _gridService = gridService;
            _userInputService = userInputService;
            _cameraRoot = cameraRoot;
        }
        
        public void Initialize(EventSubscriber eventSubscriber)
        {
            eventSubscriber.OnGridInstantiated()
                .Where(_ => _gridService != null)
                .Subscribe(_ =>
                {
                    var center = _gridService.Center;
                    _cameraRoot.position = new Vector3(center.x, 0, center.y);
                }).AddTo(_disposable);
        }

        public void UpdatePosition(
            float cameraSpeed)
        {
            var currentPosition = _cameraRoot.position;
            var movementVector = _userInputService.AxisMovementVector;
            if (movementVector == Vector3.zero) return;

            var newPosition = currentPosition + movementVector * (cameraSpeed * Time.deltaTime);

            if (!_gridService.IsPointOnGrid(new Vector2(newPosition.x, _cameraRoot.position.z)))
            {
                newPosition.x = _cameraRoot.position.x;
            }

            if (!_gridService.IsPointOnGrid(new Vector2(_cameraRoot.position.x, newPosition.z)))
            {
                newPosition.z = _cameraRoot.position.z;
            }

            _cameraRoot.position = newPosition;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}