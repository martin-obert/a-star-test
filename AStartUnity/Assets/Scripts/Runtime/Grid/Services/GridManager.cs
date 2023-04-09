using System;
using Cysharp.Threading.Tasks;
using Runtime.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    
    public sealed class GridManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;

        private IDisposable _serviceRegistrationHook;
        private GridService _gridService;
        private IUserInputService _userInputService;

        private IGridRaycastCamera _mainCamera;

        private void Awake()
        {
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");

            _mainCamera = new GridRaycastCamera(Camera.main);
            
            Assert.IsNotNull(_mainCamera, "_mainCamera != null");


            _serviceRegistrationHook =
                ServiceInjector.Instance.RegisterService<IGridService>(s =>
                    _gridService = new GridService(
                        s.GetService<IPrefabInstantiator>(),
                        s.GetService<IAddressableManager>()
                    ));
        }


        private void Start()
        {
            _userInputService = ServiceInjector.Instance.UserInputService;
            _userInputService.SelectCell.Subscribe(_ => _gridService.SelectHoveredCell()).AddTo(this);

            UniTask.Void(async () =>
            {
                var contextManager = ServiceInjector.Instance.SceneContextManager;
                var context = contextManager.GetContext();

                await UseCases.GridInitialization(
                    context,
                    _gridService,
                    ServiceInjector.Instance.AddressableManager,
                    ServiceInjector.Instance.EventPublisher
                );
            });
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Update()
        {
            _gridService.UpdateHoveringCell(_mainCamera, _userInputService.MousePosition);
        }

        public void Dispose()
        {
            _serviceRegistrationHook?.Dispose();
            _gridService?.Dispose();
        }
    }
}