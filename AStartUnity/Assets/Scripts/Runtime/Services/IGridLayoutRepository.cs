using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Grid.Services;
using Runtime.Terrains;

namespace Runtime.Services
{
    public interface IGridLayoutRepository
    {
        string[] ListSaves();

        Task<GridCellSave[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default);

        UniTask SaveAsync(IEnumerable<IGridCellViewModel> cells, CancellationToken token = default);
    }
}