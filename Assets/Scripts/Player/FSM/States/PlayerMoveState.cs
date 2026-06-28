using Agents.FSM;
using UnityEngine;

namespace Player.FSM.States
{
    public class PlayerMoveState : AbstractPlayerState
    {
        public PlayerMoveState(Player player, int clipHash) : base(player, clipHash)
        {
        }

        public override void Enter(float transition = 0.2f, int layerIndex = 0)
        {
            base.Enter(transition,layerIndex);
            _mover.SetCurrentSpeed(_player.MoveSpeed);
            
            _player.PlayerInput.OnMoveChanged += HandlePlayerMovement;
            _player.CameraController.SetIsRunning(true);
        }
        
        private void HandlePlayerMovement(Vector2 movement)
        {
            Vector3 cameraRight = _player.UIInput.MainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            
            Vector3 cameraForward = Vector3.Cross(cameraRight, _player.transform.up); //두 벡터의 수직 방향을 반환한다.
            
            Vector3 realMoveDir =
                (movement.x * cameraRight) + (movement.y * cameraForward);
            
            _mover.SetMoveDir(realMoveDir);
            
            if (movement.sqrMagnitude < INPUT_DEADLINE)
            {
                _player.ChangeState(PlayerStateEnum.IDLE);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _player.PlayerInput.OnMoveChanged -= HandlePlayerMovement;
            _player.CameraController.SetIsRunning(false);
        }
    }
}