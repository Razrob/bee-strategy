using System.Collections.Generic;
using Constructions.LevelSystemCore;

namespace Constructions
{
    public class BeeWaxTowerLevelSystem : ConstructionLevelSystemBase<BeeWaxTowerLevel>
    {
        private readonly BeeWaxTowerAttackProcessor _attackProcessor; 
        
        public BeeWaxTowerLevelSystem(IReadOnlyList<BeeWaxTowerLevel> levels, ref ResourceRepository resourceRepository, 
            ref ResourceStorage healthStorage, ref BeeWaxTowerAttackProcessor attackProcessor) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            _attackProcessor = attackProcessor;

            _attackProcessor.SetData(CurrentLevel.ProjectilesCount, CurrentLevel.Cooldown, CurrentLevel.SpawnPause, 
                CurrentLevel.Damage, CurrentLevel.MoveSpeedChangerConfig, CurrentLevel.ProjectilesType);
        }

        protected override void LevelUpLogic()
        {
            base.LevelUpLogic();
            
            _attackProcessor.SetData(CurrentLevel.ProjectilesCount, CurrentLevel.Cooldown, CurrentLevel.SpawnPause, 
                CurrentLevel.Damage, CurrentLevel.MoveSpeedChangerConfig, CurrentLevel.ProjectilesType);
        }
    }
}