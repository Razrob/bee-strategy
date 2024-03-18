using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntMeleeWorkshop : AntWorkshopBase
    {
        [SerializeField] private AntMeleeWorkshopConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntMeleeWorkshop;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntMeleeWorkshopLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}