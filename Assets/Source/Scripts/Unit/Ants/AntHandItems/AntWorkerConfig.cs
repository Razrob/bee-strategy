using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntWorkerConfig", menuName = "Configs/Ant Professions/Worker")]
    public class AntWorkerConfig : AntProfessionConfigBase
    {
        public override AntProfessionType Profession => AntProfessionType.Worker;
        
        [field: SerializeField] public int GatheringCapacity { get; private set; }
        [field: SerializeField] public float GatheringTime { get; private set; }

        public AntWorkerConfig(int gatheringCapacity, float gatheringTime, float range)
            : base(range)
        {
            GatheringCapacity = gatheringCapacity;
            GatheringTime = gatheringTime;
        }
    }
}