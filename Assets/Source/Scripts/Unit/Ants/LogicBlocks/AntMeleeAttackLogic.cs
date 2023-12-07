using System;

namespace Unit.Ants
{
    [Serializable]
    public class AntMeleeAttackLogic : MeleeAttackLogic
    {
        public AntMeleeAttackLogic(AntBase ant, AntMeleeWarriorConfig antHandItem) : base(ant.transform, ant.VisibleZone,
            antHandItem.Range, ant.Affiliation, antHandItem.Damage, 1, ant)
        { }
    }
}