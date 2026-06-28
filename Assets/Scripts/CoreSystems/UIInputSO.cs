using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreSystems
{
    [CreateAssetMenu(fileName = "UI input", menuName = "CoreSystems/UI input")]
    public class UIInputSO : ScriptableObject, Controls.IUIActions
    {
        private Controls _controls;

        public event Action<Vector2> OnMousePosChanged;
        public event Action<Vector2> OnScrollChanged;
        public event Action OnESCPressed;

        [field: SerializeField] public float MouseSensitivity { get; private set; } = 1f;

        private Camera _mainCamera;
        public Camera MainCamera => _mainCamera == null ? _mainCamera = Camera.main : _mainCamera;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.UI.SetCallbacks(this);
            }
            _controls.UI.Enable();
        }

        private void OnDisable()
        {
            _controls.UI.Disable();
        }

        public void InputActive(bool value)
        {
            if (value)
                _controls.UI.Enable();
            else
                _controls.UI.Disable();
        }

        public void SetMouseSensitivity(float value)
        {
            MouseSensitivity = Mathf.Clamp(value, 0.1f, 3f);
        }

        public void OnNavigate(InputAction.CallbackContext context) { }
        public void OnSubmit(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnESCPressed?.Invoke();
        }

        public void OnPoint(InputAction.CallbackContext context) { }
        public void OnClick(InputAction.CallbackContext context) { }
        public void OnRightClick(InputAction.CallbackContext context) { }
        public void OnMiddleClick(InputAction.CallbackContext context) { }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            OnScrollChanged?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            OnMousePosChanged?.Invoke(context.ReadValue<Vector2>() * MouseSensitivity);
        }

        public Vector3 GetHorizontalCameraForward()
        {
            Vector3 forward = MainCamera.transform.forward.normalized;
            forward.y = 0;
            return forward;
        }
    }
}