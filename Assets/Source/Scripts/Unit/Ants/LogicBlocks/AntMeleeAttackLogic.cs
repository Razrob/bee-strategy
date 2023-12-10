using System;

namespace Unit.Ants
{
    [Serializable]
    public class AntMeleeAttackLogic : MeleeAttackLogic
    {
        public AntMeleeAttackLogic(AntBase ant, AntMeleeWarriorConfig antHandItem)
            : base(ant, antHandItem.InteractionRange, antHandItem.Cooldown, antHandItem.AttackRange, antHandItem.Damage)
        { }
    }
}