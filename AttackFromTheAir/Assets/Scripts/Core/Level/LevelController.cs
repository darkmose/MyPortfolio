using UnityEngine;
using Core.Resourses;
using Core.PlayerModule;
using Core.GameLogic;
using Core.Tools;
using Core.Utilities;
using Core.Units;
using Core.Buildings;
using Core.MVP;

namespace Core.Level
{
    public interface ILevelController
    {
        SimpleEvent LevelLoadedEvent { get; }
        PlayerData PlayerData { get; }
        LevelData LevelData { get; }
        LevelDescriptor LevelDescriptor { get; }
        ILevelUnitsCollector UnitsCollector { get; }
        ILevelBuildingsCollector BuildingsCollector { get; }
        void UnloadLevel();
        void PrepareNextLevel();
    }

    public class LevelController : ILevelController
    {
        private LevelData _levelData;
        private ResourceHolder _resourceHolder;
        private ILevelProgression _levelProgression;
        private ILevelUnitsCollector _levelUnitsCollector;
        private ILevelProgressionHelper _levelProgressionHelper;
        private ILevelBuildingsCollector _levelBuildingsCollector;
        private IGameScreenPresenter _gameScreenPresenter;
        private BuildingCreateSystem _buildingCreateSystem;
        private PlayerData _playerdata;
        private LevelDescriptor _levelDescriptor;
        private AsyncInstantiationService _asyncInstantiationService;
        private PlayerWeaponsHolder _playerWeaponsHolder;
        private DroneProvider _droneProvider;
        private SimpleEvent _levelLoadedEvent = new SimpleEvent();
        private PlayerWeaponUpgradeSystem _playerWeaponUpgradeSystem;
        private readonly UnitCreateSystem _unitCreateSystem;
        private LevelSelector _levelSelector;
        public LevelData LevelData => _levelData;
        public PlayerData PlayerData => _playerdata;
        public LevelDescriptor LevelDescriptor => _levelDescriptor;
        public SimpleEvent LevelLoadedEvent => _levelLoadedEvent;
        public ILevelUnitsCollector UnitsCollector => _levelUnitsCollector;
        public ILevelBuildingsCollector BuildingsCollector => _levelBuildingsCollector;

        public LevelController(ResourceHolder resourceHolder, ILevelProgression levelProgression,
        AsyncInstantiationService asyncInstantiationService, PlayerData playerdata, UnitCreateSystem unitCreateSystem,
        ILevelUnitsCollector levelUnitsCollector, ILevelProgressionHelper levelProgressionHelper, BuildingCreateSystem buildingCreateSystem,
        ILevelBuildingsCollector levelBuildingsCollector, DroneProvider droneProvider, PlayerWeaponsHolder playerWeaponsHolder, 
        IGameScreenPresenter gameScreenPresenter, LevelSelector levelSelector, PlayerWeaponUpgradeSystem playerWeaponUpgradeSystem)
        {
            _resourceHolder = resourceHolder;
            _levelProgression = levelProgression;
            _asyncInstantiationService = asyncInstantiationService;
            _playerdata = playerdata;
            _unitCreateSystem = unitCreateSystem;
            _levelUnitsCollector = levelUnitsCollector;
            _levelProgressionHelper = levelProgressionHelper;
            _buildingCreateSystem = buildingCreateSystem;
            _levelBuildingsCollector = levelBuildingsCollector;
            _droneProvider = droneProvider;
            _playerWeaponsHolder = playerWeaponsHolder;
            _gameScreenPresenter = gameScreenPresenter;
            _levelSelector = levelSelector;
            _playerWeaponUpgradeSystem = playerWeaponUpgradeSystem;
        }

        private void ClearLevel() 
        {
            if (!System.Object.ReferenceEquals(_levelData, null))
            {
                _levelProgressionHelper.ClearGoals();
                _levelUnitsCollector.ClearUnits();
                _levelBuildingsCollector.DestroyBuildings();
                _playerdata.WeaponContainer.Dispose();
                GameObject.Destroy(_levelData.gameObject);
                _levelData = null;
            }
        }

