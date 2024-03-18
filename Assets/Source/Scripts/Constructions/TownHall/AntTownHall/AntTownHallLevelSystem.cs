using System.Collections.Generic;
using Constructions.LevelSystemCore;
using Unit.Factory;
using UnitsRecruitingSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntTownHallLevelSystem : ConstructionLevelSystemBase<AntTownHallLevel>
    {
        private UnitsRecruiter _recruiter;

        public AntTownHallLevelSystem(IReadOnlyList<AntTownHallLevel> levels, Transform spawnPosition, 
            UnitFactory unitFactory, ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage, 
            ref UnitsRecruiter recruiter) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            _recruiter = recruiter = new UnitsRecruiter(CurrentLevel.RecruitingSize, spawnPosition,
                CurrentLevel.RecruitingData, unitFactory, ref resourceRepository);
        }

        protected override void LevelUpLogic()
        {
            base.LevelUpLogic();

            _recruiter.AddStacks(CurrentLevel.RecruitingSize);
            _recruiter.SetNewDatas(CurrentLevel.RecruitingData);
        }
    }
}