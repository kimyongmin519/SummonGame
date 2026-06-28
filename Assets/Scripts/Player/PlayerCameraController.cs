using System;
using KimLIb.ModuleSystems;
using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour, IModule
    {
        [Header("카메라 기준")]
        [SerializeField] private Transform eyeAnchor;

        [Header("카메라 회전")]
        [SerializeField] private float minPitch = -70f;
        [SerializeField] private float maxPitch = 80f;
        
        [Header("카메라 달릴 때 모션")]
        [SerializeField] private float runBobFrequency = 9f;
        [SerializeField] private float runBobVertical = 0.035f;
        [SerializeField] private float runBobHorizontal = 0.018f;
        [SerializeField] private float runRoll = 0.5f;
        [SerializeField] private float bobReturnSmoothSpeed = 10f;

        private float _bobTime;
        private Vector3 _currentBobOffset;
        private float _currentRoll;

        private Player _player;
        private float _pitch;

        private bool _isRunning;

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
            Debug.Assert(_player != null, "플레이어 카메라 컨트롤러니까 오너가 플레이어야지!!");
            _player.UIInput.OnMousePosChanged += HandleLookInput;
        }
        
        public void SetIsRunning(bool isRunning) => _isRunning = isRunning;

        private void HandleLookInput(Vector2 delta)
        {
            _pitch -= delta.y;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            //y축 회전은 시네머신 컴포넌트로 하고 있음
        }
        
        private void UpdateRunBob(bool isRunning)
        {
            Vector3 targetOffset = Vector3.zero;
            float targetRoll = 0f;

            if (isRunning)
            {
                _bobTime += Time.deltaTime * runBobFrequency;

                float vertical = Mathf.Abs(Mathf.Sin(_bobTime));
                float horizontal = Mathf.Sin(_bobTime * 0.5f);

                targetOffset = new Vector3(
                    horizontal * runBobHorizontal,
                    vertical * runBobVertical,
                    0f);

                targetRoll = horizontal * runRoll;
            }
            else
            {
                _bobTime = 0f;
            }

            _currentBobOffset = Vector3.Lerp(
                _currentBobOffset,
                targetOffset,
                Time.deltaTime * bobReturnSmoothSpeed);

            _currentRoll = Mathf.Lerp(
                _currentRoll,
                targetRoll,
                Time.deltaTime * bobReturnSmoothSpeed);
        }

        private void LateUpdate()
        {
            UpdateRunBob(_isRunning);
            
            Vector3 bobWorldOffset =
                _player.transform.TransformVector(_currentBobOffset);

            transform.position =
                eyeAnchor.position + bobWorldOffset;

            transform.localRotation = Quaternion.Euler(
                _pitch,
                0f,
                _currentRoll);
        }

        private void OnDestroy()
        {
            _player.UIInput.OnMousePosChanged -= HandleLookInput;
        }
    }
}