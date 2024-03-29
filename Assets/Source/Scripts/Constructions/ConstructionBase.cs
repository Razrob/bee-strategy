using System;
using UnityEngine;
using MiniMapSystem;

public abstract class ConstructionBase : MonoBehaviour, IConstruction, IDamagable, IRepairable, IMiniMapObject,
    ITriggerable, IUnitTarget, SelectableSystem.ISelectable
{
    [SerializeField] private AffiliationEnum affiliationEnum;

    protected ResourceStorage _healthStorage;

    public bool IsSelected { get; private set; }
    public bool IsActive { get; protected set; } = true;
    
    public abstract ConstructionID ConstructionID { get; }
    public UnitTargetType TargetType => UnitTargetType.Construction;
    public MiniMapObjectType MiniMapObjectType => MiniMapObjectType.Construction;
    public Transform Transform => transform;
    public IReadOnlyResourceStorage HealthStorage => _healthStorage;
    public AffiliationEnum Affiliation => affiliationEnum;
    
    protected event Action _updateEvent;
    protected event Action _onDestroy;
    public event Action<ITriggerable> OnDisableITriggerableEvent;
    public event Action OnSelect;
    public event Action OnDeselect;
    public event Action OnDeactivation;
    
    protected void Awake()
    {
        OnAwake();
    }

    protected void Start() => OnStart();
    protected void Update() => _updateEvent?.Invoke();

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    
    public virtual void TakeDamage(IDamageApplicator damageApplicator)
    {
        _healthStorage.ChangeValue(-damageApplicator.Damage);
        if (_healthStorage.CurrentValue <= 0)
        {
            IsActive = false;
            OnDeactivation?.Invoke();
            Destroy(gameObject);
        }
    }

    public virtual void TakeRepair(IRepairApplicator repairApplicator)
    {
        _healthStorage.ChangeValue(repairApplicator.Rapair);
    }

    public void Select()
    {
        if(IsSelected) return;

        IsSelected = true;
        OnSelect?.Invoke();
    }

    public void Deselect()
    {
        if(!IsSelected) return;

        IsSelected = false;
        OnDeselect?.Invoke();
    }
    
    private void OnDestroy()
    {
        _onDestroy?.Invoke();
    }

    private void OnDisable()
    {
        OnDisableITriggerableEvent?.Invoke(this);
    }
}