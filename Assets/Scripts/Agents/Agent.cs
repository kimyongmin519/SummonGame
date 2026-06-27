using DamageSystems;
using KimLIb.ModuleSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Agents
{
    public abstract class Agent : ModuleOwner
    {
        [field:SerializeField] public bool IsSuperArmor { get; set; }
        public bool IsDead { get; protected set; }
        public UnityEvent OnHit;
        public UnityEvent OnDeath;
        
        public HealthModule Health { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            Health = GetModule<HealthModule>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            Health.OnDeath += HandleDeath;
        }

        protected virtual void OnDestroy()
        {
            Health.OnDeath -= HandleDeath;   
        }

        protected abstract void HandleHitEvent();
        
        protected virtual void HandleDeath()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }

        public virtual void ApplyDamage(float damageAmount)
        {
            if(IsSuperArmor || IsDead) return;
            
            
            OnHit?.Invoke();
            Health.ApplyDamage(damageAmount);
            
            HandleHitEvent();
        }
    }
}