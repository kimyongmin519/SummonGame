using UnityEngine;

namespace Agents
{
    public interface IMover
    {
        bool CanManualMove { get; set; }
        Vector3 Velocity { get; set; }
        void SetAutoVelocity(Vector3 velocity);
        void SetMoveDir(Vector3 moveDirection);
        void SetCurrentSpeed(float speed);
        void AddForceToMover(Vector3 force);
        public void RotateTo(Vector3 direction);
        void StopImmediately(bool stopX, bool stopY, bool stopZ);
    }
}