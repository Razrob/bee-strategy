using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class ButterflyStorage : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] private ButterflyStorageConfig config;
    
        public override AffiliationEnum Affiliation => AffiliationEnum.Butterflies;
        public override ConstructionID ConstructionID => ConstructionID.ButterflyStorage;
        
        public IConstructionLevelSystem LevelSystem { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new ButterflyStorageLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }   
}
