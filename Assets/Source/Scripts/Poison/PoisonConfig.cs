using UnityEngine;

namespace Poison
{
    [CreateAssetMenu(fileName = nameof(PoisonConfig), menuName = "Configs/" + nameof(PoisonConfig))]
    public class PoisonConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public float DamagePerSecond { get; private set; }
        [field: SerializeField] public int ExistTime { get; private set; }
    }
}