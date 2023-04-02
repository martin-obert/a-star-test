using UnityEngine;

namespace Runtime.Grid.Data
{
    public interface IGridCell 
    {
        int RowIndex { get; }
        int ColIndex { get; }
        bool IsOddRow { get; }
        Vector3 WorldPosition { get; }
        public float HeightHalf { get; }
        
        public float WidthHalf { get; }
    }
}