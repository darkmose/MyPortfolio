using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class SettingsWindowView : BaseWindowView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _goToLobbyButton;
        public override WindowType WindowType => WindowType.Settings;

        public void InitPresenter(SettingsWindowPresenter presenter)
        {
            _closeButton.onClick.AddListener(presenter.Hide);
            _goToLobbyButton.onClick.AddListener(presenter.OnGoToLobbyButtonClick);
        }

        public void SetActiveGoToLobbyButton(bool isActive) 
        {
            _goToLobbyButton.gameObject.SetActive(isActive);
        }
    }
}