using Runtime.Grid.Data;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public sealed class GridCellPresenter : MonoBehaviour
    {
        private IGridCell _cell;
        
        public void SetDataModel(IGridCell cell)
        {
            _cell = cell;
        }
    }
}