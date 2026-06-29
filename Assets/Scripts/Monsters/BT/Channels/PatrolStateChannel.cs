using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Monsters.BT.Channels
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/PatrolStateChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "PatrolStateChannel", message: "[PatrolStateEnum]", category: "Events", id: "41ffddc72c7236cb96826d91a5dcea43")]
    public sealed partial class PatrolStateChannel : EventChannel<PatrolMonsterEnum> { }
}

