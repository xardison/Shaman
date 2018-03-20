using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ReactiveUI;
using Shaman.Common.Extension;

namespace Shaman.Common.Serialize
{
    public sealed class EditContext<T> : INotifyPropertyChanged, IDisposable where T : class
    {
        private T _sourceValue;
        private T _currentValue;

        public EditContext(T sourceValue)
        {
            _sourceValue = sourceValue;

            var copy = _sourceValue.MakeCopy();

            if (_sourceValue is IList)
            {
                Type listType;

                switch (_sourceValue.GetType().Name)
                {
                    case "ReactiveList`1": listType = typeof(ReactiveList<>); break;
                    case "ObservableCollection`1": listType = typeof(ObservableCollection<>); break;
                    default: listType = typeof(IList<>); break;
                }

                copy = (T)((IList)copy).CreateGenericCollection(listType, copy);
            }

            Value = copy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            _sourceValue = null;
            _currentValue = null;
        }

        public void Apply()
        {
            UpdateDestinyFromSource(_currentValue, _sourceValue);
        }
        public void Reset()
        {
            UpdateDestinyFromSource(_sourceValue, _currentValue);
        }

        private void UpdateDestinyFromSource(T source, T destiny)
        {
            if (source is IList sourceList)
            {
                var destinyList = (IList)destiny;
                destinyList.Clear();
                foreach (var item in sourceList)
                {
                    destinyList.Add(item);
                }

                return;
            }

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties().Where(x => x.CanWrite))
            {
                propertyInfo.SetValue(destiny, propertyInfo.GetValue(source));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
