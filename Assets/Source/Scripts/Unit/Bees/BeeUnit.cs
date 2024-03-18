using Poison;
using UnityEngine;
using Zenject;

namespace Unit.Bees
{
    public abstract class BeeUnit : MovingUnit, IPoisoneable
    {
        [Inject] private PoisonConfig _poisonConfig;

        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        
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