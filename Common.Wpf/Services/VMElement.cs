using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Common.Wpf.Commands;

namespace Common.Wpf.Services
{    
    public class VMElement : DispatcherObject, INotifyPropertyChanged
    {
        // This keeps a static cache of all property names as immutable PropertyChangedEventArgs reducing the load on the GC.
        private static Dictionary<string, PropertyChangedEventArgs> propertyCache = new Dictionary<string, PropertyChangedEventArgs>();

        private WeakPropertyObserver _weakOnPropertyChanged;

        private RelayCommand _commands;
        private List<string> _suspendedNotifications = new List<string>();
        private bool _suspendNotifications;
        private bool _isValid = true;
        private Validator _validator;
        private string _localizedTypeName;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Conditional("DEBUG")]
        protected void IfInDesigner(Action callback)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                callback();
            }
        }

        public bool IsInDesigner
        {
            get { return DesignerProperties.GetIsInDesignMode(new DependencyObject()); }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged("IsValid");
                }
            }
        }

        public string LocalizedTypename
        {
            get
            {
                return _localizedTypeName;
            }
            set
            {
                if (_localizedTypeName != value)
                {
                    _localizedTypeName = value;
                    OnPropertyChanged("LocalizedTypename");
                }
            }
        }

        public void Validate()
        {
            Validator.Run();
            IsValid = Validator.Valid;
        }

        protected virtual Validator CreateValidator()
        {
            return new Validator(this);
        }

        public Validator Validator
        {
            get
            {
                if (_validator == null)
                {
                    _validator = CreateValidator();
                }

                return _validator;
            }
        }


        public WeakPropertyObserver WeakOnPropertyChanged
        {
            get
            {
                if (_weakOnPropertyChanged == null)
                    _weakOnPropertyChanged = new WeakPropertyObserver(this);

                return _weakOnPropertyChanged;
            }
        }        

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (_suspendNotifications)
            {
                _suspendedNotifications.Add(propertyName);
            }
            else
            {
                verifyPropertyName(propertyName);

                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    PropertyChangedEventArgs propertyEventArgs;
                    if (! (propertyCache.TryGetValue(propertyName, out propertyEventArgs)))
                    {
                        propertyEventArgs = new PropertyChangedEventArgs(propertyName);
                        propertyCache.Add(propertyName, propertyEventArgs);
                    }

                    handler(this, propertyEventArgs);
                }
            }
        }

        public void OnPropertyChanged(IEnumerable<string> propertyNames)
        {
            if (propertyNames == null)
                throw new ArgumentNullException("propertyNames");

            foreach (string name in propertyNames)
                OnPropertyChanged(name);
        }

        public void SuspendNotifications()
        {
            _suspendNotifications = true;
        }

        public void ResumeNotifications()
        {
            _suspendNotifications = false;

            OnPropertyChanged(_suspendedNotifications);
            _suspendedNotifications.Clear();
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void verifyPropertyName(string propertyName)
        {
            // If you raise PropertyChanged and do not specify a property name, all properties on the
            // ViewElement are considered to be changed.
            if (String.IsNullOrEmpty(propertyName))
                return;

            // Verify that the property name matches a real, public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new InvalidOperationException("Invalid property name: " + propertyName);
            }
        }

        protected virtual RelayCommand CreateCommands()
        {
            return new RelayCommand(() => { });
        }

        public RelayCommand Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = CreateCommands();
                }

                return _commands;
            }
        }        
    }

    public class WeakPropertyObserver
    {
        private VMElement _sourceElement;

        private List<WeakReference> _modelSubscribers = new List<WeakReference>();
        private Dictionary<object, PropertyChangedEventHandler> _handlers = new Dictionary<object, PropertyChangedEventHandler>();


        public WeakPropertyObserver(VMElement sourceElement)
        {
            _sourceElement = sourceElement;
        }

        public void Attach(VMElement eventSubscriber, PropertyChangedEventHandler eventHandler)
        {
            _modelSubscribers.Add(new WeakReference(eventSubscriber));

            eventSubscriber.WeakOnPropertyChanged.AttachEvent(_sourceElement, eventHandler);
        }

        public void Detach(VMElement eventSubscriber)
        {
            for (int i = _modelSubscribers.Count - 1; i >= 0; i--)
            {
                if ((_modelSubscribers[i].Target == eventSubscriber) || (_modelSubscribers[i].Target == null))
                {
                    _modelSubscribers.RemoveAt(i);
                }
            }

            eventSubscriber.WeakOnPropertyChanged.DetachEvent(_sourceElement);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            foreach (WeakReference subscriberRef in _modelSubscribers)
            {
                var subscriber = subscriberRef.Target as VMElement;
                if (subscriber != null)
                {
                    subscriber.WeakOnPropertyChanged.HandleWeakEvent(_sourceElement, args);                    
                }
                else
                {
                    _modelSubscribers.Remove(subscriberRef);
                }
            }
        }

        internal void AttachEvent(VMElement eventSource, PropertyChangedEventHandler eventHandler)
        {
            _handlers[eventSource] = eventHandler;
        }

        internal void DetachEvent(VMElement eventSource)
        {
            _handlers.Remove(eventSource);
        }       

        internal void HandleWeakEvent(VMElement eventSource, PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler eventHandler;
            if (_handlers.TryGetValue(eventSource, out eventHandler))
            {
                eventHandler(eventSource, args);
            }
        }
    }
}
