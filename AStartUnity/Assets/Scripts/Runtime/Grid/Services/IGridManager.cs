using System.Collections.Generic;
using Runtime.Grid.Data;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public interface IGridService
    {
        bool IsPointOnGrid(Vector2 point);
        Vector2 Center { get; }
        IEnumerable<IGridCell> Cells { get; }
        void CreateNewGrid(int rowCount, int colCount, ITerrainVariant[] terrainVariants);
        void InstantiateGrid(int rowCount, int colCount, IGridCell[] cells);
    }
}