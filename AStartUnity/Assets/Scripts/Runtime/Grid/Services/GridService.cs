using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Terrains;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Grid.Services
{
    internal sealed class GridService : IGridService, IDisposable
    {
        private IGridCell _hoverCell;

        public Rect Bounds { get; private set; }

        public bool IsPointOnGrid(Vector2 point) => Bounds.Contains(point);

        public Vector2 Center => Bounds.center;

        public IEnumerable<IGridCell> Cells => _currentCells;

        private readonly CompositeDisposable _disposable = new();
        private readonly PathfindingContext _pathfindingContext = new();
        private IGridCell[] _currentCells;

        public GridService(IObservable<Unit> onSelectCell)
        {
            onSelectCell.Subscribe(_ =>
            {
                var gridCell = _hoverCell;

                if (gridCell is not { IsWalkable: true }) return;

                gridCell.ToggleSelected();

                if (gridCell.IsSelected)
                {
                    _pathfindingContext.AddWaypoint(gridCell);
                }
                else
                {
                    _pathfindingContext.RemoveWaypoint();
                }
            }).AddTo(_disposable);
        }

        public UniTask SaveLayoutAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _pathfindingContext?.Dispose();
        }

        public void GenerateGrid(int rowCount, int colCount, GridCellPresenter prefab,
            ITerrainVariant[] terrainVariants)
        {
            var cells = GridGenerator.GenerateGrid(rowCount, colCount,
                ServiceInjector.Instance.AddressableManager.GetRandomTerrainVariant);

            SetCells(rowCount, colCount, cells, prefab, terrainVariants);
        }

        public void SetBounds(int rowCount, int colCount)
        {
            var minCell = GridCellHelpers.GetCellByCoords(_currentCells, 0, 0);
            var maxCell = GridCellHelpers.GetCellByCoords(_currentCells, rowCount - 1, colCount - 1);

            Bounds = Rect.MinMaxRect(minCell.WorldPosition.x, minCell.WorldPosition.z, maxCell.WorldPosition.x,
                maxCell.WorldPosition.z);
        }

        public void UpdateHoveringCell(IUserInputService userInputService)
        {
            if (_currentCells == null) return;

            var ray = GridRaycaster.GetRayFromMousePosition(Camera.main, userInputService.MousePosition);

            if (!GridRaycaster.TryGetHitOnGrid(ray, out var hitPoint)) return;

            var hoveredCell =
                GridCellHelpers.GetCellByWorldPoint(new Vector2(hitPoint.x, hitPoint.z), _currentCells);

            if (hoveredCell != null)
            {
                if (hoveredCell == _hoverCell) return;
                _hoverCell?.ToggleHighlighted(false);
                _hoverCell = hoveredCell;
                _hoverCell?.ToggleHighlighted(true);
            }
            else
            {
                _hoverCell?.ToggleHighlighted(false);
                _hoverCell = null;
            }
        }

        public void SetCells(int rowCount, int colCount, IGridCell[] cells, GridCellPresenter prefab,
            ITerrainVariant[] terrainVariants)
        {
            _currentCells = cells;
            GridGenerator.PopulateNeighbours(_currentCells);

            foreach (var gridCell in _currentCells)
            {
                Object.Instantiate(prefab)
                    .SetDataModel(gridCell, terrainVariants.First(x => x.Type == gridCell.TerrainType));
            }

            SetBounds(rowCount, colCount);
        }
    }
}