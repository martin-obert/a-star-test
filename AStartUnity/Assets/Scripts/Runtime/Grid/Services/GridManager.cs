using System;
using Cysharp.Threading.Tasks;
using Runtime.Inputs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public static class ServiceProviderHelpers
    {
        public static T GetService<T>(this IServiceProvider source) => (T)source.GetService(typeof(T));
    }

    public sealed class GridManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;

        private IDisposable _serviceRegistrationHook;
        private GridService _gridService;
        private IUserInputService _userInputService;

        private void Awake()
        {
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
            _serviceRegistrationHook =
                ServiceInjector.Instance.RegisterService<IGridService>(s =>
                    _gridService = new GridService(s.GetService<IUserInputService>().SelectCell));
        }


        private void Start()
        {
            _userInputService = ServiceInjector.Instance.UserInputService;

            Debug.Log($"rows - {rowCount}, cols - {colCount}");

            UniTask.Void(async () =>
            {
                try
                {
                    var addressableManager = ServiceInjector.Instance.AddressableManager;

                    var prefab = addressableManager.GetCellPrefab();
                    var terrainVariants = addressableManager.GetTerrainVariants();
                    await UniTask.SwitchToMainThread();

                    var context = ServiceInjector.Instance.SceneContextManager.GetContext();

                    if (context.IsValid())
                    {
                        _gridService.SetCells(context.RowCount, context.ColCount, context.Cells, prefab,
                            terrainVariants);
                    }
                    else if (autoGenerateGridOnStart)
                    {
                        _gridService.GenerateGrid(rowCount, colCount, prefab, terrainVariants);
                    }

                    ServiceInjector.Instance.EventPublisher.OnGridInstantiated();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Update()
        {
            _gridService.UpdateHoveringCell(_userInputService);
        }

        public void Dispose()
        {
            _serviceRegistrationHook?.Dispose();
            _gridService?.Dispose();
        }
    }
}