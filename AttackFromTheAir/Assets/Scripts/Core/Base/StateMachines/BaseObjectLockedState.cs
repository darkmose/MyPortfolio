using Core.States;
using Core.UI;
using Core.Utilities;
using System;

namespace Core.LobbyBase
{
    public class BaseObjectLockedState : BaseState<BaseObjectStates>
    {
        private readonly BaseObjectView _view;
        private readonly BoolProperty _isObjectUnlocked;
        private readonly IntProperty _resourcesGoal;
        private readonly IntProperty _currentResources;
        private readonly FloatProperty _unlockProgress;
        public SimpleEvent ObjectUnlockedEvent { get; } = new SimpleEvent();
        public override BaseObjectStates State => BaseObjectStates.Locked;

        public BaseObjectLockedState(BaseObjectView view, BoolProperty isObjectUnlocked, IntProperty resourcesGoal, IntProperty currentResources, FloatProperty unlockProgress)
        {
            _view = view;
            _isObjectUnlocked = isObjectUnlocked;
            _resourcesGoal = resourcesGoal;
            _currentResources = currentResources;
            _unlockProgress = unlockProgress;
        }

        public override void Enter()
        {
            if (_currentResources.Value == 0)
            {
                _view.SetObjectState(FillShaderGraphProgressBar.FillShaderProgressBarStates.Inactive);
                _currentResources.RegisterValueChangeListener(OnCurrentResourceChanged);
            }
            else
            {
                _view.SetObjectState(FillShaderGraphProgressBar.FillShaderProgressBarStates.ProgressActive);
            }

            MakeProgress();
        }

        private void OnCurrentResourceChanged(int amount)
        {
            if (amount > 0)
            {
                _view.SetObjectState(FillShaderGraphProgressBar.FillShaderProgressBarStates.ProgressActive);
                _currentResources.UnregisterValueChangeListener(OnCurrentResourceChanged);
            }
        }

        public override void Exit()
        {
            _currentResources.UnregisterValueChangeListener(OnCurrentResourceChanged);
        }

        private void CheckUnlocked()
        {
            if (_currentResources.Value >= _resourcesGoal.Value)
            {
                _isObjectUnlocked.SetValue(true);
                ObjectUnlockedEvent.Notify();
                stateMachine.SwitchToState(BaseObjectStates.Unlocked);
            }
        }

        public int AddResource(int amount)
        {
            _currentResources.SetValue(_currentResources.Value + amount, false);
            _view.AnimateGotResource();
            var change = _currentResources.Value - _resourcesGoal.Value;
            if (change > 0)
            {
                _currentResources.SetValue(_resourcesGoal.Value, false);
            }

            MakeProgress();
            CheckUnlocked();

            return change;
        }

        private void MakeProgress()
        {
            if (_resourcesGoal.Value != 0)
            {
                var progress = _currentResources.Value / (float)_resourcesGoal.Value;
                if (progress > 1f)
                {
                    progress = 1f;
                }
                _unlockProgress.SetValue(progress, false);
                _view.SetProgress(progress);
            }        
        }
    }
}