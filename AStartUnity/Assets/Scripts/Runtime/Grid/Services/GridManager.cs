using System;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager, IDisposable
    {
        private readonly PathfindingContext _pathfindingContext = new();
        private readonly IGridRaycaster _gridRaycaster = new GridRaycaster();

        private Rect _rect;

        public IGridCell[] CurrentCells { get; private set; }
        public IGridCell HoverCell { get; private set; }
        public static IGridManager Instance { get; private set; }


        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;
        [SerializeField] private GridCellRepository gridCellRepository;

        private void Awake()
        {
            if (Instance != null && !ReferenceEquals(Instance, this))
            {
                Destroy(this);
                return;
            }

            Assert.IsNotNull(gridCellRepository, "gridCellRepository != null");
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
            Instance = this;
        }

        private void Start()
        {
            UniTask.Void(async () =>
            {
                await UniTask.SwitchToMainThread();

                await gridCellRepository.InitAsync();
                if (autoGenerateGridOnStart)
                {
                    GenerateGrid();
                }
            });
            UserInputManager.Instance.SelectCell += InstanceOnSelectCell;
        }

        private void InstanceOnSelectCell(object sender, EventArgs e)
        {
            var gridCell = HoverCell;

            if (gridCell == null || !gridCell.TerrainVariant.IsWalkable) return;

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

        private void OnDestroy()
        {
            Dispose();
        }


        private void Update()
        {
            if (CurrentCells == null) return;

            var ray = _gridRaycaster.GetRayFromMousePosition(UserInputManager.Instance.MousePosition);

            if (!_gridRaycaster.TryGetHitOnGrid(ray, out var hitPoint)) return;

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


        public void GenerateGrid()
        {
            CurrentCells = GridGenerator.GenerateGrid(rowCount, colCount, gridCellRepository);

            foreach (var gridCell in CurrentCells)
            {
                gridCellRepository.GetPrefab(gridCell.TerrainVariant, transform).SetDataModel(gridCell);
            }

            var minCell = GridCellHelpers.GetCellByCoords(CurrentCells, 0, 0);
            var maxCell = GridCellHelpers.GetCellByCoords(CurrentCells, rowCount - 1, colCount - 1);
            _rect = Rect.MinMaxRect(minCell.WorldPosition.x, minCell.WorldPosition.z, maxCell.WorldPosition.x,
                maxCell.WorldPosition.z);

            MessageBus.Instance.Publish(new OnGridInstantiated());
        }

        public bool IsPointOnGrid(Vector2 point) => _rect.Contains(point);

        public void Dispose()
        {
            UserInputManager.Instance.SelectCell -= InstanceOnSelectCell;
            _pathfindingContext?.Dispose();
        }
    }
}