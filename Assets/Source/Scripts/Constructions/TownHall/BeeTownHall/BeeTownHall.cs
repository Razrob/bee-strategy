using Construction.TownHalls;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class BeeTownHall : TownHallBase
    {
        [SerializeField] private BeeTownHallConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeTownHall;
        
        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            
            gameObject.name = "TownHall";

            var takeResourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeTownHallLevelSystem(config.Levels, workerBeesSpawnPosition, _unitFactory, 
                ref takeResourceRepository, ref _healthStorage, ref _recruiter);
        }
    }  
}
