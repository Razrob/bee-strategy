using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntMeleeWarriorConfig", menuName = "Configs/Ant Professions/Melee warrior")]
    public class AntMeleeWarriorConfig : AntWarriorConfigBase
    {
        public override AntProfessionType ProfessionType => AntProfessionType.MeleeWarrior;
    }
}