using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AttackLogicBase : LogicBlockBase, IDamageApplicator
{
    protected readonly UnitBase Unit;
    protected readonly AffiliationEnum Affiliation;
    protected readonly UnitVisibleZone VisibleZone;
    protected readonly Dictionary<IUnitTarget, IDamagable> DamageableTargetsInVisibleZone = 
        new Dictionary<IUnitTarget, IDamagable>();

    public int EnemiesCount => DamageableTargetsInVisibleZone.Keys.Count;
    public bool CanAttack { get; private set; }
    public float CooldownTime { get; set; }
    public float Damage { get; }

    public event Action OnExitTargetFromAttackZone;
    public event Action OnCooldownEnd;
    
    protected AttackLogicBase(Transform transform, float attackDistance, UnitVisibleZone visibleZone,
        AffiliationEnum affiliation, float attackCooldown, float damage, UnitBase unit)
        : base(transform, attackDistance)
    {
        CanAttack = true;
        Unit = unit;
        Affiliation = affiliation;
        CooldownTime = attackCooldown;
        Damage = damage;
        
        VisibleZone = visibleZone;
        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
        
        foreach (var target in visibleZone.ContainsComponents)
            OnEnterTargetInVisibleZone(target);
    }
    
    protected void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if (target.CheckOnNullAndUnityNull()) return;
        if (!target.TryCast(out IDamagable damageable)) return;
        if (!target.TryCast(out IAffiliation targetAffiliation)) return;
        if (targetAffiliation.Affiliation == Affiliation) return;
        if (DamageableTargetsInVisibleZone.ContainsKey(target)) return;
        
        DamageableTargetsInVisibleZone.Add(target, damageable);
    }

    protected void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(DamageableTargetsInVisibleZone.Remove(target))
            OnExitTargetFromAttackZone?.Invoke();
    }
    
    /// <returns> return tru if some IDamageable stay in attack zone, else return false</returns>
    public bool CheckEnemiesInAttackZone()
    {
        foreach (var target in DamageableTargetsInVisibleZone.Keys)
            if (Distance(target) <= Range) return true;
        
        return false;
    }
    
    public override Vector3 GiveOrder(IUnitTarget target, Vector3 defaultPosition)
    {
        if (target.CheckOnNullAndUnityNull()) return defaultPosition;
        if (!target.CastPossible<IDamagable>()) return defaultPosition;
        if (!target.TryCast(out IAffiliation targetAffiliation)) return defaultPosition;
        if (targetAffiliation.Affiliation == Affiliation) return defaultPosition;
        if (!DamageableTargetsInVisibleZone.ContainsKey(target)) return defaultPosition;
        if (Distance(target) > Range) return target.Transform.position;
        
        return Transform.position;
    }

    /// <summary>
    /// Attack target, if target can't be attacked, then attack nearest enemy
    /// </summary>
    public void TryAttack(IUnitTarget target)
    {
        if(!CanAttack) return;
        
        if (!target.CheckOnNullAndUnityNull() && Distance(target) <= Range)
            Attack(target);
        else if(TryGetNearestDamageableTarget(out IUnitTarget nearestTarget))
            Attack(nearestTarget);

        Unit.StartCoroutine(Cooldown(CooldownTime));
    }
    
    protected abstract void Attack(IUnitTarget target);
    
    public bool TryGetNearestDamageableTarget(out IUnitTarget nearestTarget)
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
    
    public override UnitPathData TakePathData(IUnitTarget unitTarget)
    {
        if (unitTarget.CheckOnNullAndUnityNull() ||
            !unitTarget.CastPossible<IDamagable>() ||
            !unitTarget.TryCast(out IAffiliation affiliation) ||
            affiliation.Affiliation == Affiliation)
            return new UnitPathData(null, UnitTargetType.None, UnitPathType.Move);
            
        switch (unitTarget.TargetType)
        {
            case (UnitTargetType.Other_Unit):
                return new UnitPathData(unitTarget, UnitTargetType.Other_Unit, UnitPathType.Attack);
            case (UnitTargetType.Construction):
                return new UnitPathData(unitTarget, UnitTargetType.Construction, UnitPathType.Attack);
            default:
                return new UnitPathData(null, UnitTargetType.None, UnitPathType.Move);
        }
    }
    
    IEnumerator Cooldown(float cooldownTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        CanAttack = true;
        OnCooldownEnd?.Invoke();
    }
}