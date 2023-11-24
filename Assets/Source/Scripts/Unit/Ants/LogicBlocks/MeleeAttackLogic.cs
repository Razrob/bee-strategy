using UnityEngine;

public class MeleeAttackLogic : AttackLogicBase
{
    public MeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance, float damage)
        : base(transform, visibleZone, attackDistance, damage) { }

    protected override void TryAttack()
    {
        if(TryGetNearestDamageableTarget(out IUnitTarget nearestDamageable))
            DamageableTargets[nearestDamageable].TakeDamage(this);
    }
}