using UnityEngine;

public class RangeAttackLogic : AttackLogicBase
{
    public RangeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance, float cooldown, float damage)
        : base(transform, visibleZone, attackDistance, cooldown, damage) { }

    //TODO: spawn arrow GameObject and set them target
    protected override void TryAttack(IUnitTarget target)
    {
        if (Distance(target) > ReactionDistance) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target))  return;   
        
        DamageableTargetsInVisibleZone[target].TakeDamage(this);
    }
}