using System;
using System.Collections.Generic;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class ButterflyPoisonFlowerLevelSystem : ConstructionLevelSystemBase<ButterflyPoisonFlowerLevel>
    {
        private readonly ButterflyPoisonFlowerPoisonFogProcessor _poisonFogProcessor;
        private readonly ButterflyPoisonFlowerAttackProcessor _attackProcessor;
        private readonly SphereCollider _attackZoneCollider;
        
        public ButterflyPoisonFlowerLevelSystem(
            IReadOnlyList<ButterflyPoisonFlowerLevel> levels, 
            ref ResourceRepository resourceRepository,
            ref ResourceStorage healthStorage, 
            ref SphereCollider attackZoneCollider, 
            ref ButterflyPoisonFlowerAttackProcessor attackProcessor,
            ref ButterflyPoisonFlowerPoisonFogProcessor poisonFogProcessor) 
            : base(levels, ref resourceRepository, ref healthStorage)
        {
            _attackZoneCollider = attackZoneCollider;
            _attackProcessor = attackProcessor;
            _poisonFogProcessor = poisonFogProcessor;
            
            _attackZoneCollider.radius = CurrentLevel.AttackRadius;
            _attackProcessor.SetData(CurrentLevel.AttackCooldown, CurrentLevel.AttackDamage, CurrentLevel.DamageRadius);
            _poisonFogProcessor.SetData(CurrentLevel.FogExistTime, CurrentLevel.FogRadius, CurrentLevel.StaticFogRadius);
        }

        protected override void LevelUpLogic()
        {
            base.LevelUpLogic();

            _attackProcessor.SetData(CurrentLevel.AttackCooldown, CurrentLevel.AttackDamage, CurrentLevel.DamageRadius);
            _attackZoneCollider.radius = CurrentLevel.AttackRadius;
            _poisonFogProcessor.SetData(CurrentLevel.FogExistTime, CurrentLevel.FogRadius, CurrentLevel.StaticFogRadius);
        }
    }
}