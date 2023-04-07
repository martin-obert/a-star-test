using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Grid.Services;
using Runtime.Ui;
using UnityEngine.AddressableAssets;

namespace Runtime
{
    internal class SceneManagementService : ISceneManagementService
    {
        private readonly ISceneContextManager _sceneContextManager;
        private readonly IAddressableManager _addressableManager;
        private readonly GameDefinitions _gameDefinitions;

        public SceneManagementService(
            ISceneContextManager sceneContextManager,
            IAddressableManager addressableManager,
            GameDefinitions gameDefinitions
        )
        {
            _addressableManager = addressableManager;
            _sceneContextManager = sceneContextManager;
            _gameDefinitions = gameDefinitions;
        }

        public async UniTask LoadWorldAsync(int rowsCount, int colsCount,
            CancellationToken token = default)
        {
            _sceneContextManager.SetContext(new SceneContext
            {
                ColCount = colsCount,
                RowCount = rowsCount,
            });
            
            await _addressableManager.LoadSceneAsync(_gameDefinitions.HexGridScene, token);
        }

        public async UniTask LoadLayoutAsync(IGridCell[] cells, CancellationToken token = default)
        {
            _sceneContextManager.SetContext(new SceneContext
            {
                Cells = cells,
                RowCount = cells.Max(x => x.RowIndex),
                ColCount = cells.Max(x => x.ColIndex)
            });
            
            await _addressableManager.LoadSceneAsync(_gameDefinitions.HexGridScene, token);
        }
    }
}