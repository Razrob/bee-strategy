using UnityEngine;

namespace Unit.Ants
{
    public abstract class AntWarriorConfigBase : AntProfessionConfigBase
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        
        protected AntWarriorConfigBase(float damage, float range, float cooldown) : base(range)
        {
            Damage = damage;
            Cooldown = cooldown;
        }
    }
}