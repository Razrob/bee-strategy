using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntHouseSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntHouseSpawnConfig))]
    public class AntHouseSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntHouse> Configuration { get; private set; }
    }
}