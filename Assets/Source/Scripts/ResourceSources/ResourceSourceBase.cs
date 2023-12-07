using UnityEngine;
using System;
using MiniMapSystem;

public abstract class ResourceSourceBase : MonoBehaviour, IMiniMapObject, ITriggerable, IUnitTarget
{
    public abstract ResourceID ResourceID { get; }
    public AffiliationEnum Affiliation => AffiliationEnum.None;
    public MiniMapObjectType MiniMapObjectType => MiniMapObjectType.ResourceSource;
    
    public event Action<ITriggerable> OnDisableITriggerableEvent;
    public UnitTargetType TargetType => UnitTargetType.ResourceSource;
    
    public Transform Transform => transform;
    
    public abstract void ExtractResource(int extracted);
    
    private void OnDisable()
    {
        OnDisableITriggerableEvent?.Invoke(this);
    }
}