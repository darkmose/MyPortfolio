using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class StatisticPanelView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _value;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetValue(int value)
        {
            _value.text = value.ToString();
        }
    }
}