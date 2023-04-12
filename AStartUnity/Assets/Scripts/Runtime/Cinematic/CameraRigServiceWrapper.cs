using System;
using Runtime.Grid.Services;
using Runtime.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Cinematic
{
    /// <summary>
    /// Humble object that wraps around <see cref="CameraRigService"/> and hooks up to
    /// Unity API via <see cref="MonoBehaviour"/> lifecycles.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRigServiceWrapper : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed = 1;
        [SerializeField] private Transform root;

        private CameraRigService _cameraRigService;
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
            _cameraRigService = new CameraRigService(_gridService, _userInputService, root);
           _cameraRigService.Initialize( ServiceInjector.Instance.EventSubscriber);
        }

        private void Update()
        {
            _cameraRigService?.UpdatePosition(cameraSpeed);
        }

        private void OnDestroy()
        {
            _cameraRigService?.Dispose();
        }
    }
}