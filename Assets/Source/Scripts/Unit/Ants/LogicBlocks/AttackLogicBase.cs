using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AttackLogicBase : LogicBlockBase, IDamageApplicator
{
    protected readonly Dictionary<IUnitTarget, IDamagable> DamageableTargetsInVisibleZone = new Dictionary<IUnitTarget, IDamagable>();
    protected readonly AffiliationEnum Affiliation;
    
    protected ResourceStorage Cooldown;

    public float Damage { get; }

    protected AttackLogicBase(Transform transform, UnitVisibleZone visibleZone, float attackDistance,
        AffiliationEnum affiliation, float attackCooldown, float damage)
        : base(transform, visibleZone, attackDistance)
    {
        Affiliation = affiliation;
        Cooldown = new ResourceStorage(attackCooldown, attackCooldown);
        Damage = damage;

        foreach (var target in visibleZone.ContainsComponents)
            OnEnterTargetInVisibleZone(target);
    }
    
    protected sealed override void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if (!target.TryCast(out IDamagable damageable)) return;
        if (!target.TryCast(out IAffiliation targetAffiliation)) return;
        if (targetAffiliation.Affiliation == Affiliation) return;
        if (DamageableTargetsInVisibleZone.ContainsKey(target)) return;
        
        DamageableTargetsInVisibleZone.Add(target, damageable);
    }

    protected override void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        DamageableTargetsInVisibleZone.Remove(target);
    }

    public override void HandleUpdate(float time)
    {
        if (!TryGetNearestDamageableTarget(out IUnitTarget nearestTarget))
        {
            Cooldown.ChangeValue(time);
            return;
        }
        
        
        // if we take lag that take more time that cooldown, so we need attack some times
        float timeBuffer = Cooldown.CurrentValue + time;
        while (timeBuffer / Cooldown.Capacity >= 1)
        {
            if (!TryGetNearestDamageableTarget(out nearestTarget))
            {
                Cooldown.ChangeValue(time);
                break;
            }
            TryAttack(nearestTarget);
            timeBuffer -= Cooldown.Capacity;   
        }
        Cooldown.SetValue(timeBuffer);
    }

    public override Vector3 GiveOrder(IUnitTarget newTarget, Vector3 defaultPosition)
    {
        if (CheckOnNullAndUnityNull(newTarget)) return defaultPosition;
        if (!newTarget.CastPossible<IDamagable>()) return defaultPosition;
        if (!newTarget.TryCast(out IAffiliation targetAffiliation)) return defaultPosition;
        if (targetAffiliation.Affiliation == Affiliation) return defaultPosition;
        if (!DamageableTargetsInVisibleZone.ContainsKey(newTarget)) return defaultPosition;
        if (Distance(newTarget) > Range) return newTarget.Transform.position;

        Target = newTarget;
        
        return Transform.position;
    }

    protected bool TryGetNearestDamageableTarget(out IUnitTarget nearestTarget)
    {
        nearestTarget = null;
        float currentDistance = float.MaxValue;
        
        foreach (var target in DamageableTargetsInVisibleZone)
        {
            float distance = Distance(target.Key);
            if (distance <= Range && distance < currentDistance)
            {
                nearestTarget = target.Key;
                currentDistance = distance;
            }
        }

        return !(nearestTarget is null);
    }
    
    protected abstract void TryAttack(IUnitTarget target);
}