using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Models;

namespace Runtime.Services
{
    public interface ISceneManagementService
    {
        UniTask LoadWorldAsync(int rowsCount, int colsCount, CancellationToken token = default);
        UniTask LoadLayoutAsync(GridCellDataModel[] cells, CancellationToken token = default);
    }
}