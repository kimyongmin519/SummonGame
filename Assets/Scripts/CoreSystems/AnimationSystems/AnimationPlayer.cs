using UnityEngine;

namespace CoreSystems.AnimationSystems
{
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void PlayAnimation(AnimParamSO animParam)
        {
            animator.Play(animParam.ParamHash);
        }
    }
}