using UnityEngine;

namespace Unit.Ants.States
{
    public class AntMoveState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Move;

        private const float DistanceBuffer = 0.1f;
        
        private readonly AntBase _ant;
        
        public AntMoveState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            _ant.SetDestination(_ant.TargetPosition);
            _ant.OnTargetPositionChange += UpdateTargetPosition;
        }

        public override void OnStateExit()
        {
            _ant.SetDestination(_ant.Transform.position);
            _ant.OnTargetPositionChange -= UpdateTargetPosition;
        }

        public override void OnUpdate()
        {
            if (_ant.UnitPathData.Target.IsNullOrUnityNull())
            {
                if(Vector3.Distance(_ant.Transform.position, _ant.TargetPosition) < DistanceBuffer)
                    _ant.AutoGiveOrder(_ant.UnitPathData.Target, _ant.transform.position);
            }
            else
            {
                if (_ant.CurrentProfessionLogic.CheckDistance(_ant.UnitPathData))
                    _ant.HandleGiveOrder(_ant.UnitPathData.Target, _ant.transform.position, _ant.UnitPathData.PathType);
            }
        }

        private void UpdateTargetPosition()
        {
            _ant.SetDestination(_ant.TargetPosition);
        }
    }
}
