using Unit.Ants.Configs.Professions;
using Unit.ProfessionsCore;
using UnityEngine;

namespace Unit.Ants.Professions
{
    public class AntWorkerProfession : WorkerProfession
    {
        public AntWorkerProfession(AntBase ant, AntWorkerConfig antHandItem, ResourceRepository resourceRepository, GameObject resourceSkin)
            : base(ant, antHandItem.InteractionRange, antHandItem.GatheringCapacity, antHandItem.GatheringTime, resourceRepository, resourceSkin)
        { }
    }
}