using Runtime.DependencyInjection;
using Runtime.Framework.Services;
using Runtime.Grid.Services;
using Runtime.Inputs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Camera.Integrations
{
    /// <summary>
    /// Humble object that wraps around <see cref="CameraRigService"/> and hooks up to
    /// Unity API via <see cref="MonoBehaviour"/> lifecycles.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
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
            _cameraRigService = new CameraRigService(_gridService, root);
            _cameraRigService.Initialize(ServiceInjector.Instance.EventSubscriber);
        }

        private void Update()
        {
            _cameraRigService?.UpdatePosition(_userInputService.AxisMovementVector, cameraSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            _cameraRigService?.Dispose();
        }
    }
}