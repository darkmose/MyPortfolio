using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LoadingScreenView : BaseScreenView
    {
        [SerializeField] private Button _startLevelButton;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _loadingText;
        
        public override ScreenType Type => ScreenType.Loading;

        public void InitPresenter(LoadingScreenPresenter presenter)
        {
            _startLevelButton.onClick.AddListener(presenter.OnStartLevelButtonClick);
        }

        public void SetCurrentLevel(int level)
        {
            _currentLevel.text = level.ToString("LEVEL #");  
        }

        public void SetInteractableStartButton(bool interactable)
        {
            _startLevelButton.interactable = interactable;
        }
    }
}