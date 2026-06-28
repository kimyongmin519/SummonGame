using System;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters.NavAgentSystems
{
    public interface INavMover
    {
        NavMeshAgent NavAgent { get; }
        Vector3 NavVelocity { get; set; }
        float Speed { get; set; }
        bool IsStopped { get; set; }
        bool IsArrived { get; } //get만
        bool IsGrounded { get; }
        event Action<bool> OnGroundStatusChange;
        void SetDestination(Vector3 destination);
        public void SetSamplePosition();
        void ApplyKnockback(Vector3 direction, float force, float duration);
    }
}