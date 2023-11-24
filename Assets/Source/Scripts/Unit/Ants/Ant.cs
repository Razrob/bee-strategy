using System;
using UnityEngine;

namespace Unit.Ants
{
    public class Ant : AntUnit
    {
        private UnitVisibleZone _visibleZone;
        private AntHandItem _handItem;
        private WorkerLogic _workerLogic;
        private AttackLogicBase _attackLogic;
        
        private void Start()
        {
            UnitPool.Instance.UnitCreation(this);
        }

        public void SwitchProfession(AntProfessionType newProfessionType, AntHandItem antHandItem)
        {
            ProfessionType = newProfessionType;
            _handItem = antHandItem;

            switch (ProfessionType)
            {
                case (AntProfessionType.Worker):
                    UnitType = UnitType.Worker;
                    break;
                case (AntProfessionType.MeleeWarrior):
                    UnitType = UnitType.AttackUnit;
                    _attackLogic = new AntMeleeAttackLogic(transform, _visibleZone, _handItem);
                    break;
                case (AntProfessionType.RangeWarrior):
                    UnitType = UnitType.AttackUnit;
                    _attackLogic = new AntRangeAttackLogic(transform, _visibleZone, _handItem);
                    break;
                default: throw new NotImplementedException();
            }
        }

        public override void GiveOrder(GameObject target, Vector3 position)
        {
            if(target.TryCast(out IUnitTarget unitTarget))
                switch (UnitType)
                {
                    case (UnitType.Worker):
                        _workerLogic.GiveOrder(unitTarget);
                        SetDestination(target.transform.position);
                        break;
                }

            // if (UnitType == UnitType.Worker && target.TryCast(out IUnitTarget unitTarget))
            //     _workerLogic.GiveOrder(unitTarget, position);
        }

        public void GiveMoveOrder(Vector3 position)
        {
            SetDestination(position);
        }
    }

    public enum AntHandleItemType
    {
        WorkTool0 = 0,
        WorkTool1 = 10,
        WorkTool2 = 20,

        MeleeWeapon0 = 30,
        MeleeWeapon1 = 40,
        MeleeWeapon2 = 50,

        RangeWeapon0 = 60,
        RangeWeapon1 = 70,
        RangeWeapon2 = 80
    }
}