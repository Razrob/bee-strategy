using Unit.Ants;

public class AntRangeAttackLogic : RangeAttackLogic
{
    public AntRangeAttackLogic(AntBase ant, AntRangeWarriorConfig antHandItem) : base(ant.transform, ant.VisibleZone,
        antHandItem.Range, ant.Affiliation, antHandItem.Cooldown, antHandItem.Damage, ant,
        antHandItem.ProjectilePrefab)
    { }
}
