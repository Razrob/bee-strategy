using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeWaxTowerSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeWaxTowerSpawnConfig))]
    public class BeeWaxTowerSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<BeeWaxTower> Configuration { get; private set; }
    }
}