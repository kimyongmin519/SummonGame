using System;
using Agents;
using KimLIb.ModuleSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters.NavAgentSystems
{
    public class NavAgentRenderer : AgentRenderer, IAfterInitModule
    {
        [Header("NavAgent의 position및 rotation 제어여부")]
        [SerializeField] private bool updateRotation;
        [SerializeField] private bool updatePosition;

        [Header("NavAgent의 회전을 무시하고 강제로 회전시키기")]
        [SerializeField] private bool forceRotation;
        [SerializeField] private float forceRotationSpeed;
        
        private INavMover _navMover;
        private NavMeshAgent _navAgent;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        
        [Header("RootMotion NavMesh 보정")]
        [SerializeField] private float navMeshSampleDistance = 0.5f;
        [SerializeField] private float maxAgentDrift = 0.6f;

        private NavMeshQueryFilter _navMeshFilter;

        public bool UpdateRotationByAnimator
        {
            get => !updateRotation;
            set
            {
                updateRotation = !value;
                if (_navAgent != null)
                {
                    _navAgent.updateRotation = updateRotation;
                }
            }
        }
        

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _navMover = owner.GetModule<INavMover>();
            Debug.Assert(_navMover != null, "NavAgentRenderer는 INavMovement가 필요합니다.");
        }

        public void AfterInit()
        {
            _navAgent = _navMover.NavAgent;
            _navAgent.updatePosition = updatePosition;
            _navAgent.updateRotation = updateRotation;
            
            _navAgent = _navMover.NavAgent;

            _navMeshFilter = new NavMeshQueryFilter
            {
                agentTypeID = _navAgent.agentTypeID,
                areaMask = _navAgent.areaMask
            };
        }

        private void Update()
        {
            ForceRotationControl();
        }

        private void OnAnimatorMove()
        {
            if (_navAgent == null ||
                !_navAgent.enabled ||
                !_navAgent.isOnNavMesh)
            {
                return;
            }

            if (!updatePosition)
                ApplyConstrainedRootMotion();

            if (UpdateRotationByAnimator && !forceRotation)
            {
                _owner.transform.rotation =
                    Animator.rootRotation;
            }
        }
        
        private void ApplyConstrainedRootMotion()
        {
            Vector3 agentPosition = _navAgent.nextPosition;
            
            Vector3 nextFramePosition = _owner.transform.position + Animator.deltaPosition; // 이번 프레임 루트모션으로 이동하려는 위치
            
            nextFramePosition.y = agentPosition.y;
            
            Vector3 drift = nextFramePosition - agentPosition;// Agent가 계산한 경로에서 너무 멀어지는 것을 방지

            drift.y = 0f;

            if (drift.sqrMagnitude >
                maxAgentDrift * maxAgentDrift)
            {
                drift = drift.normalized * maxAgentDrift;

                nextFramePosition =
                    agentPosition + drift;

                nextFramePosition.y =
                    agentPosition.y;
            }

            // Harrow Agent Type의 NavMesh 위로 옮기기
            if (NavMesh.SamplePosition(nextFramePosition, out NavMeshHit hit, navMeshSampleDistance, _navMeshFilter))
            {
                nextFramePosition = hit.position;
            }
            else
            {
                nextFramePosition = agentPosition; //못찾으면 이탈이라도 하지않게 복귀
            }

            _owner.transform.position = nextFramePosition;

            _navAgent.nextPosition = nextFramePosition;
        }
        
        private void ForceRotationControl()
        {
            if(!forceRotation || _navAgent == null || _navMover.IsArrived) 
                return;

            Vector3 desiredDirection = _navAgent.steeringTarget - _owner.transform.position; //다음 바라보는 지점
            if (desiredDirection.sqrMagnitude < 0.01f) return; //완전 정지상태에 가깝다면 회전안함.
            
            desiredDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection.normalized);
            _owner.transform.rotation = Quaternion.RotateTowards(
                _owner.transform.rotation, targetRotation, forceRotationSpeed * Time.deltaTime);
        }
    }
}