using Core.Buildings;
using Core.GameLogic;
using Core.Tools;
using Core.Units;
using Core.Weapon;
using UnityEngine;
using Zenject;

public class ToolsInstaller : MonoInstaller
{
    [SerializeField] private AsyncInstantiationService _asyncInstantiationService;
    [SerializeField] private DroneContainerView _droneContainerView;
    [SerializeField] private MonoUpdater _monoUpdater;

    public override void InstallBindings()
    {
        Container.Bind<AsyncInstantiationService>().FromInstance(_asyncInstantiationService).AsSingle();
        Container.Bind<CameraVectorConversionService>().AsSingle().NonLazy();
        Container.Bind<UnitCreateSystem>().AsSingle();
        Container.Bind<UnitSpawnerCreateSystem>().AsSingle();
        Container.Bind<WeaponCreateSystem>().AsSingle();
        Container.Bind<BuildingCreateSystem>().AsSingle();
        Container.Bind<CameraZoomService>().AsSingle();
        Container.Bind<ILevelUnitsCollector>().To<LevelUnitsCollector>().AsSingle();
        Container.Bind<ILevelProgressionHelper>().To<LevelProgressionHelper>().AsSingle();
        Container.Bind<ILevelBuildingsCollector>().To<LevelBuildingsCollector>().AsSingle();
        Container.Bind<MonoUpdater>().FromInstance(_monoUpdater).AsSingle().NonLazy();
        Container.Bind<PlayerWeaponUpgradeSystem>().AsSingle();
        Container.Bind<DroneContainerView>().FromInstance(_droneContainerView).AsSingle();
        Container.Bind<IDroneSelectService>().To<DroneSelectService>().AsSingle();
        Container.Bind<UnitStateMachinesHandler>().AsSingle().NonLazy();
    }
}