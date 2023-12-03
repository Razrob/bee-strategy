using UnityEngine;

namespace Unit.Ants
{
    public class AntRangeAttackLogic : RangeAttackLogic
    {
        public AntRangeAttackLogic(Transform transform, UnitVisibleZone visibleZone, AntRangeWarriorConfig antHandItem)
            : base(transform, visibleZone, antHandItem.Range, AffiliationEnum.Ants,antHandItem.Cooldown, antHandItem.Damage) { }
    }
}