using UnityEngine;

namespace Constructions
{
    public class AntMine : ResourceProduceConstructionBase
    {
        [SerializeField] private AntMineConfig config;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntMine;
        public override ResourceProduceCoreBase ResourceProduceCoreBase => _resourceProduceCore;
        public override ResourceProduceConstructionState ProduceConstructionState => _resourceProduceConstructionState;

        private ResourceProduceCore _resourceProduceCore;
        private ResourceProduceConstructionState _resourceProduceConstructionState;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            _healthStorage = new ResourceStorage(config.HealthPoints, config.HealthPoints);
            
            _resourceProduceCore = new ResourceProduceCore(config.ResourceProduceProcessInfo);
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
            //increase produce power
            //if units count more 0, then start produce
        }

        //remove some worker unit
        public void RemoveUnit()
        {
            //decrease produce power
            //if units count equal 0, then stop produce
        }
    }
}