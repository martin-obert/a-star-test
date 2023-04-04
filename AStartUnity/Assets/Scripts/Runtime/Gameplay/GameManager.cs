using System;
using System.ComponentModel;
using Runtime.Grid.Services;
using Runtime.Inputs;
using UnityEngine;

namespace Runtime.Gameplay
{
    public sealed class GameManager : MonoBehaviour
    {
        private readonly PathfindingContext _pathfindingContext = new();

        private void Start()
        {
            UserInputManager.Instance.SelectCell += InstanceOnSelectCell;
        }


        private void InstanceOnSelectCell(object sender, EventArgs e)
        {
            var gridCell = GridManager.Instance.HoverCell;
            if (gridCell == null || !gridCell.TerrainVariant.IsWalkable) return;
            
            gridCell.ToggleSelected();
            
            if (gridCell.IsSelected)
            {
                _pathfindingContext.AddWaypoint(gridCell);
            }
            else
            {
                _pathfindingContext.RemoveWaypoint(gridCell);
            }
        }

        private void OnDestroy()
        {
            UserInputManager.Instance.SelectCell -= InstanceOnSelectCell;
        }
    }
}