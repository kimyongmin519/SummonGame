using UnityEngine;

namespace Agents
{
    public interface IRenderer
    {
        Animator Animator { get; }
        void PlayClip(int clipHash, float crossFadeDuration = 0.2f, int layerIndex = 0, float normalizedTime = 0);
    }
}