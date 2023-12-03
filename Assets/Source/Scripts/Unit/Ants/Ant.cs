using System;
using UnityEngine;

namespace Unit.Ants
{
    public class Ant : AntUnit
    {
        [SerializeField] private UnitVisibleZone visibleZone;
        [SerializeField] private GameObject resource;
        
        [SerializeField] private AntProfessionConfigBase worker;
        [SerializeField] private AntProfessionConfigBase melee;
        [SerializeField] private AntProfessionConfigBase range;
        
        private LogicBlockBase _antProfessionLogic;
        
        private void Start()
        {
            UnitPool.Instance.UnitCreation(this);
            SwitchProfession(ProfessionConfig);
            HideResource();
        }
        
        private void Update()
        {
            _antProfessionLogic.HandleUpdate(Time.deltaTime);
            
            foreach (var ability in _abilites)
            {
                ability.OnUpdate(Time.deltaTime);
            }
            
            if(Input.GetKeyDown(KeyCode.A)) SwitchProfession(worker);
            if(Input.GetKeyDown(KeyCode.S)) SwitchProfession(melee);
            if(Input.GetKeyDown(KeyCode.D)) SwitchProfession(range);
        }
        
        public void SwitchProfession(AntProfessionConfigBase antHandItem)
        {
            ProfessionConfig = antHandItem;

            switch (ProfessionConfig.Profession)
            {
                case (AntProfessionType.Worker):
                    AntWorkerLogic antWorkerLogic = new AntWorkerLogic(transform, visibleZone, ProfessionConfig as AntWorkerConfig);
                    antWorkerLogic.OnExtractionEnd += ShowResource;
                    antWorkerLogic.OnStorageResource += HideResource;
                    _antProfessionLogic = antWorkerLogic;
                    break;
                case (AntProfessionType.MeleeWarrior):
                    _antProfessionLogic = new AntMeleeAttackLogic(transform, visibleZone, ProfessionConfig as AntMeleeWarriorConfig);
                    break;
                case (AntProfessionType.RangeWarrior):
                    _antProfessionLogic  = new AntRangeAttackLogic(transform, visibleZone, ProfessionConfig as AntRangeWarriorConfig);
                    break;
                default: throw new NotImplementedException();
            }
        }


        private void ShowResource() => resource.SetActive(true);
        private void HideResource() => resource.SetActive(false);
        
        
        public override void GiveOrder(GameObject target, Vector3 position)
        {
            Debug.Log("Ant " + target);

            target.TryGetComponent(out IUnitTarget unitTarget);
            Vector3 targetPosition = _antProfessionLogic.GiveOrder(unitTarget, position);
            
            SetDestination(targetPosition);
        }
    }
}