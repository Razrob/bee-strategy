using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntRangeWorkshop : AntWorkshopBase
    {
        [SerializeField] private AntRangeWorkshopConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntRangeWorkshop;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntRangeWorkshopLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}