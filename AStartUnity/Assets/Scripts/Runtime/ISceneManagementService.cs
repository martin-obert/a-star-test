using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Grid.Services;

namespace Runtime
{
    public interface ISceneManagementService
    {
        UniTask LoadWorldAsync(int rowsCount, int colsCount, CancellationToken token = default);
        UniTask LoadLayoutAsync(GridCellSave[] cells, CancellationToken token = default);
    }
}