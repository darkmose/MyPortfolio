using Core.GameLogic;
using Core.Level;
using Zenject;

public class LevelServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ILevelController>().To<LevelController>().AsSingle();
        Container.Bind<GameEventObserversFactory>().AsSingle().NonLazy();
        Container.Bind<GameEventGenerator>().AsSingle();
        Container.Bind<LevelSelector>().AsSingle();
    }
}