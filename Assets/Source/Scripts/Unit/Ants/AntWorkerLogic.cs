using Unit.Ants;
using UnityEngine;

public class AntWorkerLogic : WorkerLogic
{
    public AntWorkerLogic(Transform transform, UnitVisibleZone visibleZone, AntWorkerConfig antHandItem) 
        : base(transform, visibleZone, antHandItem.Range, antHandItem.GatheringCapacity, antHandItem.GatheringTime) { }
}