using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class ButterflyHouse : StorageBase
    {
        [SerializeField] private ButterflyHouseConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Butterflies;
        public override ConstructionID ConstructionID => ConstructionID.ButterflyHouse;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new ButterflyHouseLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}