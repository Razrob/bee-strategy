using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntQuicksandTileSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntQuicksandTileSpawnConfig))]
    public class AntQuicksandTileSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntQuicksandTile> Configuration { get; private set; }
    }
}