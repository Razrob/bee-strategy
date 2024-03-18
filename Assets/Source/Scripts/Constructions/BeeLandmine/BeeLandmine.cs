using System.Collections.Generic;
using CustomTimer;
using UnityEngine;
using Zenject;

namespace Constructions
{
    public class BeeLandmine : ConstructionBase, IDamageApplicator
    {
        [SerializeField] private BeeLandmineConfig config;
        [SerializeField] private TriggerBehaviour _triggerBehaviour;
        [SerializeField] private LayerMask layerMask;
        
        [Inject] private readonly IConstructionFactory _constructionFactory;
        [Inject] private readonly BuildingGridConfig _buildingGridConfig;
        
        private Timer _explosionTimer;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeLandmine;

        public float Damage { get; private set; }
        
        protected override void OnAwake()
        {
            _healthStorage = new ResourceStorage(config.HealthPoints, config.HealthPoints);
            Damage = config.Damage;
            _explosionTimer = new Timer(config.ExplosionDelay, 0, true);
            _explosionTimer.OnTimerEnd += Explosion;

            _updateEvent += UpdateExplosionTimer;
        }

        protected override void OnStart()
        {
            _triggerBehaviour.EnterEvent += OnUnitEnter;
        }

        private void UpdateExplosionTimer()
            => _explosionTimer.Tick(Time.deltaTime);
        
        private void OnUnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out UnitBase unit))
            {
                if (unit.Affiliation == AffiliationEnum.Ants ||
                    unit.Affiliation == AffiliationEnum.Butterflies)
                {
                    _triggerBehaviour.EnterEvent -= OnUnitEnter;
                    _explosionTimer.Reset();
                }
            }
        }

        private void Explosion()
        {
            RaycastHit[] result = new RaycastHit[30];
            var size = Physics.SphereCastNonAlloc(transform.position, config.ExplosionRadius, Vector3.down,
                result, 0, layerMask);

            for (int i = 0; i < size; i++)
            {
                if (result[i].collider.gameObject.TryGetComponent(out IDamagable damageable) && 
                    (damageable.Affiliation == AffiliationEnum.Butterflies || 
                     damageable.Affiliation == AffiliationEnum.Ants))
                {
                    Debug.Log(Damage);
                    damageable.TakeDamage(this);
                }
            }

            FrameworkCommander.GlobalData.ConstructionsRepository.GetConstruction(transform.position, true);
            SpawnStickyTile();
            Destroy(gameObject);
        }

        private void SpawnStickyTile()
        {
            var radius = config.StickyTilesRadius;
            if(radius <= 0)
                return;

            var prevSpawnedTiles = new List<Vector3>();

            SpawnStickyTile(transform.position);
            prevSpawnedTiles.Add(transform.position);
            
            for (int i = 1; i < radius; i++)
            {
                var newSpawnedTiles = new List<Vector3>();
                for (int j = 0; j < prevSpawnedTiles.Count; j++)
                {
                    var tilePos = prevSpawnedTiles[j];
                    Vector3 hexOffsets = new Vector3(_buildingGridConfig.HexagonsOffcets.x / 2, _buildingGridConfig.HexagonsOffcets.y);

                    var posY = tilePos + new Vector3(0, 0, hexOffsets.y * 2);
                    var posNegativeY = tilePos + new Vector3(0, 0, -hexOffsets.y * 2);
                    var posXZ = tilePos + new Vector3(hexOffsets.x, 0, hexOffsets.y);
                    var posNegativeXZ = tilePos + new Vector3(-hexOffsets.x, 0, hexOffsets.y);
                    var posXNegativeZ = tilePos + new Vector3(hexOffsets.x, 0, -hexOffsets.y);
                    var posNegativeXNegativeZ = tilePos + new Vector3(-hexOffsets.x, 0, -hexOffsets.y);
                    
                    SpawnStickyTile(posY);
                    newSpawnedTiles.Add(posY);
                    
                    SpawnStickyTile(posNegativeY);
                    newSpawnedTiles.Add(posNegativeY);
                    
                    SpawnStickyTile(posXZ);
                    newSpawnedTiles.Add(posXZ);
                    
                    SpawnStickyTile(posNegativeXZ);
                    newSpawnedTiles.Add(posNegativeXZ);
                    
                    SpawnStickyTile(posXNegativeZ);
                    newSpawnedTiles.Add(posXNegativeZ);
                    
                    SpawnStickyTile(posNegativeXNegativeZ);
                    newSpawnedTiles.Add(posNegativeXNegativeZ);
                }

                prevSpawnedTiles = newSpawnedTiles;
            }
            

        }

        private void SpawnStickyTile(Vector3 position)
        {
            if( FrameworkCommander.GlobalData.ConstructionsRepository.ConstructionExist(position))
                return;
            
            BeeStickyTile construction = _constructionFactory.Create<BeeStickyTile>(ConstructionID.BeeStickyTileConstruction);
            FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(position, construction);
            construction.transform.position = position;
        }
    }
}