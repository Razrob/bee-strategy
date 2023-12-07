using System;
using UnityEngine;

[Serializable]
public class MeleeAttackLogic : AttackLogicBase
{
    public MeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance,
        AffiliationEnum affiliation, float damage, float attackCooldown, UnitBase unit)
        : base(transform, attackDistance, visibleZone, affiliation, attackCooldown, damage, unit) { }

    protected override void Attack(IUnitTarget target)
    {
        if (Distance(target) > Range) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target)) return;   
        
        DamageableTargetsInVisibleZone[target].TakeDamage(this);
    }
}