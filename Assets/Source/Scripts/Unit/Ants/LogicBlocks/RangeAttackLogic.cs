using UnityEngine;

public class RangeAttackLogic : AttackLogicBase
{
    public RangeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance, AffiliationEnum affiliation, float cooldown, float damage)
        : base(transform, visibleZone, attackDistance, affiliation, cooldown, damage) { }

    //TODO: spawn arrow GameObject and set it target
    protected override void TryAttack(IUnitTarget target)
    {
        if (Distance(target) > Range) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target))  return;   
        
        DamageableTargetsInVisibleZone[target].TakeDamage(this);
    }
}