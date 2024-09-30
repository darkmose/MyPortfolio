using System;
using UnityEngine;

namespace Core.Tools
{
    public class MonoUpdater : MonoBehaviour
    {
        private event Action _actions;
        private bool _isUpdating;
        public static MonoUpdater Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void AddMonoUpdateListener(Action updateListener)
        {
            _actions += updateListener;
            if (_actions != null) 
            {
                _isUpdating = true;
            }
        }

        public void RemoveUpdateListener(Action updateListener) 
        {
            _actions -= updateListener;
            if (_actions == null)
            {
                _isUpdating = false;
            }
        }

        public void RemoveAllListeners()
        {
            _actions = null;
            _isUpdating = false;
        }

        private void FixedUpdate()
        {
            if (_isUpdating)
            {
                _actions.Invoke();
            }
        }
    }
}