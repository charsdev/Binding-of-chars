using System;
using UnityEngine;

namespace BindingOfChars
{
    [Serializable]
    public class Property<T>
    {
        [SerializeField] private T _currentValue;
        private string _valueName;

        public Property(string valueName, T value)
        {
            _valueName = valueName;
            _currentValue = value;
        }

        public virtual T Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                OnPropertyChanged(value);
            }
        }

        protected void OnPropertyChanged(T value)
        {
            _currentValue = value;
            EventBus.Instance.TriggerEvent($"UPDATE_{_valueName.ToUpper()}", new PropertyChangeArg<T> { value = _currentValue });
        }
    }
}