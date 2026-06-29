using System;
using Monsters.NavAgentSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChaseToTarget", story: "[Monster] chase to [TargetGameObject]", category: "Action", id: "c5b18db561bc7c5d06fc84c454d857a8")]
    public partial class ChaseToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        private Vector3 _destination;
        private INavMover _navMovement;
        
        protected override Status OnStart()
        {
            if (Monster.Value == null || TargetGameObject.Value == null || Monster.Value.NavMover == null || Monster.Value.NavMover.NavAgent.enabled == false)
                return Status.Failure;
            
            _destination = TargetGameObject.Value.transform.position;
            _navMovement = Monster.Value.NavMover;
            _navMovement.SetDestination(_destination);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(TargetGameObject.Value == null) //쫒던 적이 사망하면.
                return Status.Failure;
            
            Vector3 newDestination = TargetGameObject.Value.transform.position;
            float delta = Vector3.Distance(_destination, newDestination);
            if (delta > 1f)
            {
                _destination = newDestination;
                _navMovement.SetDestination(_destination);
            }

            return _navMovement.IsArrived ? Status.Success  : Status.Running; //running이다 Fail아니야.
        }
    }
}

