using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntHouse : StorageBase
    {
        [SerializeField] private AntHouseConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntHouse;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntHouseLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}