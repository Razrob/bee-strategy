namespace Unit.Ants
{
    public class AntUnit : MovingUnit
    {
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        public AntProfessionType ProfessionType { get; protected set; } = AntProfessionType.Worker;
        public UnitType UnitType { get; protected set; } = UnitType.Worker;
    }
}