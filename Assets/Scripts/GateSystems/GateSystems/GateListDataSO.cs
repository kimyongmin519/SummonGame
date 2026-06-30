using System;
using System.Collections.Generic;
using UnityEngine;

namespace GateSystems.GateSystems
{
    [Serializable]
    public class FixedGateEntry
    {
        [field: SerializeField, Min(1)]
        public int GateNumber { get; private set; } = 1;

        [field: SerializeField]
        public GateVariantSO Variant { get; private set; }
    }

    [CreateAssetMenu(fileName = "Gate set data", menuName = "GateSystems/Gate Set")]
    public class GateListDataSO : ScriptableObject
    {
        [field: SerializeField, Min(1)]
        public int TotalGateCount { get; private set; } = 13;

        [field: Header("일반 게이트에서 선택할 전체 방 풀")]
        [field: SerializeField]
        public GateVariantSO[] RandomVariants { get; private set; }

        [field: Header("특정 번호에 반드시 나올 고정 방")]
        [field: SerializeField]
        public FixedGateEntry[] FixedGates { get; private set; }

        public bool TryGetFixedVariant(
            int gateNumber,
            out GateVariantSO variant)
        {
            if (FixedGates != null)
            {
                foreach (FixedGateEntry entry in FixedGates)
                {
                    if (entry == null ||
                        entry.GateNumber != gateNumber)
                    {
                        continue;
                    }

                    variant = entry.Variant;
                    return variant != null;
                }
            }

            variant = null;
            return false;
        }

        private void OnValidate()
        {
            if (FixedGates == null)
                return;

            HashSet<int> gateNumbers = new();

            foreach (FixedGateEntry entry in FixedGates)
            {
                if (entry == null)
                    continue;

                if (entry.GateNumber > TotalGateCount)
                {
                    Debug.LogError(
                        $"Gate {entry.GateNumber}은 " +
                        $"전체 게이트 수 {TotalGateCount}를 초과합니다.",
                        this);
                }

                if (!gateNumbers.Add(entry.GateNumber))
                {
                    Debug.LogError(
                        $"Gate {entry.GateNumber}의 " +
                        "고정 방이 중복되었습니다.",
                        this);
                }
            }
        }
    }
}