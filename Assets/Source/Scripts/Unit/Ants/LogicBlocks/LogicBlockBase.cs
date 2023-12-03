using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

[Serializable]
public abstract class LogicBlockBase
{
    protected readonly Transform Transform;
    protected readonly UnitVisibleZone VisibleZone;
    protected readonly float Range;

    [CanBeNull] protected IUnitTarget Target;
    
    protected LogicBlockBase(Transform transform, UnitVisibleZone unitVisibleZone, float range)
    {
        Transform = transform;
        VisibleZone = unitVisibleZone;
        Range = range;

        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    protected LogicBlockBase(Transform transform, UnitVisibleZone unitVisibleZone, float range, IUnitTarget target)
    {
        Transform = transform;
        VisibleZone = unitVisibleZone;
        Range = range;
        Target = target;

        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    
    /// <returns> return distance between Transform and someTarget</returns>
    protected float Distance(IUnitTarget someTarget) => Vector3.Distance(Transform.position, someTarget.Transform.position);
    
    protected abstract void OnEnterTargetInVisibleZone(IUnitTarget target);
    protected abstract void OnExitTargetInVisibleZone(IUnitTarget target);
    
    public abstract void HandleUpdate(float time);

    /// <summary>
    /// Set target for unit
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="defaultPosition"></param>
    /// <returns> return current position, if distance for a target less or equal to range, else return target position. If target is null or invalid return default position </returns>
    // /// <returns> return true, if distance for a target less then distance reaction, else return false. If target is null return false </returns>
    public virtual Vector3 GiveOrder(IUnitTarget newTarget, Vector3 defaultPosition)
    {
        Target = newTarget;
        if (CheckOnNullAndUnityNull(Target)) return defaultPosition;
        if (Distance(Target) <= Range) return Transform.position;
        return newTarget.Transform.position;
    }
    
    protected bool CheckOnNullAndUnityNull<T>(T t) => t == null || ((t is Object) && (t as Object) == null);
}