using System;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class AntWorkshopLevelBase : ConstructionLevelBase
    {
        [field: Space]
        [field: Tooltip("Mean max count of melee/range weapon or worker tools")]
        [field: SerializeField] public int Capacity { get; private set; }
    }
}