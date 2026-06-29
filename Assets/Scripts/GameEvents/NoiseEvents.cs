using KimLIb.EventSystem;
using UnityEngine;

namespace GameEvents
{
    public enum NoiseType
    {
        CrouchStep,
        Footstep,
        Running,
        Door,
        Interaction,
        Impact
    }
    
    public static class NoiseEvents
    {
        public static NoiseEvent NoiseEvent = new NoiseEvent();
    }
    
    public class NoiseEvent : GameEvent
    {
        public Vector3 Position;
        public GameObject Source;

        public float Radius;
        public float Intensity;

        public NoiseType Type;

        public NoiseEvent Init(Vector3 position, GameObject source, float radius, float intensity, NoiseType type)
        {
            Position = position;
            Source = source;
            Radius = radius;
            Intensity = intensity;
            Type = type;
            return this;
        }
    }
}