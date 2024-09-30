using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LoseScreenView : BaseScreenView
    {
        [SerializeField] private Button _retryButton;
        public override ScreenType Type => ScreenType.Lose;

        public void InitPresenter(LoseScreenPresenter presenter) 
        {
            _retryButton.onClick.AddListener(presenter.OnRetryButtonClick);
        }
    }
}