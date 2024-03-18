using CustomTimer;
using UnityEngine;

namespace Poison
{
    public class PoisonProcessor : IDamageApplicator
    {
        public float Damage { get; private set; }
        public bool InFog { get; private set; }

        private readonly IDamagable _damageable;
        private readonly Timer _damageCooldown;
        private readonly Timer _existTimer;
        private readonly int _existTime;
        private int _fogsCount;

        public PoisonProcessor(IDamagable damageable, PoisonConfig poisonConfig)
        {
            Damage = poisonConfig.DamagePerSecond;
            _existTime = poisonConfig.ExistTime;

            _damageable = damageable;

            _damageCooldown = new Timer(1, 0, true);
            _existTimer = new Timer(_existTime, 0, true);

            _damageCooldown.OnTimerEnd += CauseDamage;
            _existTimer.OnTimerEnd += OnExistTimeEnd;
        }

        public void HandleUpdate(float time)
        {
            _damageCooldown.Tick(time);
            _existTimer.Tick(time);
        }

        public void Poised(bool inPoisonFog = false)
        {
            if (inPoisonFog)
            {
                InFog = true;
                _fogsCount++;
                _existTimer.SetMaxValue(float.PositiveInfinity);
            }
            else
            {
                if (!InFog)
                    _existTimer.SetMaxValue(_existTime);
            }
            
            if (_damageCooldown.Paused)
                CauseDamage();
        }

        public void OutFromPoisonFog()
        {
            _fogsCount = Mathf.Clamp(_fogsCount - 1, 0, int.MaxValue);

            if (_fogsCount <= 0)
            {
                InFog = false;
                _existTimer.SetMaxValue(_existTime);
            }
        }

        public void Stop()
        {
            _damageCooldown.Reset(true);
            _existTimer.Reset(true);
        }

        private void CauseDamage()
        {
            _damageCooldown.Reset();
            _damageable.TakeDamage(this);
        }

        private void OnExistTimeEnd()
        {
            _damageCooldown.Reset(true);
            _existTimer.Reset(true);
        }
    }
}