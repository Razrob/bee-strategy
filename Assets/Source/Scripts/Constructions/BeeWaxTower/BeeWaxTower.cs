using Constructions.LevelSystemCore;
using Projectiles.Factory;
using UnityEngine;
using Zenject;

namespace Constructions
{
    public class BeeWaxTower : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] private TriggerBehaviour attackZone;
        [SerializeField] private BeeWaxTowerConfig config;
        
        [Inject] private readonly ProjectileFactory _projectileFactory;
        [Inject] private readonly IConstructionFactory _constructionFactory;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeWaxTower;

        public IConstructionLevelSystem LevelSystem { get; private set; }

        private BeeWaxTowerAttackProcessor _attackProcessor;

        protected override void OnAwake()
        {
            base.OnAwake();

            _attackProcessor = new BeeWaxTowerAttackProcessor(_projectileFactory, attackZone, transform);
            
            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new BeeWaxTowerLevelSystem(config.Levels, ref resourceRepository, ref _healthStorage, ref _attackProcessor);

            _updateEvent += UpdateAttackProcessor;
            OnDestruction += SpawnStickyTile;
        }

        private void UpdateAttackProcessor()
            => _attackProcessor.HandleUpdate(Time.deltaTime);

        private void SpawnStickyTile()
        {
            FrameworkCommander.GlobalData.ConstructionsRepository.GetConstruction(transform.position, true);
            
            ConstructionBase construction = _constructionFactory.Create<ConstructionBase>(ConstructionID.BeeStickyTileConstruction);
            FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(transform.position, construction);
            construction.transform.position = transform.position;
        }
        
        //TODO: remove this temporary code, when ui will be create
        [ContextMenu("Level up")]
        private void LevelUp()
            => LevelSystem.TryLevelUp();
    }
}