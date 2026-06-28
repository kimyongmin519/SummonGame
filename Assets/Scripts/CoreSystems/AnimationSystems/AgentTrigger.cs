using System;
using KimLIb.ModuleSystems;
using UnityEngine;

namespace CoreSystems.AnimationSystems
{
    public class AgentTrigger : MonoBehaviour, IModule
    {
        private ModuleOwner _owner;
        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        public event Action OnAttackEndTrigger;
        public event Action OnEffectTrigger;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void OnAnimationEndInvoke() => OnAnimationEnd?.Invoke();
        public void OnAttackTriggerInvoke() => OnAttackTrigger?.Invoke();
        public void OnAttackEndTriggerInvoke() => OnAttackEndTrigger?.Invoke();
        public void OnEffectTriggerInvoke() => OnEffectTrigger?.Invoke();
    }
}