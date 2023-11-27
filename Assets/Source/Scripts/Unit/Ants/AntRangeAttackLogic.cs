using UnityEngine;

namespace Unit.Ants
{
    public class AntRangeAttackLogic : RangeAttackLogic
    {
        public AntRangeAttackLogic(Transform transform, UnitVisibleZone visibleZone, AntHandItem antHandItem)
            : base(transform, visibleZone, antHandItem.Range, antHandItem.Cooldown, antHandItem.Power) { }
    }
}