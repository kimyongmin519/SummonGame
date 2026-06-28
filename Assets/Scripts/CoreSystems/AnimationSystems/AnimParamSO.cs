using UnityEngine;

namespace CoreSystems.AnimationSystems
{
    [CreateAssetMenu(fileName = "Anim param", menuName = "Animator/Anim params", order = 0)]
    public class AnimParamSO : ScriptableObject
    {
        [field: SerializeField] public string ParamName { get; private set; }
        [field: SerializeField] public int ParamHash { get; private set; }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                ParamHash = Animator.StringToHash(ParamName);
            }
        }
    }
}