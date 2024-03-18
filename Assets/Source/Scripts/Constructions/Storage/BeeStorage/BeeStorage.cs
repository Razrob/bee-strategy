using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class BeeStorage : StorageBase
    {
        [SerializeField] private BeeStorageConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeStorage;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeStorageLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}