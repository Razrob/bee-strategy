using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(ButterflyPoisonFlowerSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(ButterflyPoisonFlowerSpawnConfig))]
    public class ButterflyPoisonFlowerSpawnConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public ConstructionSpawnConfiguration<ButterflyPoisonFlower> Configuration { get; private set; }
    }
}