using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreSystems
{
    [CreateAssetMenu(fileName = "Player input", menuName = "CoreSystems/Player input", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        private Controls _controls;

        public event Action<Vector2> OnMoveChanged;
        public event Action OnCrunchPressed;
        public event Action<bool> OnInteractPressed;

        private Vector2 _moveDir;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveChanged?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnCrunchPressed?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }
    }
}