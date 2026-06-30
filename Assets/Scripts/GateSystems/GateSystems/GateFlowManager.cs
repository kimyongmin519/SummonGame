using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GateSystems.GateSystems
{
    public class GateFlowManager : MonoBehaviour
    {
        [Header("게이트 리스트")]
        [SerializeField] private GateListDataSO gateListData;

        [Header("세팅 값")]
        [SerializeField] private Transform firstGateEntry;
        [SerializeField, Min(1)] private int firstGateNumber = 1;
        [SerializeField, Min(0)] private int maxBehindNumber = 1;
        [SerializeField, Min(0)] private int maxFrontNumber = 1;

        public int CurrentGateNumber { get; private set; }

        private readonly List<GateMap> _activeGates = new();
        
        private void Awake()
        {
            Debug.Assert(
                gateListData != null,
                "GateFlowManager에 GateSet이 필요합니다.");

            Debug.Assert(
                firstGateEntry != null,
                "첫 게이트 생성 기준점이 필요합니다.");
        }

        private void Start()
        {
            if (gateListData == null || firstGateEntry == null)
                return;

            CurrentGateNumber = firstGateNumber;

            int targetGateNumber = Mathf.Min(
                CurrentGateNumber + maxFrontNumber,
                gateListData.TotalGateCount);

            GenerateThrough(targetGateNumber);
        }

        public GateMap GenerateGate(int gateNumber, Transform previousExit)
        {
            if (gateNumber < 1 ||
                gateNumber > gateListData.TotalGateCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(gateNumber),
                    $"Gate 번호는 1부터 " +
                    $"{gateListData.TotalGateCount}까지 가능합니다.");
            }

            if (TryGetActiveGate(gateNumber, out GateMap activeGate))
            {
                return activeGate;
            }

            GateVariantSO variant = SelectVariant(gateNumber);

            GameObject instance = Instantiate(variant.Prefab);
            GateMap gate = instance.GetComponent<GateMap>();

            if (gate == null)
            {
                Destroy(instance);

                throw new MissingComponentException($"{variant.Prefab.name}에 GateMap 컴포넌트가 없습니다.");
            }

            if (gate.EntryPoint == null ||
                gate.ExitPoint == null)
            {
                Destroy(instance);

                throw new MissingReferenceException($"{variant.Prefab.name}의 EntryPoint 또는 ExitPoint가 없습니다.");
            }

            AlignGate(gate.transform, gate.EntryPoint, previousExit);

            gate.Initialize(gateNumber, variant, this);

            _activeGates.Add(gate);
            _activeGates.Sort((a, b) => a.GateNumber.CompareTo(b.GateNumber));

            return gate;
        }

        public void NotifyGateEntered(GateMap gate)
        {
            if (gate == null ||
                gate.GateNumber < CurrentGateNumber)
            {
                return;
            }

            CurrentGateNumber = gate.GateNumber;

            int targetGateNumber = Mathf.Min(CurrentGateNumber + maxFrontNumber, gateListData.TotalGateCount);

            GenerateThrough(targetGateNumber);
            RemoveOldGates();
        }

        private GateVariantSO SelectVariant(int gateNumber)
        {
            // 고정방이 존재하면 랜덤 선택보다 우선한다.
            if (gateListData.TryGetFixedVariant(
                    gateNumber,
                    out GateVariantSO fixedVariant))
            {
                return fixedVariant;
            }

            return SelectWeightedRandomVariant();
        }

        private GateVariantSO SelectWeightedRandomVariant()
        {
            GateVariantSO[] randomVariants = gateListData.RandomVariants ?? Array.Empty<GateVariantSO>();

            List<GateVariantSO> candidates =
                randomVariants.Where(variant =>
                        variant != null &&
                        variant.Prefab != null &&
                        variant.Weight > 0f)
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException("GateSet의 랜덤 방 풀에 생성 가능한 GateVariant가 없습니다.");
            }

            float totalWeight =
                candidates.Sum(x => x.Weight);

            float selectedPoint =
                UnityEngine.Random.value * totalWeight;

            foreach (GateVariantSO candidate in candidates)
            {
                selectedPoint -= candidate.Weight;

                if (selectedPoint <= 0f)
                    return candidate;
            }

            return candidates[^1];
        }

        private void GenerateThrough(int targetGateNumber)
        {
            int nextGateNumber = _activeGates.Count == 0
                    ? firstGateNumber : _activeGates[^1].GateNumber + 1; //^1은 마지막의 첫번째 요소

            Transform previousExit = _activeGates.Count == 0 ? 
                firstGateEntry : _activeGates[^1].ExitPoint;

            while (nextGateNumber <= targetGateNumber)
            {
                GateMap gate = GenerateGate(nextGateNumber, previousExit);

                previousExit = gate.ExitPoint;
                nextGateNumber++;
            }
        }

        private static void AlignGate(Transform gateRoot, Transform entryPoint, Transform previousExit)
        {
            // Entry와 이전 Exit가 서로 마주보게 한다.
            Quaternion targetEntryRotation =
                previousExit.rotation
                * Quaternion.Euler(0f, 180f, 0f);

            Quaternion rotationDelta =
                targetEntryRotation
                * Quaternion.Inverse(entryPoint.rotation);

            gateRoot.rotation =
                rotationDelta * gateRoot.rotation;

            gateRoot.position +=
                previousExit.position - entryPoint.position;
        }

        private void RemoveOldGates()
        {
            int minimumGateNumber =
                CurrentGateNumber - maxBehindNumber;

            for (int i = _activeGates.Count - 1;
                 i >= 0;
                 i--)
            {
                GateMap gate = _activeGates[i];

                if (gate.GateNumber >= minimumGateNumber)
                    continue;

                _activeGates.RemoveAt(i);
                Destroy(gate.gameObject);
            }
        }

        private bool TryGetActiveGate(int gateNumber, out GateMap gate)
        {
            gate = _activeGates.FirstOrDefault(
                x => x.GateNumber == gateNumber);

            return gate != null;
        }
    }
}