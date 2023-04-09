﻿using System;
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
        private IGridCellViewModel _hoverCellViewModel;
        private readonly Random _random = new();
        private readonly IPrefabInstantiator _prefabInstantiator;
        private readonly IAddressableManager _addressableManager;

        public Rect Bounds { get; private set; }

        public bool IsPointOnGrid(Vector2 point) => Bounds.Contains(point);

        public Vector2 Center => Bounds.center;

        public IEnumerable<IGridCellViewModel> Cells => _currentCells;

        private readonly CompositeDisposable _disposable = new();
        private readonly PathfindingContext _pathfindingContext = new();
        private IGridCellViewModel[] _currentCells;

        public GridService(IPrefabInstantiator prefabInstantiator, IAddressableManager addressableManager)
        {
            _addressableManager = addressableManager ?? throw new ArgumentNullException(nameof(addressableManager));
            _prefabInstantiator = prefabInstantiator ?? throw new ArgumentNullException(nameof(prefabInstantiator));
        }

        public void SelectHoveredCell()
        {
            var gridCell = _hoverCellViewModel;

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

        public void CreateNewGrid(int rowCount, int colCount)
        {
            var cells = GridGenerator.GenerateGrid(rowCount, colCount, () => GetRandomTerrainVariant());

            InstantiateGrid(rowCount, colCount, cells);
        }

        public void SetBounds(int rowCount, int colCount)
        {
            var minCell = GridCellHelpers.GetCellByCoords(_currentCells, 0, 0);
            var maxCell = GridCellHelpers.GetCellByCoords(_currentCells, rowCount - 1, colCount - 1);

            Bounds = Rect.MinMaxRect(minCell.WorldPosition.x, minCell.WorldPosition.z, maxCell.WorldPosition.x,
                maxCell.WorldPosition.z);
        }

        public void UpdateHoveringCell(IGridRaycastCamera mainCamera, Vector2 mousePosition)
        {
            if (_currentCells == null) return;

            var ray = GridRaycaster.GetRayFromMousePosition(mainCamera, mousePosition);

            if (!GridRaycaster.TryGetHitOnGrid(ray, out var hitPoint)) return;

            var hoveredCell =
                GridCellHelpers.GetCellByWorldPoint(new Vector2(hitPoint.x, hitPoint.z), _currentCells);

            if (hoveredCell != null)
            {
                if (hoveredCell == _hoverCellViewModel) return;
                _hoverCellViewModel?.ToggleHighlighted(false);
                _hoverCellViewModel = hoveredCell;
                _hoverCellViewModel?.ToggleHighlighted(true);
            }
            else
            {
                _hoverCellViewModel?.ToggleHighlighted(false);
                _hoverCellViewModel = null;
            }
        }

        public void InstantiateGrid(int rowCount, int colCount, IGridCellViewModel[] cells)
        {
            if (rowCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, "must be greater than 0");
            if (colCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(colCount), colCount, "must be greater than 0");
            _currentCells = cells;

            if (cells == null) throw new ArgumentNullException(nameof(cells));

            if (!cells.Any())
                throw new InvalidOperationException("Sequence contains no elements. Empty cell array provided");

            GridGenerator.PopulateNeighbours(_currentCells);

            foreach (var gridCell in _currentCells)
            {
                var terrainVariant = _addressableManager.GetTerrainVariantByType(gridCell.TerrainType);
                var controller = _prefabInstantiator
                    .InstantiateGridCellPresenter(gridCell);
                controller.SetMainTexture(terrainVariant.TextureOverride);
            }

            SetBounds(rowCount, colCount);
        }

        private ITerrainVariant GetRandomTerrainVariant()
        {
            var source = _addressableManager.GetTerrainVariants();
            return source[_random.Next(0, source.Length)];
        }
    }
}