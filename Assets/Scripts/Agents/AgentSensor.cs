using System;
using KimLIb.ModuleSystems;
using UnityEngine;

namespace Agents
{
    public class AgentSensor : MonoBehaviour, IModule
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private LayerMask whatIsObstacle;
        [SerializeField] private int maxColliderCount = 5;
        [SerializeField] private float eyesOffset;

        private ModuleOwner _owner;
        private Collider[] _colliderResults;
        
        public Collider[] ColliderResults => _colliderResults;

        [SerializeField] private bool isDebug = false;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            Debug.Assert(maxColliderCount > 0, $"최대 컬라이더 카운트는 반드시 0보다 커야합니다. : {gameObject}");
            _colliderResults = new Collider[maxColliderCount];
        }

        public bool IsTargetInViewAngle(Transform targetTrm, float viewAngle)
        {
            Vector3 direction = targetTrm.position - transform.position;
            direction.y = 0;
            float angle = Vector3.Angle(transform.forward, direction);
            return angle <= viewAngle * 0.5f;
        }

        public bool IsTargetIsInSight(Transform targetTrm)
        {
            Vector3 targetPosition = targetTrm.position;
            Vector3 eyePos = transform.position + Vector3.up * eyesOffset;
            Vector3 direction = targetPosition - eyePos;
            float distance = direction.magnitude;
            if (Physics.Raycast(eyePos, direction.normalized, out RaycastHit hit, distance, whatIsObstacle))
            {
                return false; //장애물이 시야를 가로막는중이다.
            }
            return true;
        }
        
        public bool IsTargetInViewRadius(Transform targetTrm, float viewRadius)
            => (targetTrm.position - transform.position).sqrMagnitude <=  viewRadius * viewRadius;
        public int FindTargetsInRadius(float viewRadius)
            => Physics.OverlapSphereNonAlloc(transform.position, viewRadius, _colliderResults, whatIsTarget);

        private void OnDrawGizmos()
        {
            if (!isDebug) return;
            Gizmos.color = Gizmos.color;
            Gizmos.DrawSphere(transform.position + Vector3.up * eyesOffset, 0.2f);
        }
    }
}