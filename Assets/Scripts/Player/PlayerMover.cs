using System;
using Agents;
using CoreSystems.AnimationSystems;
using KimLIb.ModuleSystems;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour, IMover, IModule
    {
        public bool CanManualMove { get; set; } = true;

        private Player _player;
        private CharacterController _characterController;
        private Vector3 _moveDirection; //받은 y값을 z로
        private Vector3 _velocity;
        private float _verticalVelocity;
        private Vector3 _autoVelocity;
        private float _gravity;
        private float _currentSpeed;
        
        public bool IsGround => _characterController.isGrounded;

        private float _mouseXPadding;

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
            Debug.Assert(_player != null, "플레이어 무버는 오너가 반드시 플레이어컨트롤러여야합니다!");
            _characterController = _player.GetComponent<CharacterController>();

            _player.UIInput.OnMousePosChanged += HandleMouseDeltaX;
            
            _gravity = Physics.gravity.y;
        }

        private void HandleMouseDeltaX(Vector2 pos)
        {
            _mouseXPadding += pos.x;
        }

        private void OnDestroy()
        {
            _player.UIInput.OnMousePosChanged -= HandleMouseDeltaX;
        }

        public void SetAutoVelocity(Vector3 velocity)
        {
            _moveDirection = Vector3.zero;
            _autoVelocity = velocity;
        }

        public void SetMoveDir(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
        }
        
        public void SetCurrentSpeed(float speed)
        {
            _currentSpeed = speed;
        }
        
        private void FixedUpdate()
        {
            CalculateVelocity();
            ApplyGravity();
            Move();
        }

        private void CalculateVelocity()
        {
            _velocity = _moveDirection * (_currentSpeed * Time.fixedDeltaTime);
            
            if (Mathf.Abs(_mouseXPadding) < Mathf.Epsilon)
                return;

            _player.transform.Rotate(Vector3.up, _mouseXPadding, Space.World);

            _mouseXPadding = 0f;
        }
        
        private void Move()
        {
            _characterController.Move(_velocity);
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f;
            else
                _verticalVelocity += _gravity * Time.fixedDeltaTime;

            _velocity.y = _verticalVelocity * (Time.fixedDeltaTime * 5);
        }
        
        public void RotateTo(Vector3 direction)
        {
            if (direction.magnitude < Mathf.Epsilon) return;
            direction.y = 0;
            _player.transform.forward = direction.normalized;
        }
        

        public void StopImmediately(bool stopX, bool stopY, bool stopZ)
        {
            if (stopX)
                _velocity.x = 0;
            if (stopY)
                _velocity.y = 0;
            if (stopZ)
                _velocity.z = 0;
        }
    }
}