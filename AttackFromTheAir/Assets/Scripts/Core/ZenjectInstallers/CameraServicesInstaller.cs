using UnityEngine;
using Zenject;
using Core.Tools;

public class CameraServicesInstaller : MonoInstaller
{
    [SerializeField] private CameraFXController _cameraFXController;

    public override void InstallBindings()
    {
        Container.Bind<CameraFXController>().FromInstance(_cameraFXController).AsSingle();
    }
}