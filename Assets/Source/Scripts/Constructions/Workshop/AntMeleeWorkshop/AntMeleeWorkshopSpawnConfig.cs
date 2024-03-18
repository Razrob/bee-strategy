using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntMeleeWorkshopSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(AntMeleeWorkshopSpawnConfig))]
    public class AntMeleeWorkshopSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<AntMeleeWorkshop> Configuration { get; private set; }
    }
}