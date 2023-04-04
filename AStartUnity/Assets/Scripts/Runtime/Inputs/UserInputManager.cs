using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inputs
{
    internal sealed class UserInputManager : MonoBehaviour, IUserInputManager
    {

        public static IUserInputManager Instance { get; private set; }

        public event EventHandler SelectCell;
        public Vector2 MousePosition => Input.mousePosition;

        private void Awake()
        {
            if (Instance != null && !ReferenceEquals(Instance, this))
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnSelectCell()
        {
            SelectCell?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                OnSelectCell();
            }
        }
    }
}