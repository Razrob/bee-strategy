using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntRangeWarriorConfig", menuName = "Configs/Ant Professions/Range warrior")]
    public class AntRangeWarriorConfig : AntWarriorConfigBase
    {
        public override AntProfessionType Profession => AntProfessionType.RangeWarrior;
        public AntRangeWarriorConfig(float power, float cooldown, float range) : base(power, range, cooldown) { }
    }
}