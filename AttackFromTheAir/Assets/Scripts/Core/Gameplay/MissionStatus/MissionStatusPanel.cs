using Core.Events;
using Core.Level;
using Core.UI;
using System;
using System.Collections.Generic;

namespace Core.GameLogic
{
    public interface IMissionStatus : IDisposable
    {
        void InitMissionGoals(LevelDescriptor levelDescriptor);
        void LinkView(IMissionStatusPanelView missionStatusPanelView);
    }

    public class MissionStatusPanel : IMissionStatus
    {
        private List<IMissionStatusPanelView> _views;
        private LevelDescriptor _currentLevel;
        private Dictionary<GameEventType, MissionGoalStatus> _mainGoalsStatus;
        private Dictionary<GameEventType, MissionGoalStatus> _additionalGoalsStatus;
        private Dictionary<GameEventType, MissionGoalStatus> _loseConditionStatus;

        public MissionStatusPanel()
        {
            _mainGoalsStatus = new Dictionary<GameEventType, MissionGoalStatus>();
            _additionalGoalsStatus = new Dictionary<GameEventType, MissionGoalStatus>();
            _loseConditionStatus = new Dictionary<GameEventType, MissionGoalStatus>();
            EventAggregator.Subscribe<GameEvent>(OnGameEvent);
        }

        private void OnGameEvent(object sender, GameEvent data)
        {
            var gameEventType = data.GameEventType;
            if (_mainGoalsStatus.ContainsKey(gameEventType))
            {
                var value = _mainGoalsStatus[gameEventType].Current.Value + 1;
                _mainGoalsStatus[gameEventType].Current.SetValue(value, false);
            }
            if (_additionalGoalsStatus.ContainsKey(gameEventType))
            {
                var value = _additionalGoalsStatus[gameEventType].Current.Value + 1;
                _additionalGoalsStatus[gameEventType].Current.SetValue(value, false);
            }
            if (_loseConditionStatus.ContainsKey(gameEventType))
            {
                var value = _loseConditionStatus[gameEventType].Current.Value + 1;
                _loseConditionStatus[gameEventType].Current.SetValue(value, false);
            }
        }

        public void Dispose()
        {
            _views?.Clear();
        }

        public void InitMissionGoals(LevelDescriptor levelDescriptor)
        {
            _currentLevel = levelDescriptor;
            _mainGoalsStatus.Clear();
            _additionalGoalsStatus.Clear();
            _loseConditionStatus.Clear();

            var mainGoals = levelDescriptor.GameModeSettings.EventsToWin.Events;
            var additionalGoals = levelDescriptor.GameModeSettings.AdditionalGoals.Events;
            var loseConditions = levelDescriptor.GameModeSettings.EventsToLose.Events;

            foreach (var goal in mainGoals)
            {
                if (true)
                {

                }
            }
            InitViews();
        }

        public void LinkView(IMissionStatusPanelView missionStatusPanelView)
        {
            _views.Add(missionStatusPanelView);
            missionStatusPanelView.LinkModel(this);
        }

        public void UnlinkView(IMissionStatusPanelView missionStatusPanelView)
        {
            _views.Remove(missionStatusPanelView);
        }

        private void InitViews()
        {
            var gamemodeSettings = _currentLevel.GameModeSettings;
            foreach (var view in _views)
            {
                view.Clear();
                view.InitWinGoals(gamemodeSettings.EventsToWin.Events);
                view.InitAdditionalGoals(gamemodeSettings.AdditionalGoals.Events);
                view.InitLoseConditions(gamemodeSettings.EventsToLose.Events);
            }
        }
    }

    public class MissionGoalStatus
    {
        public GameEventType GameEventType;
        public IntProperty Current;
        public IntProperty Goal;
    }
}