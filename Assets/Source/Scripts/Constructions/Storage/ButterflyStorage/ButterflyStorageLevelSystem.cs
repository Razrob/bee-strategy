using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class ButterflyStorageLevelSystem : ConstructionLevelSystemBase<ButterflyStorageLevel>
    {
        public ButterflyStorageLevelSystem(IReadOnlyList<ButterflyStorageLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healPoints)
            : base(levels, ref resourceRepository, ref healPoints)
        { }
    }
}