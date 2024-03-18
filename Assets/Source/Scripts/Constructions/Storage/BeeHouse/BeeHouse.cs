using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class BeeHouse : StorageBase
    {
        [SerializeField] private BeeHouseConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeHouse;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeHouseLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}