using System;
using UnityEngine;

[Serializable]
public class MeleeAttackLogic : AttackLogicBase
{
    public MeleeAttackLogic(UnitBase unit, float interactionRange, float attackCooldown, float attackRange, float damage)
        : base(unit, interactionRange, attackCooldown, attackRange, damage) { }

    protected override void Attack(IUnitTarget target)
    {
        if (target.TryCast(out IDamagable damageable))
            damageable.TakeDamage(this);
#if UNITY_EDITOR
        else
            Debug.LogWarning($"Target {target} can't be attacked");
#endif
    }
}