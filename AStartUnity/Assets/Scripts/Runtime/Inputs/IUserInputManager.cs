using System;
using UniRx;
using UnityEngine;

namespace Runtime.Inputs
{
    public class UserInputService : IUserInputService, IDisposable
    {
        private readonly Subject<Unit> _selectCell = new();
        public IObservable<Unit> SelectCell => _selectCell;
        
        public Vector2 MousePosition { get; set; }
        public Vector3 AxisMovementVector { get; set; }

        public void OnSelectCell() => _selectCell?.OnNext(Unit.Default);
        
        public void Dispose()
        {
            _selectCell?.Dispose();
        }
    }
    
    public interface IUserInputService
    {
        IObservable<Unit> SelectCell { get; }
        Vector2 MousePosition { get; }
        Vector3 AxisMovementVector { get; }
    }
}