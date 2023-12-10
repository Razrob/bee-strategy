using UnityEngine;

public interface IUnitTarget
{
    public Transform Transform { get; }
    public UnitTargetType TargetType { get; }
    public AffiliationEnum Affiliation { get; }
}
