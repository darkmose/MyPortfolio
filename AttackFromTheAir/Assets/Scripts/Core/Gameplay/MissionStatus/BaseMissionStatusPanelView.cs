using Core.Level;
using Core.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public interface IMissionStatusPanelView
    {
        void InitWinGoals(List<GameEventGoal> gameEventGoals);
        void InitAdditionalGoals(List<GameEventGoal> gameEventGoals);
        void InitLoseConditions(List<GameEventGoal> gameEventGoals);
        void LinkModel(IMissionStatus missionStatus);
        void Clear();
    }

    public abstract class BaseMissionStatusPanelView : MonoBehaviour, IMissionStatusPanelView
    {
        [SerializeField] private Transform _root;
        protected Transform Root => _root;
        private IMissionStatus _model;

        public virtual void Clear()
        {
            _root.ClearAllChild();
        }

        public abstract void InitAdditionalGoals(List<GameEventGoal> gameEventGoals);
        public abstract void InitLoseConditions(List<GameEventGoal> gameEventGoals);
        public abstract void InitWinGoals(List<GameEventGoal> gameEventGoals);

        public void LinkModel(IMissionStatus missionStatus)
        {
            _model = missionStatus;
        }
    }
}