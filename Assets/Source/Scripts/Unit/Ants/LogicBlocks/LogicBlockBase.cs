using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class LogicBlockBase
{
    [field: SerializeField]protected  Transform Transform;
    [field: SerializeField]protected  UnitVisibleZone VisibleZone;
    [field: SerializeField]protected  float ReactionDistance;
    [field: SerializeField]protected  ResourceStorage Cooldown;

    [CanBeNull] protected IUnitTarget Target;
    
    protected LogicBlockBase(Transform transform, UnitVisibleZone unitVisibleZone, float reactionDistance, float cooldown)
    {
        Transform = transform;
        VisibleZone = unitVisibleZone;
        ReactionDistance = reactionDistance;
        Cooldown = new ResourceStorage(cooldown,cooldown);

        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    protected LogicBlockBase(Transform transform, UnitVisibleZone unitVisibleZone, float reactionDistance, float cooldown, IUnitTarget target)
    {
        Transform = transform;
        VisibleZone = unitVisibleZone;
        ReactionDistance = reactionDistance;
        Cooldown = new ResourceStorage(cooldown,cooldown);
        Target = target;

        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    
    /// <returns> return distance between Transform and someTarget</returns>
    protected float Distance(IUnitTarget someTarget) 
        => 
            Vector3.Distance(
                Transform.position, 
                someTarget.Transform.position);
    
    protected abstract void OnEnterTargetInVisibleZone(IUnitTarget target);
    protected abstract void OnExitTargetInVisibleZone(IUnitTarget target);
    
    public abstract void HandleUpdate(float time);

    /// <summary>
    /// Set target for unit
    /// </summary>
    /// <param name="newTarget"></param>
    /// <returns> return true, if distance for a target less then distance reaction, else return false </returns>
    public virtual bool GiveOrder(IUnitTarget newTarget)
    {
        Target = newTarget;
        return CheckOnNullAndUnityNull(Target) || (Distance(Target) <= ReactionDistance);
    }
    
    protected bool CheckOnNullAndUnityNull<T>(T t) => t == null || ((t is Object) && (t as Object) == null);
}