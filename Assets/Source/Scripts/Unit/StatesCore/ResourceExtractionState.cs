using Unit.ProfessionsCore;
using UnityEngine;

namespace Unit.States
{
    public class ResourceExtractionState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.ExtractionResource;

        private readonly MovingUnit _unit;
        private readonly WorkerProfession _workerProfession;
        
        public ResourceExtractionState(MovingUnit unit, WorkerProfession workerProfession)
        {
            _unit = unit;
            _workerProfession = workerProfession;
        }

        public override void OnStateEnter()
        {
            if (_workerProfession.ResourceExtractionProcessor.GotResource ||
                !_unit.CurrentPathData.Target.TryCast(out ResourceSourceBase resourceSource))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Some problem:" +
                                 $"| {!_unit.CurrentPathData.Target.TryCast(out resourceSource)}");
#endif
                _unit.AutoGiveOrder(null);
                return;
            }
            
            _workerProfession.ResourceExtractionProcessor.OnResourceExtracted += ResourceExtracted;
            _workerProfession.ResourceExtractionProcessor.StartExtraction(resourceSource.ResourceID);
        }

        public override void OnStateExit()
        {
            _workerProfession.ResourceExtractionProcessor.OnResourceExtracted -= ResourceExtracted;
            _workerProfession.ResourceExtractionProcessor.AbortExtraction();
        }

        public override void OnUpdate()
        {
            
        }

        private void ResourceExtracted()
        {
            _unit.AutoGiveOrder(null);
        }
    }
}