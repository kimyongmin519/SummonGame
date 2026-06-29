using System;
using Agents;
using CoreSystems.AnimationSystems;
using CoreSystems.MapSystems;
using Monsters.BT;
using Monsters.NavAgentSystems;
using Reflex.Attributes;
using Unity.Behavior;
using UnityEngine;

namespace Monsters
{
    public abstract class AbstractMonster : Agent
    {
        #region 임시 코드

        [field:SerializeField] public float DetectRadius { get; private set; }
        [field:SerializeField] public float ViewAngle {get; private set;}

        #endregion
        
        public BehaviorGraphAgent BTAgent { get; private set; }
        public INavMover NavMover { get; private set; }
        public IRenderer AgentRenderer { get; private set; }
        public AgentTrigger Trigger { get; private set; }
        public AgentSensor Sensor { get; private set; }
        [field:Header("맵 관련 (리플렉스)")]
        [Inject] [field: SerializeField] public WayPoints StageWayPoints { get; private set; }
        public int CurrentWayPoint { get; set; } = -1;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            BTAgent = GetComponent<BehaviorGraphAgent>();
            NavMover = GetModule<INavMover>();
            AgentRenderer = GetModule<IRenderer>();
            Trigger = GetModule<AgentTrigger>();
            Sensor = GetModule<AgentSensor>();
        }

        protected virtual void Start()
        {
            BTAgent.SetVariableValue(BTVar.Monster, this);
        }
    }
}