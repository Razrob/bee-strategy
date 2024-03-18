using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntWorkerWorkshop : AntWorkshopBase
    {
        [SerializeField] private AntWorkerWorkshopConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntWorkerWorkshop;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntWorkerWorkshopLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage);
        }
    }
}