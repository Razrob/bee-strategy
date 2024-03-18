using System;
using Constructions.LevelSystemCore;
using MoveSpeedChangerSystem;
using Projectiles;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class BeeWaxTowerLevel : ConstructionLevelBase
    {
        [field: SerializeField] public ProjectileType ProjectilesType { get; private set; }
        [field: SerializeField] [field: Range(0, 4)] public int ProjectilesCount { get; private set; }
        [field: SerializeField] [field: Range(0, 1)] public float SpawnPause { get; private set; }
        [field: SerializeField] [field: Range(0, 100)] public float Damage { get; private set; }
        [field: SerializeField] [field: Range(0, 10)] public float Cooldown { get; private set; }
        [field: SerializeField] public MoveSpeedChangerConfig MoveSpeedChangerConfig { get; private set; }
    }
}