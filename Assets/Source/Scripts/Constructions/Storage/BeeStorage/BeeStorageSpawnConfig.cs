using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeStorageSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeStorageSpawnConfig))]
    public class BeeStorageSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<BeeStorage> Configuration { get; private set; }
    }
}