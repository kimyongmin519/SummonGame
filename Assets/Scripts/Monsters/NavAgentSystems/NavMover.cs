using System;
using KimLIb.ModuleSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters.NavAgentSystems
{
    public class NavMover : MonoBehaviour, INavMover, IModule
    {
        [Header("그라운드 체크 그 값")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float groundedCheckInterval = 0.1f;
        private float _groundedCheckTimer;
        
        public NavMeshAgent NavAgent { get; private set; }
        private float _gravity;
        private float _verticalVelocity;
        
        private Coroutine _knockbackCoroutine;

        public Vector3 NavVelocity
        {
            get => NavAgent != null ? NavAgent.velocity : Vector3.zero;
            set
            {
                if(NavAgent != null)
                    NavAgent.velocity = value;
            }
        }

        public float Speed
        {
            get => NavAgent != null ? NavAgent.speed : 0f;
            set
            {
                if(NavAgent != null)
                    NavAgent.speed = value;
            }
        }

        public bool IsStopped
        {
            get => NavAgent != null && NavAgent.isStopped;
            set
            {
                if(NavAgent != null)
                    NavAgent.isStopped = value;
            }
        }
        public bool IsArrived 
            => NavAgent != null && NavAgent.enabled
               && (!NavAgent.pathPending && NavAgent.remainingDistance <= NavAgent.stoppingDistance * 0.5f);

        private bool _isGrounded;
        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                bool before = _isGrounded;
                _isGrounded =  value;
                if (before != _isGrounded)
                    OnGroundStatusChange?.Invoke(_isGrounded);
            }
        }
        public void Initialize(ModuleOwner owner)
        {
            NavAgent = owner.GetComponent<NavMeshAgent>();
            Debug.Assert(NavAgent != null, $"NavMovement는 반드시 owner NavMeshAgent가 있어야 합니다!!!!!! {gameObject.name}");
        }
        public event Action<bool> OnGroundStatusChange;
        public void SetDestination(Vector3 destination)
        {
            NavAgent.SetDestination(destination);
        }

        private void ResetOffMeshAndPath()
        {
            NavAgent.ResetPath(); //가려던 경로 리셋
            NavAgent.CompleteOffMeshLink(); //오프메시 링크의 마지막 지점으로 이동시키고 다시 경로 계산
        }

        public void SetSamplePosition()
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) //트랜스폼 위치를 중심으로 2반지름 반경만큼 모든 Areas를 조사
                NavAgent.Warp(hit.position); //찾는데 성공하면 이동 시키기
        }

        public void ApplyKnockback(Vector3 direction, float force, float duration)
        {
            
        }

    }
}