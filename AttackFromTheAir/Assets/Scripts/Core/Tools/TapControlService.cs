using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Tools
{
    public class TapControlService : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        private static TapControlService _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void LockTap()
        {
            _instance._eventSystem.enabled = false;
        }

        public static void UnlockTap() 
        {
            _instance._eventSystem.enabled = true;
        }

        public static void LockTapForDelay(float delay = 0.15f)
        {
            Tools.Timer.SetTimer(delay, () =>
            {
                UnlockTap();
            });
        }
    }
}