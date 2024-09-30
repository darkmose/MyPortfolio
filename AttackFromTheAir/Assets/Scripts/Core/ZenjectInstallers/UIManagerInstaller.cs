using Core.MVP;
using UnityEngine;
using Zenject;

public class UIManagerInstaller : MonoInstaller
{
    [SerializeField] private UIManagerAdvanced _uIManager;

    public override void InstallBindings()
    {
        Container.Bind<GameScreenModel>().AsSingle();
        Container.Bind<GameScreenUseCases>().AsSingle();
        Container.Bind<GameScreenProxyView>().AsSingle();
        Container.Bind<IGameScreenPresenter>().To<GameScreenPresenter>().AsSingle();
        
        Container.Bind<LobbyScreenModel>().AsSingle();
        Container.Bind<LobbyScreenUseCases>().AsSingle();
        Container.Bind<LobbyScreenProxyView>().AsSingle();
        Container.Bind<ILobbyScreenPresenter>().To<LobbyScreenPresenter>().AsSingle();

        Container.Bind<ControlPanelScreenProxyView>().AsSingle();
        Container.Bind<IControlPanelScreenPresenter>().To<ControlPanelScreenPresenter>().AsSingle();

        Container.Bind<IWinScreenPresenter>().To<WinScreenPresenter>().AsSingle();
        Container.Bind<WinScreenProxyView>().AsSingle();

        Container.Bind<ILoseScreenPresenter>().To<LoseScreenPresenter>().AsSingle();
        Container.Bind<LoseScreenProxyView>().AsSingle();

        Container.Bind<ILoadingScreenPresenter>().To<LoadingScreenPresenter>().AsSingle();
        Container.Bind<LoadingScreenModel>().AsSingle();
        Container.Bind<LoadingScreenProxyView>().AsSingle();

        Container.Bind<IUIManager>().FromInstance(_uIManager).AsSingle();
    }
}