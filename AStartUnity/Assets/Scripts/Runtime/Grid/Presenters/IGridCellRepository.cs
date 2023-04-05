using Runtime.Grid.Data;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellRepository
    {
        GridCellPresenter GetPrefab(ITerrainVariant terrainVariants, Transform parent);
    }
}