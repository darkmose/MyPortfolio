using System.Collections.Generic;

namespace Core.Utilities
{

    public class SimpleEvent
    {
        private List<System.Action> _eventListeners = new List<System.Action>();
        private List<System.Action> _eventListenersSafe = new List<System.Action>();
        private List<System.Action> _eventListenersWithClearing = new List<System.Action>();

        public void AddListener(System.Action listener, bool removeAfterNotify = false)
        {
            if (!_eventListeners.Contains(listener))
            {
                if (removeAfterNotify)
                {
                    _eventListenersWithClearing.Add(listener);
                }
                else
                {
                    _eventListeners.Add(listener);
                }
            }
        }

        public void RemoveListener(System.Action listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _eventListeners.Clear();
        }
        
        public void Notify()
        {
            _eventListenersSafe.Clear();
            _eventListenersSafe.AddRange(_eventListeners);
            _eventListenersSafe.AddRange(_eventListenersWithClearing);

            foreach (var action in _eventListenersSafe)
            {
                action?.Invoke();
            }

            _eventListenersWithClearing.Clear();
        }
    }

    public class SimpleEvent<T>
    {
        private List<System.Action<T>> _eventListeners = new List<System.Action<T>>();
        private List<System.Action<T>> _eventListenersSafe = new List<System.Action<T>>();
        private List<System.Action<T>> _eventListenersWithClearing = new List<System.Action<T>>();
        private T _value;
        public void AddListener(System.Action<T> listener, bool removeAfterNotify = false)
        {
            if (removeAfterNotify)
            {
                _eventListenersWithClearing.Add(listener);
            }
            else
            {
                _eventListeners.Add(listener);
            }
        }

        public void SetValue(T value)
        {
            _value = value;
        }

        public void RemoveListener(System.Action<T> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _eventListeners.Clear();
        }

        public void Notify(T value)
        {
            _eventListenersSafe.Clear();
            _eventListenersSafe.AddRange(_eventListeners);
            _eventListenersSafe.AddRange(_eventListenersWithClearing);

            foreach (var action in _eventListenersSafe)
            {
                action?.Invoke(value);
            }

            _eventListenersWithClearing.Clear();
        }
    }

    public class SimpleEvent<T0, T1>
    {
        private List<System.Action<T0, T1>> _eventListeners = new List<System.Action<T0, T1>>();
        private List<System.Action<T0, T1>> _eventListenersSafe = new List<System.Action<T0, T1>>();
        private T0 _value0;
        private T1 _value1;
        public void AddListener(System.Action<T0, T1> listener)
        {
            _eventListeners.Add(listener);
        }

        public void SetValue(T0 value0, T1 value1)
        {
            _value0 = value0;
            _value1 = value1;
        }

        public void RemoveListener(System.Action<T0, T1> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _eventListeners.Clear();
        }

        public void Notify(T0 value0, T1 value1)
        {
            _eventListenersSafe.Clear();
            _eventListenersSafe.AddRange(_eventListeners);

            foreach (var action in _eventListenersSafe)
            {
                action?.Invoke(value0, value1);
            }
        }
    }    
    
    public class SimpleEvent<T0, T1, T2>
    {
        private List<System.Action<T0, T1, T2>> _eventListeners = new List<System.Action<T0, T1, T2>>();
        private List<System.Action<T0, T1, T2>> _eventListenersSafe = new List<System.Action<T0, T1, T2>>();
        private T0 _value0;
        private T1 _value1;
        private T2 _value2;

        public void AddListener(System.Action<T0, T1, T2> listener)
        {
            _eventListeners.Add(listener);
        }

        public void SetValue(T0 value0, T1 value1, T2 value2)
        {
            _value0 = value0;
            _value1 = value1;
            _value2 = value2;
        }

        public void RemoveListener(System.Action<T0, T1, T2> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _eventListeners.Clear();
        }

        public void Notify(T0 value0, T1 value1, T2 value2)
        {
            _eventListenersSafe.Clear();
            _eventListenersSafe.AddRange(_eventListeners);

            foreach (var action in _eventListenersSafe)
            {
                action?.Invoke(value0, value1, value2);
            }
        }
    }
}