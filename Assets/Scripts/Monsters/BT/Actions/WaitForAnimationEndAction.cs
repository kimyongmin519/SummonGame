using System;
using CoreSystems.AnimationSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimationEnd", story: "[Monster] wait for animationEnd", category: "Action", id: "06dcf5fdf0ab69f1ee351d4a7cf0bb9e")]
    public partial class WaitForAnimationEndAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;

        private AgentTrigger _trigger;
        private bool _isAnimationEnd;
        
        protected override Status OnStart()
        {
            if (Monster.Value == null || Monster.Value.Trigger == null)
                return Status.Failure;
            
            _isAnimationEnd = false;
            _trigger = Monster.Value.Trigger;
            _trigger.OnAnimationEnd += HandleAnimationEnd;
            return Status.Running;
        }

        private void HandleAnimationEnd() => _isAnimationEnd = true;

        protected override Status OnUpdate()
        {
            return _isAnimationEnd ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            if (_trigger != null)
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
        }
    }
}

