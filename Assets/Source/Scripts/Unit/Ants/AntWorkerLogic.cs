using Unit.Ants;
using UnityEngine;

public class AntWorkerLogic : WorkerLogic
{
    public AntWorkerLogic(Transform transform, UnitVisibleZone visibleZone, AntHandItem antHandItem) 
        : base(transform, visibleZone, antHandItem.Range, antHandItem.Cooldown, (int)antHandItem.Power) { }
}