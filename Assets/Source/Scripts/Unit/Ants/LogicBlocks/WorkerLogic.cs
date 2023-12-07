using System;
using UnityEngine;

public class WorkerLogic : LogicBlockBase
{
    public int ExtractionCapacity  { get; private set; }
    public ResourceID ExtractedResourceID { get; private set; }
    public bool GotResource { get; private set; }
    
    public event Action OnResourceExtracted;
    public event Action OnResourcesStoraged;
    
    public WorkerLogic(Transform transform, float range,  int gatheringCapacity, float gatheringSpeed)
        : base(transform, range)
    {
        ExtractionCapacity = gatheringCapacity;
    }

    public void ResourceExtracted(ResourceID resourceID)
    {
        ExtractedResourceID = resourceID;
        GotResource = true;
        
        OnResourceExtracted?.Invoke();
    }

    public void ResourcesStoraged()
    {
        GotResource = false;
        OnResourcesStoraged?.Invoke();
    }
    
    public override Vector3 GiveOrder(IUnitTarget target, Vector3 defaultPosition)
    {
        if (target.CheckOnNullAndUnityNull())
            return defaultPosition;

        if (target.TargetType != UnitTargetType.ResourceSource ||
            target.TargetType != UnitTargetType.Construction)
            return defaultPosition;

        if (Distance(target) > Range) 
            return target.Transform.position;

        return Transform.position;
    }
    
    public override UnitPathData TakePathData(IUnitTarget unitTarget)
    {
        if(unitTarget.CheckOnNullAndUnityNull())
            return new UnitPathData(null, UnitTargetType.None, UnitPathType.Move);

        switch (unitTarget.TargetType)
        {
            case (UnitTargetType.ResourceSource):
                return GotResource
                    ? new UnitPathData(unitTarget, UnitTargetType.Construction, UnitPathType.Storage_Resource)
                    : new UnitPathData(unitTarget, UnitTargetType.ResourceSource, UnitPathType.Collect_Resource);
            case (UnitTargetType.Construction):
                if (unitTarget.CastPossible<BuildingProgressConstruction>())
                    return new UnitPathData(unitTarget, UnitTargetType.Construction, UnitPathType.Build_Construction);
                
                if (unitTarget.CastPossible<TownHall>() && GotResource)
                    return new UnitPathData(unitTarget, UnitTargetType.Construction, UnitPathType.Storage_Resource);  
                
                return new UnitPathData(null, UnitTargetType.None, UnitPathType.Move);
            default: 
                return new UnitPathData(null, UnitTargetType.None, UnitPathType.Move);
        }    
    }
}