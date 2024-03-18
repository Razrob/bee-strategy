using Poison;
using UnityEngine;
using Zenject;

namespace Unit.Ants
{
    public abstract class AntUnit : MovingUnit, IPoisoneable
    {
        [Inject] private PoisonConfig _poisonConfig;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;

        public PoisonProcessor PoisonProcessor { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            PoisonProcessor = new PoisonProcessor(this, _poisonConfig);
        }
        
        public override void HandleUpdate(float time)
        {
            base.HandleUpdate(time);
            
            PoisonProcessor.HandleUpdate(Time.deltaTime);
        }
        
        public override void OnElementReturn()
        {
            base.OnElementReturn();

            PoisonProcessor.Stop();
        }
    }
}