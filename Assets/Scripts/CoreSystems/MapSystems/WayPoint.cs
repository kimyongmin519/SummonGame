using UnityEngine;

namespace CoreSystems.MapSystems
{
    public class WayPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            float height = 10f;
            
            Gizmos.color = Color.orangeRed;
            Gizmos.DrawCube(Position + Vector3.up * (height / 2),new Vector3(0.25f,height,0.25f));
        }
    }
}