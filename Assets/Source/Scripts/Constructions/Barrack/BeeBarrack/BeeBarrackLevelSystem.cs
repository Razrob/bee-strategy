using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;
using Unit.Factory;
using UnitsRecruitingSystemCore;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class BeeBarrackLevelSystem : ConstructionLevelSystemBase<BeeBarrackLevel>
    {
        private UnitsRecruiter _recruiter;

        public BeeBarrackLevelSystem(IReadOnlyList<BeeBarrackLevel> levels, Transform spawnPosition, 
            UnitFactory unitFactory, ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage,
            ref UnitsRecruiter recruiter) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            _recruiter = recruiter = new UnitsRecruiter(CurrentLevel.RecruitingSize, spawnPosition,
                CurrentLevel.BeesRecruitingData, unitFactory, ref resourceRepository);
        }

        protected override void LevelUpLogic()
        {
            base.LevelUpLogic();

            _recruiter.AddStacks(CurrentLevel.RecruitingSize);
            _recruiter.SetNewDatas(CurrentLevel.BeesRecruitingData);
        }
    }
}