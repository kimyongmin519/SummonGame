using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMonster", story: "Stop [Monster]", category: "Action", id: "fba661f89d48c9f16a79af4fdcccb81f")]
    public partial class StopMonsterAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;

        protected override Status OnStart()
        {
            if(Monster.Value == null || Monster.Value.NavMover == null)
                return Status.Failure;
            
            if (Monster.Value.NavMover.NavAgent.enabled)
                Monster.Value.NavMover.NavAgent.ResetPath();
            return Status.Success;
        }
    }
}

