using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntStorageLevelSystem : ConstructionLevelSystemBase<AntStorageLevel>
    {
        public AntStorageLevelSystem(IReadOnlyList<AntStorageLevel> levels,
            ref ResourceRepository resourceRepository, ref ResourceStorage healPoints)
            : base(levels, ref resourceRepository, ref healPoints)
        { }
    }
}