using UnityEngine;

namespace Constructions
{
    public class BeeSiegeWeaponsBarrack : BarrackBase
    {
        [SerializeField] private BeeSiegeWeaponsBarrackConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeSiegeWeaponsBarrack;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeSiegeWeaponsBarrackLevelSystem(config.Levels, unitsSpawnPosition, unitFactory,
                ref resourceRepository, ref _healthStorage, ref recruiter);
        }
    }
}