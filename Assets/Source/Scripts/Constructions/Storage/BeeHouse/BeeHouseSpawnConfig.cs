using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeHouseSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeHouseSpawnConfig))]
    public class BeeHouseSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<BeeHouse> Configuration { get; private set; }
    }
}