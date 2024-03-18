using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntHouseLevelSystem : ConstructionLevelSystemBase<AntHouseLevel>
    {
        public AntHouseLevelSystem(IReadOnlyList<AntHouseLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        { }
    }
}