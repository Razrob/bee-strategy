using System.Collections.Generic;
using UnityEngine;

public class WorkerLogic : LogicBlockBase
{
    private readonly Gathering _gathering;
    private readonly BuildConstruction _buildConstruction;
    private bool _targetInTheRange;
    
    public WorkerLogic(Transform transform, UnitVisibleZone visibleZone, float reactionDistance, float cooldown, int gatheringCapacity)
        : base(transform, visibleZone, reactionDistance, cooldown)
    {
        _gathering = new Gathering(transform, gatheringCapacity, cooldown);
        _buildConstruction = new BuildConstruction();
        _targetInTheRange = false;
    }

    protected override void OnEnterTargetInVisibleZone(IUnitTarget target) { }

    protected override void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(CheckOnNullAndUnityNull(Target)) return;
        if(Target != target) return;
        
        _gathering.OnTargetExit();
        _buildConstruction.OnTargetExit(target);
    }
    
    public override void HandleUpdate(float time)
    {
        if(CheckOnNullAndUnityNull(Target)) return;
        
        if (Distance(Target) <= ReactionDistance)
        {
            if (!_targetInTheRange)
            {
                _gathering.OnTargetEnter();
                _buildConstruction.OnTargetEnter(Target);
                _targetInTheRange = true;
            }
        }
        else
        {
            if (_targetInTheRange)
            {
                _gathering.OnTargetExit();
                _buildConstruction.OnTargetExit(Target);
                _targetInTheRange = false;
            }
            return;
        }

        if (_targetInTheRange)
        {
            _gathering.OnTargetStay();
            _buildConstruction.OnTargetStay();
        }
    }

    
    public override bool GiveOrder(IUnitTarget newTarget)
    {
        Debug.Log($"logic {newTarget}");
        Target = newTarget;
        if (CheckOnNullAndUnityNull(Target))
        {
            _gathering.GiveOrder(null);
            _buildConstruction.GiveOrder(null);
            return false;
        }
        
        switch (newTarget.TargetType)
        {
            case UnitTargetType.ResourceSource:
                _gathering.GiveOrder(Target);
                _buildConstruction.GiveOrder(null);
                break;
            case UnitTargetType.Construction:
                _gathering.GiveOrder(Target);
                _buildConstruction.GiveOrder(Target);
                break;
            default:
                _gathering.GiveOrder(null);
                _buildConstruction.GiveOrder(null);
                return false;
        }

        if (!VisibleZone.Contains(Target)) return false;
        if (Distance(Target) > ReactionDistance) return false;

        _gathering.OnTargetEnter();
        _buildConstruction.OnTargetEnter(Target);
        return true;
    }

    private class Gathering
    {
        private readonly Transform _resourceSkin;
        private readonly int _gatheringCapacity;
        private readonly float _gatheringTIme;

        private IUnitTarget _target;
        
        public float GatherTimer { get; private set; } 
        public bool IsGathering { get; private set; }
        public bool GotResource { get; private set; }

        public Gathering(Transform unit, int gatheringCapacity, float gatheringTIme)
        {
            _resourceSkin = unit.GetChild(1);
            _gatheringCapacity = gatheringCapacity;
            _gatheringTIme = gatheringTIme;

            GatherTimer = 0;
            IsGathering = GotResource = false;
        }

        public void GiveOrder(IUnitTarget newTarget)
        {
            Debug.Log($"gathering {newTarget}");
            if (newTarget == null)
            {
                _target = null;
                return;
            }

            GatherTimer = 0;
            IsGathering = false;
            switch (newTarget.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    if (newTarget.CastPossible<PollenStorage>())
                        _target = newTarget;
                    else
                        _target = null;
                    break;
                case UnitTargetType.Construction:
                    if (GotResource && newTarget.CastPossible<TownHall>()) 
                        _target = newTarget;
                    else
                        _target = null;
                    break;
                default: return;
            }
            Debug.Log($"gathering2 {_target}");
        }    

        public void OnTargetEnter()
        { 
            if(_target == null) return;
            
            switch (_target.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    if (!_target.CastPossible<PollenStorage>()) 
                        return;
                    if(GotResource) 
                        return;
                    IsGathering = true;
                    break;
                case UnitTargetType.Construction:
                    if (!GotResource) 
                        return;
                    if (!_target.CastPossible<TownHall>()) 
                        return;
                    ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _gatheringCapacity);
                    _resourceSkin.transform.gameObject.SetActive(false);
                    IsGathering = false;
                    GotResource = false;
                    break;
                default: return;
            }
        }

        public void OnTargetExit()
        {
            if(_target == null) return;
            
            IsGathering = false;
            GatherTimer = 0;
        }

        public void OnTargetStay()
        {
            if (_target == null) return;

            switch (_target.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    if (!IsGathering) return;
                    if (GotResource) return;
                    
                    GatherTimer += Time.deltaTime;
                    if (GatherTimer > _gatheringTIme)
                    {
                        GatherTimer = 0;
                        IsGathering = false;
                        
                        if (_target.TryCast(out PollenStorage pollenStorage))
                        {
                            pollenStorage.ExtractPollen(_gatheringCapacity);
                            
                            Debug.Log("SetActive");
                            _resourceSkin.transform.gameObject.SetActive(true);
                            GotResource = true;
                        }
                    }
                    break;
                
                case UnitTargetType.Construction:
                    if (!GotResource) 
                        return;
                    if (!_target.CastPossible<TownHall>()) 
                        return;
                    ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _gatheringCapacity);
                    _resourceSkin.transform.gameObject.SetActive(false);
                    GotResource = false;
                    break;
                
                default: return;
            }
        }
    }

    private class BuildConstruction
    {
        public bool IsFindingBuild { get; private set; }
        public bool IsBuilding { get; private set; }
        private Vector3 _destination;
        // private IUnitTarget _target;
        
        public BuildConstruction()
        {
            IsFindingBuild = IsBuilding = false;
        }

        public void OnTargetEnter(IUnitTarget unitTarget)
        {
            switch (unitTarget.TargetType)
            {
                case UnitTargetType.Construction:
                     if (!unitTarget.TryCast(out BuildingProgressConstruction progressConstruction))
                         return;
                    
                     Debug.Log("constuct");
                     IsBuilding = true;
                     IsFindingBuild = false;
                     progressConstruction.WorkerArrived = true;
                    break;
                default: return;
            }
        }

        public void OnTargetExit(IUnitTarget unitTarget)
        {
            if (unitTarget.TargetType != UnitTargetType.Construction ||
                !unitTarget.TryCast(out BuildingProgressConstruction progressConstruction)) return;
            
            Debug.Log("Un constuct");
            progressConstruction.WorkerArrived = false;
            IsBuilding = false;
            IsFindingBuild = false;
        }

        //TODO: update construction building progress in the unit logic, at the moment this logic on the construction side
        public void OnTargetStay()
        {
            
        }
        
        //TODO: rework OnTargetStay and next rework GiveOrder
        public void GiveOrder(IUnitTarget newTarget)
        {
            
        }    
    }
}