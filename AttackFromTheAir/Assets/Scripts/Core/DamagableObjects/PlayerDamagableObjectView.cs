using Core.Tools;
using Zenject;

namespace Core.GameLogic
{
    public class PlayerDamagableObjectView : BaseDamagableObjectView
    {
        private CameraFXController _cameraFXController;

        private void Awake()
        {
            var di = DISimple.ServiceLocator.Resolve<DiContainer>();
            _cameraFXController = di.Resolve<CameraFXController>();
        }

        public override void OnObjectDamaged(IDamagableObject damagableObject, int damage)
        {
            _cameraFXController.AnimatePlayerDamaged();
        }
    }
}