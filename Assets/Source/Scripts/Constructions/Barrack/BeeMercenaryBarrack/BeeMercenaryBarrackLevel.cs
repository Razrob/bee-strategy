using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;
using UnityEngine;
using UnitsRecruitingSystemCore;

namespace Constructions
{
    [Serializable]
    public class BeeMercenaryBarrackLevel : ConstructionLevelBase
    {
        [SerializeField] [Range(1, 6)] private int recruitingSize = 1;
        [SerializeField] private List<UnitRecruitingData> recruitingData;

        public int RecruitingSize => recruitingSize;
        public IReadOnlyList<UnitRecruitingData> RecruitingData => recruitingData;
    }
}