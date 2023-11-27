namespace Unit.Ants
{
    [System.Serializable]
    public class AntHandItem
    {
        public AntHandleItemType AntHandleItemType { get; private set; }
        public float Power { get; private set; }
        public float Range { get; private set; }
        public float Cooldown { get; private set; }

        public AntHandItem(float power, float range, float cooldown)
        {
            Power = power;
            Range = range;
            Cooldown = cooldown;
        }
    }
}