using Constructions.LevelSystemCore;
using Poison;
using Projectiles.Factory;
using UnityEngine;
using Zenject;

namespace Constructions
{
    public class ButterflyPoisonFlower : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] private ButterflyPoisonFlowerConfig config;
        [SerializeField] private TriggerBehaviour triggerBehaviour;
        [SerializeField] private SphereCollider attackZoneCollider;

        [Inject] private ProjectileFactory _projectileFactory;
        [Inject] private PoisonFogFactory _poisonFogFactory;

        private ButterflyPoisonFlowerAttackProcessor _attackProcessor;
        private ButterflyPoisonFlowerPoisonFogProcessor _poisonFogProcessor;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Butterflies;
        public override ConstructionID ConstructionID => ConstructionID.ButterflyPoisonFlower;
        
        public IConstructionLevelSystem LevelSystem { get; private set; }
        
        protected override void OnAwake()
        {
            _attackProcessor = new ButterflyPoisonFlowerAttackProcessor(transform, _projectileFactory, triggerBehaviour);
            _poisonFogProcessor = new ButterflyPoisonFlowerPoisonFogProcessor(transform, _poisonFogFactory);
            
            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new ButterflyPoisonFlowerLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage, 
                ref attackZoneCollider, ref _attackProcessor, ref _poisonFogProcessor);
            
            OnDestruction += _attackProcessor.KillCooldownTimer;
            OnDestruction += _poisonFogProcessor.SpawnPoisonFog;

            _updateEvent += UpdateAttackProcessor;
        }

        private void UpdateAttackProcessor()
            => _attackProcessor.HandleUpdate(Time.deltaTime);

        //TODO: remove this temporary code, when ui will be create
        [ContextMenu("LevelUp")]
        private void LevelUp() 
            => LevelSystem.TryLevelUp();
    }
}