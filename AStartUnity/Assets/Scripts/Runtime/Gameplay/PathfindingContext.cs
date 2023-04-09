using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using PathFinding;
using Runtime.Grid.Data;

namespace Runtime.Gameplay
{
    public sealed class PathfindingContext : IDisposable
    {
        private IGridCellViewModel _start;
        private IGridCellViewModel _destination;
        private IGridCellViewModel[] _selectedPath;
        private bool _isPlotting;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public void AddWaypoint(IGridCellViewModel gridCellViewModel)
        {
            if(_isPlotting) return;

            if (_start != null && _destination != null)
            {
                ClearWaypoint(_start);
                ClearWaypoint(_destination);
                ClearPath();
                _start = gridCellViewModel;
                HighlightWaypoint(_start);
                _destination = null;
                return;
            }

            if (_start == null && !DoesWaypointOverlap(_destination, gridCellViewModel))
            {
                _start = gridCellViewModel;
                HighlightWaypoint(_start);
            }

            if (_destination == null && !DoesWaypointOverlap(_start, gridCellViewModel))
            {
                _destination = gridCellViewModel;
                HighlightWaypoint(_destination);
            }

            if (_start == null || _destination == null) return;
            UniTask.Void(async () =>
            {
                try
                {
                    _isPlotting = true;

                    await UniTask.SwitchToThreadPool();
                    
                    _selectedPath = AStar.GetPath(_start, _destination).OfType<IGridCellViewModel>().ToArray();
                    
                    await UniTask.SwitchToMainThread(_cancellationTokenSource.Token);
                    
                    HighlightPath();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    _isPlotting = false;
                }
            });
        }

        private static bool DoesWaypointOverlap(IGridCellViewModel a, IGridCellViewModel b)
        {
            return ReferenceEquals(a, b);
        }

        public void RemoveWaypoint()
        {
            ClearWaypoint(_start);
            _start = null;

            ClearWaypoint(_destination);
            _destination = null;

            ClearPath();
        }

        private static void HighlightWaypoint(IGridCellViewModel gridCellViewModel)
        {
            gridCellViewModel?.ToggleSelected(true);
        }

        private static void ClearWaypoint(IGridCellViewModel gridCellViewModel)
        {
            gridCellViewModel?.ToggleSelected(false);
        }

        private void HighlightPath()
        {
            if (_selectedPath == null) return;
            foreach (var cell in _selectedPath)
            {
                cell.TogglePinned(true);
            }
        }

        private void ClearPath()
        {
            if (_selectedPath == null) return;
            foreach (var cell in _selectedPath)
            {
                cell.TogglePinned(false);
            }

            _selectedPath = null;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}