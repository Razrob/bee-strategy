using System;
using UnityEngine;

namespace Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour, IDamageApplicator, IPoolable<ProjectileBase, ProjectileType>,
        IPoolEventListener
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }

        public float Damage { get; private set; }
        public abstract ProjectileType ProjectileType { get; }
        public ProjectileType Identifier => ProjectileType;
        
        protected IUnitTarget Target;

        public event Action<ProjectileBase> ElementReturnEvent;
        public event Action<ProjectileBase> ElementDestroyEvent;

        public void HandleUpdate(float time)
        {
            CheckTargetOnNull();
            Move(time);
        }

        public void SetDamage(IDamageApplicator damage) => Damage = damage.Damage;
        public void SetDamage(float damage) => Damage = damage;

        public void SetTarget(IUnitTarget target)
        {
            Target = target;
            Target.OnDeactivation += OnTargetDeactivation;
        }

        public virtual void OnElementReturn() => gameObject.SetActive(false);

        public virtual void OnElementExtract() => gameObject.SetActive(true);

        private void OnTargetDeactivation()
        {
            Target.OnDeactivation -= OnTargetDeactivation;
            ReturnInPool();
        }
        
        protected void CheckTargetOnNull()
        {
            if(Target.IsAnyNull())
                ReturnInPool();
        }
        
        protected virtual void CollideWithTarget(IUnitTarget target)
        {
            if (target.TryCast(out IDamagable damagable))
                damagable.TakeDamage(this);

            ReturnInPool();
        }

        protected void ReturnInPool() => ElementReturnEvent?.Invoke(this);
        
        protected virtual void Move(float time)
        {
            var step = MoveSpeed * time;
            transform.position = Vector3.MoveTowards(transform.position, Target.Transform.position, step);
        }

        private void OnTriggerEnter(Collider someCollider)
        {
            if (someCollider.TryGetComponent(out IUnitTarget target) && target == Target)
                CollideWithTarget(target);
        }
        
        private void OnDestroy()
        {
            ElementDestroyEvent?.Invoke(this);
        }
    }
}