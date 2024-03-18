using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(ButterflyStorageSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(ButterflyStorageSpawnConfig))]
    public class ButterflyStorageSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<ButterflyStorage> Configuration { get; private set; }

    }
}