using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Terrains;

namespace Runtime.Services
{
    public interface IGridLayoutRepository
    {
        string[] ListSaves();

        UniTask<IGridCellViewModel[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default);

        UniTask SaveAsync(IEnumerable<IGridCellViewModel> cells, CancellationToken token = default);
    }
}