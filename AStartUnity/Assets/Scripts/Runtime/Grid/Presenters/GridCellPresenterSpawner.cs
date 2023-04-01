using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    [CreateAssetMenu(menuName = "Grid/Cell Presenter Spawner", fileName = "GridCellPresenterSpawner", order = 0)]
    public sealed class GridCellPresenterSpawner : ScriptableObject
    {
        [SerializeField] private GridCellPresenter presenterPrefab;

        public GridCellPresenter SpawnOne(IGridCell cell, Transform parent)
        {
            var instance = Instantiate(presenterPrefab, parent);
            instance.SetDataModel(cell);
            return instance;
        }
    }
}