using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI.Wrapper
{
    public class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errorsBy_propertyName = new Dictionary<string, List<string>>();

        protected Dictionary<string, List<string>> Errors { get { return _errorsBy_propertyName; } }


        public bool HasErrors => _errorsBy_propertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string _propertyName)
        {
            return _errorsBy_propertyName.ContainsKey(_propertyName)
                   ? _errorsBy_propertyName[_propertyName] : null;
        }

        protected virtual void OnErrorChanged(string _propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(_propertyName));
            base.OnPropertyChanged(nameof(HasErrors));

        }


        protected void AddError(string _propertyName, string error)
        {
            if (!_errorsBy_propertyName.ContainsKey(_propertyName))
            {
                _errorsBy_propertyName[_propertyName] = new List<string>();
            }

            if (!_errorsBy_propertyName[_propertyName].Contains(error))
            {
                _errorsBy_propertyName[_propertyName].Add(error);
                OnErrorChanged(_propertyName);
            }


        }

        protected void ClearErrors(string _propertyName)
        {
            if (_errorsBy_propertyName.ContainsKey(_propertyName))
            {
                _errorsBy_propertyName.Remove(_propertyName);
                OnErrorChanged(_propertyName);
            }
        }

        protected void ClearErrors()
        {
            foreach (var _propertyName in Errors.Keys.ToList())
            {
                Errors.Remove(_propertyName);
                OnErrorChanged(_propertyName);
            }
        }

    }
}
