using UnityEngine;

namespace Unit.Ants.States
{
    public class AntStorageResourceState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.StorageResource;

        private readonly AntBase _ant;

        private WorkerLogic _workerLogic;
        private TownHall _townHall;
        
        public AntStorageResourceState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.Worker ||
                !_ant.CurrentProfessionLogic.TryCast(out _workerLogic) ||
                !_workerLogic.GotResource ||
                !_ant.UnitPathData.Target.TryCast(out _townHall))
            {
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{!_ant.CurrentProfessionLogic.TryCast(out _workerLogic)} | " +
                                 $"{!_ant.UnitPathData.Target.TryCast(out _townHall)}");
                
                _ant.GiveOrder(null, _ant.transform.position);
                // _ant.StateMachine.SetState(EntityStateID.Stay);
                return;
            }
            
            ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _workerLogic.ExtractionCapacity);
            _workerLogic.ResourcesStoraged();
            
            _ant.GiveOrder(null, _ant.transform.position);
            // _ant.StateMachine.SetState(EntityStateID.Stay);
        }

        public override void OnStateExit()
        {

        }

        public override void OnUpdate()
        {
            
        }
        
    }
}