using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeLandmineConfig), menuName = "Configs/Constructions/Main/" + nameof(BeeLandmineConfig))]
    public class BeeLandmineConfig : ConstructionBuyCostConfig
    {
        [field: SerializeField] public float HealthPoints { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float ExplosionDelay { get; private set; }
        [field: SerializeField] public float ExplosionRadius { get; private set; }
        [field: SerializeField] [field: Range(0, 5)] public int StickyTilesRadius { get; private set; }
    }
}