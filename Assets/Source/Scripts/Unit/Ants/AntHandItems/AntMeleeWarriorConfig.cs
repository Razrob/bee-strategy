using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntMeleeWarriorConfig", menuName = "Configs/Ant Professions/Melee warrior")]
    public class AntMeleeWarriorConfig : AntWarriorConfigBase
    {
        public override AntProfessionType Profession => AntProfessionType.MeleeWarrior;
        public AntMeleeWarriorConfig(float damage, float cooldown, float range) : base(damage, range, cooldown) { }
    }
}