using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(ButterflyHouseSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(ButterflyHouseSpawnConfig))]
    public class ButterflyHouseSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<ButterflyHouse> Configuration { get; private set; }
    }
}