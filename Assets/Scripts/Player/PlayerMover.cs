using Agents;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour, IMover
    {
        public bool CanManualMove { get; set; }
        public Vector3 Velocity { get; set; }
        public void SetAutoVelocity(Vector3 velocity)
        {
            
        }

        public void SetMoveDir(Vector3 moveDirection)
        {
        }

        public void SetCurrentSpeed(float speed)
        {
        }

        public void AddForceToMover(Vector3 force)
        {
        }

        public void RotateTo(Vector3 direction)
        {
        }

        public void StopImmediately(bool stopX, bool stopY, bool stopZ)
        {
        }
    }
}