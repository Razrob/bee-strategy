using System;
using UnityEngine;

[Serializable]
public abstract class LogicBlockBase
{
    protected readonly UnitBase Unit;
    
    protected AffiliationEnum Affiliation => Unit.Affiliation;
    protected Transform Transform => Unit.transform;
    
    public readonly float InteractionRange;
    
    protected LogicBlockBase(UnitBase unit, float interactionRange)
    {
        Unit = unit;
        InteractionRange = interactionRange;
    }
    
    /// <returns>
    /// return distance between unit and someTarget
    /// </returns>
    public float Distance(IUnitTarget someTarget) => Vector3.Distance(Transform.position, someTarget.Transform.position);
    
    /// <returns>
    /// return true if distance between unit and someTarget less or equal interaction range, else return false
    /// </returns>
    public bool CheckInteractionDistance(IUnitTarget someTarget) => Distance(someTarget) <= InteractionRange;
    
    /// <returns>
    /// return true if unit can doing something with target in pathData on current distance, else return false
    /// </returns>
    public virtual bool CheckDistance(UnitPathData pathData) => CheckInteractionDistance(pathData.Target);
    
    /// <returns>
    /// return target move position. If target is invalid return default position
    /// </returns>
    public Vector3 AutoGiveOrder(IUnitTarget unitTarget, Vector3 defaultPosition, out UnitPathData unitPathData)
    {
        if(unitTarget.IsNullOrUnityNull())
        {
            unitPathData = new UnitPathData(null, UnitPathType.Move);
            return defaultPosition;
        }
        
        unitPathData = TakeAutoPathData(unitTarget);
        if(unitPathData.Target.IsNullOrUnityNull())
        {
            unitPathData = new UnitPathData(null, UnitPathType.Move);
            return defaultPosition;
        }

        return TakeMovePosition(unitPathData);
    }
    
    /// <returns>
    /// return target move position and unitPathData with pathType, if pathType invalid return unitPathData with UnitPathType.Move
    /// </returns>
    public Vector3 HandleGiveOrder(IUnitTarget unitTarget, UnitPathType pathType, Vector3 defaultPosition,
        out UnitPathData unitPathData)
    {
        if(!ValidateHandleOrder(unitTarget, pathType))
        {
            unitPathData = new UnitPathData(null, UnitPathType.Move);
            return defaultPosition;
        }
        
        unitPathData = new UnitPathData(unitTarget, pathType);

        return TakeMovePosition(unitPathData);
    }
    
    protected Vector3 TakeMovePosition(UnitPathData pathData)
    {
        return CheckDistance(pathData)
            ? Transform.position
            : pathData.Target.Transform.position;
    }

    protected abstract UnitPathData TakeAutoPathData(IUnitTarget unitTarget);

    /// <summary>
    /// Used in method HandleGiveOrder(...), if return false, then HandleGiveOrder(...) return default position
    /// </summary>
    /// <returns>
    /// return true if values is valid, else return false
    /// </returns>
    protected abstract bool ValidateHandleOrder(IUnitTarget target, UnitPathType pathType);
}