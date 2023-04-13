using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Models;
using Runtime.Grid.Services;

namespace Runtime.Services
{
    public interface IGridLayoutRepository
    {
        string[] ListSaves();

        Task<GridCellDataModel[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default);

        UniTask SaveAsync(IEnumerable<IGridCellViewModel> cells, CancellationToken token = default);
    }
}