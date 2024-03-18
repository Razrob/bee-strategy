using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeLandmineSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeLandmineSpawnConfig))]
    public class BeeLandmineSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<BeeLandmine> Configuration { get; private set; }
    }
}