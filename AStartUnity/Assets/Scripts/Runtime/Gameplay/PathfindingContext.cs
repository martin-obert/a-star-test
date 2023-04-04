using System.Linq;
using PathFinding;
using Runtime.Grid.Data;

namespace Runtime.Gameplay
{
    public sealed class PathfindingContext
    {
        private IGridCell _start;
        private IGridCell _destination;
        private IGridCell[] _selectedPath;


        public void AddWaypoint(IGridCell gridCell)
        {
            if (_start != null && _destination != null)
            {
                ClearWaypoint(_start);
                ClearWaypoint(_destination);
                ClearPath();
                _start = gridCell;
                HighlightWaypoint(_start);
                _destination = null;
                return;
            }

            if (_start == null && !DoesWaypointOverlap(_destination, gridCell))
            {
                _start = gridCell;
                HighlightWaypoint(_start);
            }

            if (_destination == null && !DoesWaypointOverlap(_start, gridCell))
            {
                _destination = gridCell;
                HighlightWaypoint(_destination);
            }

            if (_start == null || _destination == null) return;
            _selectedPath = AStar.GetPath(_start, _destination).OfType<IGridCell>().ToArray();
            HighlightPath();
        }

        private static bool DoesWaypointOverlap(IGridCell a, IGridCell b)
        {
            return ReferenceEquals(a, b);
        }

        public void RemoveWaypoint(IGridCell gridCell)
        {
            ClearWaypoint(_start);
            _start = null;

            ClearWaypoint(_destination);
            _destination = null;

            ClearPath();
        }

        private static void HighlightWaypoint(IGridCell gridCell)
        {
            gridCell?.ToggleSelected(true);
        }

        private static void ClearWaypoint(IGridCell gridCell)
        {
            gridCell?.ToggleSelected(false);
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
    }
}