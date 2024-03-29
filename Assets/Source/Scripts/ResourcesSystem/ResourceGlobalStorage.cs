using System;
using UnityEngine;

public class ResourceGlobalStorage : MonoBehaviour
{
    public static ResourceRepository ResourceRepository { get; private set; }
    public ResourceConfig[] resourceConfigs;

    public static event Action ResourceChanged;

    private void Awake()
    {
        ResourceRepository = new ResourceRepository(resourceConfigs);
        ResourceRepository.CreateResource(ResourceID.Pollen,10, 10);
        ResourceRepository.CreateResource(ResourceID.Bees_Wax,10, 10);
        ResourceRepository.CreateResource(ResourceID.Housing,10, 10);
        ResourceRepository.CreateResource(ResourceID.Honey,10, 10);

        ResourceRepository.OnResourceAdd += OnResourceAdded;

        foreach (var element in ResourceRepository.Resources)
            element.Value.OnChange += OnResourceChanged;

    }
    
    public static void ChangeCapacity(ResourceID resourceID, float capacity)
    {
        ResourceBase resourceBase = ResourceRepository.GetResource(resourceID);
        resourceBase.SetCapacity(resourceBase.Capacity + capacity);
    }

    public static void ChangeValue(ResourceID resourceID, float value)
    {
        ResourceBase resourceBase = ResourceRepository.GetResource(resourceID);
        resourceBase.ChangeValue(value);
    }
    
    public static ResourceBase GetResource(ResourceID resourceID)
    {
        return ResourceRepository.GetResource(resourceID);
    }
    
    private void Start()
    {
        OnResourceChanged();
    }

    private void OnResourceChanged()
    {
        ResourceChanged?.Invoke();
    }

    private void OnResourceAdded(ResourceBase resourceBase)
    {
        ResourceChanged?.Invoke();
    }
}
