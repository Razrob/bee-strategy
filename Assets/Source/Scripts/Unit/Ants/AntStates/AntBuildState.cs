using UnityEngine;

namespace Unit.Ants.States
{
    public class AntBuildState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Build;

        private readonly AntBase _ant;
        
        public AntBuildState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.Worker ||
                _ant.UnitPathData.TargetType != UnitTargetType.Construction ||
                _ant.UnitPathData.Target.CheckOnNullAndUnityNull() ||
                !_ant.UnitPathData.Target.TryCast(out BuildingProgressConstruction buildingProgressConstruction))
            {
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{_ant.UnitPathData.TargetType} | " +
                                 $"{_ant.UnitPathData.Target.CheckOnNullAndUnityNull()} | " +
                                 $"{!_ant.UnitPathData.Target.TryCast(out buildingProgressConstruction)}");
                
                _ant.GiveOrder(null, _ant.transform.position);
                // _ant.StateMachine.SetState(EntityStateID.Stay);
                return;
            }
            
            buildingProgressConstruction.WorkerArrived = true;
            buildingProgressConstruction.OnTimerEnd += EndOfBuildConstruction;
        }

        public override void OnStateExit()
        {
            
        }

        public override void OnUpdate()
        {
            //TODO: update construction building progress in the unit logic, at the moment this logic on the construction side
        }

        private void EndOfBuildConstruction(BuildingProgressConstruction buildingProgressConstruction)
        {
            buildingProgressConstruction.WorkerArrived = false;

            _ant.GiveOrder(null, _ant.transform.position);
        }
    }
}