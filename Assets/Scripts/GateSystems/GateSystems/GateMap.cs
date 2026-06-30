using UnityEngine;

namespace GateSystems.GateSystems
{
    public class GateMap : MonoBehaviour
    {
        [field: SerializeField]
        public Transform EntryPoint { get; private set; }

        [field: SerializeField]
        public Transform ExitPoint { get; private set; }

        public int GateNumber { get; private set; }
        public GateVariantSO Variant { get; private set; }

        private GateFlowManager _flowManager;
        private bool _entered;

        public void Initialize(
            int gateNumber,
            GateVariantSO variant,
            GateFlowManager flowManager)
        {
            GateNumber = gateNumber;
            Variant = variant;
            _flowManager = flowManager;
            _entered = false;

            gameObject.name = $"Gate_{gateNumber:00}_{variant.VariantId}";
        }

        public void NotifyPlayerEntered()
        {
            if (_entered)
                return;

            _entered = true;
            _flowManager.NotifyGateEntered(this);
        }

        private void OnValidate()
        {
            Debug.Assert(EntryPoint != null, $"{name}: EntryPoint가 필요합니다.");
            Debug.Assert(ExitPoint != null, $"{name}: ExitPoint가 필요합니다.");
        }
    }
}