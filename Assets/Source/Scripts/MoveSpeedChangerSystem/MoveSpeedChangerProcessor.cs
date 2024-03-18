using System.Collections.Generic;
using System.Linq;
using CustomTimer;
using UnityEngine;
using UnityEngine.AI;

namespace MoveSpeedChangerSystem
{
    public class MoveSpeedChangerProcessor
    {
        private readonly ProcessorBlock _speedUpBlock;
        private readonly ProcessorBlock _speedDownBlock;

        public MoveSpeedChangerProcessor(NavMeshAgent navMeshAgent, float mainSpeed)
        {
            _speedUpBlock = new ProcessorBlock(navMeshAgent, mainSpeed);
            _speedDownBlock = new ProcessorBlock(navMeshAgent, mainSpeed);
        }

        public void HandleUpdate(float time)
        {
            _speedUpBlock.HandleUpdate(time);
            _speedDownBlock.HandleUpdate(time);
        }

        public void Invoke(MoveSpeedChangerConfig moveSpeedChangerConfig, bool hardSet = false)
        {
            if (moveSpeedChangerConfig.Power >= 0)
                _speedUpBlock.Invoke(moveSpeedChangerConfig, hardSet);
            else
                _speedDownBlock.Invoke(moveSpeedChangerConfig, hardSet);
        }

        private class ProcessorBlock
        {
            private readonly float _mainSpeed;
            private readonly NavMeshAgent _navMeshAgent;
            private readonly List<ProcessorBlockCell> _cells;

            private ProcessorBlockCell _currentBlockCell;

            public ProcessorBlock(NavMeshAgent navMeshAgent, float mainSpeed)
            {
                _navMeshAgent = navMeshAgent;
                _mainSpeed = mainSpeed;
                _cells = new List<ProcessorBlockCell>();
            }

            public void HandleUpdate(float time)
            {
                var cells = _cells.ToList();
                foreach (var cell in cells)
                    cell.HandleUpdate(time);
            }

            public void Invoke(MoveSpeedChangerConfig moveSpeedChangerConfig, bool hardSet)
            {
                foreach (var cell in _cells)
                    if (cell.Power == moveSpeedChangerConfig.Power)
                    {
                        cell.TrySetTime(moveSpeedChangerConfig.Time, hardSet);
                        return;
                    }

                var processorBlockCell =
                    new ProcessorBlockCell(_navMeshAgent, moveSpeedChangerConfig, _mainSpeed);
                processorBlockCell.Timer.OnTimerEnd += Recheck;
                _cells.Add(processorBlockCell);

                if (_currentBlockCell == null)
                {
                    _currentBlockCell = processorBlockCell;
                    _currentBlockCell.Activate();
                    return;
                }

                if (Mathf.Abs(_currentBlockCell.Power) < Mathf.Abs(processorBlockCell.Power))
                {
                    _currentBlockCell.DeActivate();
                    _currentBlockCell = processorBlockCell;
                    _currentBlockCell.Activate();
                }
            }

            private void Recheck()
            {
                for (int i = 0; i < _cells.Count; i++)
                {
                    if (_cells[i].Timer.TimerIsEnd)
                    {
                        _cells[i].DeActivate();
                        _cells.RemoveAt(i);
                        i--;
                    }
                }

                if (!_currentBlockCell.Timer.TimerIsEnd)
                    return;

                _currentBlockCell = null;
                var currentIndex = -1;
                var currentPower = 0;

                for (int i = 0; i < _cells.Count; i++)
                {
                    if (Mathf.Abs(_cells[i].Power) > Mathf.Abs(currentPower))
                    {
                        currentIndex = i;
                        currentPower = _cells[i].Power;
                    }
                }

                if (currentIndex > -1)
                {
                    _currentBlockCell = _cells[currentIndex];
                    _currentBlockCell.Activate();
                }
            }

            private class ProcessorBlockCell
            {
                public readonly int Power;
                public readonly Timer Timer;

                private readonly NavMeshAgent _navMeshAgent;
                private readonly float _changeSpeed;
                private bool _isActive;

                public ProcessorBlockCell(NavMeshAgent navMeshAgent, MoveSpeedChangerConfig moveSpeedChangerConfig,
                    float mainSpeed)
                {
                    _navMeshAgent = navMeshAgent;
                    Power = moveSpeedChangerConfig.Power;
                    _changeSpeed = mainSpeed * Power / 100;
                    Timer = new Timer(moveSpeedChangerConfig.Time);
                }

                public void HandleUpdate(float time)
                    => Timer.Tick(time);

                public void Activate()
                {
                    if (_isActive)
                        return;

                    _isActive = true;
                    _navMeshAgent.speed += _changeSpeed;
                }

                public void DeActivate()
                {
                    if (!_isActive)
                        return;

                    _isActive = false;
                    _navMeshAgent.speed -= _changeSpeed;
                }
                
                public void TrySetTime(float time, bool hardSet)
                {
                    if (hardSet || Timer.MaxTime - Timer.CurrentTime < time)
                        Timer.SetMaxValue(time);
                }
            }
        }
    }
}