using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class MissionGoalPanelView : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private Color _mainGoalColor;
        [SerializeField] private Color _additionalGoalColor;
        [SerializeField] private TextMeshProUGUI _goalDescription;
        [SerializeField] private Image _goalIcon;
        [SerializeField] private Toggle _checkMark;
        private string _descriptionFormat = string.Empty;
        private int _goal;

        public void SetMainGoal(bool isMainGoal)
        {
            _bg.color = isMainGoal ? _mainGoalColor : _additionalGoalColor;
        }

        public void SetGoalDescriptionFormat(string descriptionFormat)
        {
            _descriptionFormat = descriptionFormat;
        }

        public void SetData(int current)
        {
            _checkMark.SetIsOnWithoutNotify(current == _goal);
            var result = string.Format(_descriptionFormat, current, _goal);
            _goalDescription.text = result;
        }

        public void InitGoal(int goal)
        {
            _goal = goal;
        }

        public void SetGoalIcon(Sprite icon)
        {
            _goalIcon.sprite = icon;
        }
    }
}