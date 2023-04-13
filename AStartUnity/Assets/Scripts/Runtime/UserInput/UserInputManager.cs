using System;
using Runtime.DependencyInjection;
using Runtime.Grid.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inputs
{
    internal sealed class UserInputManager : MonoBehaviour
    {
        private IDisposable _serviceRegistrationHook;
        private UserInputService _service;

        private void Awake()
        {
            _serviceRegistrationHook =
                ServiceInjector.Instance.RegisterService<IUserInputService>(_service = new UserInputService());
        }

        private void Update()
        {
            _service.AxisMovementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _service.MousePosition = Input.mousePosition;
            
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                _service.OnSelectCell();
            }
        }

        private void OnDestroy()
        {
            _serviceRegistrationHook?.Dispose();
        }
    }
}