using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.DependencyInjection;
using Runtime.Grid.Models;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Services;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Integrations
{
    /// <summary>
    /// Facade object for <see cref="IGridService"/>, exposing API for other objects
    ///TODO: this should be renamed to GridFacade
    /// </summary>
    public sealed class GridManager : MonoBehaviour, IDisposable
    {
        [Serializable]
        private class TestTerrain
        {
            public TerrainType[] terrainTypes;
        }

        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;

        [SerializeField] private TestTerrain[] _terrains;

        private IDisposable _serviceRegistrationHook;
        private GridService _gridService;
        private IUserInputService _userInputService;

        private IGridRaycastCamera _mainCamera;

        private void Awake()
        {
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");

            _mainCamera = new GridRaycastCamera(UnityEngine.Camera.main);

            Assert.IsNotNull(_mainCamera, "_mainCamera != null");


            _serviceRegistrationHook =
                ServiceInjector.Instance.RegisterService<IGridService>(s =>
                    _gridService = new GridService(
                        s.GetService<IPrefabInstantiatorService>(),
                        s.GetService<IAddressableManager>()
                    ));
        }


        private void Start()
        {
            _userInputService = ServiceInjector.Instance.UserInputService;
            _userInputService.SelectCell.Subscribe(_ => _gridService.SelectHoveredCell()).AddTo(this);

            UniTask.Void(async () =>
            {
                var contextManager = ServiceInjector.Instance.GridSetupManager;
                var context = contextManager.GetContext();

                // For editor we can define explicitly the grid data to render
                if (Application.isEditor && _terrains.Any())
                {
                    Debug.LogWarning(
                        "Debug grid layout has been loaded from component definition. The SceneContext has been overriden");
                    context = GetDebugSceneContext();
                }

                await UseCases.GridInitialization(
                    context,
                    _gridService,
                    ServiceInjector.Instance.EventPublisher
                );
            });
        }

        private GridSetup GetDebugSceneContext()
        {
            var context = new GridSetup
            {
                Cells = _terrains.Select((x, r) => x.terrainTypes.Select((y, c) => new GridCellDataModel
                {
                    TerrainType = y,
                    ColIndex = c,
                    RowIndex = r,
                })).SelectMany(x => x).ToArray(),
                ColCount = _terrains.Length,
                RowCount = _terrains.Length
            };
            return context;
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