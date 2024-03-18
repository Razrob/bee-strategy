using System;
using Constructions.LevelSystemCore;
using UnityEngine;

namespace Constructions
{
    [Serializable]
    public class AntAphidFarmLevel : ConstructionLevelBase
    {
        [field: Space]
        [field: SerializeField] public int UnitsCount { get; private set; }
        [field: SerializeField] public ResourceProduceProccessInfo ResourceProduceProcessInfo { get; private set; }
    }
}