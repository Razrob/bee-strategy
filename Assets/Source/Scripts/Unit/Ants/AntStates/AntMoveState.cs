using UnityEngine;

namespace Unit.Ants.States
{
    public class AntMoveState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Move;
        
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
            if (_ant.UnitPathData.TargetType == UnitTargetType.None)
            {
                if(Vector3.Distance(_ant.Transform.position, _ant.TargetPosition) < 0.1f)
                    _ant.GiveOrder(_ant.UnitPathData.Target, _ant.transform.position);
            }
            else
            {
                if (_ant.CurrentProfessionLogic.Distance(_ant.UnitPathData.Target) <= _ant.CurrentProfessionLogic.Range)
                {
                    _ant.GiveOrder(_ant.UnitPathData.Target, _ant.Transform.position);
                }
            }
        }

        private void UpdateTargetPosition()
        {
            _ant.SetDestination(_ant.TargetPosition);
        }
    }
}
