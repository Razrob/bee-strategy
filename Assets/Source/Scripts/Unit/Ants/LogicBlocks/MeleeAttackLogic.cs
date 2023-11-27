using System;
using UnityEngine;

[Serializable]
public class MeleeAttackLogic : AttackLogicBase
{
    public MeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance, float damage, float attackCooldown)
        : base(transform, visibleZone, attackDistance, attackCooldown, damage) { }

    protected override void TryAttack(IUnitTarget target)
    {
        Debug.Log("Try attack");
        
        if (Distance(target) > ReactionDistance) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target)) return;   
        
        Debug.Log("Attacked");
        DamageableTargetsInVisibleZone[target].TakeDamage(this);
    }
}