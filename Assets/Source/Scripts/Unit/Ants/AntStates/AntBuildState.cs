using UnityEngine;

namespace Unit.Ants.States
{
    public class AntBuildState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Build;

        private readonly AntBase _ant;
        private BuildingProgressConstruction _buildingProgressConstruction;
        
        public AntBuildState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.Worker ||
                _ant.UnitPathData.TargetType != UnitTargetType.Construction ||
                _ant.UnitPathData.Target.IsNullOrUnityNull() ||
                !_ant.UnitPathData.Target.TryCast(out _buildingProgressConstruction))
            {
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{_ant.UnitPathData.TargetType} | " +
                                 $"{_ant.UnitPathData.Target.IsNullOrUnityNull()} | " +
                                 $"{!_ant.UnitPathData.Target.TryCast(out _buildingProgressConstruction)}");
                
                _ant.AutoGiveOrder(null, _ant.transform.position);
                return;
            }
            
            _buildingProgressConstruction.WorkerArrived = true;
            _buildingProgressConstruction.OnTimerEnd += EndOfBuildConstruction;
        }

        public override void OnStateExit()
        {
            _buildingProgressConstruction.WorkerArrived = false;
            _buildingProgressConstruction.OnTimerEnd -= EndOfBuildConstruction;
        }

        public override void OnUpdate()
        {
            //TODO: update construction building progress in the unit logic, at the moment this logic on the construction side
        }

        private void EndOfBuildConstruction(BuildingProgressConstruction buildingProgressConstruction)
        {
            buildingProgressConstruction.WorkerArrived = false;

            _ant.AutoGiveOrder(null, _ant.transform.position);
        }
    }
}