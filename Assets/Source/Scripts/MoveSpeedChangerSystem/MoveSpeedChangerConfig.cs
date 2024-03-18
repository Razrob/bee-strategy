using System;
using UnityEngine;

namespace MoveSpeedChangerSystem
{
    [Serializable]
    public class MoveSpeedChangerConfig
    {
        private const int MinPowerValue = -100;
        private const int MaxPowerValue = 100;

        [field: SerializeField]
        [field: Range(MinPowerValue, MaxPowerValue)]
        public int Power { get; private set; }

        [field: SerializeField]
        [field: Range(0, 10)]
        public float Time { get; private set; }

        public MoveSpeedChangerConfig(int power, float time)
        {
            Power = Mathf.Clamp(power, MinPowerValue, MaxPowerValue);
            Time = Mathf.Clamp(time, 0, float.PositiveInfinity);
        }
    }
}