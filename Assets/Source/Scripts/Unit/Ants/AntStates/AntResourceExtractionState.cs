using UnityEngine;

namespace Unit.Ants.States
{
    public class AntResourceExtractionState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.ExtractionResource;

        private readonly AntBase _ant;

        private WorkerLogic _workerLogic;
        private ResourceSourceBase _resourceSource;
        
        public AntResourceExtractionState(AntBase ant)
        {
            _ant = ant;
        }

        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.Worker ||
                !_ant.CurrentProfessionLogic.TryCast(out _workerLogic) ||
                _workerLogic.GotResource ||
                !_ant.UnitPathData.Target.TryCast(out _resourceSource))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Some problem: {_ant.ProfessionType} " +
                                 $"| {!_ant.CurrentProfessionLogic.TryCast(out _workerLogic)} " +
                                 $"| {!_ant.UnitPathData.Target.TryCast(out _resourceSource)}");
#endif
                _ant.AutoGiveOrder(null, _ant.transform.position);
                return;
            }
            
            _workerLogic.OnResourceExtracted += ResourceExtracted;
            _workerLogic.StartExtraction(_resourceSource.ResourceID);
        }

        public override void OnStateExit()
        {
            _workerLogic.OnResourceExtracted -= ResourceExtracted;
            _workerLogic.StopExtraction();
        }

        public override void OnUpdate() { }

        private void ResourceExtracted()
        {
            _ant.AutoGiveOrder(null, _ant.transform.position);
        }
    }
}