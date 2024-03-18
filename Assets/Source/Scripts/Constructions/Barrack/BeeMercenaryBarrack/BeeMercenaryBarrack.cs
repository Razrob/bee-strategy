using UnityEngine;

namespace Constructions
{
    public class BeeMercenaryBarrack : BarrackBase
    {
        [SerializeField] private BeeMercenaryBarrackConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeMercenaryBarrack;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeMercenaryBarrackLevelSystem(config.Levels, unitsSpawnPosition, unitFactory,
                ref resourceRepository, ref _healthStorage, ref recruiter);
        }
    }
}