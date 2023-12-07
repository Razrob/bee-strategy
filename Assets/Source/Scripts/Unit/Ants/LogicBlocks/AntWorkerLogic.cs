using Unit.Ants;

public class AntWorkerLogic : WorkerLogic
{
    public AntWorkerLogic(AntBase ant, AntWorkerConfig antHandItem) 
        : base(ant.transform, antHandItem.Range, antHandItem.GatheringCapacity, antHandItem.GatheringTime) { }
}