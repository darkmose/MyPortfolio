using TMPro;
using UnityEngine;

namespace Core.GameLogic
{
    public class MissionLoseConditionPanelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _condition;
        private string _descriptionFormat = string.Empty;
        private int _goal;

        public void SetData(int current)
        {
            var result = string.Format(_descriptionFormat, current, _goal);
            _condition.text = result;
        }

        public void InitConditionGoal(int goal)
        {
            _goal = goal;
        }

        public void SetConditionDescriptionFormat(string conditionDescriptionFormat)
        {
            _descriptionFormat = conditionDescriptionFormat;
        }
    }
}