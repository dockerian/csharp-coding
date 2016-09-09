using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace Common.Wpf.Services
{
    public class AsyncAction : VMElement
    {
        #region Constructors

        public AsyncAction(string requestName)
        {
            _asyncActionName = requestName;
            _asyncAddedTime = System.DateTime.Now;
        }
        public AsyncAction(string requestName, Action<object> callbackOnSuccess) : this(requestName)
        {
            CallbackOnSuccess = callbackOnSuccess;
            CheckOnDependency = delegate(object o) { return true; };
        }
        public AsyncAction(string requestName, 
            Action<object> callbackOnSuccess, Action<object> callbackOnFailure) : this(requestName, callbackOnSuccess)
        {
            CallbackOnFailure = callbackOnFailure;
        }
        public AsyncAction(string requestName, Func<object, bool?> checkOnDependency, 
            Action<object> callbackOnSuccess) : this(requestName)
        {
            CallbackOnSuccess = callbackOnSuccess;
            CheckOnDependency = checkOnDependency;
        }
        public AsyncAction(string requestName, Func<object, bool?> checkOnDependency, 
            Action<object> callbackOnSuccess, Action<object> callbackOnFailure) : this(requestName)
        {
            CheckOnDependency = checkOnDependency;
            CallbackOnFailure = callbackOnFailure;
            CallbackOnSuccess = callbackOnSuccess;
        }

        #endregion

        #region Properties

        protected Action<object> CallbackOnSuccess;
        protected Action<object> CallbackOnFailure;
        protected Func<object, bool?> CheckOnDependency;

        public bool? CheckResult;

        private string _asyncActionName = string.Empty;
        public string AsyncActionName
        {
            get { return _asyncActionName; }
            set {
                _asyncActionName = value;
                OnPropertyChanged("AsyncActionName");
            }
        }

        private string _asyncActionText = string.Empty;
        public string AsyncActionText
        {
            get { return _asyncActionText; }
            set {
                _asyncActionText = value;
                OnPropertyChanged("AsyncActionText");
            }
        }

        private DateTime _asyncAddedTime;
        public DateTime AsyncAddedTime
        {
            get { return _asyncAddedTime; }
            set {
                _asyncAddedTime = value;
                OnPropertyChanged("AsyncAddedTime");
            }
        }

        private DateTime _callBeginTime;
        public DateTime CallBeginTime
        {
            get { return _callBeginTime; }
            set {
                _callBeginTime = value;
                OnPropertyChanged("CallBeginTime");
            }
        }

        private DateTime _callEndTime;
        public DateTime CallEndTime
        {
            get { return _callEndTime; }
            set {
                _callEndTime = value;
                OnPropertyChanged("CallEndTime");
            }
        }

        public bool HasDependency
        {
            get { return CheckOnDependency != null; }
        }

        #endregion

        #region Methods

        public void Callback(object parameter)
        {
            CallBeginTime = System.DateTime.Now;

            if (CheckOnDependency != null)
            {
                CheckResult = this.CheckOnDependency(parameter);
            }
            if (CheckResult == null) return;

            if (CheckResult == true)
            {
                if (CallbackOnSuccess != null)
                {
                    CallbackOnSuccess(parameter);
                }
            }
            else // on failure of dependency check
            {
                if (CallbackOnFailure != null)
                {
                    CallbackOnFailure(parameter);
                }
            }
        }
        public void Callback(bool checkResult, object parameter)
        {
            CallBeginTime = System.DateTime.Now;

            if (checkResult)
            {
                if (CallbackOnSuccess != null) CallbackOnSuccess(parameter);
            }
            else // on failure of dependency check
            {
                if (CallbackOnFailure != null) CallbackOnFailure(parameter);
            }
        }
        public void Callback(object dependency, object callback_parameter)
        {
            CallBeginTime = System.DateTime.Now;

            this.CheckDependency(dependency);

            this.Callback(callback_parameter);
        }
        public void Callback(object dependency, object callbackOnSuccess_parameter, object callbackOnFailure_parameter)
        {
            CallBeginTime = System.DateTime.Now;

            this.CheckDependency(dependency);

            if (CheckResult == null) return;

            if (CheckResult == true)
            {
                if (CallbackOnSuccess != null) CallbackOnSuccess(callbackOnSuccess_parameter);
            }
            else // on failure of dependency check
            {
                if (CallbackOnFailure != null) CallbackOnFailure(callbackOnFailure_parameter);
            }
        }

        public bool? CheckDependency(object o)
        {
            if (CheckOnDependency == null)
            {
                return CheckResult = null;
            }
            return this.CheckResult = CheckOnDependency(o);
        }

        #endregion

    }
}