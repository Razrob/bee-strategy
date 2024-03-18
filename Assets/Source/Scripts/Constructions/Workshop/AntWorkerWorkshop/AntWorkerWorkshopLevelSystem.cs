using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntWorkerWorkshopLevelSystem : ConstructionLevelSystemBase<AntWorkerWorkshopLevel>
    {
        public AntWorkerWorkshopLevelSystem(IReadOnlyList<AntWorkerWorkshopLevel> levels,
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            
        }
    }
}