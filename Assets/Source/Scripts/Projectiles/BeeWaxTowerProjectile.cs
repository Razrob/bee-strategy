using MoveSpeedChangerSystem;

namespace Projectiles
{
    public class BeeWaxTowerProjectile : ProjectileBase
    {
        private MoveSpeedChangerConfig _moveSpeedChangerConfig;
        
        public override ProjectileType ProjectileType => ProjectileType.BeeWaxTowerProjectile;

        public void SetSpeedChangerConfig(MoveSpeedChangerConfig moveSpeedChangerConfig)
            => _moveSpeedChangerConfig = moveSpeedChangerConfig;
        
        protected override void CollideWithTarget(IUnitTarget target)
        {
            if (target.TryCast(out IMoveSpeedChangeable speedChangeable))
                speedChangeable.MoveSpeedChangerProcessor.Invoke(_moveSpeedChangerConfig);
            
            if (target.TryCast(out IDamagable damageable))
                damageable.TakeDamage(this);

            ReturnInPool();
        }
    }
}