using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntRangeWarriorConfig", menuName = "Configs/Ant Professions/Range warrior")]
    public class AntRangeWarriorConfig : AntWarriorConfigBase
    {
        [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }

        public override AntProfessionType Profession => AntProfessionType.RangeWarrior;
    }
}