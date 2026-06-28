using UnityEngine;

namespace Player.FSM
{
    [CreateAssetMenu(fileName = "player State list", menuName = "FSM/Player State list", order = 10)]
    public class PlayerStateListSO : ScriptableObject
    {
        public string stateEnum;
        public PlayerStateSO[] states;
    }
}