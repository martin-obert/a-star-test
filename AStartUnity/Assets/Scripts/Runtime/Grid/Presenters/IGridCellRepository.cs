using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellRepository
    {
        GridCellPresenter GetPrefab(TerrainType terrainType, Transform parent);
        UniTask InitAsync();
    }
}