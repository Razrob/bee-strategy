using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntStorage : StorageBase
    {
        [SerializeField] private AntStorageConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntStorage;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntStorageLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}