using System;
using System.Collections.Generic;
using System.Linq;
using Unit.Ants.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.Ants
{
    public abstract class AntBase : AntUnit
    {
        public abstract AntType AntType { get; }
        
        [SerializeField] private GameObject resource;
        [SerializeField] private AntProfessionConfigBase defaultProfessionConfig;
        [SerializeField] private Animator animator;
        
        private AntProfessionConfigBase _currentProfessionConfig;
        private UnitPathData _unitPathData;
        
        public UnitType UnitType => _currentProfessionConfig.UnitType;
        public EntityStateID CurrentState => _stateMachine.ActiveState;
        public AntProfessionType ProfessionType => _currentProfessionConfig.ProfessionType;
        public AntProfessionRang ProfessionRang => _currentProfessionConfig.AntProfessionRang;
        public AntProfessionType TargetProfessionType { get; private set; }
        public AntProfessionRang TargetProfessionRang { get; private set; }
        
        public UnitPathData UnitPathData => _unitPathData;
        public LogicBlockBase CurrentProfessionLogic { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public event Action OnTargetPositionChange;
        
        private void Start()
        {
            UnitPool.Instance.UnitCreation(this);
            HideResource();
            SetProfession(defaultProfessionConfig);

            List<EntityStateBase> stateBases = new List<EntityStateBase>()
            {
                new AntStayState(this),
                new AntMoveState(this),
                new AntBuildState(this),
                new AntResourceExtractionState(this),
                new AntStorageResourceState(this),
                new AntAttackState(this),
                new AntSwitchProfessionState(this)
            };
            _stateMachine = new EntityStateMachine(stateBases, EntityStateID.Stay);

            TargetPosition = transform.position;
            AutoGiveOrder(null, TargetPosition);
        }

        public EntityStateID state;
        private void Update()
        {
            state = _stateMachine.ActiveState;
            
            _stateMachine.OnUpdate();
            
            foreach (var ability in _abilites)
                ability.OnUpdate(Time.deltaTime);
        }
        
        private void ShowResource() => resource.SetActive(true);
        private void HideResource() => resource.SetActive(false);

        private void SetProfession(AntProfessionConfigBase newProfession)
        {
            if (!newProfession.AntsAccess.Contains(AntType) ||
                newProfession == _currentProfessionConfig) 
                return;

            _currentProfessionConfig = newProfession;
            switch (ProfessionType)
            {
                case (AntProfessionType.Worker):
                    var workerLogic = new AntWorkerLogic(this, _currentProfessionConfig as AntWorkerConfig);
                    workerLogic.OnResourceExtracted += ShowResource;
                    workerLogic.OnStorageResources += HideResource;
                    CurrentProfessionLogic = workerLogic;
                    break;
                case (AntProfessionType.MeleeWarrior):
                    CurrentProfessionLogic = new AntMeleeAttackLogic(this, _currentProfessionConfig as AntMeleeWarriorConfig);
                    break;
                case (AntProfessionType.RangeWarrior):
                    CurrentProfessionLogic  = new AntRangeAttackLogic(this, _currentProfessionConfig as AntRangeWarriorConfig);
                    break;
                default: throw new NotImplementedException();
            }

            animator.runtimeAnimatorController = _currentProfessionConfig.AnimatorController;
        }
        
        public void SwitchProfession(AntProfessionConfigBase newProfession)
        {
            SetProfession(newProfession);
            AutoGiveOrder(null, transform.position);
        }
        
        public override void GiveOrder(GameObject target, Vector3 position)
        {
            position.y = 0;
            AutoGiveOrder(target.GetComponent<IUnitTarget>(), position);
        }
        
        public void GiveOrderSwitchProfession(IUnitTarget unitTarget, Vector3 defaultPosition,
            AntProfessionType newProfessionType, AntProfessionRang newProfessionRang)
        {
            if(newProfessionType == ProfessionType && newProfessionRang == ProfessionRang || 
               unitTarget.IsNullOrUnityNull())
            {
                AutoGiveOrder(unitTarget, defaultPosition);
                return;
            }

            TargetProfessionType = newProfessionType;
            TargetProfessionRang = newProfessionRang;
            
            HandleGiveOrder(unitTarget, defaultPosition, UnitPathType.Switch_Profession);
        }
        
        public void AutoGiveOrder(IUnitTarget unitTarget, Vector3 defaultPosition)
        {
            defaultPosition.y = 0;
            Vector3 newTargetPos = CurrentProfessionLogic.AutoGiveOrder(unitTarget, defaultPosition, out _unitPathData);
            SetMovePosition(newTargetPos);
        }

        public void HandleGiveOrder(IUnitTarget unitTarget, Vector3 defaultPosition, UnitPathType unitPathType)
        {
            defaultPosition.y = 0;

            Vector3 newTargetPos = CurrentProfessionLogic.HandleGiveOrder(unitTarget, unitPathType, defaultPosition, out _unitPathData);
            SetMovePosition(newTargetPos);
        }

        private void SetMovePosition(Vector3 newTargetPosition)
        {
            newTargetPosition.y = 0;
            if(TargetPosition != newTargetPosition)
            {
                TargetPosition = newTargetPosition;
                OnTargetPositionChange?.Invoke();
            }
            
            Vector3 curPos = transform.position;
            curPos.y = 0;
            if (TargetPosition == curPos)
            {
                var newState = UnitPathData.PathType switch
                {
                    UnitPathType.Attack => EntityStateID.Attack,
                    UnitPathType.Collect_Resource => EntityStateID.ExtractionResource,
                    UnitPathType.Storage_Resource => EntityStateID.StorageResource,
                    UnitPathType.Build_Construction => EntityStateID.Build,
                    UnitPathType.Move => EntityStateID.Stay,
                    UnitPathType.Switch_Profession => EntityStateID.SwitchProfession,
                    UnitPathType.Repair_Construction => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };

                StateMachine.SetState(newState);
            }
            else
            {
                StateMachine.SetState(EntityStateID.Move);
            }
        }
    }
}