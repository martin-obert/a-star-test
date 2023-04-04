using System;
using UnityEngine;

namespace Runtime.Inputs
{
    public interface IUserInputManager
    {
        event EventHandler SelectCell;
        Vector2 MousePosition { get; }
    }
}