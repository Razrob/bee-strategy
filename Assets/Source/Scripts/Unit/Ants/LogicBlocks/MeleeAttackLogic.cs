using System;
using UnityEngine;

[Serializable]
public class MeleeAttackLogic : AttackLogicBase
{
    public MeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance, AffiliationEnum affiliation, float damage, float attackCooldown)
        : base(transform, visibleZone, attackDistance, affiliation, attackCooldown, damage) { }

    protected override void TryAttack(IUnitTarget target)
    {
        if (Distance(target) > Range) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target)) return;   
        
        DamageableTargetsInVisibleZone[target].TakeDamage(this);
    }
}