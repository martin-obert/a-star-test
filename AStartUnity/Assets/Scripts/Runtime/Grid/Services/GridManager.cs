using System;
using System.Collections.Generic;
using Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager
    {
        public static IGridManager Instance { get; private set; }
        public IGridCell[] CurrentCells { get; private set; }

        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;
        [SerializeField] private GridCellPresenterSpawner spawner;

        private void Awake()
        {
            Assert.IsNotNull(spawner, "spawner != null");
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
            
            if (Instance != null && !ReferenceEquals(Instance, this))
            {
                Destroy(this);
                return;
            }

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
                // TODO: store the instance for further reuse
                spawner.SpawnOne(gridCell, transform);
            }
        }
    }
}