using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class ButterflyHouseLevelSystem : ConstructionLevelSystemBase<ButterflyHouseLevel>
    {
        public ButterflyHouseLevelSystem(IReadOnlyList<ButterflyHouseLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        { }
    }
}