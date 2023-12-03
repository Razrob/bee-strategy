using System;
using UnityEngine;

public class WorkerLogic : LogicBlockBase
{
    private readonly ResourceExtraction _resourceExtraction;
    private readonly BuildConstruction _buildConstruction;
    
    private bool _targetInTheRange;
    
    public event Action OnExtractionEnd;
    public event Action OnStorageResource;
    
    public WorkerLogic(Transform transform, UnitVisibleZone visibleZone, float reactionDistance,  int gatheringCapacity, float gatheringSpeed)
        : base(transform, visibleZone, reactionDistance)
    {
        _resourceExtraction = new ResourceExtraction(gatheringCapacity, gatheringSpeed);
        _buildConstruction = new BuildConstruction();
        _targetInTheRange = false;

        _resourceExtraction.OnExtractionEnd += OnExtractionEndMethod;
        _resourceExtraction.OnStorageResource += OnStorageResourceMethod;
    }

    protected override void OnEnterTargetInVisibleZone(IUnitTarget target) { }

    protected override void OnExitTargetInVisibleZone(IUnitTarget target)
    {
        if(CheckOnNullAndUnityNull(Target)) return;
        if(Target != target) return;
        
        _resourceExtraction.OnTargetExit();
        _buildConstruction.OnTargetExit(target);
    }
    
    public override void HandleUpdate(float time)
    {
        if(CheckOnNullAndUnityNull(Target)) return;
        
        if (Distance(Target) <= Range)
        {
            if (!_targetInTheRange)
            {
                _resourceExtraction.OnTargetEnter();
                _buildConstruction.OnTargetEnter(Target);
                _targetInTheRange = true;
            }
        }
        else
        {
            if (_targetInTheRange)
            {
                _resourceExtraction.OnTargetExit();
                _buildConstruction.OnTargetExit(Target);
                _targetInTheRange = false;
            }
            return;
        }

        if (_targetInTheRange)
        {
            _resourceExtraction.OnTargetStay();
            _buildConstruction.OnTargetStay();
        }
    }

    
    public override Vector3 GiveOrder(IUnitTarget newTarget, Vector3 defaultPosition)
    {
        Debug.Log($"logic {newTarget}");
        Target = newTarget;
        if (CheckOnNullAndUnityNull(Target))
        {
            _resourceExtraction.GiveOrder(null);
            _buildConstruction.GiveOrder(null);
            return defaultPosition;
        }
        
        switch (newTarget.TargetType)
        {
            case UnitTargetType.ResourceSource:
                _resourceExtraction.GiveOrder(Target);
                _buildConstruction.GiveOrder(null);
                break;
            case UnitTargetType.Construction:
                _resourceExtraction.GiveOrder(Target);
                _buildConstruction.GiveOrder(Target);
                break;
            default:
                _resourceExtraction.GiveOrder(null);
                _buildConstruction.GiveOrder(null);
                return defaultPosition;
        }

        if (!VisibleZone.Contains(Target)) return Target.Transform.position;
        if (Distance(Target) > Range) return Target.Transform.position;

        _resourceExtraction.OnTargetEnter();
        _buildConstruction.OnTargetEnter(Target);
        return Transform.position;
    }

    private void OnExtractionEndMethod() => OnExtractionEnd?.Invoke();
    private void OnStorageResourceMethod() => OnStorageResource?.Invoke();
    
    private class ResourceExtraction
    {
        private readonly int _extractionCapacity;
        private readonly float _extractionTIme;
        
        private IUnitTarget _target;
        
        public float ExtractionTimer { get; private set; } 
        public bool Extracting { get; private set; }
        public bool GotResource { get; private set; }

        public event Action OnExtractionEnd;
        public event Action OnStorageResource;
        
        public ResourceExtraction(int extractionCapacity, float extractionTIme)
        {
            _extractionCapacity = extractionCapacity;
            _extractionTIme = extractionTIme;

            ExtractionTimer = 0;
            Extracting = GotResource = false;
        }

        public void GiveOrder(IUnitTarget newTarget)
        {
            Debug.Log($"gathering {newTarget}");
            if (newTarget == null)
            {
                _target = null;
                return;
            }

            ExtractionTimer = 0;
            Extracting = false;
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
                    Extracting = true;
                    break;
                case UnitTargetType.Construction:
                    if (!GotResource) 
                        return;
                    if (!_target.CastPossible<TownHall>()) 
                        return;
                    ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _extractionCapacity);
                    OnStorageResource?.Invoke();
                    Extracting = false;
                    GotResource = false;
                    break;
                default: return;
            }
        }

        public void OnTargetExit()
        {
            if(_target == null) return;
            
            Extracting = false;
            ExtractionTimer = 0;
        }

        public void OnTargetStay()
        {
            if (_target == null) return;

            switch (_target.TargetType)
            {
                case UnitTargetType.ResourceSource:
                    if (!Extracting) return;
                    if (GotResource) return;
                    
                    ExtractionTimer += Time.deltaTime;
                    if (ExtractionTimer > _extractionTIme)
                    {
                        ExtractionTimer = 0;
                        Extracting = false;
                        
                        if (_target.TryCast(out PollenStorage pollenStorage))
                        {
                            pollenStorage.ExtractPollen(_extractionCapacity);
                            
                            Debug.Log("SetActive");
                            GotResource = true;
                            OnExtractionEnd?.Invoke();
                        }
                    }
                    break;
                
                case UnitTargetType.Construction:
                    if (!GotResource) 
                        return;
                    if (!_target.CastPossible<TownHall>()) 
                        return;
                    ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _extractionCapacity);
                    OnStorageResource?.Invoke();
                    Extracting = false;
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
        private IUnitTarget _target;
        private BuildingProgressConstruction _buildTarget;
        
        public BuildConstruction()
        {
            IsFindingBuild = IsBuilding = false;
        }

        public void OnTargetEnter(IUnitTarget unitTarget)
        {
            // if(_target != unitTarget) return;
            //
            // switch (unitTarget.TargetType)
            // {
            //     case UnitTargetType.Construction:
            //          if (!unitTarget.TryCast(out BuildingProgressConstruction progressConstruction))
            //              return;
            //         
            //          Debug.Log("construct");
            //          IsBuilding = true;
            //          IsFindingBuild = false;
            //          progressConstruction.WorkerArrived = true;
            //         break;
            //     default: return;
            // }
            
            Debug.Log("construct");
            IsBuilding = true;
            IsFindingBuild = false;
            _buildTarget.WorkerArrived = true;
        }

        public void OnTargetExit(IUnitTarget unitTarget)
        {
            if (unitTarget.TargetType != UnitTargetType.Construction ||
                !unitTarget.TryCast(out BuildingProgressConstruction progressConstruction)) return;
            
            Debug.Log("Un construct");
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
            if(newTarget == null) return ;
            if(!newTarget.TryCast(out BuildingProgressConstruction progressConstruction)) return;
            
            _target = newTarget;
            _buildTarget = progressConstruction;
        }    
    }
}