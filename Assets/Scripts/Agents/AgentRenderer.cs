using CoreSystems.AnimationSystems;
using KimLIb.ModuleSystems;
using UnityEngine;

namespace Agents
{
    public class AgentRenderer : MonoBehaviour, IRenderer, IModule
    {
        public Animator Animator { get; set;}
        protected ModuleOwner _owner;
        
        public virtual void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            Animator = GetComponent<Animator>();
        }

        public void PlayClip(int clipHash, float crossFadeDuration = 0.2f, int layerIndex = 0, float normalizedTime = 0)
        {
            Animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }
    }
}