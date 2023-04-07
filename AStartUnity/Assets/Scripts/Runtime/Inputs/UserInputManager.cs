using System;
using Runtime.Grid.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inputs
{
    internal sealed class UserInputManager : MonoBehaviour, IUserInputManager
    {
        public event EventHandler SelectCell;
        public Vector2 MousePosition => Input.mousePosition;
        public Vector3 AxisMovementVector => new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        private void Awake()
        {
            UnitOfWork.Instance.RegisterService<IUserInputManager>(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                OnSelectCell();
            }
        }

        private void OnDestroy()
        {
            if (UnitOfWork.Instance)
                UnitOfWork.Instance.RemoveService<IUserInputManager>();
        }
        
        private void OnSelectCell()
        {
            SelectCell?.Invoke(this, EventArgs.Empty);
        }
    }
}