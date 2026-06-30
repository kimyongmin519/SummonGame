using UnityEngine;

namespace GateSystems.GateSystems
{
    [RequireComponent(typeof(Collider))]
    public class GateEntryTrigger : MonoBehaviour
    {
        [SerializeField] private GateMap gate;

        private void Reset()
        {
            gate = GetComponentInParent<GateMap>();
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            gate.NotifyPlayerEntered();
        }
    }
}
