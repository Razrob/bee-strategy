using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntFortLevelSystem : ConstructionLevelSystemBase<AntFortLevel>
    {
        public AntFortLevelSystem(IReadOnlyList<AntFortLevel> levels, ref ResourceRepository resourceRepository,
            ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            
        }
    }
}