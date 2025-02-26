using Core.LobbyBase;
using Core.UI;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LobbyScreenBasePageView : BaseView, IPageView
    {
        [SerializeField] private BaseBuildingTabGenerator _tabGenerator;
        [SerializeField] private TwoStateButton _resourceSendButton;
        [SerializeField] private BaseObjectSelectorView _baseObjectSelectorView;

        [SerializeField] private TextMeshProUGUI _sameObjectsAmount;
        [SerializeField] private TextMeshProUGUI _objectLevel;
        [SerializeField] private TextMeshProUGUI _isUnlockedText;
        [SerializeField] private TextMeshProUGUI _resourcesProgressData;
        [SerializeField] private Image _resourcesProgressBar;
        [SerializeField] private Image _buildingIcon;

        [SerializeField] private Button _advLevelUpButton;
        [SerializeField] private Button _softCurrencyLevelUpButton;
        [SerializeField] private Button _hardCurrencyLevelUpButton;
        [SerializeField] private TextMeshProUGUI _softCurrencyCost;
        [SerializeField] private TextMeshProUGUI _hardCurrencyCost;
        [SerializeField] private Button _previousObjectButton;
        [SerializeField] private Button _nextObjectButton;
        [SerializeField] private RawImage _baseRenderImage;
        [SerializeField] private PlayerBaseControlPanel _playerBaseControlPanel;

        [SerializeField] private GameObject _baseObjectInfoPanel;
        [SerializeField] private GameObject _baseInfoPanel;
        [SerializeField] private GameObject _baseObjectTabsPanel;
        [SerializeField] private Button _switchOnBaseObserveMode;
        [SerializeField] private Button _switchOffBaseObserveMode;
        [SerializeField] private Button _observeNextBaseButton;
        [SerializeField] private Button _observePreviousBaseButton;

        [SerializeField] private Image _baseUnlockProgressBar;
        [SerializeField] private TextMeshProUGUI _baseUnlockProgress;
        [SerializeField] private TextMeshProUGUI _baseName;

        [SerializeField] private GameObject _baseComingSoonPanel;
        [SerializeField] private GameObject _baseDetailsPanel;
        [SerializeField] private TextMeshProUGUI _baseDetailsText;

        public RawImage BaseRenderImage => _baseRenderImage;
        public override ViewType View => ViewType.Panel;
        public BaseBuildingTabGenerator TabGenerator => _tabGenerator;
        public BaseObjectSelectorView BaseObjectSelectorView => _baseObjectSelectorView;
        public PlayerBaseControlPanel PlayerBaseControlPanel => _playerBaseControlPanel;

        public void InitPresenter(IPagePresenter presenter)
        {
            if (presenter is LobbyScreenBasePagePresenter basePagePresenter)
            {
                _resourceSendButton.PointerDownEvent.AddListener(basePagePresenter.OnResourceSendButtonDown);
                _resourceSendButton.PointerUpEvent.AddListener(basePagePresenter.OnResourceSendButtonUp);
                _advLevelUpButton.onClick.AddListener(basePagePresenter.OnAdvLevelUpButtonClick);
                _softCurrencyLevelUpButton.onClick.AddListener(basePagePresenter.OnSoftCurrencyLevelUpButtonClick);
                _hardCurrencyLevelUpButton.onClick.AddListener(basePagePresenter.OnHardCurrencyLevelUpButtonClick);
                _previousObjectButton.onClick.AddListener(basePagePresenter.OnPreviousObjectButtonClick);
                _nextObjectButton.onClick.AddListener(basePagePresenter.OnNextObjectButtonClick);
                _switchOnBaseObserveMode.onClick.AddListener(basePagePresenter.OnSwitchBaseObserveModeButtonClick);
                _switchOffBaseObserveMode.onClick.AddListener(basePagePresenter.OnSwitchBaseObserveModeButtonClick);
                _observeNextBaseButton.onClick.AddListener(basePagePresenter.OnSwitchBaseToNextButtonClick);
                _observePreviousBaseButton.onClick.AddListener(basePagePresenter.OnSwitchBaseToPreviousButtonClick);
            }
        }

        public void SetBaseDetails(string details)
        {
            _baseDetailsText.text = details;
        }

        public void SetActiveComingSoonPanel(bool isActive)
        {
            _baseComingSoonPanel.SetActive(isActive);
        }

        public void SetActiveBaseDetailsPanel(bool isActive)
        {
            _baseDetailsPanel.SetActive(isActive);
        }

        public void SetInteractableObserveNextBaseButton(bool isInteractable)
        {
            _observeNextBaseButton.interactable = isInteractable;
        }

        public void SetInteractableObservePreviousBaseButton(bool isInteractable)
        {
            _observePreviousBaseButton.interactable = isInteractable;
        }

        public void SetActiveBaseInfoPanel(bool isActive)
        {
            _baseInfoPanel.SetActive(isActive);
            _baseObjectInfoPanel.SetActive(!isActive);
            _baseObjectTabsPanel.SetActive(!isActive);
        }

        public void SetBaseName(string name)
        {
            _baseName.text = name;
        }

        public void SetBaseUnlockProgress(float progress)
        {
            _baseUnlockProgressBar.fillAmount = progress;
            _baseUnlockProgress.text = progress.ToString("0%");
        }

        public void SetSameObjectsAmount(int amount)
        {
            _sameObjectsAmount.text = amount.ToString("AMOUNT : 0");

            _nextObjectButton.interactable = amount > 1;
            _previousObjectButton.interactable = amount > 1;
        }

        public void SetObjectLevel(int level)
        {
            _objectLevel.text = level.ToString("LVL 0");
        }

        public void SetUnlockedStatus(bool isUnlocked)
        {
            _isUnlockedText.text = isUnlocked ? "Unlocked" : "Locked";
        }

        private string SetCurrentAmount(string input, int currentAmount)
        {
            string pattern = @"(\d+)\s*out\s*(\d+)";
            string updatedText = Regex.Replace(input, pattern, match => $"{currentAmount} out {match.Groups[2].Value}");
            return updatedText;
        }

        private string SetGoalAmount(string input, int goalAmount)
        {
            string pattern = @"(\d+)\s*out\s*(\d+)";
            string updatedText = Regex.Replace(input, pattern, match => $"{match.Groups[1].Value} out {goalAmount}");
            return updatedText;
        }

        public void SetCurrentResourceAmount(int amount)
        {
            _resourcesProgressData.text = SetCurrentAmount(_resourcesProgressData.text, amount);
        }

        public void SetResourceGoal(int goal)
        {
            _resourcesProgressData.text = SetGoalAmount(_resourcesProgressData.text, goal);
        }

        public void SetResourceProgress(float progress)
        {
            _resourcesProgressBar.fillAmount = progress;
        }

        public void SetBuildingIcon(Sprite icon)
        {
            _buildingIcon.sprite = icon;
        }

        public void SetLvlUpSoftCurrencyCost(int currency)
        {
            _softCurrencyCost.text = currency.ToString();
        }

        public void SetLvlUpHardCurrencyCost(int currency)
        {
            _hardCurrencyCost.text = currency.ToString();
        }

        public void SetSoftCurrencyLvlUpButtonIntaractable(bool isInteractable)
        {
            _softCurrencyLevelUpButton.interactable = isInteractable;
        }

        public void SetSoftCurrencyLvlUpButtonVisible(bool isVisible)
        {
            _softCurrencyLevelUpButton.gameObject.SetActive(isVisible);
        }

        public void SetHardCurrencyLvlUpButtonIntaractable(bool isInteractable)
        {
            _hardCurrencyLevelUpButton.interactable = isInteractable;
        }

        public void SetHardCurrencyLvlUpButtonVisible(bool isVisible)
        {
            _hardCurrencyLevelUpButton.gameObject.SetActive(isVisible);
        }
    }
}