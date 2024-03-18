using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntQuicksandTileConfig), menuName = "Configs/Constructions/Main/" + nameof(AntQuicksandTileConfig))]
    public class AntQuicksandTileConfig : ConstructionBuyCostConfig
    {
        [field: SerializeField] public float HealthPoints { get; private set; }
    }
}