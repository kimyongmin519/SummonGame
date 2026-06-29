using Agents;
using UnityEngine;

namespace Player.FSM.States
{
    public class PlayerCrouchingState : AbstractPlayerStanceState
    {
        public PlayerCrouchingState(Player player, IMover mover, Animator animator, CharacterController characterController, PlayerSettingDataSO settingsData) : base(player, mover, animator, characterController, settingsData)
        {
        }

        public override void Enter()
        {
            CharacterController.height = SettingsData.crouchingHeight;

            CharacterController.center = SettingsData.crouchingCenter;

            Mover.SetCurrentSpeed(SettingsData.crouchSpeed);
        }

        public override void Update()
        {
            Animator.SetFloat(CrouchHash, 1f, SettingsData.blendTime, Time.deltaTime); //뒤에 델타타임을 넣으면 이 델타타임에 따라서 값을 변화
        }
    }
}