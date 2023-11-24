using UnityEngine;

public class RangeAttackLogic : AttackLogicBase
{
    public RangeAttackLogic(Transform parent, UnitVisibleZone attackZone, float attackDistance, float damage)
        : base(parent, attackZone, attackDistance, damage) { }

    //TODO: spawn arrow GameObject and set them target
    protected override void TryAttack()
    {
        if(TryGetNearestDamageableTarget(out IUnitTarget nearestDamageable))
            DamageableTargets[nearestDamageable].TakeDamage(this);
    }
}