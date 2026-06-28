using System;
using Agents;
using Agents.FSM;
using CoreSystems;
using Player.FSM;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : Agent
    {
        #region 임시 코드

        [field:SerializeField] public float MoveSpeed { get; private set; }

        #endregion
        [field:SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field:SerializeField] public UIInputSO UIInput { get; private set; }
        [SerializeField] private PlayerStateListSO stateList;
        public PlayerCameraController CameraController { get; private set; }
        private PlayerStateMachine _stateMachine;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _stateMachine = new PlayerStateMachine(this, stateList.states);
            CameraController = GetModule<PlayerCameraController>();
        }

        private void Start()
        {
            ChangeState(PlayerStateEnum.IDLE);
        }

        protected override void HandleHitEvent()
        {
            
        }
        
        public void ChangeState(PlayerStateEnum state)
            => _stateMachine.ChangeState((int)state);
    }
}