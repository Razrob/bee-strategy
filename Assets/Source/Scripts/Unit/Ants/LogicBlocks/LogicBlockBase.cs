using System;
using UnityEngine;

[Serializable]
public abstract class LogicBlockBase
{
    protected readonly Transform Transform;
    public readonly float Range;
    
    protected LogicBlockBase(Transform transform, float range)
    {
        Transform = transform;
        Range = range;
    }
    
    /// <returns> return distance between Transform and someTarget</returns>
    public float Distance(IUnitTarget someTarget) => Vector3.Distance(Transform.position, someTarget.Transform.position);
    
    /// <summary>
    /// Set target for unit
    /// </summary>
    /// <returns> return current position, if distance for a target less or equal to range, else return target position. If target is null or invalid return default position </returns>
    public virtual Vector3 GiveOrder(IUnitTarget target, Vector3 defaultPosition)
    {
        if (target.CheckOnNullAndUnityNull()) return defaultPosition;
        if (Distance(target) <= Range) return Transform.position;
        return target.Transform.position;
    }
    
    public abstract UnitPathData TakePathData(IUnitTarget unitTarget);
}