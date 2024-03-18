using System.Collections.Generic;
using CustomTimer;
using MoveSpeedChangerSystem;
using StickySystem;
using UnityEditor.Rendering;
using UnityEngine;

namespace Constructions
{
    public class BeeStickyTile : ConstructionBase
    {
        [SerializeField] private BeeStickyTileConfig config;
        [SerializeField] private TriggerBehaviour stickZone;

        private readonly List<BufferBeforeApplyStick> _buffers = new List<BufferBeforeApplyStick>();
        private Timer _existsTimer;
        
        public override AffiliationEnum Affiliation => AffiliationEnum.Bees;
        public override ConstructionID ConstructionID => ConstructionID.BeeStickyTileConstruction;

        protected override void OnAwake()
        {
            _healthStorage = new ResourceStorage(config.HealthPoints, config.HealthPoints);
            _existsTimer = new Timer(config.ExistTime);
            _existsTimer.OnTimerEnd += DestructStickyTile;
            
            _updateEvent += UpdateExistsTimer;
            _updateEvent += UpdateTimers;
        }

        protected override void OnStart()
        {
            stickZone.EnterEvent += OnUnitEnter;
            stickZone.ExitEvent += OnUnitExit;
        }

        private void UpdateExistsTimer()
            => _existsTimer.Tick(Time.deltaTime);
        
        private void UpdateTimers()
        {
            var time = Time.deltaTime;
            foreach (var buffer in _buffers)
                buffer.DelayBeforeApplyTimer.Tick(time);

            for (int i = 0; i < _buffers.Count; i++)
                if (_buffers[i].DelayBeforeApplyTimer.TimerIsEnd)
                {
                    TryStick(_buffers[i]);
                    
                    _buffers.RemoveAt(i);
                    i--;
                }
        }

        private void TryStick(BufferBeforeApplyStick bufferBeforeApplyStick)
        {
            if (!stickZone.Contains(bufferBeforeApplyStick.Triggerable)) 
                return;
            
            var moveSpeedChangerConfig = bufferBeforeApplyStick.MoveSpeedChangeable.Affiliation == AffiliationEnum.Bees 
                    ? new MoveSpeedChangerConfig(config.BeeMoveSpeedChangerConfig.Power, float.PositiveInfinity) 
                    : new MoveSpeedChangerConfig(config.EnemyMoveSpeedChangerConfig.Power, float.PositiveInfinity);
                        
            if (bufferBeforeApplyStick.TryCast(out IStickeable stickable))
                stickable.StickyProcessor.BecameSticky(moveSpeedChangerConfig.Time, true);
                        
            bufferBeforeApplyStick.MoveSpeedChangeable.MoveSpeedChangerProcessor.Invoke(moveSpeedChangerConfig, true);
        }
        
        private void OnUnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IMoveSpeedChangeable moveSpeedChangeable))
            {
                if(!_buffers.Contains(c => c.MoveSpeedChangeable == moveSpeedChangeable))
                    _buffers.Add(new BufferBeforeApplyStick(triggerable, moveSpeedChangeable, config.DelayBeforeApply));
            }
        }

        private void OnUnitExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out IMoveSpeedChangeable moveSpeedChangeable))
            {
                var index = _buffers.IndexOf(c => c.MoveSpeedChangeable == moveSpeedChangeable);
                if (index > -1)
                    _buffers.RemoveAt(index);
                else
                {
                    var moveSpeedChangerConfig = moveSpeedChangeable.Affiliation == AffiliationEnum.Bees
                            ? config.BeeMoveSpeedChangerConfig
                            : config.EnemyMoveSpeedChangerConfig;
                    
                    if (triggerable.TryCast(out IStickeable stickeable))
                        stickeable.StickyProcessor.BecameSticky(moveSpeedChangerConfig.Time, true);
                    moveSpeedChangeable.MoveSpeedChangerProcessor.Invoke(moveSpeedChangerConfig, true);
                }
            }
        }

        private void DestructStickyTile()
        {
            FrameworkCommander.GlobalData.ConstructionsRepository.GetConstruction(transform.position, true);
            Destroy(gameObject);
        }
        
        private struct BufferBeforeApplyStick
        {
            public readonly ITriggerable Triggerable;
            public readonly IMoveSpeedChangeable MoveSpeedChangeable;
            public readonly Timer DelayBeforeApplyTimer;

            public BufferBeforeApplyStick(ITriggerable triggerable, IMoveSpeedChangeable moveSpeedChangeable,float timer)
            {
                Triggerable = triggerable;
                MoveSpeedChangeable = moveSpeedChangeable;
                DelayBeforeApplyTimer = new Timer(timer);
            }
        }
    }
}