using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class WinScreenView : BaseScreenView
    {
        [SerializeField] private Button _nextLevelButton;
        public override ScreenType Type => ScreenType.Win;

        public void InitPresenter(WinScreenPresenter presenter)
        {
            _nextLevelButton.onClick.AddListener(presenter.OnNextLevelButtonClick);
        }
    }
}