using System.Collections;
using UnityEngine;

namespace Unit.Ants.States
{
    public class AntResourceExtractionState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.ExtractionResource;

        private WorkerLogic _workerLogic;
        private ResourceSourceBase _resourceSource;
        private readonly AntBase _ant;

        private Coroutine _coroutine;
        
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
                Debug.LogWarning($"Some problem: {_ant.ProfessionType} " +
                                 $"| {!_ant.CurrentProfessionLogic.TryCast(out _workerLogic)} " +
                                 $"| {!_ant.UnitPathData.Target.TryCast(out _resourceSource)}");
                
                _ant.GiveOrder(null, _ant.transform.position);
                // _ant.StateMachine.SetState(EntityStateID.Stay);
                return;
            }
            
            _coroutine = _ant.StartCoroutine(ExtractResource(2));
        }

        public override void OnStateExit()
        {
            if(_coroutine != null)
                _ant.StopCoroutine(_coroutine);
        }

        public override void OnUpdate() { }
        
        IEnumerator ExtractResource(float extractionTime)
        {
            yield return new WaitForSeconds(extractionTime);

            if (_resourceSource is null)
            {
                _ant.GiveOrder(null, _ant.transform.position);
                yield break;
            }
            
            _resourceSource.ExtractResource(_workerLogic.ExtractionCapacity);
            _workerLogic.ResourceExtracted(_resourceSource.ResourceID);
            _ant.GiveOrder(null, _ant.transform.position);
        }
    }
}