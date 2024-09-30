using Cinemachine;
using Configuration;
using Core.MVP;
using UnityEngine;

namespace Core.Tools
{
    public class CameraZoomService
    {
        private IGameScreenPresenter _gameScreenPresenter;
        private GameConfiguration _gameConfiguration;
        private CinemachineBrain _cinemachineBrain;
        private CameraFXController _cameraFXController;

        public CameraZoomService(IGameScreenPresenter gameScreenPresenter, GameConfiguration gameConfiguration, CameraFXController cameraFXController)
        {
            _gameScreenPresenter = gameScreenPresenter;
            _gameConfiguration = gameConfiguration;
            _gameScreenPresenter.ZoomNormalizedValueChangedEvent.AddListener(OnZoomNormalizedChanged);
            _gameScreenPresenter.ZoomStartChangingEvent.AddListener(OnStartChangingZoom);
            _cameraFXController = cameraFXController;
        }

        private void OnStartChangingZoom()
        {
            _cameraFXController.AnimateMozaicGlitch();
        }

        public void Init()
        {
            if (_cinemachineBrain == null)
            {
                _cinemachineBrain = Cinemachine.CinemachineCore.Instance.GetActiveBrain(0);
            }
        }

        private void OnZoomNormalizedChanged(float zoomValue)
        {
            var fovMin = _gameConfiguration.PlayerConfiguration.CameraZoomFOVMin;
            var fovMax = _gameConfiguration.PlayerConfiguration.CameraZoomFOVMax;

            var fovValue = Mathf.Lerp(fovMin, fovMax, zoomValue);
            var cam = _cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
            cam.m_Lens.FieldOfView = fovValue;
        }
    }
}