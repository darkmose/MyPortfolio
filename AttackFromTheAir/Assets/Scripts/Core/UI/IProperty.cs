using System;

namespace Core.UI
{
    public interface IProperty<T> : IPropertyReadOnly<T>
    {
        void SetValue(T newValue, bool force);
    }

    public interface IPropertyReadOnly<T>
    {
        T Value { get; }
        void RegisterValueChangeListener(System.Action<T> listener);
        void UnregisterValueChangeListener(System.Action<T> listener);
        void RemoveAllListeners();
    }

    public class BoolProperty : IProperty<bool>
    {
        public bool Value { get; private set; }
        public event Action<bool> OnValueChanged;

        public BoolProperty(bool initValue)
        {
            Value = initValue;
        }

        public BoolProperty()
        {
        }

        public void RegisterValueChangeListener(Action<bool> listener)
        {
            OnValueChanged += listener;
        }

        public void RemoveAllListeners()
        {
            OnValueChanged = null;
        }

        public void SetValue(bool newValue, bool force = false)
        {
            if (Value != newValue || force)
            {
                Value = newValue;
                OnValueChanged?.Invoke(newValue);
            }        
        }

        public void UnregisterValueChangeListener(Action<bool> listener)
        {
            OnValueChanged -= listener;
        }
    }

    public class CustomProperty<T> : IProperty<T>
    {
        public T Value { get; private set; }
        public event System.Action<T> OnValueChanged;

        public CustomProperty(T initValue)
        {
            Value = initValue;
        }

        public void RegisterValueChangeListener(Action<T> listener)
        {
            OnValueChanged += listener;
        }

        public void RemoveAllListeners()
        {
            OnValueChanged = null;
        }

        public void SetValue(T newValue, bool force)
        {
            if (Value == null) 
            {
                Value = newValue;
                RaiseOnValueChangedEvent();
                return;
            }
            else if (!Value.Equals(newValue) || force)
            {
                Value = newValue;
                RaiseOnValueChangedEvent();
            }
        }

        public void UnregisterValueChangeListener(Action<T> listener)
        {
            OnValueChanged -= listener;
        }

        private void RaiseOnValueChangedEvent()
        {
            OnValueChanged?.Invoke(Value);
        }
    }

    public class IntProperty : IProperty<int>
    {
        public int Value { get; private set; }

        public event System.Action<int> OnValueChanged;

        public IntProperty(int newValue)
        {
            SetValue(newValue, true);
        }

        public void SetValue(int newValue, bool force)
        {
            if (Value != newValue || force)
            {
                Value = newValue;
                RaiseOnValueChangedEvent();
            }
        }

        public void RemoveAllListeners()
        {
            OnValueChanged = null;
        }

        private void RaiseOnValueChangedEvent()
        {
            OnValueChanged?.Invoke(Value);
        }

        public void RegisterValueChangeListener(Action<int> listener)
        {
            OnValueChanged += listener;
        }

        public void UnregisterValueChangeListener(Action<int> listener)
        {
            OnValueChanged -= listener;
        }
    }

    public class FloatProperty : IProperty<float>
    {
        public float Value { get; private set; }

        public event Action<float> OnValueChanged;

        public void RegisterValueChangeListener(Action<float> listener)
        {
            OnValueChanged += listener;
        }

        public void SetValue(float newValue, bool force)
        {
            if (Value != newValue || force)
            {
                Value = newValue;
                RaiseOnValueChangedEvent();
            }
        }

        public void RemoveAllListeners()
        {
            OnValueChanged = null;
        }

        private void RaiseOnValueChangedEvent()
        {
            OnValueChanged?.Invoke(Value);
        }

        public void UnregisterValueChangeListener(Action<float> listener)
        {
            OnValueChanged -= listener;
        }


    }
}