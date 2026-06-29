using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Monsters.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateTo", story: "[Monster] rotate to [TargetGameObject]", category: "Action", id: "746fe43433ee475379edc2c647add4df")]
    public partial class RotateToAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractMonster> Monster;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        [SerializeReference] public BlackboardVariable<float> RotateSpeed = new(10f);
        [SerializeReference] public BlackboardVariable<float> RotateDuration = new(0.4f);

        private float _startTime;

        protected override Status OnStart()
        {
            if (Monster.Value == null || TargetGameObject.Value == null)
                return Status.Failure;

            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_startTime + RotateDuration.Value < Time.time)
            {
                return Status.Success;
            }

            Vector3 direction = (TargetGameObject.Value.transform.position - Monster.Value.transform.position);
            direction.y = 0;
            if (direction.magnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                Monster.Value.transform.rotation = Quaternion.Lerp(
                    Monster.Value.transform.rotation, targetRotation, RotateSpeed.Value * Time.deltaTime);
            }
            
            return Status.Running;
        }
    }
}

