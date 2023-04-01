using System.Collections.Generic;
using Grid;
using PathFinding;

namespace Runtime.Grid.Data
{
    /// <summary>
    /// Implementation of a <see cref="IAStarNode"/>
    /// </summary>
    public sealed class GridCell : IAStarNode, IGridCell
    {
        /// <summary>
        /// Row position on the grid
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Column position on the grid
        /// </summary>
        public int ColIndex { get; set; }
        
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