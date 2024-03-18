using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntFortSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntFortSpawnConfig))]
    public class AntFortSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntFort> Configuration { get; private set; }
    }
}