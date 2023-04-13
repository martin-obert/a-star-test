using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Definitions;
using Runtime.Grid.Models;
using Runtime.Grid.Services;

namespace Runtime.Services
{
    internal class SceneManagementService : ISceneManagementService
    {
        private readonly IGridSetupManager _gridSetupManager;
        private readonly IAddressableManager _addressableManager;
        private readonly GameDefinitions _gameDefinitions;

        public SceneManagementService(
            IGridSetupManager gridSetupManager,
            IAddressableManager addressableManager,
            GameDefinitions gameDefinitions
        )
        {
            _addressableManager = addressableManager;
            _gridSetupManager = gridSetupManager;
            _gameDefinitions = gameDefinitions;
        }

        public async UniTask LoadWorldAsync(int rowsCount, int colsCount,
            CancellationToken token = default)
        {
            _gridSetupManager.SetContext(new GridSetup
            {
                ColCount = colsCount,
                RowCount = rowsCount,
            });
            
            await _addressableManager.LoadSceneAsync(_gameDefinitions.HexGridScene, token);
        }

        public async UniTask LoadLayoutAsync(GridCellDataModel[] cells, CancellationToken token = default)
        {
            _gridSetupManager.SetContext(new GridSetup
            {
                Cells = cells,
                RowCount = cells.Max(x => x.RowIndex),
                ColCount = cells.Max(x => x.ColIndex)
            });
            
            await _addressableManager.LoadSceneAsync(_gameDefinitions.HexGridScene, token);
        }
    }
}