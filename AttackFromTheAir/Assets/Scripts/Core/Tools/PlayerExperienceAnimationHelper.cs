using Core.PlayerModule;
using Core.Resourses;
using System;

namespace Core.MVP
{
    public class PlayerExperienceAnimationHelper : IDisposable
    {
        private PlayerExperienceAnimationHelperView _view;
        private ExperienceHolder _experienceHolder;
        private IPlayerExpirienceService _playerExpirienceService;
        private int _maxExperienceValue = 0;
        private int _currentExperienceValue;
        private int _currentLevel;
        private int _additionalExperienceValue;
        private float _currentExperiencePercentageValue;
        private ExperienceType _experienceType;

        public int CurrentLevel => _currentLevel;
        public int CurrentExperienceValue => _currentExperienceValue;
        public float CurrentExperiencePercentageValue => _currentExperiencePercentageValue;
        public int AdditionalExpirienceValue => _additionalExperienceValue;

        public PlayerExperienceAnimationHelper(IPlayerExpirienceService playerExpirienceService, ExperienceHolder experienceHolder, ExperienceType experienceType)
        {
            _playerExpirienceService = playerExpirienceService;
            _experienceHolder = experienceHolder;
            _experienceType = experienceType;
        }

        public void LinkView(PlayerExperienceAnimationHelperView view)
        {
            _view = view;
            view.LinkModel(this);
        }

        public void Init()
        {
            if (_experienceType == ExperienceType.PlayerExp)
            {
                _currentLevel = _playerExpirienceService.PlayerLevel.Value;
                _maxExperienceValue = _experienceHolder.GetRequiredExperienceForPlayerLevel(_currentLevel + 1);
                _currentExperienceValue = (int)_playerExpirienceService.PlayerExpirience.Value;
            }
            else
            {
                _currentLevel = _playerExpirienceService.BaseLevel.Value;
                _maxExperienceValue = _experienceHolder.GetRequiredExperienceForBaseLevel(_currentLevel + 1);
                _currentExperienceValue = (int)_playerExpirienceService.BaseExpirience.Value;
            }
            _currentExperiencePercentageValue = (float)_currentExperienceValue / (float)_maxExperienceValue;
            _additionalExperienceValue = 0;
            _view.SetInitialData(_currentLevel, _additionalExperienceValue, _currentExperiencePercentageValue);
        }

        public void MultiplyExperience(float multiplier)
        {
            var expToAdd = (int)((float)_additionalExperienceValue * multiplier) - _additionalExperienceValue;
            AddExperience(expToAdd);
        }

        public void AddExperience(int experience)
        {
            if (experience <= 0) return;

            _additionalExperienceValue += experience;

            var levelsToAdd = 0;
            var canAddLevel = true;
            var currentExp = _currentExperienceValue + _additionalExperienceValue;
            _view.AnimateExperienceValueToAdd(_additionalExperienceValue - experience, _additionalExperienceValue, 1f);

            while (canAddLevel)
            {
                if (_experienceType == ExperienceType.PlayerExp)
                {
                    _maxExperienceValue = _experienceHolder.GetRequiredExperienceForPlayerLevel(_currentLevel + levelsToAdd + 1);
                }
                else
                {
                    _maxExperienceValue = _experienceHolder.GetRequiredExperienceForBaseLevel(_currentLevel + levelsToAdd + 1);
                }

                if (currentExp > _maxExperienceValue)
                {
                    currentExp -= _maxExperienceValue;
                    levelsToAdd++;
                }
                else
                {
                    canAddLevel = false;
                }
            }

            var experiencePercentage = currentExp / (float)_maxExperienceValue;

            if (levelsToAdd > 0)
            {
                var newLevel = _currentLevel + levelsToAdd;
                _view.AnimateLevelUp(_currentLevel, newLevel, _currentExperiencePercentageValue, experiencePercentage);
                _currentLevel = newLevel;
            }
            else
            {
                _view.AnimateCurrentExperiencePercentage(_currentExperiencePercentageValue, experiencePercentage, 1f);
            }
            _currentExperienceValue = currentExp;
            _currentExperiencePercentageValue = experiencePercentage;
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
        public override string ToString()
        {
            return $"ExpType: {_experienceType}\n" +
                $"AdditionalExp: {_additionalExperienceValue}";
        }
    }
}