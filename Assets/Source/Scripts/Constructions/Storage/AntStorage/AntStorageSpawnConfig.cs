using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntStorageSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntStorageSpawnConfig))]
    public class AntStorageSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntStorage> Configuration { get; private set; }
    }
}