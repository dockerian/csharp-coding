using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Extensions
{
    static class UIHelper
    {
        public static bool IsInDebugMode
        {
            get { return System.Diagnostics.Debugger.IsAttached; }
        }
        public static bool IsInDesignMode
        {
            get {
                bool _isInDesignMode = false;

#if SILVERLIGHT
                _isInDesignMode = DesignerProperties.IsInDesignTool; 
#else 
                var prop = DesignerProperties.IsInDesignModeProperty; 
                _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(
                    prop, typeof(FrameworkElement)).Metadata.DefaultValue; 
#endif 

                string app = System.Windows.Application.Current.ToString();

                return (
                    _isInDesignMode ||
                    app == "System.Windows.Application" || 
                    app == "Microsoft.Expression.Blend.BlendApplication" ||
                    Process.GetCurrentProcess().IsDesigner()
                    );
            }
        }

        #region RunInBackground

        public static void InvokeAsRequired(this DispatcherObject control, Action func)
        {
            if (control.Dispatcher.Thread != Thread.CurrentThread)
            {
                control.Dispatcher.Invoke(DispatcherPriority.Normal, func);
            }
            else // call the function directly
            {
                func.Invoke();
            }
        }

        private class RunInBackgroundArgument
        {
            public DispatcherObject Control { get; set; }
            public Action BackgroundAction { get; set; }
            public Action UIAction { get; set; }
        }
        private static void RunInBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            RunInBackgroundArgument arg = e.Argument as RunInBackgroundArgument;

            if (arg != null)
            {
                var control = arg.Control;
                var backgroundAction = arg.BackgroundAction;
                var uiAction = arg.UIAction;

                if (backgroundAction != null)
                {
                    try
                    {
                        backgroundAction();
                    }
                    catch
                    { 
                    }
                    e.Result = arg;
                }
            }
        }
        private static void RunInBackground_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            RunInBackgroundArgument arg = e.Result as RunInBackgroundArgument;

            if (arg != null)
            {
                var control = arg.Control;
                var backgroundAction = arg.BackgroundAction;
                var uiAction = arg.UIAction;

                if (uiAction != null && control != null)
                {
                    control.InvokeAsRequired(uiAction);
                }
            }

            var bgWorker = sender as BackgroundWorker;

            if (bgWorker != null)
            {
                bgWorker.DoWork -= RunInBackground_DoWork;
                bgWorker.RunWorkerCompleted -= RunInBackground_RunWorkerComplete;
            }
        }
        public static void RunInBackground(Action backgroundAction, DispatcherObject control, Action uiAction)
        {
            BackgroundWorker bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };

            var arg = new RunInBackgroundArgument()
            {
                Control = control,
                BackgroundAction = backgroundAction,
                UIAction = uiAction
            };

            bgWorker.DoWork += RunInBackground_DoWork;
            bgWorker.RunWorkerCompleted += RunInBackground_RunWorkerComplete;
            bgWorker.RunWorkerAsync(arg);
        }

        #endregion

    }

}
