using UnityEngine;

public class ProjectileBehaviourRework : MonoBehaviour, IDamageApplicator
{
    [field: SerializeField] public float Damage { get; set; }
    public float speed = 1f;

    private IUnitTarget _target;
    
    void Update()
    {
        if(CheckOnNullAndUnityNull(_target))
        {
            Destroy(gameObject);
            return;
        }
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.Transform.position, step);
    }
    
    public void SetTarget(IUnitTarget target)
    {
        _target = target;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out IUnitTarget target) && target == _target)
        {
            if (target.TryCast(out IDamagable damagable))
                damagable.TakeDamage(this);

            Destroy(gameObject);
        }
    }
    
    public bool CheckOnNullAndUnityNull<T>(T t) => t == null || ((t is Object) && (t as Object) == null);

}
