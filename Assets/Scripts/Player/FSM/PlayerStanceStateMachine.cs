using System.Collections.Generic;
using Agents;
using Agents.FSM;
using Player.FSM.States;
using UnityEngine;

namespace Player.FSM
{
    public class PlayerStanceStateMachine
    {
        public PlayerStanceStateEnum CurrentType
        {
            get;
            private set;
        }

        public bool IsCrouching =>
            CurrentType == PlayerStanceStateEnum.CROUCHING;

        private readonly Dictionary<PlayerStanceStateEnum, AbstractPlayerStanceState> _states;

        private readonly CharacterController _characterController;
        private readonly PlayerSettingDataSO _settings;

        private AbstractPlayerStanceState _currentState;
        private bool _initialized;

        public PlayerStanceStateMachine(Player player, IMover mover, Animator animator, CharacterController characterController, PlayerSettingDataSO settings)
        {
            _characterController = characterController;
            _settings = settings;

            _states = new()
            {
                {
                    PlayerStanceStateEnum.STANDING,
                    new PlayerStandingState(player, mover, animator, characterController, settings)
                },
                {
                    PlayerStanceStateEnum.CROUCHING,
                    new PlayerCrouchingState(player, mover, animator, characterController, settings)
                }
            };
        }

        public void Toggle()
        {
            if (IsCrouching)
            {
                if (CanStand())
                    ChangeState(PlayerStanceStateEnum.STANDING);
            }
            else
            {
                ChangeState(PlayerStanceStateEnum.CROUCHING);
            }
        }

        public void ChangeState(PlayerStanceStateEnum nextType)
        {
            if (_initialized && CurrentType == nextType)
                return;

            _currentState?.Exit();

            CurrentType = nextType;
            _currentState = _states[nextType];
            _initialized = true;

            _currentState.Enter();
        }

        public void UpdateMachine()
        {
            _currentState?.Update();
        }

        private bool CanStand()
        {
            Transform transform = _characterController.transform;
            float radius = _characterController.radius;
            Vector3 center = transform.TransformPoint(_settings.standingCenter);
            float halfSegment = Mathf.Max(0f, _settings.standingHeight * 0.5f - radius);
            Vector3 bottom = center - transform.up * halfSegment;
            Vector3 top = center + transform.up * halfSegment;
            return !Physics.CheckCapsule(bottom, top, radius, _settings.ceilingMask, QueryTriggerInteraction.Ignore);
        }
    }
}