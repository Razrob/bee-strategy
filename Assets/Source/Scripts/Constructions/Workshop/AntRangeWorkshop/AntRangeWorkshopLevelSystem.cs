using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntRangeWorkshopLevelSystem : ConstructionLevelSystemBase<AntRangeWorkshopLevel>
    {
        public AntRangeWorkshopLevelSystem(IReadOnlyList<AntRangeWorkshopLevel> levels,
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            
        }
    }
}