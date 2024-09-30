using UnityEngine;
using Core.UI;
using UnityEngine.UI;

namespace Core.MVP
{
    public class GameScreenView : BaseScreenView
    {
        [SerializeField] private ZoomScalePanelView _zoomScalePanelView;
        [SerializeField] private FireButtonAnimationHelper _fireButtonAnimationHelper;
        [SerializeField] private Image _currentWeaponIcon;
        [SerializeField] private Image _secondWeaponIcon;
        [SerializeField] private Image _extraWeaponIcon;
        [SerializeField] private FireButtonView _fireButton;
        [SerializeField] private Button _extraFireButton;
        [SerializeField] private Button _switchWeaponButton;
        [SerializeField] private Button _switchNightVisionButton;
        [SerializeField] private Button _settingsButtonClick;
        [SerializeField] private TargetingService _targetingService;
        public TargetingService TargetingService => _targetingService;
        public ZoomScalePanelView ZoomScalePanelView => _zoomScalePanelView;
        public FireButtonView FireButton => _fireButton;
        public FireButtonAnimationHelper FireButtonAnimationHelper => _fireButtonAnimationHelper;
        public override ScreenType Type => ScreenType.Game;

        protected override void OnAwake()
        {
            base.OnAwake();            
        }

        public void InitPresenter(GameScreenPresenter presenter)
        {
            _fireButton.ClickEvent.AddListener(presenter.OnWeaponFireButtonClick);
            _extraFireButton.onClick.AddListener(presenter.OnExtraWeaponFireButtonClick);
            _switchWeaponButton.onClick.AddListener(presenter.OnWeaponSwitchButtonClick);
            _switchNightVisionButton.onClick.AddListener(presenter.OnSwitchNightVisionButtonClick);
            _settingsButtonClick.onClick.AddListener(presenter.OnSettingsButtonClick);
            _fireButton.PointerDownEvent.AddListener(presenter.UseCases.WeaponStartContinuosFire);
            _fireButton.PointerUpEvent.AddListener(presenter.UseCases.WeaponStopContinuousFire);
        }

        public void SetCurrentWeaponIcon(Sprite icon)
        {
            _currentWeaponIcon.sprite = icon;
        }

        public void SetSecondWeaponIcon(Sprite icon)
        {
            _secondWeaponIcon.sprite = icon;
        }

        public void SetExtraWeaponIcon(Sprite icon)
        {
            _extraWeaponIcon.sprite = icon;
        }
    }
}