using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class InfoPopupView : BasePopupView
    {
        [SerializeField] private TextMeshProUGUI _infoText;
        [SerializeField] private Button _closeButton;
        public override PopupType Type => PopupType.Info;
        public override ViewType View => ViewType.Popup;

        public void SetText(string text)
        {
            _infoText.text = text;
        }

        public void InitPresenter(InfoPopupPresenter presenter)
        {
            _closeButton.onClick.AddListener(presenter.OnCloseButtonClick);
        }
    }
}