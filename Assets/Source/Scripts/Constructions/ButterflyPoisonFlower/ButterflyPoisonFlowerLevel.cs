using System;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class ButterflyPoisonFlowerLevel : ConstructionLevelBase
    { 
        [field: Space]
        [field: SerializeField] [field: Range(0, 25)] public float AttackDamage { get; private set; }
        [field: SerializeField] [field: Range(0, 10)] public float AttackCooldown { get; private set; }
        [field: SerializeField] [field: Range(0, 20)] public float AttackRadius { get; private set; }
        [field: Tooltip("If value > 0 then projectile will damaged other enemies in radius")]
        [field: SerializeField] [field: Range(0, 20)] public float DamageRadius { get; private set; }
        [field: Space]
        [field: SerializeField] [field: Range(0, 20)] public float FogRadius { get; private set; }
        [field: SerializeField] [field: Range(0, 20)] public float FogExistTime { get; private set; }
        [field: Tooltip("If value > 0 then spawn poison fog around flower")]
        [field: SerializeField] [field: Range(0, 20)] public float StaticFogRadius { get; private set; }
    }
}