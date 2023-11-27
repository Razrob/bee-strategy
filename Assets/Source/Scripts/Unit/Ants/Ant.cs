using System;
using UnityEngine;

namespace Unit.Ants
{
    public class Ant : AntUnit
    {
        [SerializeField] private AntProfessionType startProfession = AntProfessionType.Worker;
        [SerializeField] private UnitVisibleZone visibleZone;
        
        private AntHandItem _handItem;
        private LogicBlockBase _antProfessionLogic;

        private MeleeAttackLogic _meleeAttackLogic;
        private RangeAttackLogic _rangeAttackLogic;
        
        private void Start()
        {
            UnitPool.Instance.UnitCreation(this);
            SwitchProfession(startProfession, new AntHandItem(10, 10, 2));
        }
        
        private void Update()
        {
            _antProfessionLogic.HandleUpdate(Time.deltaTime);
            
            foreach (var abilite in _abilites)
            {
                abilite.OnUpdate(Time.deltaTime);
            }
            
            if(Input.GetKeyDown(KeyCode.A)) SwitchProfession(AntProfessionType.Worker, new AntHandItem(10, 10, 2));
            if(Input.GetKeyDown(KeyCode.S)) SwitchProfession(AntProfessionType.MeleeWarrior, new AntHandItem(10, 10, 2));
            if(Input.GetKeyDown(KeyCode.D)) SwitchProfession(AntProfessionType.RangeWarrior, new AntHandItem(10, 10, 2));
        }
        
        public void SwitchProfession(AntProfessionType newProfessionType, AntHandItem antHandItem)
        {
            ProfessionType = newProfessionType;
            _handItem = antHandItem;

            switch (ProfessionType)
            {
                case (AntProfessionType.Worker):
                    UnitType = UnitType.Worker;
                    _antProfessionLogic = new AntWorkerLogic(transform, visibleZone, _handItem);
                    break;
                case (AntProfessionType.MeleeWarrior):
                    UnitType = UnitType.AttackUnit;
                    _antProfessionLogic = _meleeAttackLogic= new AntMeleeAttackLogic(transform, visibleZone, _handItem);
                    break;
                case (AntProfessionType.RangeWarrior):
                    UnitType = UnitType.AttackUnit;
                    _antProfessionLogic = _rangeAttackLogic = new AntRangeAttackLogic(transform, visibleZone, _handItem);
                    break;
                default: throw new NotImplementedException();
            }
        }

        public override void GiveOrder(GameObject target, Vector3 position)
        {
            Debug.Log("Ant " + target);

            if (target.TryGetComponent(out IUnitTarget unitTarget))
                _antProfessionLogic.GiveOrder(unitTarget);
            else
            {
                _antProfessionLogic.GiveOrder(null);
            }
            
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