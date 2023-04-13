using UnityEngine;

namespace Runtime.Grid.Integrations
{
    /// <summary>
    /// Exposes required functionality for grid cell, decouples from Unity API and allows better testing
    /// </summary>
    public interface IGridCellTransform
    {
        Vector3 Position { get; set; }
    }
}