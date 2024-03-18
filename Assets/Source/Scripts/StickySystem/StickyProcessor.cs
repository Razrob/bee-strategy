using CustomTimer;

namespace StickySystem
{
    public class StickyProcessor
    { 
        public bool IsSticky { get; private set; }
    
        private readonly Timer _existTimer; 
    
        public StickyProcessor()
        {
            _existTimer = new Timer(0, 0, true);
            _existTimer.OnTimerEnd += OnExistTimerEnd;
        }

        public void BecameSticky(float time, bool hardSet)
        {
            if (hardSet || _existTimer.MaxTime - _existTimer.CurrentTime < time)
                _existTimer.SetMaxValue(time);
            else
                return;
        
            IsSticky = true;
        }

        private void OnExistTimerEnd()
        {
            IsSticky = false;
        }
    } 
}
