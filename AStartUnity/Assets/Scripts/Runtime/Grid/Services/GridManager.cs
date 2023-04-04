﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager
    {
        public static IGridManager Instance { get; private set; }

        [SerializeField] private Transform debugPoint;
        private readonly IGridRaycaster _gridRaycaster = new GridRaycaster();
        public IGridCell[] CurrentCells { get; private set; }
        public IGridCell HoverCell { get; private set; }

        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;
        [SerializeField] private GridCellPresenterSpawner spawner;

        private void Awake()
        {
            if (Instance != null && !ReferenceEquals(Instance, this))
            {
                Destroy(this);
                return;
            }

            Assert.IsNotNull(spawner, "spawner != null");
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
            Instance = this;
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
                spawner.SpawnOne(gridCell, transform);
            }
        }

        private void Update()
        {
            var ray = _gridRaycaster.GetRayFromMousePosition(UserInputManager.Instance.MousePosition);

            if (!_gridRaycaster.TryGetHitOnGrid(ray, out var hitPoint)) return;

            if (debugPoint)
            {
                debugPoint.position = hitPoint;
            }

            var hoveredCell =
                GridCellHelpers.GetCellByWorldPoint(new Vector2(hitPoint.x, hitPoint.z), CurrentCells);

            if (hoveredCell != null)
            {
                if (hoveredCell == HoverCell) return;
                HoverCell?.ToggleHighlighted(false);
                HoverCell = hoveredCell;
                HoverCell?.ToggleHighlighted(true);
            }
            else
            {
                HoverCell?.ToggleHighlighted(false);
                HoverCell = null;
            }
        }
    }
}