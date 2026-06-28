using Agents;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters
{
    public class PatrolMonster : Agent
    {
        [field:SerializeField] public BehaviorGraphAgent BTAgent { get; private set; }
        public NavMeshAgent NavAgent { get; private set; }
        public INavMover NavMover { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            NavAgent = GetComponent<NavMeshAgent>();
            NavMover = GetModule<INavMover>();
        }

        protected override void HandleHitEvent()
        {
            
        }
    }
}