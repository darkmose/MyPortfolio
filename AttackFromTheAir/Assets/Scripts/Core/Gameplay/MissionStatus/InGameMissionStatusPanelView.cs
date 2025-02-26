using Core.Level;
using Core.Resourses;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class InGameMissionStatusPanelView : BaseMissionStatusPanelView
    {
        [SerializeField] private MissionGoalPanelView _goalPanelPrefab;
        [SerializeField] private MissionLoseConditionPanelView _loseConditionPanelPrefab;
        [SerializeField] private GameObject _expandedPanel;
        [SerializeField] private GameObject _shrinkedPanel;
        [SerializeField] private Button _openPanel;
        [SerializeField] private Button _closePanel;
        [SerializeField] private MissionGoalResourceProvider _missionGoalResourceProvider;

        private void OnEnable()
        {
            _openPanel.onClick.AddListener(ExpandPanel);
            _closePanel.onClick.AddListener(ShrinkPanel);
        }

        private void OnDisable()
        {
            _openPanel.onClick.RemoveAllListeners();
            _closePanel.onClick.RemoveAllListeners();
        }

        private void ShrinkPanel()
        {
            _expandedPanel.SetActive(false);
            _shrinkedPanel.SetActive(true);
        }

        private void ExpandPanel()
        {
            _shrinkedPanel.SetActive(false);
            _expandedPanel.SetActive(true);
        }

        public override void InitAdditionalGoals(List<GameEventGoal> gameEventGoals)
        {
            foreach (var goal in gameEventGoals)
            {
                var missionGoalResource = _missionGoalResourceProvider.ProvideMissionGoalResource(goal.GameEventType);
                var goalPanelInstance = Instantiate(_goalPanelPrefab, Root);
                var descriptionFormat = missionGoalResource.StatusFormat;
                var goalIcon = missionGoalResource.GoalIcon;
                goalPanelInstance.SetGoalDescriptionFormat(descriptionFormat);
                goalPanelInstance.SetGoalIcon(goalIcon);
                goalPanelInstance.SetMainGoal(false);
                goalPanelInstance.InitGoal(goal.Amount); 
            }
        }

        public override void InitLoseConditions(List<GameEventGoal> gameEventGoals)
        {
            foreach (var condition in gameEventGoals)
            {
                var missionGoalResource = _missionGoalResourceProvider.ProvideMissionGoalResource(condition.GameEventType);
                var loseConditionPanelInstance = Instantiate(_loseConditionPanelPrefab, Root);
                var descriptionFormat = missionGoalResource.StatusFormat;
                var goalIcon = missionGoalResource.GoalIcon;
                loseConditionPanelInstance.SetConditionDescriptionFormat(descriptionFormat);
                loseConditionPanelInstance.InitConditionGoal(condition.Amount);
            }
        }

        public override void InitWinGoals(List<GameEventGoal> gameEventGoals)
        {
            foreach (var goal in gameEventGoals)
            {
                var missionGoalResource = _missionGoalResourceProvider.ProvideMissionGoalResource(goal.GameEventType);
                var goalPanelInstance = Instantiate(_goalPanelPrefab, Root);
                var descriptionFormat = missionGoalResource.StatusFormat;
                var goalIcon = missionGoalResource.GoalIcon;
                goalPanelInstance.SetGoalDescriptionFormat(descriptionFormat);
                goalPanelInstance.SetGoalIcon(goalIcon);
                goalPanelInstance.SetMainGoal(true);
                goalPanelInstance.InitGoal(goal.Amount);
            }
        }
    }
}