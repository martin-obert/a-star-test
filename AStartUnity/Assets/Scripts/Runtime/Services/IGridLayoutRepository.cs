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

        UniTask<IGridCell[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default);

        UniTask SaveAsync(IEnumerable<IGridCell> cells, CancellationToken token = default);
    }
}