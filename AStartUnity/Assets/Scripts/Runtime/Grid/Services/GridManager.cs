using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Services
{
    public sealed class GridManager : MonoBehaviour, IGridManager, IDisposable
    {
        [SerializeField] private int rowCount = 1;
        [SerializeField] private int colCount = 1;
        [SerializeField] private bool autoGenerateGridOnStart;

        private readonly PathfindingContext _pathfindingContext = new();

        private IGridRaycaster _gridRaycaster;

        private Rect _rect;

        private IUserInputManager _userInputManager;
        private EventPublisher _eventPublisher;

        public IGridCell[] CurrentCells { get; private set; }
        public IGridCell HoverCell { get; private set; }

        public bool IsPointOnGrid(Vector2 point) => _rect.Contains(point);

        public Vector2 Center => _rect.center;

        private void Awake()
        {
            Assert.IsTrue(rowCount > 0, "rowCount > 0");
            Assert.IsTrue(colCount > 0, "colCount > 0");
            UnitOfWork.Instance.RegisterService<IGridManager>(this);
        }


        private void Start()
        {
            _gridRaycaster = UnitOfWork.Instance.GridRaycaster;
            _userInputManager = UnitOfWork.Instance.UserInputManager;
            _eventPublisher = UnitOfWork.Instance.EventPublisher;

            ReadGameManagerSetup();

            Debug.Log($"rows - {rowCount}, cols - {colCount}");

            UniTask.Void(async () =>
            {
                await UniTask.SwitchToMainThread();
                await UnitOfWork.Instance.GridCellRepository.InitAsync();

                if (autoGenerateGridOnStart)
                {
                    GenerateGrid();
                }
            });
            _userInputManager.SelectCell += InstanceOnSelectCell;
        }


        private void OnDestroy()
        {
            Dispose();
        }


        private void Update()
        {
            if (CurrentCells == null) return;

            var ray = _gridRaycaster.GetRayFromMousePosition(_userInputManager.MousePosition);

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


        private void ReadGameManagerSetup()
        {
            var context = UnitOfWork.Instance.SceneContextManager.GetContext();
            rowCount = context.RowCount;
            colCount = context.ColCount;
            CurrentCells = context.Cells;
            Debug.Log($"Reading game manager grid setup");
        }

        private void InstanceOnSelectCell(object sender, EventArgs e)
        {
            var gridCell = HoverCell;

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


        public void GenerateGrid()
        {
            CurrentCells ??= GridGenerator.GenerateGrid(rowCount, colCount, UnitOfWork.Instance.TerrainVariantRepository);

            GridGenerator.PopulateNeighbours(CurrentCells);
            
            foreach (var gridCell in CurrentCells)
            {
                UnitOfWork.Instance.GridCellRepository.GetPrefab(gridCell.TerrainType, transform)
                    .SetDataModel(gridCell);
            }

            var minCell = GridCellHelpers.GetCellByCoords(CurrentCells, 0, 0);
            var maxCell = GridCellHelpers.GetCellByCoords(CurrentCells, rowCount - 1, colCount - 1);

            _rect = Rect.MinMaxRect(minCell.WorldPosition.x, minCell.WorldPosition.z, maxCell.WorldPosition.x,
                maxCell.WorldPosition.z);

            _eventPublisher.OnGridInstantiated();
        }

        public UniTask SaveLayoutAsync(CancellationToken token = default) =>
            UnitOfWork.Instance.GridLayoutRepository.SaveAsync(CurrentCells, token);

        public void Dispose()
        {
            if (UnitOfWork.Instance)
                UnitOfWork.Instance.RemoveService<IGridManager>();

            _userInputManager.SelectCell -= InstanceOnSelectCell;

            _pathfindingContext?.Dispose();
        }
    }
}