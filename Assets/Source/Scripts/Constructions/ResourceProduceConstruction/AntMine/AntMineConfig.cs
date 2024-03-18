using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(AntMineConfig), menuName = "Configs/Constructions/Main/" + nameof(AntMineConfig))]
    public class AntMineConfig : ConstructionBuyCostConfig
    {
        [field: SerializeField] public float HealthPoints { get; private set; }
        [field: SerializeField] public int UnitsCount { get; private set; }
        [field: SerializeField] public ResourceProduceProccessInfo ResourceProduceProcessInfo { get; private set; }
    }
}