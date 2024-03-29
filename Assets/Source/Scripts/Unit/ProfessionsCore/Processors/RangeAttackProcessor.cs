﻿using Projectiles;
using Projectiles.Factory;
using UnityEngine;

namespace Unit.ProfessionsCore.Processors
{
    public class RangeAttackProcessor : AttackProcessorBase
    {
        private readonly ProjectileType _projectileType;
        private readonly ProjectileFactory _projectilesFactory;
        
        public RangeAttackProcessor(UnitBase unit, float attackRange, float damage,
            CooldownProcessor cooldownProcessor, ProjectileType projectileType, ProjectileFactory projectilesFactory)
            : base(unit, attackRange, damage, cooldownProcessor)
        {
            _projectileType = projectileType;
            _projectilesFactory = projectilesFactory;
        }
        
        protected override void Attack(IUnitTarget target)
        {
            if (!target.CastPossible<IDamagable>())
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Target {target} can't be attacked");
#endif
                return;
            }

            var projectile = _projectilesFactory.Create(_projectileType);
            projectile.transform.position = Transform.position;
            projectile.SetDamage(this);
            projectile.SetTarget(target);
        }
    }
}