using System;
using CoreSystems.AnimationSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "PlayClip", story: "[Monster] play [AnimParamClip]", category: "Action", id: "e9c36568ad7121542a81bcccd4641e54")]
    public partial class PlayClipAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;
        [SerializeReference] public BlackboardVariable<AnimParamSO> AnimParamClip;
        
        [SerializeReference] public BlackboardVariable<int> Layer;
        [SerializeReference] public BlackboardVariable<float> Position;
        [SerializeReference] public BlackboardVariable<float> CrossDuration = new(0.2f);

        protected override Status OnStart()
        {
            if (Monster.Value == null || Monster.Value.AgentRenderer == null || AnimParamClip.Value == null)
                return Status.Failure;
            
            Monster.Value.AgentRenderer.PlayClip(AnimParamClip.Value.ParamHash, CrossDuration.Value, Layer.Value, Position.Value);
            return Status.Success;
        }
    }
}

