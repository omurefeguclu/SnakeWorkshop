namespace SnakeWorkshop.Scripts.Helpers
{
    public class UpdateTimerHelper
    {
        private float _timer;
        private readonly float _interval;
        
        private bool _isPaused;
        
        public UpdateTimerHelper(float interval, bool paused = false)
        {
            _interval = interval;
            _isPaused = paused;
        }
        
        public void ResetTimer()
        {
            _timer = 0;
        }
        public void TogglePause(bool isPaused)
        {
            _isPaused = isPaused;
        }
        public bool OnTimerInterval()
        {
            if (_isPaused)
                return false;
            
            _timer += UnityEngine.Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0;
                return true;
            }
            
            return false;
        }
    }
}