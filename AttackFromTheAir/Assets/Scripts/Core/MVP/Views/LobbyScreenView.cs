using Core.GameLogic;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LobbyScreenView : BaseScreenView
    {
        public override ScreenType Type => ScreenType.Game;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _prevLevelButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _baseExpPercent;
        [SerializeField] private TextMeshProUGUI _baseLevel;
        [SerializeField] private TextMeshProUGUI _playerExpPercent;
        [SerializeField] private TextMeshProUGUI _playerLevel;
        [SerializeField] private TextMeshProUGUI _dollarsAmount;
        [SerializeField] private TextMeshProUGUI _hammersAmount;
        [SerializeField] private TextMeshProUGUI _gemsAmount;
        [SerializeField] private LobbyTabToggles _tabToggles;
        [SerializeField] private PlayerWeaponUpgradeSystemView _weaponUpgradeSystemView;
        [SerializeField] private GamemodePanelSelector _gamemodePanelSelector;
        [SerializeField] private Image _missionPreview;
        public PlayerWeaponUpgradeSystemView WeaponUpgradeSystemView => _weaponUpgradeSystemView;
        public GamemodePanelSelector GamemodePanelSelector => _gamemodePanelSelector;
        public LobbyTabToggles TabToggles => _tabToggles;
        
        public void InitPresenter(LobbyScreenPresenter presenter)
        {
            _startButton.onClick.AddListener(presenter.OnStartButtonClickHandler);
            _prevLevelButton.onClick.AddListener(presenter.OnLevelSelectorLeftArrowClick);
            _nextLevelButton.onClick.AddListener(presenter.OnLevelSelectorRightArrowClick);
        }

        //public void SetMissionDescription(string description)
        //{
        //    _missionDescription.text = description;
        //}

        public void SetMissionPreview(Sprite preview)
        {
            _missionPreview.sprite = preview;
        }

        public void SetCurrentLevel(int level)
        {
            _currentLevel.text = $"Level {level}";  
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        public void SetPlayerLevel(int level) 
        {
            _playerLevel.text = level.ToString("LVL.#");
        }

        public void SetPlayerExpPercent(float normalizedExp)
        {
            _playerExpPercent.text = normalizedExp.ToString("0%");
        }

        public void SetBaseLevel(int level)
        {
            _baseLevel.text = level.ToString("LVL.#");
        }

        public void SetBaseExpPercent(float normalizedExp)
        {
            _baseExpPercent.text = normalizedExp.ToString("0%");
        }

        public void SetDollarsAmount(int amount)
        {
            _dollarsAmount.text = amount.ToString("$#");
        }

        public void SetHammersAmount(int amount) 
        {
            _hammersAmount.text = amount.ToString();
        }

        public void SetGemsAmount(int amount)
        {
            _gemsAmount.text = amount.ToString();
        }
    }
}