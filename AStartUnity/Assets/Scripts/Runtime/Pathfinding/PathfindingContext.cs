using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using PathFinding;
using Runtime.Grid.Models;

namespace Runtime.Pathfinding
{
    /// <summary>
    /// Holds current state of a pathfinding and abstracts logic for finding path by setting start/destination waypoint. 
    /// </summary>
    public sealed class PathfindingContext : IDisposable
    {
        private IGridCellViewModel _start;
        private IGridCellViewModel _destination;
        private IGridCellViewModel[] _selectedPath;
        private bool _isPlotting;
        private CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Sets either start or destination waypoint. If both waypoints are set, then calculates the cheapest path using <see cref="AStar.GetPath"/>
        /// </summary>
        /// <param name="waypointCell">Cell to set as waypoint</param>
        public void AddWaypoint(IGridCellViewModel waypointCell)
        {
            if(_isPlotting) return;

            if (_start != null && _destination != null)
            {
                ClearWaypoint(_start);
                ClearWaypoint(_destination);
                ClearPath();
                _start = waypointCell;
                HighlightWaypoint(_start);
                _destination = null;
                return;
            }

            if (_start == null && !DoesWaypointOverlap(_destination, waypointCell))
            {
                _start = waypointCell;
                HighlightWaypoint(_start);
            }

            if (_destination == null && !DoesWaypointOverlap(_start, waypointCell))
            {
                _destination = waypointCell;
                HighlightWaypoint(_destination);
            }

            if (_start == null || _destination == null) return;

            CancelCurrentPathfinding();

            UniTask.Void(async (t) =>
            {
                try
                {
                    _isPlotting = true;

                    await UniTask.SwitchToThreadPool();
                    
                    _selectedPath = AStar.GetPath(_start, _destination).OfType<IGridCellViewModel>().ToArray();
                    
                    await UniTask.SwitchToMainThread(t);
                    
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
                    CancelCurrentPathfinding();
                }
            }, _cancellationTokenSource.Token);
        }

        private void CancelCurrentPathfinding()
        {
            if(_cancellationTokenSource.IsCancellationRequested) return;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
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