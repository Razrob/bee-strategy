using UnityEngine;

public class ResourceBase : ResourceStorage
{
    private ResourceID _id;
    private Sprite _icon;

    public ResourceID ID => _id;
    public Sprite Icon => _icon;

    public ResourceBase(ResourceConfig config, float currentValue, float capacity):base(currentValue, capacity) 
    {
        _id = config.ID;
        _icon = config.Icon;
    }
}
