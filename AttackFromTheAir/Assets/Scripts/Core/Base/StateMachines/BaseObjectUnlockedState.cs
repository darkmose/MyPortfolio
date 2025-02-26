using Core.States;
using Core.UI;
using Core.Utilities;

namespace Core.LobbyBase
{
    public class BaseObjectUnlockedState : BaseState<BaseObjectStates>
    {
        private readonly BaseObjectView _view;
        private readonly IntProperty _resourcesGoal;
        private readonly IntProperty _currentResources;
        private readonly FloatProperty _unlockProgress;
        public SimpleEvent LevelUpEvent { get; } = new SimpleEvent();
        public override BaseObjectStates State => BaseObjectStates.Unlocked;

        public BaseObjectUnlockedState(BaseObjectView view, IntProperty resourcesGoal, IntProperty currentResources, FloatProperty unlockProgress)
        {
            _view = view;
            _resourcesGoal = resourcesGoal;
            _currentResources = currentResources;
            _unlockProgress = unlockProgress;
        }

        public override void Enter()
        {
            _view.SetObjectState(FillShaderGraphProgressBar.FillShaderProgressBarStates.Unlocked);
            _view.AnimateObjectUnlocked();
            MakeProgress();
        }

        public override void Exit()
        {
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
            CheckLevelUp();
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
            }
        }

        private void CheckLevelUp()
        {
            if (_currentResources.Value >= _resourcesGoal.Value)
            {
                _view.AnimateObjectLevelUp();
                LevelUpEvent.Notify();
            }            
        }
    }
}