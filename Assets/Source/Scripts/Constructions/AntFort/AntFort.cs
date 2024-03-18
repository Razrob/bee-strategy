using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntFort : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] private AntFortConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntFort;
        
        public IConstructionLevelSystem LevelSystem { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntFortLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}