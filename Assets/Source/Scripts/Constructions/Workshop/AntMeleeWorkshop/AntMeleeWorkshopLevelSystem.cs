using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntMeleeWorkshopLevelSystem : ConstructionLevelSystemBase<AntMeleeWorkshopLevel>
    {
        public AntMeleeWorkshopLevelSystem(IReadOnlyList<AntMeleeWorkshopLevel> levels, 
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            
        }
    }
}