using Configuration;
using Core.Resourses;
using UnityEngine;
using Zenject;

public class ScriptablesInstaller : MonoInstaller
{
    [SerializeField] private GameConfiguration _gameConfiguration;
    [SerializeField] private PlayerWeaponSpriteProvider _weaponSpriteProvider;
    [SerializeField] private ResourceHolder _resourceHolder;
    [SerializeField] private DroneProvider _droneProvider;
    [SerializeField] private PlayerWeaponsHolder _playerWeaponsHolder;

    public override void InstallBindings()
    {
        Container.Bind<GameConfiguration>().FromInstance(_gameConfiguration).AsSingle();
        Container.Bind<PlayerWeaponSpriteProvider>().FromInstance(_weaponSpriteProvider).AsSingle();
        Container.Bind<ResourceHolder>().FromInstance(_resourceHolder).AsSingle();
        Container.Bind<DroneProvider>().FromInstance(_droneProvider).AsSingle();
        Container.Bind<PlayerWeaponsHolder>().FromInstance(_playerWeaponsHolder).AsSingle();
    }
}