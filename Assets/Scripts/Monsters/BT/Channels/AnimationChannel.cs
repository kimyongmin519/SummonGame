using System;
using CoreSystems.AnimationSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Monsters.BT.Channels
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChannel", message: "[AnimParam]", category: "Events", id: "595b12cde6725e5a25661432cbdf327b")]
    public sealed partial class AnimationChannel : EventChannel<AnimParamSO> { }
}

