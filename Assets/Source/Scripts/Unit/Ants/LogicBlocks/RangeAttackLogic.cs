using UnityEngine;

public class RangeAttackLogic : AttackLogicBase
{
    protected readonly GameObject ProjectilePrefab;

    public RangeAttackLogic(UnitBase unit, float interactionRange, float cooldown,
        float attackRange, float damage, GameObject projectilePrefab)
        : base(unit, interactionRange, cooldown, attackRange, damage)
    {
        ProjectilePrefab = projectilePrefab;
    }

    protected override void Attack(IUnitTarget target)
    {
        if(!target.CastPossible<IDamagable>())
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Target {target} can't be attacked");
#endif
            return;
        }
        
        var projectile = Object.Instantiate(ProjectilePrefab, Unit.transform.position,
            ProjectilePrefab.transform.rotation);
        if (projectile.TryGetComponent(out ProjectileBehaviourRework projectileBehaviour))
            projectileBehaviour.SetTarget(target);
        else
            Object.Destroy(projectile);
    }
}