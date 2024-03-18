using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeMercenaryBarrackSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeMercenaryBarrackSpawnConfig))]
    public class BeeMercenaryBarrackSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<BeeMercenaryBarrack> Configuration { get; private set; }
    }
}