        private void PrepareLevel()
        {
            if (_resourceHolder.Levels.Count > 0)
            {
                _levelDescriptor = _resourceHolder.GetLevelRepeatly(_levelSelector.CurrentLevel.Value);
                _levelProgressionHelper.InitGoals(_levelDescriptor.GameModeSettings);

                //_asyncInstantiationService.Instantiate(_levelDescriptor.LevelData, Vector3.zero, Quaternion.identity, null, (LevelData levelData) =>
                //{
                //    _levelData = levelData;
                //    PrepareUnits();
                //    PreparePlayer();
                //    //PrepareBuildings();
                //    _levelLoadedEvent.Notify();
                //});
                //TODO MAKE ADDRESSABLE LOADING

                _levelData = GameObject.Instantiate<LevelData>(_levelDescriptor.LevelData);
                PrepareUnits();
                PrepareBuildings();
                PreparePlayer();
                _levelLoadedEvent.Notify();
            }
        }

        private void PrepareBuildings()
        {
            _buildingCreateSystem.GetModelsForViews(_levelData.BuildingViewsDescriptors);
        }

        private void PrepareUnits()
        {
            _unitCreateSystem.CreateUnits(_levelData.UnitSpawnPointsDescriptors);
        }

        private void PreparePlayer()
        {
            _playerdata.transform.position = _levelData.PlayerStartPoint.position;
            _playerdata.transform.rotation = _levelData.PlayerStartPoint.rotation;

            var drone = _droneProvider.ProvideByType(PlayerDroneType.SmallDrone);
            var mainWeapon = _playerWeaponsHolder.GetWeapon(drone.MainWeapon);
            var secondWeapon = _playerWeaponsHolder.GetWeapon(drone.SecondWeapon);
            var extraWeapon = _playerWeaponsHolder.GetExtraWeapon(PlayerExtraWeaponType.AirStrike);

            var mainWeaponInstance = GameObject.Instantiate<PlayerWeapon>(mainWeapon, _playerdata.WeaponContainer.Container);
            var secondWeaponInstance = GameObject.Instantiate<PlayerWeapon>(secondWeapon, _playerdata.WeaponContainer.Container);
            var extraWeaponInstance = GameObject.Instantiate<PlayerExtraWeapon>(extraWeapon, _playerdata.WeaponContainer.Container);

            _playerWeaponUpgradeSystem.InitWeaponDecorator(mainWeaponInstance);
            _playerWeaponUpgradeSystem.InitWeaponDecorator(secondWeaponInstance);
            _playerWeaponUpgradeSystem.InitExtraWeaponDecorator(extraWeaponInstance);

            mainWeaponInstance.InitWeaponProjectilePool();
            secondWeaponInstance.InitWeaponProjectilePool();
            extraWeaponInstance.InitWeaponProjectilePool();

            _playerdata.WeaponContainer.InitMainWeapon(mainWeaponInstance);
            _playerdata.WeaponContainer.InitSecondWeapon(secondWeaponInstance);
            _playerdata.WeaponContainer.InitExtraWeapon(extraWeaponInstance);

            _playerdata.WeaponContainer.InitContainer();
            _playerdata.CameraController.InitDroneConfiguration(drone.DroneCameraConfiguration);
            _gameScreenPresenter.Init();
            var targetingService = _gameScreenPresenter.ProxyView.View.TargetingService;

            var units = _levelUnitsCollector.GetAllUnits();
            var buildings = _levelBuildingsCollector.GetAllBuildings();
            targetingService.InitTargets(units, buildings);
        }

        public void PrepareNextLevel()
        {
            PrepareLevel();
        }

        public void UnloadLevel()
        {
            ClearLevel();
        }
    }
}