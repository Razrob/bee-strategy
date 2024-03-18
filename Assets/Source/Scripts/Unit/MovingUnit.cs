using MoveSpeedChangerSystem;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovingUnit : UnitBase
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SomeTestAbility_1 _ability1;
    [SerializeField] private SomeTestAbility_2 _ability2;

    private float _startMaxSpeed;

    public Vector3 Velocity => _navMeshAgent.velocity;
   
    private void Awake()
    {
        _abilites.Add(_ability1);
        _abilites.Add(_ability2);

        _startMaxSpeed = _navMeshAgent.speed;

        MoveSpeedChangerProcessor = new MoveSpeedChangerProcessor(_navMeshAgent, _startMaxSpeed);
        
        OnAwake();
    }
    
    private void Start()
    {
        UnitPool.Instance.UnitCreation(this);
        
        OnStart();
    }

    public override void HandleUpdate(float time)
    {
        base.HandleUpdate(time);
        
        MoveSpeedChangerProcessor.HandleUpdate(time);
        
        foreach (var ability in _abilites)
            ability.OnUpdate(Time.deltaTime);
    }

    protected virtual void OnAwake() { }
    
    protected virtual void OnStart() { }

    public void SetDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void Warp(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
    
    public virtual void GiveOrder(GameObject target, Vector3 position) 
        => AutoGiveOrder(target.GetComponent<IUnitTarget>(), position);

    public void UseAbility(int abilityIndex) 
        => _abilites[abilityIndex].OnUse();
}
    