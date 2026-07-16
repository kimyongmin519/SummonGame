using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractSystems
{
    public class InvokeTrigger : MonoBehaviour
    {
        [SerializeField] private bool isOnlyFirstEnter;
        [SerializeField] private LayerMask targetLayer;
        
        public UnityEvent OnTriggerEnterEvent;
        private bool _isEnter = false; //최초 트리거만 감지할 때 쓸거임

        private void OnTriggerEnter(Collider other)
        {
            if (_isEnter) return;
            
            if ((1 << other.gameObject.layer & targetLayer) != 0)
            {
                OnTriggerEnterEvent?.Invoke();
                if (isOnlyFirstEnter)
                    _isEnter = true;
            }
        }
    }
}