using MoveSpeedChangerSystem;
using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeStickyTileConfig), menuName = "Configs/Constructions/Main/" + nameof(BeeStickyTileConfig))]
    public class BeeStickyTileConfig : ConstructionBuyCostConfig
    {
        [field: SerializeField] public float HealthPoints { get; private set; }
        [field: SerializeField] public float DelayBeforeApply { get; private set; }
        [field: SerializeField] [field: Range(0, 30)] public float ExistTime { get; private set; }
        [field: SerializeField] public MoveSpeedChangerConfig EnemyMoveSpeedChangerConfig { get; private set; }
        [field: SerializeField] public MoveSpeedChangerConfig BeeMoveSpeedChangerConfig { get; private set; }
    }
}