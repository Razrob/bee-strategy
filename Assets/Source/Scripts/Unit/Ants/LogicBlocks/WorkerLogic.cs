using System;
using System.Collections;
using UnityEngine;

public class WorkerLogic : LogicBlockBase
{
    public int ExtractionCapacity  { get; private set; }
    public float ExtractionTime  { get; private set; }
    public ResourceID ExtractedResourceID { get; private set; }
    public bool GotResource { get; private set; }
    public bool Extraction { get; private set; }

    private Coroutine _extractionCoroutine;

    public event Action OnResourceExtracted;
    public event Action OnStorageResources;
    
    public WorkerLogic(UnitBase unit, float interactionRange,  int gatheringCapacity, float extractionTime)
        : base(unit, interactionRange)
    {
        ExtractionCapacity = gatheringCapacity;
        ExtractionTime = extractionTime;
    }
    
    protected override bool ValidateHandleOrder(IUnitTarget target, UnitPathType pathType)
    {
        if (target.IsNullOrUnityNull()) 
            return false;
        
        switch (pathType)
        {
            case UnitPathType.Build_Construction:
                if (target.TargetType == UnitTargetType.Construction &&
                    //TODO: create ants constructions
                    // unitTarget.Affiliation == Affiliation &&
                    target.CastPossible<BuildingProgressConstruction>())
                    return true;
                break;
            case UnitPathType.Repair_Construction:
                if (target.TargetType == UnitTargetType.Construction &&
                    target.Affiliation == Affiliation &&
                    target.CastPossible<ConstructionBase>())
                    return true;
                break;
            case UnitPathType.Collect_Resource:
                if (target.TargetType == UnitTargetType.ResourceSource &&
                    target.CastPossible<ResourceSourceBase>() &&
                    !GotResource)
                    return true;
                break;
            case UnitPathType.Storage_Resource:
                if (target.TargetType == UnitTargetType.Construction &&
                    target.CastPossible<TownHall>() &&
                    GotResource)
                    return true;
                break;
            case UnitPathType.Switch_Profession:
                if (Affiliation == AffiliationEnum.Ants &&
                    target.TargetType == UnitTargetType.Construction)
                    // TODO: create construction for switching professions
                    return true;
                break;
        }

        return false;
    }
    
    protected override UnitPathData TakeAutoPathData(IUnitTarget unitTarget)
    {
        if(unitTarget.IsNullOrUnityNull())
            return new UnitPathData(null, UnitPathType.Move);

        switch (unitTarget.TargetType)
        {
            case (UnitTargetType.ResourceSource):
                return GotResource
                    ? new UnitPathData(unitTarget, UnitPathType.Storage_Resource)
                    : new UnitPathData(unitTarget, UnitPathType.Collect_Resource);
            case (UnitTargetType.Construction):
                if (unitTarget.CastPossible<BuildingProgressConstruction>())
                    return new UnitPathData(unitTarget, UnitPathType.Build_Construction);
                
                if (unitTarget.CastPossible<TownHall>() && GotResource)
                    return new UnitPathData(unitTarget, UnitPathType.Storage_Resource);  
                
                return new UnitPathData(null, UnitPathType.Move);
            default: 
                return new UnitPathData(null, UnitPathType.Move);
        }    
    }

    /// <summary>
    /// Start resource extraction coroutine
    /// </summary>
    public void StartExtraction(ResourceID resourceID)
    {
        ExtractedResourceID = resourceID;
        Unit.StartCoroutine(ExtractResource(ExtractionTime, resourceID));
    }

    /// <summary>
    /// Stop resource extraction coroutine
    /// </summary>
    public void StopExtraction()
    {
        if(_extractionCoroutine is null) return;
        
        Unit.StopCoroutine(_extractionCoroutine);
    }
    
    /// <summary>
    /// Inform worker logic about put resource in storage
    /// </summary>
    public void StorageResources()
    {
        GotResource = false;
        OnStorageResources?.Invoke();
    }
    
    IEnumerator ExtractResource(float extractionTime, ResourceID resourceID)
    {
        Extraction = true;
        yield return new WaitForSeconds(extractionTime);
        Extraction = false;
        GotResource = true;
        ExtractedResourceID = resourceID;
        OnResourceExtracted?.Invoke();
    }
}