using Core.DISimple;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        ServiceLocator.Register<DiContainer>(Container);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<DiContainer>();
    }
}