using Unit.Ants;

public class AntRangeAttackLogic : RangeAttackLogic
{
    public AntRangeAttackLogic(AntBase ant, AntRangeWarriorConfig antHandItem) 
        : base(ant, antHandItem.InteractionRange, antHandItem.Cooldown, antHandItem.AttackRange, 
            antHandItem.Damage, antHandItem.ProjectilePrefab)
    { }
}
