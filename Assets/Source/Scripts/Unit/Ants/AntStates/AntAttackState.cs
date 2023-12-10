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
            if(_ant.ProfessionType != AntProfessionType.MeleeWarrior && _ant.ProfessionType != AntProfessionType.RangeWarrior ||
               !_ant.CurrentProfessionLogic.TryCast(out _attackLogic) ||
               !CheckEnemiesInAttackZone())
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Some problem: " +
                                 $"{_ant.ProfessionType} | " +
                                 $"{!_ant.CurrentProfessionLogic.TryCast(out _attackLogic)}");           
#endif
                _ant.AutoGiveOrder(null, _ant.transform.position);
                return;
            }
            
            _attackLogic.OnCooldownEnd += TryAttack;
            _attackLogic.OnExitEnemyFromAttackZone += OnExitEnemyFromAttackZone;

            if(_attackLogic.CanAttack) TryAttack();
        }

        public override void OnStateExit()
        {
            _attackLogic.OnCooldownEnd -= TryAttack;
            _attackLogic.OnExitEnemyFromAttackZone -= OnExitEnemyFromAttackZone;
        }

        public override void OnUpdate()
        {
            if (!CheckEnemiesInAttackZone())
                _ant.AutoGiveOrder(null, _ant.transform.position);
        }
        
        private void TryAttack() => _attackLogic.TryAttack(_ant.UnitPathData.Target);

        private bool CheckEnemiesInAttackZone() => _attackLogic.CheckEnemiesInAttackZone();
        
        private void OnExitEnemyFromAttackZone()
        {
            if(_attackLogic.EnemiesCount <= 0)
                _ant.AutoGiveOrder(null, _ant.transform.position);
        }
    }
}