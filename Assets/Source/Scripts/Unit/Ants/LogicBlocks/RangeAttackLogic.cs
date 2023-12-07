using UnityEngine;

public class RangeAttackLogic : AttackLogicBase
{
    protected readonly GameObject ProjectilePrefab;

    public RangeAttackLogic(Transform transform, UnitVisibleZone visibleZone, float attackDistance,
        AffiliationEnum affiliation, float cooldown, float damage, UnitBase unit, GameObject projectilePrefab)
        : base(transform, attackDistance, visibleZone, affiliation, cooldown, damage, unit)
    {
        ProjectilePrefab = projectilePrefab;
    }

    //TODO: spawn arrow GameObject and set it target
    protected override void Attack(IUnitTarget target)
    {
        if (Distance(target) > Range) return;
        if(!DamageableTargetsInVisibleZone.ContainsKey(target))  return;   
        
        DamageableTargetsInVisibleZone[target].TakeDamage(this);

        var projectile = Object.Instantiate(ProjectilePrefab, Unit.transform.position,
            ProjectilePrefab.transform.rotation);
        if (projectile.TryGetComponent(out ProjectileBehaviourRework projectileBehaviour))
            projectileBehaviour.SetTarget(target);
        else
            Object.Destroy(projectile);
    }
}