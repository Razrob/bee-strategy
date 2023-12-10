using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntWorkerConfig", menuName = "Configs/Ant Professions/Worker")]
    public class AntWorkerConfig : AntProfessionConfigBase
    {
        public override AntProfessionType ProfessionType => AntProfessionType.Worker;
        
        [field: SerializeField] public int GatheringCapacity { get; private set; }
        [field: SerializeField] public float GatheringTime { get; private set; }
    }
}