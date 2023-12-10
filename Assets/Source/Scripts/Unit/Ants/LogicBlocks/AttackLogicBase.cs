using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AttackLogicBase : LogicBlockBase, IDamageApplicator
{
    protected readonly UnitVisibleZone VisibleZone;
    protected readonly Dictionary<IUnitTarget, IDamagable> DamageableTargetsInVisibleZone = 
        new Dictionary<IUnitTarget, IDamagable>();

    public int EnemiesCount => DamageableTargetsInVisibleZone.Keys.Count;
    public bool CanAttack { get; protected set; }
    public float CooldownTime { get; protected set; }
    public float AttackRange { get; protected set; }
    public float Damage { get; protected set; }

    public event Action OnExitEnemyFromAttackZone;
    public event Action OnCooldownEnd;
    
    protected AttackLogicBase(UnitBase unit, float interactionRange, float attackCooldown, float attackRange, float damage)
        : base(unit, interactionRange)
    {
        CanAttack = true;
        CooldownTime = attackCooldown;
        AttackRange = attackRange;
        Damage = damage;
        
        VisibleZone = unit.VisibleZone;
        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
        
        foreach (var target in VisibleZone.ContainsComponents)
            OnEnterTargetInVisibleZone(target);
    }
    
    protected void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if (target.IsNullOrUnityNull() ||
            target.Affiliation == Affiliation ||
            !target.TryCast(out IDamagable damageable) || 
            DamageableTargetsInVisibleZone.ContainsKey(target)) 
            return;
        
        DamageableTargetsInVisibleZone.Add(target, damageable);
    }

    protected void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(DamageableTargetsInVisibleZone.Remove(target))
            OnExitEnemyFromAttackZone?.Invoke();
    }
    
    /// <returns>
    /// return true if distance between unit and someTarget less or equal attack range, else return false
    /// </returns>
    public bool CheckAttackDistance(IUnitTarget someTarget) => Distance(someTarget) <= AttackRange;
    
    /// <returns> return tru if some IDamageable stay in attack zone, else return false</returns>
    public bool CheckEnemiesInAttackZone()
    {
        foreach (var target in DamageableTargetsInVisibleZone.Keys)
            if (CheckAttackDistance(target)) return true;
        
        return false;
    }
    
    /// <summary>
    /// Attack target, if target can't be attacked, then attack nearest enemy
    /// </summary>
    public void TryAttack(IUnitTarget target)
    {
        if(!CanAttack) return;
        
        if (!target.IsNullOrUnityNull() && CheckAttackDistance(target) && target.CastPossible<IDamagable>())
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
            if (distance <= AttackRange && distance < currentDistance)
            {
                nearestTarget = target.Key;
                currentDistance = distance;
            }
        }

        return !(nearestTarget is null);
    }
   
    protected override bool ValidatePathType(IUnitTarget unitTarget, UnitPathType pathType)
    {
        switch (pathType)
        {
            case UnitPathType.Attack:
                if (unitTarget.Affiliation != Affiliation && 
                    unitTarget.CastPossible<IDamagable>())
                    return true;
                break;
            case UnitPathType.Switch_Profession:
                if (Affiliation == AffiliationEnum.Ants &&
                    unitTarget.TargetType == UnitTargetType.Construction)
                    // TODO: create construction for switching professions
                    return true;
                break;
        }

        return false;
    }
    
    protected override UnitPathData TakeAutoPathData(IUnitTarget unitTarget)
    {
        if (unitTarget.IsNullOrUnityNull() ||
            !unitTarget.CastPossible<IDamagable>() ||
            unitTarget.Affiliation == Affiliation)
            return new UnitPathData(null, UnitPathType.Move);
            
        switch (unitTarget.TargetType)
        {
            case (UnitTargetType.Other_Unit):
                return new UnitPathData(unitTarget, UnitPathType.Attack);
            case (UnitTargetType.Construction):
                return new UnitPathData(unitTarget, UnitPathType.Attack);
            default:
                return new UnitPathData(null, UnitPathType.Move);
        }
    }
    
    public override bool CheckDistance(UnitPathData pathData)
    {
        switch (pathData.PathType)
        {
            case UnitPathType.Attack:
                return CheckAttackDistance(pathData.Target);
            default:
                return CheckInteractionDistance(pathData.Target);
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