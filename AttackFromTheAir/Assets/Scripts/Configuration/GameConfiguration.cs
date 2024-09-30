using Core.PlayerModule;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Game Config")]
    public class GameConfiguration : SerializedScriptableObject
    {
        public PlayerConfiguration PlayerConfiguration;
        public List<WalletInitConfig> WalletInitConfigs;
    }

    [System.Serializable]
    public sealed class PlayerConfiguration
    {
        public float ControlSensitivity = 1f;
        public float CameraRotationSpeed = 1f;
        public float YawTangageSpeed = 1f;
        public float CameraMoveSpeedMin = 10f;
        public float CameraMoveSpeedMax = 30f;
        public float CameraZoomScaleMin = 1f;
        public float CameraZoomScaleMax = 5f;
        public float CameraZoomFOVMin = 90f;
        public float CameraZoomFOVMax = 40f;
        public float DefaultNormalizedZoomScale = 0.5f;
    }

    [System.Serializable]
    public sealed class WalletInitConfig
    {
        public MoneyType MoneyType;
        public int InitAmount;
    }
}