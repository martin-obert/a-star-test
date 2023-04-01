using System.Collections.Generic;
using PathFinding;

namespace Grid
{
    /// <summary>
    /// Implementation of a <see cref="IAStarNode"/>
    /// </summary>
    public sealed class GridCell : IAStarNode
    {
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
