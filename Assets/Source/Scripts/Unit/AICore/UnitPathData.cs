public class UnitPathData
{
    public readonly IUnitTarget Target;
    public readonly UnitPathType PathType;

    public UnitTargetType TargetType => Target.TargetType;

    public UnitPathData(IUnitTarget target, UnitPathType pathType)
    {
        Target = target;
        PathType = pathType;
    }
}
