using Core.Tools;
using Core.UI;
using Core.UnitsImplementation;
using Core.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Core.GameLogic
{
    public class DroneStatus
    {
        private IntProperty _availableCount = new IntProperty(1);
        private BoolProperty _canUse = new BoolProperty(true);
        private BoolProperty _isLocked = new BoolProperty(true);
        private CustomProperty<string> _cooldownRemain = new CustomProperty<string>(string.Empty);
        private BoolProperty _isSelected = new BoolProperty(false);
        private SimpleEvent<DroneStatus> _cooldownOverEvent = new SimpleEvent<DroneStatus>();
        private float _cooldownDuration;
        private Tweener _timer;        
        private TimerHandler _timerHandler;
        private int _maxAvaialableCount = 1;
        public IPropertyReadOnly<bool> CanUse => _canUse;
        public IPropertyReadOnly<string> CooldownRemain => _cooldownRemain;
        public IPropertyReadOnly<int> AvailableCount => _availableCount;
        public IPropertyReadOnly<int> AdditionalCount => _availableCount;
        public IPropertyReadOnly<bool> IsLocked => _isLocked;
        public IPropertyReadOnly<bool> IsSelected => _isSelected;
        public SimpleEvent<DroneStatus> CooldownOverEvent => _cooldownOverEvent;
        public float RemainTimerTime => _timer == null ? 0f : _timerHandler.RemainTime;

        public DroneStatus(float cooldownDuration)
        {
            InitCooldownDuration(cooldownDuration);
        }

        public void InitMaxCount(int maxCount)
        {
            if (_maxAvaialableCount == _availableCount.Value)
            {
                _availableCount.SetValue(maxCount, false);
            }
            else if(_availableCount.Value != 0 && _canUse.Value)
            {
                var delta = maxCount - _maxAvaialableCount;
                _availableCount.SetValue(_availableCount.Value + delta, false);
            }
            _maxAvaialableCount = maxCount;
        }

        public void SetLocked(bool locked)
        {
            _isLocked.SetValue(locked);
        }

        public void InitCooldownDuration(float cooldownDuration)
        {
            _cooldownDuration = cooldownDuration;
        }

        public void SetAvailableCount(int availableCount)
        {
            _availableCount.SetValue(availableCount, false);
        }

        public void HandleUse()
        {
            var available = _availableCount.Value - 1;
            SetAvailableCount(available);
        }

        public void Select()
        {
            _isSelected.SetValue(true);
        }

        public void Unselect()
        {
            _isSelected.SetValue(false);
        }

        public void SetCanUse()
        {
            _timer?.Kill();
            _timer = null;
            _timerHandler = null;
            _canUse.SetValue(true);
        }

        public void StartCooldown(float cooldownDuration = -1f)
        {
            if (cooldownDuration < 0f)
            {
                cooldownDuration = _cooldownDuration;
            }
            _timer?.Kill();
            _canUse.SetValue(false);
            _timerHandler = Timer.SetBackwardTimer(cooldownDuration, OnTimerComplete, OnTimerUpdate);
            _timer = _timerHandler.Timer;
        }

        private void SetCooldownRemain(float remainTimeNormalized)
        {
            var convertedSeconds = TimeConvertionTools.ConvertSeconds(_timerHandler.RemainTime);
            _cooldownRemain.Notify(convertedSeconds);
        }

        private void OnTimerUpdate(float remainTime)
        {
            SetCooldownRemain(remainTime);
        }

        private void OnTimerComplete()
        {
            _timer = null;
            _timerHandler = null;
            _cooldownRemain.Notify(string.Empty);
            _canUse.SetValue(true);
            _cooldownOverEvent.Notify(this);
        }
    }
}