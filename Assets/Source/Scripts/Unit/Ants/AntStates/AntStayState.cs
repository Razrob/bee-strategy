using System;
using UnityEngine;

namespace Unit.Ants.States
{
    public class AntStayState : EntityStateBase
    {
        private readonly AntBase _ant;
        
        public override EntityStateID EntityStateID => EntityStateID.Stay;
        
        private AttackLogicBase _attackLogic;
        
        public event Action OnUpdateEvent; 

        public AntStayState(AntBase ant)
        {
            _ant = ant;
        }

        public override void OnStateEnter()
        {
            _ant.OnSwitchProfession += UpdateLogic;
            UpdateLogic();
        }

        public override void OnStateExit()
        {
            _ant.OnSwitchProfession -= UpdateLogic;
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
            _ant.GiveOrder(target, target.Transform.position);
        }

        private void UpdateLogic()
        {
            if ((_ant.ProfessionType == AntProfessionType.MeleeWarrior || _ant.ProfessionType == AntProfessionType.RangeWarrior) &&
                _ant.CurrentProfessionLogic.TryCast(out _attackLogic))
            {
                CheckEnemiesInAttackZone();

                OnUpdateEvent += CheckEnemiesInAttackZone;
            }
        }
    }
}