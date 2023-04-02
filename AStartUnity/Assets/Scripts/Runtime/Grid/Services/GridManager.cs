using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager
    {
        public IGridCell[] CurrentCells { get; private set; }
        public List<GridCellPresenter> Presenters { get; private set; } = new List<GridCellPresenter>();

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


    }
}