using System;
using System.Collections.Generic;
using System.Linq;
using Unit.Ants.States;
using UnityEngine;

namespace Unit.Ants
{
    public abstract class AntBase : AntUnit
    {
        public abstract AntType CurrentAntType { get; }
        
        [SerializeField] private GameObject resource;
        [SerializeField] protected AntProfessionConfigBase DefaultProfessionConfig;
        
        private AntProfessionConfigBase _currentProfessionConfig;
        
        public AntProfessionType ProfessionType => _currentProfessionConfig.Profession;
        public UnitType UnitType => _currentProfessionConfig.UnitType; 
        public EntityStateID CurrentState => _stateMachine.ActiveState;
        
        public LogicBlockBase CurrentProfessionLogic { get; private set; }
        public UnitPathData UnitPathData { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public event Action OnTargetPositionChange;
        public event Action OnSwitchProfession;
        
        private void Start()
        {
            UnitPool.Instance.UnitCreation(this);
            HideResource();
            TargetPosition = transform.position;
            SetProfession(DefaultProfessionConfig);

            List<EntityStateBase> stateBases = new List<EntityStateBase>()
            {
                new AntStayState(this),
                new AntMoveState(this),
                new AntBuildState(this),
                new AntResourceExtractionState(this),
                new AntStorageResourceState(this),
                new AntAttackState(this)
            };
            _stateMachine = new EntityStateMachine(stateBases, EntityStateID.Stay);

            GiveOrder(null, TargetPosition);
        }
        
        private void Update()
        {
            _stateMachine.OnUpdate();
            
            foreach (var ability in _abilites)
                ability.OnUpdate(Time.deltaTime);
        }
        
        private void ShowResource() => resource.SetActive(true);
        private void HideResource() => resource.SetActive(false);

        private void SetProfession(AntProfessionConfigBase newProfession)
        {
            if (!newProfession.AntsAccess.Contains(CurrentAntType))
                return;

            if(newProfession == _currentProfessionConfig) 
                return;

            _currentProfessionConfig = newProfession;
            switch (ProfessionType)
            {
                case (AntProfessionType.Worker):
                    var workerLogic = new AntWorkerLogic(this, _currentProfessionConfig as AntWorkerConfig);
                    CurrentProfessionLogic = workerLogic;
                    workerLogic.OnResourceExtracted += ShowResource;
                    workerLogic.OnResourcesStoraged += HideResource;
                    break;
                case (AntProfessionType.MeleeWarrior):
                    CurrentProfessionLogic = new AntMeleeAttackLogic(this, _currentProfessionConfig as AntMeleeWarriorConfig);
                    break;
                case (AntProfessionType.RangeWarrior):
                    CurrentProfessionLogic  = new AntRangeAttackLogic(this, _currentProfessionConfig as AntRangeWarriorConfig);
                    break;
                default: throw new NotImplementedException();
            }  
        }
        
        public void SwitchProfession(AntProfessionConfigBase newProfession)
        {
            SetProfession(newProfession);

            GiveOrder(UnitPathData?.Target, TargetPosition);
        }

        public override void GiveOrder(GameObject target, Vector3 position)
        {
            position.y = 0;
            
            GiveOrder(target.GetComponent<IUnitTarget>(), position);
        }

        public void GiveOrder(IUnitTarget unitTarget, Vector3 defaultPosition)
        {
            defaultPosition.y = 0;
            
            UnitPathData = CurrentProfessionLogic.TakePathData(unitTarget);
            Vector3 newTargetPosition = CurrentProfessionLogic.GiveOrder(unitTarget, defaultPosition);
            newTargetPosition.y = 0;
            if(TargetPosition != newTargetPosition)
            {
                TargetPosition = newTargetPosition;
                OnTargetPositionChange?.Invoke();
            }
            else
                TargetPosition = newTargetPosition;
            
            Vector3 curPos = transform.position;
            curPos.y = 0;
            
            var prevState = CurrentState;
            EntityStateID newState;
            if (TargetPosition == curPos)
            {
                switch (UnitPathData.PathType)
                {
                    case UnitPathType.Attack:
                        newState = EntityStateID.Attack;
                        break;
                    case UnitPathType.Collect_Resource:
                        newState = EntityStateID.ExtractionResource;
                        break;
                    case UnitPathType.Storage_Resource:
                        newState = EntityStateID.StorageResource;
                        break;
                    case UnitPathType.Build_Construction:
                        newState = EntityStateID.Build;
                        break;
                    case UnitPathType.Move:
                        newState = EntityStateID.Stay;
                        break;
                    default: throw new NotImplementedException();
                }
                
                StateMachine.SetState(newState);
            }
            else
            {
                newState = EntityStateID.Move;
                StateMachine.SetState(newState);
            }

            if (prevState == newState)
            {
                OnSwitchProfession?.Invoke();
            }
        }
    }
}