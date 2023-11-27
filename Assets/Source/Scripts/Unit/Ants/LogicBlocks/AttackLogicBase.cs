using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AttackLogicBase : LogicBlockBase, IDamageApplicator
{
    protected readonly Dictionary<IUnitTarget, IDamagable> DamageableTargetsInVisibleZone = new Dictionary<IUnitTarget, IDamagable>();
    
    public float Damage { get; }

    protected AttackLogicBase(Transform transform, UnitVisibleZone visibleZone, float attackDistance, float attackCooldown, float damage)
        : base(transform, visibleZone, attackDistance, attackCooldown)
    {
        Damage = damage;
    }
    
    protected override void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if (!target.TryCast(out IDamagable damageable)) return;
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

    public override bool GiveOrder(IUnitTarget newTarget)
    {
        if (CheckOnNullAndUnityNull(newTarget)) return false;
        if (!newTarget.CastPossible<IDamagable>()) return false;
        if (!DamageableTargetsInVisibleZone.ContainsKey(newTarget)) return false;
        
        return true;
    }

    protected bool TryGetNearestDamageableTarget(out IUnitTarget nearestTarget)
    {
        nearestTarget = null;
        float currentDistance = float.MaxValue;
        
        foreach (var target in DamageableTargetsInVisibleZone)
        {
            float distance = Distance(target.Key);
            if (distance <= ReactionDistance && distance < currentDistance)
            {
                nearestTarget = target.Key;
                currentDistance = distance;
            }
        }

        return !(nearestTarget is null);
    }
    
    protected abstract void TryAttack(IUnitTarget target);
}