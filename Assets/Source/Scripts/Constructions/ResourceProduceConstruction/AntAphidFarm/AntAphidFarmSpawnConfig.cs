using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntAphidFarmSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntAphidFarmSpawnConfig))]
    public class AntAphidFarmSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntAphidFarm> Configuration { get; private set; }
    }
}