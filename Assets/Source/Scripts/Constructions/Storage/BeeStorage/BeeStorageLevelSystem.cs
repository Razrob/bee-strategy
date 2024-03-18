using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class BeeStorageLevelSystem : ConstructionLevelSystemBase<BeeStorageLevel>
    {
        public BeeStorageLevelSystem(IReadOnlyList<BeeStorageLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healPoints)
            : base(levels, ref resourceRepository, ref healPoints)
        { }
    }
}