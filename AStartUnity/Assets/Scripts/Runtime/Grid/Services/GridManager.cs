﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager
    {
        [SerializeField] private Transform debugPoint;
        private readonly IGridRaycaster _gridRaycaster = new GridRaycaster();
        public IGridCell[] CurrentCells { get; private set; }
        public IGridCell _lastHovered;
        public List<GridCellPresenter> Presenters { get; } = new();

        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;
        [SerializeField] private GridCellPresenterSpawner spawner;

        private void Awake()
        {
            Assert.IsNotNull(spawner, "spawner != null");
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
        }

        private void Start()
        {
            if (autoGenerateGridOnStart)
            {
                GenerateGrid();
            }
        }

        public void GenerateGrid()
        {
            CurrentCells = GridGenerator.GenerateGrid(rowCount, colCount);

            foreach (var gridCell in CurrentCells)
            {
                Presenters.Add(spawner.SpawnOne(gridCell, transform));
            }
        }

        // TODO: this is only temporary for debug purposes
        private void Update()
        {
            var ray = _gridRaycaster.GetRayFromMousePosition();

            if (!_gridRaycaster.TryGetHitOnGrid(ray, out var hitPoint)) return;

            if (debugPoint)
            {
                debugPoint.position = hitPoint;
            }

            var hoveredCell =
                GridCellHelpers.GetCellByWorldPoint(new Vector2(hitPoint.x, hitPoint.z), CurrentCells);

            if (hoveredCell != null)
            {
                if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                {
                    hoveredCell.ToggleSelected();
                    _lastHovered = null;
                }
                else
                {
                    if (hoveredCell == _lastHovered) return;
                    _lastHovered?.ToggleHighlighted(false);
                    _lastHovered = hoveredCell;
                    _lastHovered?.ToggleHighlighted(true);
                }
            }
            else
            {
                _lastHovered?.ToggleHighlighted(false);
                _lastHovered = null;
            }
        }
    }
}