using Agents;
using UnityEngine;

namespace Player.FSM.States
{
    public class PlayerStandingState : AbstractPlayerStanceState
    {
        public PlayerStandingState(Player player, IMover mover, Animator animator, CharacterController characterController, PlayerSettingDataSO settingsData) : base(player, mover, animator, characterController, settingsData)
        {
        }

        public override void Enter()
        {
            CharacterController.height =
                SettingsData.standingHeight;

            CharacterController.center =
                SettingsData.standingCenter;
        }

        public override void Update()
        {
            Animator.SetFloat(CrouchHash, 0f, SettingsData.blendTime, Time.deltaTime);
        }
    }
}