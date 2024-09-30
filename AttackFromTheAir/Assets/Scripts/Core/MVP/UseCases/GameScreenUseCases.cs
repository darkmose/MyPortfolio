using Core.GameLogic;
using Core.States;
using Core.Tools;


namespace Core.MVP
{
    public class GameScreenUseCases
    {
        private CameraFXController _cameraFXController;
        private IStateMachine<GameplayStates> _states;
        private PlayerWeaponContainer _weaponContainer;

        public GameScreenUseCases(PlayerData playerData, CameraFXController cameraFXController, IStateMachine<GameplayStates> states)
        {
            _weaponContainer = playerData.WeaponContainer;
            _cameraFXController = cameraFXController;
            _states = states;
        }

        public void MainWeaponFireButtonHandler()
        {
            _weaponContainer.Fire();
        }

        public void ExtraWeaponFireButtonHandler()
        {
            _weaponContainer.FireExtraWeapon();
        }
        public void SwitchWeaponButtonHandler()
        {
            _weaponContainer.SwitchWeapon();
        }

        public void WeaponStartContinuosFire()
        {
            _weaponContainer.StartContinuousFire();
        }

        public void WeaponStopContinuousFire()
        {
            _weaponContainer.StopContinousFire();
        }

        public void OnSettingsButtonClick()
        {
            _states.SwitchToState(GameplayStates.Unload);
        }

        public void SwitchNightVision()
        {
            var enabled = _cameraFXController.IsNightVisionEnabled;
            _cameraFXController.EnableNightVision(!enabled);
        }
    }
}