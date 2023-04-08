using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Runtime.Gameplay;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Terrains;
using UniRx;
using UnityEngine;
using Random = System.Random;

[assembly: InternalsVisibleTo("Grid.Editor.Tests")]

namespace Runtime.Grid.Services
{
    internal sealed class GridService : IGridService, IDisposable
    {
        private IGridCell _hoverCell;
        private readonly Random _random = new();
        private readonly IPrefabInstantiator _prefabInstantiator;

        public Rect Bounds { get; private set; }

        public bool IsPointOnGrid(Vector2 point) => Bounds.Contains(point);

        public Vector2 Center => Bounds.center;

        public IEnumerable<IGridCell> Cells => _currentCells;

        private readonly CompositeDisposable _disposable = new();
        private readonly PathfindingContext _pathfindingContext = new();
        private IGridCell[] _currentCells;

        public GridService(IPrefabInstantiator prefabInstantiator)
        {
            _prefabInstantiator = prefabInstantiator ?? throw new ArgumentNullException(nameof(prefabInstantiator));
        }

        public void SelectHoveredCell()
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
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _pathfindingContext?.Dispose();
        }

        public void CreateNewGrid(int rowCount, int colCount, ITerrainVariant[] terrainVariants)
        {
            var cells = GridGenerator.GenerateGrid(rowCount, colCount, () => GetRandomTerrainVariant(terrainVariants));

            InstantiateGrid(rowCount, colCount, cells);
        }

        public void SetBounds(int rowCount, int colCount)
        {
            var minCell = GridCellHelpers.GetCellByCoords(_currentCells, 0, 0);
            var maxCell = GridCellHelpers.GetCellByCoords(_currentCells, rowCount - 1, colCount - 1);

            Bounds = Rect.MinMaxRect(minCell.WorldPosition.x, minCell.WorldPosition.z, maxCell.WorldPosition.x,
                maxCell.WorldPosition.z);
        }

        public void UpdateHoveringCell(Camera mainCamera, IUserInputService userInputService)
        {
            if (_currentCells == null) return;

            var ray = GridRaycaster.GetRayFromMousePosition(mainCamera, userInputService.MousePosition);

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

        public void InstantiateGrid(int rowCount, int colCount, IGridCell[] cells)
        {
            _currentCells = cells;

            GridGenerator.PopulateNeighbours(_currentCells);

            foreach (var gridCell in _currentCells)
            {
                _prefabInstantiator.InstantiateGridCellPresenter(gridCell);
            }

            SetBounds(rowCount, colCount);
        }

        private ITerrainVariant GetRandomTerrainVariant(ITerrainVariant[] source)
        {
            return source[_random.Next(0, source.Length)];
        }
    }
}