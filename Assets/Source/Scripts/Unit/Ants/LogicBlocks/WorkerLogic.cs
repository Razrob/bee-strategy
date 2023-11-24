using System;
using Unit.Ants;
using UnityEngine;

public class WorkerLogic : LogicBlockBase
{
    private readonly AntUnit _unit;
    private readonly WorkerDuty _workerDuty;
    
    public WorkerLogic(UnitVisibleZone visibleZone, float reactionDistance, AntUnit unit, WorkerDuty workerDuty)
        : base(visibleZone, reactionDistance)
    {
        _unit = unit;
        _workerDuty = workerDuty;
    }

    protected override void OnEnterTargetInVisibleZone(IUnitTarget target)
    {
        if(Target != target) return;
        
        throw new NotImplementedException();
    }

    protected override void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(Target != target) return;

        throw new NotImplementedException();
    }
    
    public override void HandleUpdate()
    {
        throw new NotImplementedException();
    }
    
    public override bool GiveOrder(IUnitTarget newTarget)
    {
        Target = newTarget;

        if (!VisibleZone.Contains(Target)) return false;
        
            switch (Target.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    _workerDuty.isFindingRes = true;
                    _workerDuty.WorkingOnGO = Target.Transform.gameObject;
                    break;

                default:
                    _workerDuty.isFindingRes = false;
                    _workerDuty.isGathering = false;
                    _workerDuty.isBuilding = false;
                    _workerDuty.isFindingBuild = false;
                    break;
            }
            
        return false;
    }

    public void OnReachDestination()
    {
        switch (Target.TargetType)
        {
            case UnitTargetType.ResourceSource:
           
                break;
            case UnitTargetType.Construction:
                
                break;
            default: throw new NotImplementedException();
        }
    }

    private class Gathering
    {
        private readonly MovingUnit _unit;
        private readonly Transform _resourceSkin;

        private IUnitTarget _workingTarget;
        private float _gatheringTIme = 3;
        private Vector3 _destination;
        private int _loadCapacity;
        
        public float GatherTimer { get; private set; } 
        public bool IsGathering { get; private set; }
        public bool IsFindingRes { get; private set; }
        public bool GotResource { get; private set; }

        public Gathering(MovingUnit unit)
        {
            _unit = unit;
            _resourceSkin = _unit.transform.parent.GetChild(2);
        }

        void OnTargetEnter(IUnitTarget unitTarget)
        {
            switch (unitTarget.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    break;
                case UnitTargetType.Construction:
                    if (!GotResource || !unitTarget.CastPossible<TownHall>()) break;
                    
                    GotResource = false;
                    _resourceSkin.transform.gameObject.SetActive(false);
                    IsFindingRes = true;
                    ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _loadCapacity);

                    _unit.SetDestination(_destination);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        void OnTargetExit(IUnitTarget unitTarget)
        {
            if(!unitTarget.CastPossible<ResourceSourceBase>()) return;
            
            if (unitTarget == _workingTarget)
                GatherTimer = 0;
        }

        void OnTargetStay(IUnitTarget unitTarget)
        {
            if (!(unitTarget.TryCast(out PollenStorage resourceSource) && resourceSource.isPollinated)) return;
            
            if (IsFindingRes && !IsGathering)
                IsGathering = true;

            if (IsGathering)
                GatherTimer += Time.deltaTime;

            if (GatherTimer > _gatheringTIme && IsGathering)
            {
                GotResource = true;
                _resourceSkin.transform.gameObject.SetActive(true);
                IsFindingRes = false;
                IsGathering = false;

                unitTarget.Transform.parent.GetComponent<PollenStorage>().ExtractPollen(_loadCapacity);

                GameObject townHall = GameObject.Find("TownHall");
                _destination = townHall.transform.position;
                _unit.SetDestination(_destination);
                _destination = _workingTarget.Transform.position;
            }
        }
    }

    private class BuildConstruction
    {
        private readonly MovingUnit _unit;
        public bool IsFindingBuild { get; private set; }
        public bool IsBuilding { get; private set; }
        private Vector3 _destination;
        private IUnitTarget _workingTarget;
        
        public BuildConstruction(MovingUnit unit)
        {
            IsFindingBuild = IsBuilding = false;
            
            _unit = unit;
        }

        void OnTargetEnter(IUnitTarget unitTarget)
        {
            switch (unitTarget.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    break;
                case UnitTargetType.Construction:
                    if (!unitTarget.TryCast(out BuildingProgressConstruction progressConstruction))
                        return;
                    IsBuilding = true;
                    IsFindingBuild = false;
                    progressConstruction.WorkerArrived = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        void OnTargetExit(IUnitTarget unitTarget)
        {
            if (unitTarget.TargetType != UnitTargetType.Construction ||
                !unitTarget.TryCast(out BuildingProgressConstruction progressConstruction)) return;
                
            progressConstruction.WorkerArrived = false;
        }

        //TODO: update construction building progress
        void OnTargetStay(IUnitTarget unitTarget)
        {

        }
    }
}