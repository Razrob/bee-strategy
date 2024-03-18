using System.Collections.Generic;
using DG.Tweening;
using Projectiles;
using Projectiles.Factory;
using UnityEngine;

namespace Constructions
{
    public class ButterflyPoisonFlowerAttackProcessor
    {
        private readonly List<UnitBase> _enemies = new List<UnitBase>();
        private readonly ProjectileFactory _projectileFactory;
        private readonly TriggerBehaviour _triggerBehaviour;
        private readonly Transform _flowerPosition;
        
        private Sequence _cooldownTimer;
        private float _attackCooldown;
        private float _attackDamage;
        private float _damageRadius;

        public ButterflyPoisonFlowerAttackProcessor(Transform flowerPosition, 
            ProjectileFactory projectileFactory, TriggerBehaviour triggerBehaviour)
        {
            _flowerPosition = flowerPosition;
            _projectileFactory = projectileFactory;
            _triggerBehaviour = triggerBehaviour;
            
            _triggerBehaviour.EnterEvent += OnUnitEnter;
            _triggerBehaviour.ExitEvent += OnUnitExit;
            
            _cooldownTimer = DOTween.Sequence()
                .SetUpdate(UpdateType.Manual)
                .AppendInterval(0);
        }

        public void HandleUpdate(float time)
        {
            AttackCooldownTick(time);
        }

        public void SetData(float newAttackCooldown, float newAttackDamage, float newDamageRadius)
        {
            _attackCooldown = newAttackCooldown;
            _attackDamage = newAttackDamage;
            _damageRadius = newDamageRadius;
        }
        
        public void KillCooldownTimer()
            => _cooldownTimer.Kill();

        private void AttackCooldownTick(float time)
            => _cooldownTimer?.ManualUpdate(time, time);
        
        private void TryAttack()
        {
            if (_enemies.Count <= 0)
                return;

            UnitBase target = null;
            float distance = float.MaxValue;
            foreach (var enemy in _enemies)
            {
                var newDistance = Vector3.Distance(_flowerPosition.position, enemy.transform.position);
                if (newDistance <= distance)
                {
                    target = enemy;
                    distance = newDistance;
                }
            }
            
            var projectile = _projectileFactory.Create(ProjectileType.ButterflyPoisonFlowerProjectile).Cast<ButterflyPoisonFlowerProjectile>();
            projectile.SetTarget(target);
            projectile.SetDamage(_attackDamage);
            projectile.SetDamageRadius(_damageRadius);
            projectile.transform.position = _flowerPosition.position;
            
            if (_enemies.Count > 0)
            {
                _cooldownTimer = DOTween.Sequence()
                    .SetUpdate(UpdateType.Manual)
                    .AppendInterval(_attackCooldown)
                    .AppendCallback(TryAttack);
            }
        }
        
        private void OnUnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out UnitBase unit))
            {
                if (unit.Affiliation == AffiliationEnum.Ants ||
                    unit.Affiliation == AffiliationEnum.Bees || 
                    !_enemies.Contains(unit))
                {
                    _enemies.Add(unit);
                    
                    if(!_cooldownTimer.active)
                        TryAttack();
                }
            }
        }

        private void OnUnitExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out UnitBase unit))
            {
                _enemies.Remove(unit);

                if (_enemies.Count <= 0)
                    _cooldownTimer?.Kill();
            }
        }
    }
}