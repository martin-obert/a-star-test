using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;

namespace Runtime.Grid.Services
{
    public interface IGridLayoutRepository
    {
        string[] ListSaves();
        UniTask<IGridCell[]> LoadAsync(string filename, CancellationToken token = default);
        UniTask SaveAsync(IGridCell[] cells, CancellationToken token = default);
    }
}