using UnityEngine;

public abstract class ResourceProduceCoreBase
{
    protected abstract ResourceProduceProccessInfoBase _produceProccessInfo { get; }
    protected ResourceStorage _producedResource;

    public bool ProduceResourceIsFull => _producedResource.CurrentValue >= _producedResource.Capacity;
    public IReadOnlyResourceStorage ProducedResource => _producedResource;
    public ResourceID TargetResourceID => _produceProccessInfo.TargetResourceID;

    public ResourceProduceCoreBase(ResourceProduceProccessInfoBase produceProccessInfo)
    {
        _producedResource = new ResourceStorage(0, produceProccessInfo.ProducedResourceCapacity);
    }

    public void SetResourceProduceProccessInfo(ResourceProduceProccessInfoBase produceProccessInfo)
    {
        SetResourceProduceProccessInfoCallback(produceProccessInfo);
        _producedResource.SetCapacity(produceProccessInfo.ProducedResourceCapacity);
    }

    protected abstract void SetResourceProduceProccessInfoCallback(ResourceProduceProccessInfoBase produceProccessInfo);

    public void Tick(float time)
    {
        if (_producedResource.Capacity <= _producedResource.CurrentValue)
            return;

        int producedCount = CalculateProducedResourceCount(time);

        if (producedCount is 0)
            return;

        _producedResource.ChangeValue(producedCount);
    }

    protected abstract int CalculateProducedResourceCount(float time);

    public int ExtractProducedResources(int count)
    {
        int extractedCount = Mathf.Min(count, _producedResource.CurrentValueInt);
        _producedResource.ChangeValue(-extractedCount);
        return extractedCount;
    }

    public int ExtractAllProducedResources() => ExtractProducedResources(_producedResource.CurrentValueInt);
}