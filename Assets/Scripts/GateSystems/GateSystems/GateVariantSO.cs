using UnityEngine;

namespace GateSystems.GateSystems
{
    [CreateAssetMenu(fileName = "Gate variant data", menuName = "GateSystems/Gate Variant", order = 0)]
    public class GateVariantSO : ScriptableObject
    {
        [field: SerializeField]
        public string VariantId { get; private set; }

        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: SerializeField, Min(0f)]
        public float Weight { get; private set; } = 1f;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(VariantId) && Prefab != null)
                VariantId = Prefab.name;
        }
    }
}
