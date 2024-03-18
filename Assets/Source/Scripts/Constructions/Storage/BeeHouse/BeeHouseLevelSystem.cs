using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class BeeHouseLevelSystem : ConstructionLevelSystemBase<BeeHouseLevel>
    {
        public BeeHouseLevelSystem(IReadOnlyList<BeeHouseLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        { }
    }
}