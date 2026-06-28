using Agents.FSM;
using UnityEngine;

namespace Player.FSM.States
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public PlayerIdleState(Player player, int clipHash) : base(player, clipHash)
        {
        }

        public override void Enter(float transition = 0.2f, int layerIndex = 0)
        {
            base.Enter(transition,layerIndex);
            _mover.SetMoveDir(Vector3.zero);
            
            _player.PlayerInput.OnMoveChanged += HandleMoveChange;
        }


        private void HandleMoveChange(Vector2 movement)
        {
            if (movement.sqrMagnitude > INPUT_DEADLINE)
            {
                _player.ChangeState(PlayerStateEnum.MOVE);
            }
        }

        public override void Exit()
        {
            _player.PlayerInput.OnMoveChanged -= HandleMoveChange;
            base.Exit();
        }
    }
}