using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntTownHallSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntTownHallSpawnConfig))]
    public class AntTownHallSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntTownHall> Configuration { get; private set; }
    }
}