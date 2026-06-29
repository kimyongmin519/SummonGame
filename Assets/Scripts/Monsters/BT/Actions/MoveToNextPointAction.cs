using System;
using CoreSystems.MapSystems;
using Monsters.NavAgentSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToNextPoint", story: "[Monster] move to point", category: "Action", id: "60e45897c220f44f1a401ee73f23fe14")]
    public partial class MoveToNextPointAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;

        private INavMover _navMovement;
        
        protected override Status OnStart()
        {
            if(Monster.Value == null || Monster.Value.NavMover == null || Monster.Value.StageWayPoints == null)
                return Status.Failure;
            
            _navMovement = Monster.Value.NavMover;
            WayPoints stageWayPoints = Monster.Value.StageWayPoints;

            int index = Monster.Value.CurrentWayPoint;
            
            index = index < 0 ? stageWayPoints.GetClosestPointIndexFromPosition(Monster.Value.transform.position)
                : stageWayPoints.GetNextWayPoint(index);
            
            if(index < 0) return Status.Failure;
            
            Monster.Value.CurrentWayPoint = index; //갱신
            
            WayPoint targetPoint = stageWayPoints[index];
            _navMovement.SetDestination(targetPoint.Position);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_navMovement.IsArrived)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}

