using Core.MVP;
using Core.PlayerModule;
using Core.Resourses;
using Core.UI;
using UnityEngine;

namespace Core.Level
{
    public class LevelSelector
    {
        private ILevelProgression _levelProgression;
        private ILobbyScreenPresenter _lobbyScreenPresenter;
        private int _maxLevel;
        private IntProperty _currentLevel = new IntProperty(1);
        private CustomProperty<GameMode> _gameMode = new CustomProperty<GameMode>(Level.GameMode.Attack);
        private CustomProperty<string> _missionDescription = new CustomProperty<string>(string.Empty);
        private CustomProperty<Sprite> _missionPreview = new CustomProperty<Sprite>(null);
        private ResourceHolder _resourceHolder;

        public IPropertyReadOnly<int> CurrentLevel => _currentLevel;
        public IPropertyReadOnly<GameMode> GameMode => _gameMode;
        public IPropertyReadOnly<Sprite> MissionPreview => _missionPreview;
        public IPropertyReadOnly<string> MissionDescription => _missionDescription;

        public LevelSelector(ILevelProgression levelProgression, ILobbyScreenPresenter lobbyScreenPresenter, ResourceHolder resourceHolder)
        {
            _levelProgression = levelProgression;
            _resourceHolder = resourceHolder;
            _lobbyScreenPresenter = lobbyScreenPresenter;
            _lobbyScreenPresenter.LeftLevelSelectorArrowEvent.AddListener(OnPreviousLevel);
            _lobbyScreenPresenter.RightLevelSelectorArrowEvent.AddListener(OnNextLevel);
            _levelProgression.CurrentMaxLevel.RegisterValueChangeListener(OnMaxLevelChanged);
            OnMaxLevelChanged(_levelProgression.CurrentMaxLevel.Value);
        }

        private void OnNextLevel()
        {
            if (_currentLevel.Value < _maxLevel)
            {
                _currentLevel.SetValue(_currentLevel.Value + 1, false);
                UpdateLevelData();
            }
        }

        private void OnPreviousLevel()
        {
            if (_currentLevel.Value > 1)
            {
                _currentLevel.SetValue(_currentLevel.Value - 1, false);
                UpdateLevelData();
            }
        }

        private void UpdateLevelData()
        {
            var levelDescriptor = _resourceHolder.GetLevelRepeatly(_currentLevel.Value);
            var preview = levelDescriptor.MissionPreview;
            var description = levelDescriptor.MissionDescription;
            var gamemode = levelDescriptor.GameMode;

            _gameMode.SetValue(gamemode, false);
            _missionDescription.SetValue(description, false);
            _missionPreview.SetValue(preview, false);
        }

        private void OnMaxLevelChanged(int value)
        {
            _currentLevel.SetValue(value, true);
            _maxLevel = value;
            UpdateLevelData();
        }
    }
}