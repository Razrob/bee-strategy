using System;

namespace Unit.Ants.States
{
    public class AntStayState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.Stay;
        
        private readonly AntBase _ant;
        
        private AttackLogicBase _attackLogic;
        
        public event Action OnUpdateEvent; 

        public AntStayState(AntBase ant)
        {
            _ant = ant;
        }

        public override void OnStateEnter()
        {
            if ((_ant.ProfessionType == AntProfessionType.MeleeWarrior ||
                 _ant.ProfessionType == AntProfessionType.RangeWarrior) &&
                 _ant.CurrentProfessionLogic.TryCast(out _attackLogic))
            {
                CheckEnemiesInAttackZone();
                
                OnUpdateEvent += CheckEnemiesInAttackZone;
            }
        }

        public override void OnStateExit()
        {
            if (_attackLogic != null)
                OnUpdateEvent -= CheckEnemiesInAttackZone;
        }

        public override void OnUpdate()
        {
            OnUpdateEvent?.Invoke();
        }

        private void CheckEnemiesInAttackZone()
        {
            if (!_attackLogic.CheckEnemiesInAttackZone()) return;
            
            _attackLogic.TryGetNearestDamageableTarget(out IUnitTarget target);
            _ant.AutoGiveOrder(target, target.Transform.position);
        }
    }
}