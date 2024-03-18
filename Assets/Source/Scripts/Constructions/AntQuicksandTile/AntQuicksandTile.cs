using UnityEngine;

namespace Constructions
{
    public class AntQuicksandTile : ConstructionBase
    {
        [SerializeField] private AntQuicksandTileConfig config;
        [SerializeField] private TriggerBehaviour triggerBehaviour;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public override ConstructionID ConstructionID => ConstructionID.AntQuicksandTile;

        protected override void OnAwake()
        {
            base.OnAwake();
            
            _healthStorage = new ResourceStorage(config.HealthPoints, config.HealthPoints);
        }

        protected override void OnStart()
        {
            triggerBehaviour.EnterEvent += OnUnitEnter;
            triggerBehaviour.ExitEvent += OnUnitExit;
        }

        private void OnUnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out UnitBase unit))
            {
                
            }
        }

        private void OnUnitExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out UnitBase unit))
            {
                
            }
        }
    }
}