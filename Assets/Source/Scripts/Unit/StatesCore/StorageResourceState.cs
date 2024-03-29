using Constructions;
using Unit.ProfessionsCore;
using UnityEngine;

namespace Unit.States
{
    public class StorageResourceState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.StorageResource;

        private readonly MovingUnit _unit;
        private readonly WorkerProfession _workerProfession;
        
        public StorageResourceState(MovingUnit unit, WorkerProfession workerProfession)
        {
            _unit = unit;
            _workerProfession = workerProfession;
        }
        
        public override void OnStateEnter()
        {
            if (!_workerProfession.ResourceExtractionProcessor.GotResource ||
                !_unit.CurrentPathData.Target.CastPossible<BeeTownHall>())
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Some problem: " +
                                 $"{!_unit.CurrentPathData.Target.CastPossible<BeeTownHall>()}");            
#endif
                _unit.AutoGiveOrder(null);
                return;
            }
            
            ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, _workerProfession.ResourceExtractionProcessor.ExtractionCapacity);
            _workerProfession.ResourceExtractionProcessor.StorageResources();
            
            _unit.AutoGiveOrder(null);
        }

        public override void OnStateExit()
        {

        }

        public override void OnUpdate()
        {
            
        }
    }
}