using System;
using Agents;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[Monster] find [TargetGameObject]", category: "Action", id: "054f147086e75d6383886642a2f61778")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        protected override Status OnStart()
        {
            //이미 적을 감지했거나 값들이 잘못들어가 있으면 Fail
            if (Monster.Value == null)
                return Status.Failure;

            if (TargetGameObject.Value != null)
                return Status.Success;

            AgentSensor sensor = Monster.Value.Sensor;

            int detectCount = sensor.FindTargetsInRadius(Monster.Value.DetectRadius);
            if(detectCount <= 0) return Status.Failure;


            for (int i = 0; i < detectCount; i++)
            {
                Transform findTarget = sensor.ColliderResults[i].transform;
                if (!sensor.IsTargetInViewAngle(findTarget, Monster.Value.ViewAngle))
                    continue; //시야각 안에 없다면 실패
                if(!sensor.IsTargetIsInSight(findTarget))
                    continue; //시야에 장애물이 있다면 실패
                
                TargetGameObject.Value = findTarget.gameObject;
                break;
            }
            
            return TargetGameObject.Value == null ? Status.Failure : Status.Success;
        }
    }
}

