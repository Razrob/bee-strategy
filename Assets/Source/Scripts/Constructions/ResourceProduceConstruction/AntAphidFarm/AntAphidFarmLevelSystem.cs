using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    [Serializable]
    public class AntAphidFarmLevelSystem : ConstructionLevelSystemBase<AntAphidFarmLevel>
    {
        private readonly ResourceProduceCore _resourceProduceCore;
        
        public AntAphidFarmLevelSystem(IReadOnlyList<AntAphidFarmLevel> levels, ref ResourceProduceCore resourceProduceCore,
            ref ResourceRepository resourceRepository, ref ResourceStorage healthStorage) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            _resourceProduceCore = resourceProduceCore = new ResourceProduceCore(CurrentLevel.ResourceProduceProcessInfo);
        }

        protected override void LevelUpLogic()
        {
            base.LevelUpLogic();

            _resourceProduceCore.SetResourceProduceProccessInfo(CurrentLevel.ResourceProduceProcessInfo);
        }
    }
}