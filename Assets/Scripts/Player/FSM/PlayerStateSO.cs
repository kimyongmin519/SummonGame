using CoreSystems.AnimationSystems;
using UnityEngine;

namespace Player.FSM
{
    [CreateAssetMenu(fileName = "player State data", menuName = "FSM/State SO", order = 0)]
    public class PlayerStateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public int stateIndex;
        public AnimParamSO stateParam;
    }
}