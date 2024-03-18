using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    public class AntAphidFarm : ResourceProduceConstructionBase, IEvolveConstruction
    {
        [SerializeField] private AntAphidFarmConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntAphidFarm;
        public override ResourceProduceCoreBase ResourceProduceCoreBase => _resourceProduceCore;
        public override ResourceProduceConstructionState ProduceConstructionState => _resourceProduceConstructionState;

        public IConstructionLevelSystem LevelSystem { get; private set; }
        
        private ResourceProduceCore _resourceProduceCore;
        private ResourceProduceConstructionState _resourceProduceConstructionState;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            LevelSystem = new AntAphidFarmLevelSystem(config.Levels, ref _resourceProduceCore, ref resourceRepository, ref _healthStorage);
            
            _resourceProduceConstructionState = ResourceProduceConstructionState.Paused;

            _updateEvent += OnUpdate;
        }

        private void OnUpdate()
        {
            if (_resourceProduceConstructionState != ResourceProduceConstructionState.Proccessing)
                return;
            
            if (!_resourceProduceCore.ProduceResourceIsFull)
                _resourceProduceConstructionState = ResourceProduceConstructionState.Completed;
            
            _resourceProduceCore.Tick(Time.deltaTime);
        }

        public void ExtractResource()
        {
            ResourceBase resource = ResourceGlobalStorage.GetResource(_resourceProduceCore.TargetResourceID);
            var produceResource = _resourceProduceCore.ProducedResource;
            
            if (produceResource.CurrentValue > 0 && resource.CurrentValue < resource.Capacity)
            {
                int extractValue = (int)produceResource.CurrentValue;
                extractValue = (int)Mathf.Clamp(extractValue, 0, (resource.Capacity - resource.CurrentValue));
                int addResource = _resourceProduceCore.ExtractProducedResources(extractValue);
                ResourceGlobalStorage.ChangeValue(_resourceProduceCore.TargetResourceID, addResource);
            }
        }

        //add some worker unit
        public void AddUnit()
        {
            //if units count more 0, then start produce
            //increase produce power
        }

        //remove some worker unit
        public void RemoveUnit()
        {
            //if units count equal 0, then stop produce
            //decrease produce power
        }
    }
}