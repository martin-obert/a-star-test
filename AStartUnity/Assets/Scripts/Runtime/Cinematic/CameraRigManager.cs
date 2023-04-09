using System;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Services;
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

        private readonly CameraRigService _cameraRigService = new();
        private IUserInputService _userInputService;
        private IGridService _gridService;
        private void Awake()
        {
            Assert.IsNotNull(root, "root != null");
        }

        private void Start()
        {
            _gridService = ServiceInjector.Instance.GridService;
            _userInputService = ServiceInjector.Instance.UserInputService;

            ServiceInjector.Instance.EventSubscriber
                .OnGridInstantiated()
                .Where(_ => _gridService != null)
                .Subscribe(_ =>
                {
                    var center = _gridService.Center;
                    root.transform.position = new Vector3(center.x, 0, center.y);
                }).AddTo(this);
        }

        private void Update()
        {
            _cameraRigService.UpdatePosition(root, cameraSpeed, _userInputService.AxisMovementVector, _gridService.IsPointOnGrid);
        }
    }
}