using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Ants.States
{
    public class AntAttackState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Attack;

        private readonly AntBase _ant;

        private AttackLogicBase _attackLogic;
        
        public AntAttackState(AntBase ant)
        {
            _ant = ant;
        }
        
        public override void OnStateEnter()
        {
            if (_ant.ProfessionType != AntProfessionType.MeleeWarrior && _ant.ProfessionType != AntProfessionType.RangeWarrior ||
                !_ant.CurrentProfessionLogic.TryCast(out _attackLogic) ||
                !CheckTargetsExistInAttackZone())
            {
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{!_ant.CurrentProfessionLogic.TryCast(out _attackLogic)}");
                
                _ant.GiveOrder(null, _ant.transform.position);
                // _ant.StateMachine.SetState(EntityStateID.Stay);
                return;
            }
            
            _attackLogic.OnCooldownEnd += TryAttack;
            _attackLogic.OnExitTargetFromAttackZone += OnExitTargetFromAttackZone;

            if(_attackLogic.CanAttack) TryAttack();
        }

        public override void OnStateExit()
        {
            _attackLogic.OnCooldownEnd -= TryAttack;
        }

        public override void OnUpdate()
        {
            if (!CheckTargetsExistInAttackZone())
                // _ant.StateMachine.SetState(EntityStateID.Stay);
                _ant.GiveOrder(null, _ant.transform.position);
        }
        
        private void TryAttack() => _attackLogic.TryAttack(_ant.UnitPathData.Target);

        private bool CheckTargetsExistInAttackZone() => _attackLogic.CheckEnemiesInAttackZone();
        
        private void OnExitTargetFromAttackZone()
        {
            if(_attackLogic.EnemiesCount <= 0)
                _ant.GiveOrder(null, _ant.transform.position);
                // _ant.StateMachine.SetState(EntityStateID.Stay);
        }
    }
}