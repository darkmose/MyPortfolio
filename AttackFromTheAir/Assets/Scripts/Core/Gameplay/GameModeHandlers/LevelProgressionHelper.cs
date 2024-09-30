using Core.Events;
using Core.Level;
using Core.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public interface ILevelProgressionHelper
    {
        SimpleEvent<List<LevelRewardDescriptor>> GetRewardEvent { get; }
        SimpleEvent MainGoalsAchievedEvent { get; }
        SimpleEvent AdditionalGoalsAchievedEvent { get; }
        SimpleEvent LoseEvent { get; }
        bool HasMainReward { get; }
        bool HasAdditionalReward { get; }
        bool HasLoseReward { get; }
        void InitGoals(GameModeSettings gameModeSettings);
        void ClearGoals();
        void StartObserving();
        void StopObserving();
    }

    public class LevelProgressionHelper : ILevelProgressionHelper
    {
        private SimpleEvent<List<LevelRewardDescriptor>> _rewardEvent = new SimpleEvent<List<LevelRewardDescriptor>>();
        private SimpleEvent _mainGoalsAchievedEvent = new SimpleEvent();
        private SimpleEvent _additionalGoalsAchievedEvent = new SimpleEvent();
        private SimpleEvent _loseEvent = new SimpleEvent();
        private Dictionary<GameEventType, int> _winGoalsDict;
        private Dictionary<GameEventType, int> _additionalGoalsDict;
        private Dictionary<GameEventType, int> _loseGoalsDict;
        private List<LevelRewardDescriptor> _mainWinRewards;
        private List<LevelRewardDescriptor> _additionalWinRewards;
        private List<LevelRewardDescriptor> _loseRewards;
        private GameEventGenerator _gameEventGenerator;
        private bool _hasMainReward;
        private bool _hasAdditionalReward;
        private bool _hasLoseReward;
        public SimpleEvent<List<LevelRewardDescriptor>> GetRewardEvent => _rewardEvent;
        public SimpleEvent MainGoalsAchievedEvent => _mainGoalsAchievedEvent;
        public SimpleEvent AdditionalGoalsAchievedEvent => _additionalGoalsAchievedEvent;
        public SimpleEvent LoseEvent => _loseEvent;
        public bool HasMainReward => _hasMainReward;
        public bool HasAdditionalReward => _hasAdditionalReward;
        public bool HasLoseReward => _hasLoseReward;

        public LevelProgressionHelper(GameEventGenerator gameEventGenerator)
        {
            _winGoalsDict = new Dictionary<GameEventType, int>();
            _additionalGoalsDict = new Dictionary<GameEventType, int>();
            _loseGoalsDict = new Dictionary<GameEventType, int>();
            _gameEventGenerator = gameEventGenerator;
            EventAggregator.Subscribe<GameEvent>(OnGameEvent);
        }

        private void OnGameEvent(object sender, GameEvent data)
        {
            if (_winGoalsDict.ContainsKey(data.GameEventType))
            {
                Debug.Log("Win Goal Event");
                _winGoalsDict[data.GameEventType]--;
                if (_winGoalsDict[data.GameEventType] == 0)
                {
                    _mainGoalsAchievedEvent.Notify();
                }
            }
            if (_additionalGoalsDict.ContainsKey(data.GameEventType))
            {
                Debug.Log("Additional Goal Event");
                _additionalGoalsDict[data.GameEventType]--;
                if (_additionalGoalsDict[data.GameEventType] == 0) 
                {
                    _additionalGoalsAchievedEvent.Notify();
                }
            }
            if (_loseGoalsDict.ContainsKey(data.GameEventType))
            {
                Debug.Log("Lose Goal Event");
                _loseGoalsDict[data.GameEventType]--;
                if (_loseGoalsDict[data.GameEventType] == 0)
                {
                    _loseEvent.Notify();
                }
            }
        }

        public void ClearGoals()
        {
            _gameEventGenerator.ClearEvents();
            _winGoalsDict.Clear();
            _additionalGoalsDict.Clear();
            _loseGoalsDict.Clear();
            _mainWinRewards?.Clear();
            _additionalWinRewards?.Clear();
            _loseRewards?.Clear();
            _hasMainReward = false;
            _hasLoseReward = false;
            _hasAdditionalReward = false;
        }

        public void InitGoals(GameModeSettings gameModeSettings)
        {
            var mainWinRewards = gameModeSettings.EventsToWin.Rewards;
            if (mainWinRewards != null && mainWinRewards.Count > 0)
            {
                _mainWinRewards = mainWinRewards;
                _hasMainReward = true;
            }
            else 
            {
                _hasMainReward = false;
            }

            var additionalRewards = gameModeSettings.AdditionalGoals.Rewards;
            if (additionalRewards != null && additionalRewards.Count > 0)
            {
                _additionalWinRewards = additionalRewards;
                _hasAdditionalReward = true;
            }
            else
            {
                _hasAdditionalReward = false;
            }

            var loseRewards = gameModeSettings.EventsToLose.Rewards;
            if (loseRewards != null && loseRewards.Count > 0)
            {
                _loseRewards = loseRewards;
                _hasLoseReward = true;
            }
            else
            {
                _hasLoseReward = false; 
            }

            var allGameEvents = new List<GameEventType>();

            foreach (var @event in gameModeSettings.EventsToWin.Events)
            {
                allGameEvents.Add(@event.GameEventType);
                _winGoalsDict[@event.GameEventType] = @event.Amount;
            }
            foreach (var @event in gameModeSettings.AdditionalGoals.Events)
            {
                allGameEvents.Add(@event.GameEventType);
                _additionalGoalsDict[@event.GameEventType] = @event.Amount;
            }
            foreach (var @event in gameModeSettings.EventsToLose.Events)
            {
                allGameEvents.Add(@event.GameEventType);
                _loseGoalsDict[@event.GameEventType] = @event.Amount;
            }

            _gameEventGenerator.InitEvents(allGameEvents);
        }

        public void StartObserving()
        {
            _gameEventGenerator.StartEventsObserve();
        }

        public void StopObserving()
        {
            _gameEventGenerator.StopEventsObserve();
        }
    }
}