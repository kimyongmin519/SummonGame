using Agents.FSM;
using UnityEngine;

namespace Player.FSM.States
{
    public class PlayerMoveState : AbstractPlayerState
    {
        private Vector2 _movement;
        public PlayerMoveState(Player player, int clipHash) : base(player, clipHash)
        {
        }

        public override void Enter(float transition = 0.2f, int layerIndex = 0)
        {
            base.Enter(transition,layerIndex);
            _player.PlayerInput.OnMoveChanged += HandleMovementChange;
            
            _player.CameraController.SetIsRunning(true);
        }

        public override void Update()
        {
            base.Update();
            Vector3 cameraRight = _player.UIInput.MainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            
            Vector3 cameraForward = Vector3.Cross(cameraRight, _player.transform.up); //두 벡터의 수직 방향을 반환한다.
            
            Vector3 realMoveDir =
                (_movement.x * cameraRight) + (_movement.y * cameraForward).normalized;
            
            _mover.SetMoveDir(realMoveDir);
            _renderer.Animator.SetFloat(MoveXHash, _movement.x, _player.PlayerSettingData.blendTime, Time.deltaTime);
            _renderer.Animator.SetFloat(MoveZHash, _movement.y,_player.PlayerSettingData.blendTime, Time.deltaTime);
            
            Debug.Log(_movement.y < 0 && !_player.IsCrouching);
            if (_movement.y < 0 && !_player.IsCrouching)
                _mover.SetCurrentSpeed(_player.PlayerSettingData.backStandSpeed);
            else if (!_player.IsCrouching)
                _mover.SetCurrentSpeed(_player.PlayerSettingData.standSpeed);
            else
                _mover.SetCurrentSpeed(_player.PlayerSettingData.crouchSpeed);
            
            if (_movement.sqrMagnitude < INPUT_DEADLINE)
            {
                _player.ChangeState(PlayerStateEnum.IDLE);
            }
            
        }
        
        private void HandleMovementChange(Vector2 movement) => _movement = movement;

        public override void Exit()
        {
            base.Exit();
            _player.PlayerInput.OnMoveChanged -= HandleMovementChange;
            _player.CameraController.SetIsRunning(false);
        }
    }
}