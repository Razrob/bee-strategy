using UnityEngine;

namespace Unit.Ants.Configs.Professions
{
    public abstract class AntWarriorConfigBase : AntProfessionConfigBase
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
    }
}