using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackLogicBase : LogicBlockBase, IDamageApplicator
{
    private readonly Transform _transform;
    private readonly UnitVisibleZone _visibleZone;

    protected Dictionary<IUnitTarget, IDamagable> DamageableTargets;

    public float Damage { get; }

    protected AttackLogicBase(Transform transform, UnitVisibleZone visibleZone, float attackDistance, float damage) 
        : base(visibleZone, attackDistance)
    {
        _transform = transform;
        _visibleZone = visibleZone;
        Damage = damage;
    }
    
    protected override void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if(!target.TryCast(out IDamagable damageable)) return;
        
        DamageableTargets.Add(target, damageable);
    }

    protected override void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(!target.CastPossible<IDamagable>()) return;
        
        DamageableTargets.Remove(target);
    }

    public override void HandleUpdate()
    {
        throw new NotImplementedException();
    }

    public override bool GiveOrder(IUnitTarget newTarget)
    {
        throw new NotImplementedException();
    }

    protected bool TryGetNearestDamageableTarget(out IUnitTarget nearestTarget)
    {
        nearestTarget = null;
        float currentDistance = float.MaxValue;
        foreach (var target in DamageableTargets)
        {
            if (target.CastPossible<IDamagable>())
            {
                float distance = Vector3.Distance(target.Key.Transform.position, _transform.position);
                if (distance <= ReactionDistance && distance < currentDistance)
                {
                    nearestTarget = target.Key;
                    currentDistance = distance;
                }
            }
        }

        return nearestTarget is null;
    }

    protected abstract void TryAttack();
}