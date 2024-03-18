namespace MoveSpeedChangerSystem
{
    public interface IMoveSpeedChangeable
    {
        AffiliationEnum Affiliation { get; }
    
        public MoveSpeedChangerProcessor MoveSpeedChangerProcessor { get; }
    }
}
