using Agents;
using UnityEngine;

namespace Player.FSM.States
{
    public abstract class AbstractPlayerStanceState
    {
        protected static readonly int CrouchHash = Animator.StringToHash("Crouch");

        protected readonly Player Player;
        protected readonly IMover Mover;
        protected readonly Animator Animator;
        protected readonly CharacterController CharacterController;
        protected readonly PlayerSettingDataSO SettingsData;

        protected AbstractPlayerStanceState(Player player, IMover mover, Animator animator, CharacterController characterController, PlayerSettingDataSO settingsData)
        {
            Player = player;
            Mover = mover;
            Animator = animator;
            CharacterController = characterController;
            SettingsData = settingsData;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public abstract void Update();
    }
}