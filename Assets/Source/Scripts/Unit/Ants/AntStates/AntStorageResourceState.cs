using UnityEngine;

namespace Unit.Ants.States
{
    public class AntStorageResourceState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.StorageResource;

        private readonly AntBase _ant;

        private WorkerLogic _workerLogic;
        
        public AntStorageResourceState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.Worker ||
                !_ant.CurrentProfessionLogic.TryCast(out _workerLogic) ||
                !_workerLogic.GotResource ||
                !_ant.UnitPathData.Target.CastPossible<TownHall>())
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{!_ant.CurrentProfessionLogic.TryCast(out _workerLogic)} | " +
                                 $"{!_ant.UnitPathData.Target.CastPossible<TownHall>()}");            
#endif
                _ant.AutoGiveOrder(null, _ant.transform.position);
                return;
            }
            
            ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _workerLogic.ExtractionCapacity);
            _workerLogic.StorageResources();
            
            _ant.AutoGiveOrder(null, _ant.transform.position);
        }

        public override void OnStateExit()
        {

        }

        public override void OnUpdate()
        {
            
        }
    }
}