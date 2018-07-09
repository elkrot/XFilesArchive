using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace XFilesArchive.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase,
    IValidatableTrackingObject, IValidatableObject
    {
        private Dictionary<string, object> _originalValues;

        private List<IValidatableTrackingObject> _trackingObjects;

        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IValidatableTrackingObject>();
           InitializeComplexProperties(model);
            InitializeCollectionProperties(model);
            Validate();
        }
        public T Model { get; }
        protected virtual TValue GetValue<TValue>([CallerMemberName]string _propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(_propertyName).GetValue(Model); ;
        }
        //protected TValue GetValue<TValue>([CallerMemberName] string _propertyName = null)
        //{
        //    var propertyInfo = Model.GetType().GetProperty(_propertyName);
        //    return (TValue)propertyInfo.GetValue(Model);
        //}
        //protected virtual void SetValue<TValue>(TValue value, [CallerMemberName]string _propertyName = null)
        //{
        //    typeof(T).GetProperty(_propertyName).SetValue(Model, value);
        //    OnPropertyChanged(_propertyName);
        //    ValidatePropertyInternal(_propertyName);
        //}


        protected void SetValue<TValue>(TValue newValue,
         [CallerMemberName] string _propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(_propertyName);
            var currentValue = propertyInfo.GetValue(Model);
            if (!Equals(currentValue, newValue))
            {
                UpdateOriginalValue(currentValue, newValue, _propertyName);
                propertyInfo.SetValue(Model, newValue);
                Validate();
                OnPropertyChanged(_propertyName);
                OnPropertyChanged(_propertyName + "IsChanged");
                ValidatePropertyInternal(_propertyName);
            }
        }




        private void ValidatePropertyInternal(string _propertyName)
        {
            ClearErrors(_propertyName);
            var errors = ValidateProperty(_propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(_propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string _propertyName)
        {
            return null;
        }



        /*--------------------------------*/



        protected virtual void InitializeComplexProperties(T model)
        {
        }

        protected virtual void InitializeCollectionProperties(T model)
        {
        }



        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged);

        public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

        public void AcceptChanges()
        {
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.AcceptChanges();
            }
            OnPropertyChanged("");
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);
            }
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.RejectChanges();
            }
            Validate();
            OnPropertyChanged("");
        }

       

        protected TValue GetOriginalValue<TValue>(string _propertyName)
        {
            return _originalValues.ContainsKey(_propertyName)
              ? (TValue)_originalValues[_propertyName]
              : GetValue<TValue>(_propertyName);
        }

        protected bool GetIsChanged(string _propertyName)
        {
            return _originalValues.ContainsKey(_propertyName);
        }

       

        private void Validate()
        {
            ClearErrors();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this,null,null);
            try
            {
                Validator.TryValidateObject(this, context, results, true);
            }
            catch (Exception ex)
            {

                throw;
            }
           

            if (results.Any())
            {
                var _propertyNames = results.SelectMany(r => r.MemberNames).Distinct().ToList();

                foreach (var _propertyName in _propertyNames)
                {
                    Errors[_propertyName] = results
                      .Where(r => r.MemberNames.Contains(_propertyName))
                      .Select(r => r.ErrorMessage)
                      .Distinct()
                      .ToList();
                    OnErrorChanged(_propertyName);
                }
            }
            OnPropertyChanged(nameof(IsValid));
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string _propertyName)
        {
            if (!_originalValues.ContainsKey(_propertyName))
            {
                _originalValues.Add(_propertyName, currentValue);
                OnPropertyChanged("IsChanged");
            }
            else
            {
                if (Equals(_originalValues[_propertyName], newValue))
                {
                    _originalValues.Remove(_propertyName);
                    OnPropertyChanged("IsChanged");
                }
            }
        }

        protected void RegisterCollection<TWrapper, TModel>(
         ChangeTrackingCollection<TWrapper> wrapperCollection,
         List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                modelCollection.Clear();
                modelCollection.AddRange(wrapperCollection.Select(w => w.Model));
                Validate();
            };
            RegisterTrackingObject(wrapperCollection);
        }

        protected void RegisterComplex<TModel>(ModelWrapper<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject(IValidatableTrackingObject trackingObject)
        {
            if (!_trackingObjects.Contains(trackingObject))
            {
                _trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += TrackingObjectPropertyChanged;
            }
        }

        private void TrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged))
            {
                OnPropertyChanged(nameof(IsChanged));
            }
            else if (e.PropertyName == nameof(IsValid))
            {
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    yield break;
        //}
    }


}
