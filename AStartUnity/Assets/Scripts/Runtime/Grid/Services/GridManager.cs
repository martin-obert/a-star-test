using System;
using Cysharp.Threading.Tasks;
using Runtime.Inputs;
using Runtime.Messaging;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public static class ServiceProviderHelpers
    {
        public static T GetService<T>(this IServiceProvider source) => (T)source.GetService(typeof(T));
    }

    public static class UseCases
    {
        public static async UniTask GridInitialization(
            SceneContext context,
            IGridService gridService,
            IAddressableManager addressableManager,
            EventPublisher eventPublisher)
        {
            try
            {
                await UniTask.SwitchToMainThread();
                var terrainVariants = addressableManager.GetTerrainVariants();
                if (!context.HasCells())
                {
                    gridService.CreateNewGrid(context.RowCount, context.ColCount, terrainVariants);
                }
                else if (context.HasCells())
                {
                    gridService.InstantiateGrid(context.RowCount, context.ColCount, context.Cells);
                }

                eventPublisher.OnGridInstantiated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


    public sealed class GridManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;

        private IDisposable _serviceRegistrationHook;
        private GridService _gridService;
        private IUserInputService _userInputService;

        private Camera _mainCamera;

        private void Awake()
        {
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");

            _mainCamera = Camera.main;
            Assert.IsNotNull(_mainCamera, "_mainCamera != null");

            _serviceRegistrationHook =
                ServiceInjector.Instance.RegisterService<IGridService>(s =>
                    _gridService = new GridService(s.GetService<IPrefabInstantiator>()));
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
            _gridService.UpdateHoveringCell(_mainCamera, _userInputService);
        }

        public void Dispose()
        {
            _serviceRegistrationHook?.Dispose();
            _gridService?.Dispose();
        }
    }
}