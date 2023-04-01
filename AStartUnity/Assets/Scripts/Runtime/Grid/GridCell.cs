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
        /// <list type="bullet">
        ///     <listheader>
        ///         Defines position of this cell in grid.
        ///     </listheader>
        ///     <item>
        ///         <term>Vector2Int.y </term>
        ///         <description>COL Index</description>
        ///     </item>
        ///     <item>
        ///         <term>Vector2Int.x </term>
        ///         <description>ROW Index</description>
        ///     </item>
        /// </list>
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
