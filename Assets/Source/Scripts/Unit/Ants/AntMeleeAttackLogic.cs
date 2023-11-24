using UnityEngine;

namespace Unit.Ants
{
    public class AntMeleeAttackLogic : MeleeAttackLogic
    {
        public AntMeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, AntHandItem antHandItem)
            : base(transform, visibleZone, antHandItem.Range, antHandItem.Power) { }
    }
}