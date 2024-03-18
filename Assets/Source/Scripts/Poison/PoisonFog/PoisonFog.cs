using System;
using CustomTimer;
using UnityEngine;

namespace Poison
{
    public class PoisonFog : MonoBehaviour, IPoolable<PoisonFog>, IPoolEventListener
    {
        [SerializeField] private TriggerBehaviour triggerBehaviour;
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private GameObject skinParent;

        private Timer _existTimer;

        public event Action<PoisonFog> ElementReturnEvent;
        public event Action<PoisonFog> ElementDestroyEvent;

        private void Awake()
        {
            _existTimer = new Timer(0, 0, true);
            _existTimer.OnTimerEnd += RemoveFog;
        }

        private void Start()
        {
            triggerBehaviour.EnterEvent += UnitEnter;
            triggerBehaviour.ExitEvent += UnitExit;
        }

        public void SetData(float fogRadius, float fogExistTime)
        {
            triggerCollider.radius = fogRadius;
            skinParent.transform.localScale = new Vector3(fogRadius, fogRadius, fogRadius);
            _existTimer.SetMaxValue(fogExistTime);
        }

        public void HandleUpdate(float time)
            => _existTimer.Tick(time);

        public void RemoveFog()
            => ElementReturnEvent?.Invoke(this);

        public void OnElementReturn()
        {
            gameObject.SetActive(false);
            _existTimer.SetPause();
        }

        public void OnElementExtract()
            => gameObject.SetActive(true);
        
        private void UnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IPoisoneable poisoneable))
                poisoneable.PoisonProcessor.Poised(true);
        }

        private void UnitExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IPoisoneable poisoneable))
                poisoneable.PoisonProcessor.OutFromPoisonFog();
        }
    }
}