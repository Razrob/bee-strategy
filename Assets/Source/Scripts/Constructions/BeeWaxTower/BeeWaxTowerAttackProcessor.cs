using System;
using System.Collections.Generic;
using CustomTimer;
using MoveSpeedChangerSystem;
using Projectiles;
using Projectiles.Factory;
using UnityEngine;

namespace Constructions
{
    public class BeeWaxTowerAttackProcessor
    {
        private readonly SpawnProcessor _spawnProcessor;
        private readonly TriggerBehaviour _attackZone;
        private readonly List<IUnitTarget> _targets;
        private readonly Transform _spawnTransform;
        private readonly Timer _cooldown;
        
        private int _projectilesCount;
        
        public BeeWaxTowerAttackProcessor(ProjectileFactory projectileFactory, TriggerBehaviour attackZone, Transform spawnTransform)
        {
            _attackZone = attackZone;
            _spawnTransform = spawnTransform;
            _targets = new List<IUnitTarget>();

            _spawnProcessor = new SpawnProcessor(projectileFactory, spawnTransform);
            _spawnProcessor.OnEndSpawn += ResetCooldown;
            
            _attackZone.EnterEvent += OnTargetEnter;
            _attackZone.ExitEvent += OnTargetExit;

            _cooldown = new Timer(0, 0, true);
            _cooldown.OnTimerEnd += TryAttack;
        }

        public void HandleUpdate(float time)
        {
            _spawnProcessor.HandleUpdate(time);
            _cooldown.Tick(time);
        }

        public void SetData(int projectilesCount, float cooldown, float spawnPause, float damage, MoveSpeedChangerConfig moveSpeedChangerConfig, ProjectileType projectileType)
        {
            _projectilesCount = projectilesCount;
            _cooldown.SetMaxValue(cooldown, false);
            
            _spawnProcessor.SetData(spawnPause, damage, moveSpeedChangerConfig, projectileType);
        }

        private void OnTargetEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IUnitTarget target) && target.Affiliation != AffiliationEnum.Bees && target.CastPossible<IDamagable>())
            {
                if(_targets.Contains(target))
                    return;
                
                _targets.Add(target);
                
                if(_cooldown.TimerIsEnd)
                    TryAttack();
            }
        }
        
        private void OnTargetExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IUnitTarget target) && target.Affiliation != AffiliationEnum.Bees && target.CastPossible<IDamagable>())
                _targets.Remove(target);
        }
        
        private void TryAttack()
        {
            if(_targets.Count <= 0)
                return;

            List<(IUnitTarget, float)> targetsWithDistance = new List<(IUnitTarget, float)>();

            foreach (var target in _targets)
                targetsWithDistance.Add((target, Vector3.Distance(target.Transform.position, _spawnTransform.position)));

            targetsWithDistance.Sort(
                (x, y) =>
                {
                    if (x.Item2 < y.Item2)
                        return 0;
                    else
                        return 1;
                }
            );

            List<IUnitTarget> tars = new List<IUnitTarget>(); 
            int targetIndex = 0;
            for (int i = 0; i < _projectilesCount; i++)
            {
                tars.Add(targetsWithDistance[targetIndex].Item1);
                targetIndex++;
                if (targetIndex >= _targets.Count)
                    targetIndex = 0;
            }
            
            _spawnProcessor.InvokeAttack(tars);
        }

        private void ResetCooldown()
            => _cooldown.Reset();
        
        private class SpawnProcessor
        {
            private readonly ProjectileFactory _projectileFactory;
            private readonly TriggerBehaviour _attackZone;
            private readonly Transform _spawnTransform;
            private readonly Timer _spawnPauseTimer;
            
            private MoveSpeedChangerConfig _moveSpeedChangerConfig;
            private ProjectileType _projectileType;
            private List<IUnitTarget> _targets;
            private float _damage;
            
            public event Action OnEndSpawn;
            
            public SpawnProcessor(ProjectileFactory projectileFactory, Transform spawnTransform)
            {
                _projectileFactory = projectileFactory;
                _spawnTransform = spawnTransform;
                _spawnPauseTimer = new Timer(0, 0, true);
                _spawnPauseTimer.OnTimerEnd += SpawnProjectile;
            }

            public void HandleUpdate(float time)
                => _spawnPauseTimer.Tick(time);
            
            public void SetData(float spawnPause, float damage, MoveSpeedChangerConfig moveSpeedChangerConfig, ProjectileType projectileType)
            {
                _spawnPauseTimer.SetMaxValue(spawnPause, false);
                _moveSpeedChangerConfig = moveSpeedChangerConfig;
                _projectileType = projectileType;
                _damage = damage;
            }
            
            public void InvokeAttack(List<IUnitTarget> targets)
            {
                _targets = targets;
                SpawnProjectile();
            }
            
            private void SpawnProjectile()
            {
                if (_targets == null || _targets.Count <= 0)
                    return;
                
                var target = _targets[0];
                _targets.RemoveAt(0);
                
                if(target.IsAnyNull() || !target.IsActive)
                    return;
                
                var projectile = _projectileFactory.Create(_projectileType).Cast<BeeWaxTowerProjectile>();
                projectile.SetTarget(target);
                projectile.SetDamage(_damage);
                projectile.SetSpeedChangerConfig(_moveSpeedChangerConfig);
                projectile.transform.position = _spawnTransform.position;
                
                if(_targets.Count > 0)
                    _spawnPauseTimer.Reset();
                else
                    OnEndSpawn?.Invoke();
            }
        }
    }
}