﻿using System;
using UnityEngine;

namespace Unit.ProfessionsCore.Processors
{
    public abstract class AttackProcessorBase : IReadOnlyAttackProcessor, IDamageApplicator
    {
        private readonly CooldownProcessor _cooldownProcessor;
        private readonly AttackZoneProcessor _attackZoneProcessor;
        protected readonly Transform Transform;
        
        public int EnemiesCount => _attackZoneProcessor.EnemiesCount;
        public float AttackRange => _attackZoneProcessor.AttackRange;
        public float Damage { get; }
        
        public event Action OnEnterEnemyInZone;
        public event Action OnExitEnemyFromZone;

        protected AttackProcessorBase(UnitBase unit, float attackRange, float damage, CooldownProcessor cooldownProcessor)
        {
            Transform = unit.Transform;
            
            Damage = damage;
            
            _cooldownProcessor = cooldownProcessor;

            _attackZoneProcessor = new AttackZoneProcessor(unit, attackRange);
            _attackZoneProcessor.OnEnterEnemyInZone += EnterEnemyInZone;
            _attackZoneProcessor.OnExitEnemyFromZone += ExitEnemyFromZone;
        }
        
        /// <returns>
        /// return true if distance between unit and someTarget less or equal attack range, else return false
        /// </returns>
        public bool CheckAttackDistance(IUnitTarget someTarget) => _attackZoneProcessor.Targets.ContainsKey(someTarget);

        /// <returns> return tru if some IDamageable stay in attack zone, else return false</returns>
        public bool CheckEnemiesInAttackZone() => EnemiesCount > 0;

        /// <summary>
        /// Attack target, if target can't be attacked, then attack nearest enemy
        /// </summary>
        public void TryAttack(IUnitTarget target)
        {
            if (_cooldownProcessor.IsCooldown) return;
            
            if (!target.IsAnyNull() && CheckAttackDistance(target) && target.CastPossible<IDamagable>() ||
                TryGetNearestDamageableTarget(out target))
            {
                Attack(target);
                _cooldownProcessor.StartCooldown();
            }
        }
        
        protected abstract void Attack(IUnitTarget target);

        private bool TryGetNearestDamageableTarget(out IUnitTarget nearestTarget)
        {
            nearestTarget = null;
            float currentDistance = float.MaxValue;

            foreach (var target in _attackZoneProcessor.Targets)
            {
                float distance = Distance(target.Key);
                if (distance < currentDistance)
                {
                    nearestTarget = target.Key;
                    currentDistance = distance;
                }
            }

            return !(nearestTarget is null);
        }
        
        private float Distance(IUnitTarget someTarget) => Vector3.Distance(Transform.position, someTarget.Transform.position);

        private void EnterEnemyInZone() => OnEnterEnemyInZone?.Invoke();

        private void ExitEnemyFromZone() => OnExitEnemyFromZone?.Invoke();
    }
}