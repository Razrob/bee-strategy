using Unit.Ants;

public class AntWorkerLogic : WorkerLogic
{
    public AntWorkerLogic(AntBase ant, AntWorkerConfig antHandItem) 
        : base(ant, antHandItem.InteractionRange, antHandItem.GatheringCapacity, antHandItem.GatheringTime) { }
}