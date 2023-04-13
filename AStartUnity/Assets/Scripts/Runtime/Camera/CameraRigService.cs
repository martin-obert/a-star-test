using System;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Messaging;
using UniRx;
using UnityEngine;

namespace Runtime.Framework.Services
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridService"><see cref="IGridService"/></param>
        /// <param name="cameraRoot">Transform that will be moved.</param>
        public CameraRigService(
            IGridService gridService,
            Transform cameraRoot)
        {
            _gridService = gridService;
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

        /// <summary>
        /// <para>
        /// Move with camera in the direction of the <see cref="movementVector"/> for the amount of <see cref="cameraSpeed"/>.
        /// </para>
        /// <para>
        /// Camera position is clipped into the bounds of a grid.</para>
        /// </summary>
        /// <param name="movementVector">Direction of movement</param>
        /// <param name="cameraSpeed">Movement amount</param>
        public void UpdatePosition(Vector3 movementVector,
            float cameraSpeed)
        {
            if (movementVector == Vector3.zero || cameraSpeed == 0) return;
            
            var currentPosition = _cameraRoot.position;

            var newPosition = currentPosition + movementVector * cameraSpeed;

            // Do check for separate axis for grid "edge sliding" movement
            
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