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
    
    protected override bool ValidatePathType(IUnitTarget unitTarget, UnitPathType pathType)
    {
        switch (pathType)
        {
            case UnitPathType.Build_Construction:
                if (unitTarget.TargetType == UnitTargetType.Construction &&
                    //TODO: create ants constructions
                    // unitTarget.Affiliation == Affiliation &&
                    unitTarget.CastPossible<BuildingProgressConstruction>())
                    return true;
                break;
            case UnitPathType.Repair_Construction:
                if (unitTarget.TargetType == UnitTargetType.Construction &&
                    unitTarget.Affiliation == Affiliation &&
                    unitTarget.CastPossible<ConstructionBase>())
                    return true;
                break;
            case UnitPathType.Collect_Resource:
                if (unitTarget.TargetType == UnitTargetType.ResourceSource &&
                    unitTarget.CastPossible<ResourceSourceBase>() &&
                    !GotResource)
                    return true;
                break;
            case UnitPathType.Storage_Resource:
                if (unitTarget.TargetType == UnitTargetType.Construction &&
                    unitTarget.CastPossible<TownHall>() &&
                    GotResource)
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

    public void StartExtraction(ResourceID resourceID)
    {
        Unit.StartCoroutine(ExtractResource(ExtractionTime, resourceID));
    }

    public void StopExtraction()
    {
        if(_extractionCoroutine is null) return;
        
        Unit.StopCoroutine(_extractionCoroutine);
    }
    
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