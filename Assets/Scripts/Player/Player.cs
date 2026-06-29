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
        [field:SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field:SerializeField] public UIInputSO UIInput { get; private set; }
        [field:SerializeField] public PlayerSettingDataSO PlayerSettingData { get; private set; }
        [SerializeField] private PlayerStateListSO stateList;
        public PlayerCameraController CameraController { get; private set; }
        private PlayerStateMachine _stateMachine;
        private PlayerStanceStateMachine _stanceStateMachine;
        public bool IsCrouching => _stanceStateMachine?.IsCrouching ?? false;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _stateMachine = new PlayerStateMachine(this, stateList.states);
            _stanceStateMachine = new PlayerStanceStateMachine(this, GetModule<IMover>(),
                GetModule<IRenderer>().Animator
                , GetComponent<CharacterController>(), PlayerSettingData);
            CameraController = GetModule<PlayerCameraController>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            PlayerInput.OnCrunchPressed += HandleCrouchPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerInput.OnCrunchPressed -= HandleCrouchPressed;
        }

        private void Start()
        {
            ChangeState(PlayerStateEnum.IDLE);
            _stanceStateMachine.ChangeState(PlayerStanceStateEnum.STANDING);
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
            _stanceStateMachine.UpdateMachine();
        }
        
        private void HandleCrouchPressed()
        {
            _stanceStateMachine.Toggle();
        }

        protected override void HandleHitEvent()
        {
            
        }
        
        public void ChangeState(PlayerStateEnum state)
            => _stateMachine.ChangeState((int)state);
    }
}