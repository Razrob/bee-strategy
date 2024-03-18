using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntRangeWorkshopSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntRangeWorkshopSpawnConfig))]
    public class AntRangeWorkshopSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntRangeWorkshop> Configuration { get; private set; }
    }
}