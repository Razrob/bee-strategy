using UnityEngine;

namespace Constructions
{
    public class BeeBarrack : BarrackBase
    {
        [SerializeField] private BeeBarrackConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeBarrack;

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeBarrackLevelSystem(config.Levels, unitsSpawnPosition, unitFactory,
                ref resourceRepository, ref _healthStorage, ref recruiter);
        }
        
        //TODO: remove this legacy code, use BarrackBase.RecruitUnit(...)
        public void RecruitBees(UnitType beeID)
        {
            int freeStackIndex = recruiter.FindFreeStack();

            if (freeStackIndex == -1)
            {
                UI_Controller._ErrorCall("All stacks are busy");
                return;
            }

            if (!recruiter.CheckCosts(beeID))
            {
                UI_Controller._ErrorCall("Need more resources");
                return;
            }

            recruiter.RecruitUnit(beeID, freeStackIndex);
        }
    }
}