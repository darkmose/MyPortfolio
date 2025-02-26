using TMPro;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectDataPanelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _keyValue;

        public void SetKeyValue(string key, string value)
        {
            _keyValue.text = $"{key} : {value}";
        }

        public void SetInfo(string info)
        {
            _keyValue.text = info;
        }
    }
}