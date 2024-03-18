using Construction.TownHalls;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntTownHall : TownHallBase
    {
        [SerializeField] private AntTownHallConfig config;

        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntTownHall;

        public override IConstructionLevelSystem LevelSystem { get; protected set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            
            FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(transform.position, this);
            var takeResourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntTownHallLevelSystem(config.Levels, workerBeesSpawnPosition, _unitFactory, 
                ref takeResourceRepository, ref _healthStorage, ref _recruiter);
        }
        
        //TODO: temporary code. Remove this, when ants ui will be create
        [ContextMenu("RecruitAntStandard")]
        private void RecruitAntStandard() => RecruitUnit(UnitType.AntStandard);
        [ContextMenu("RecruitAntBig")]
        private void RecruitAntBig() => RecruitUnit(UnitType.AntBig);
        [ContextMenu("RecruitAntFly")]
        private void RecruitAntFly() => RecruitUnit(UnitType.AntFlying);
    }
}