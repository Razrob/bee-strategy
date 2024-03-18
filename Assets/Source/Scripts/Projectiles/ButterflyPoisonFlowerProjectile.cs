using UnityEngine;

namespace Projectiles
{
    public class ButterflyPoisonFlowerProjectile : ProjectileBase
    {
        [SerializeField] private LayerMask layerMask;
        
        public override ProjectileType ProjectileType => ProjectileType.ButterflyPoisonFlowerProjectile;

        private float _damageRadius;
        
        public void SetDamageRadius(float damageRadius)
        {
            _damageRadius = damageRadius;
        }
        
        protected override void CollideWithTarget(IUnitTarget target)
        {
            if (target.TryCast(out IDamagable mainDamageable))
                mainDamageable.TakeDamage(this);

            if (_damageRadius > 0)
            {
                RaycastHit[] result = new RaycastHit[30];
                var size = Physics.SphereCastNonAlloc(transform.position, _damageRadius, Vector3.down,
                    result, 0, layerMask);

                for (int i = 0; i < size; i++)
                {
                    var dam = new DividedDamage(this, 2);
                    if (result[i].collider.gameObject.TryGetComponent(out IDamagable damageable) && 
                        (damageable.Affiliation == AffiliationEnum.Bees || 
                         damageable.Affiliation == AffiliationEnum.Ants) &&
                        damageable != mainDamageable)
                    {
                        damageable.TakeDamage(dam);
                    }
                }  
            }
            
            ReturnInPool();
        }

        public override void OnElementReturn()
        {
            base.OnElementReturn();
            _damageRadius = 0;
        }

        private class DividedDamage : IDamageApplicator
        {
            public float Damage { get; }

            public DividedDamage(IDamageApplicator damage, float divideScale) => Damage = damage.Damage / divideScale;
        }
    }
}