using System.Collections.Generic;
using Grid;
using PathFinding;
using UnityEngine;

namespace Runtime.Grid
{
    /// <summary>
    /// Implementation of a <see cref="IAStarNode"/>
    /// </summary>
    public sealed class GridCell : IAStarNode, IGridCell
    {
        /// <summary>
        /// Defines position of this cell in grid.
        /// ROW Index = <see cref="Vector2Int.x"/>
        /// COL Index = <see cref="Vector2Int.y"/>
        /// </summary>
        public Vector2Int GridPosition { get; set; }
        
        public IEnumerable<IAStarNode> Neighbours { get; }
        
        public float CostTo(IAStarNode neighbour)
        {
            throw new System.NotImplementedException();
        }

        public float EstimatedCostTo(IAStarNode target)
        {
            throw new System.NotImplementedException();
        }
    }
}
