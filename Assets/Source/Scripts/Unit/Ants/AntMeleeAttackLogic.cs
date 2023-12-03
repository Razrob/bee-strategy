using System;
using UnityEngine;

namespace Unit.Ants
{
    [Serializable]
    public class AntMeleeAttackLogic : MeleeAttackLogic
    {
        public AntMeleeAttackLogic(Transform transform, UnitVisibleZone visibleZone, AntMeleeWarriorConfig antHandItem)
            : base(transform, visibleZone, antHandItem.Range, AffiliationEnum.Ants,antHandItem.Damage, 1) { }
    }
}