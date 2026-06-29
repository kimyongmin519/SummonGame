using UnityEngine;

namespace Player.FSM
{
    [CreateAssetMenu(fileName = "Player setting data", menuName = "CoreSystems/Player setting data", order = 0)]
    public class PlayerSettingDataSO : ScriptableObject
    {
        [Header("서있을 때")]
        public float standSpeed = 8f;
        public float backStandSpeed = 6f;
        
        [Header("앉았을 때")]
        public float crouchSpeed = 2.5f;

        [Header("서있을 때 콜라이더")]
        public float standingHeight = 2f;
        public Vector3 standingCenter =
            new(0f, 1f, 0f);

        [Header("앉아있을 때 콜라이더")]
        public float crouchingHeight = 1.2f;
        public Vector3 crouchingCenter =
            new(0f, 0.6f, 0f);

        [Header("그 외")]
        public LayerMask ceilingMask;
        public float blendTime = 0.15f;
    }
}