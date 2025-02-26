using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UnitDestoyPanel : MonoBehaviour
    {
        [SerializeField] private Image _iconUnit;
        [SerializeField] private TextMeshProUGUI _countUnitDestroyText;

        public void SetIconUniyDestroy(Sprite unity)
        {
            _iconUnit.sprite = unity;
        }

        public void SetCountDestroyUnit(string count)
        {
            _countUnitDestroyText.text = count;
        }

        public void StatusActivPanel(bool onOffStatus)
        {
            gameObject.SetActive(onOffStatus);
        }

    }
}
