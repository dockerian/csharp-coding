using System;
using System.Timers;
using Microsoft.Win32;
using Common.Extensions;


namespace Common.Wpf.Services
{
    public class TimeSourceDriver
    {
        #region DateTimeChangeArgs
        public class DateTimeChangedArgs : EventArgs
        {
            private DateTimeOffset _dateTimeValue;

            public DateTimeChangedArgs(DateTimeOffset dateTimeValue)
            {
                _dateTimeValue = dateTimeValue;
            }

            public DateTimeOffset DateTimeValue { get { return _dateTimeValue; } }
        }
        #endregion

        private Timer _timer = new Timer();

        public TimeSourceDriver(double interval)
        {
            _timer.Interval = interval;
        }

        public event EventHandler<DateTimeChangedArgs> DateTimeChanged;

        #region Functions

        private void systemEvents_TimeChanged(object sender, EventArgs e)
        {
            TimeZoneInfo.ClearCachedData();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTimeOffset now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.Local);
            DateTimeChanged.Raise(this, new DateTimeChangedArgs(now));
        }

        #endregion

        public void Shutdown()
        {
            SystemEvents.TimeChanged -= new EventHandler(systemEvents_TimeChanged);

            _timer.Stop();
            _timer.Elapsed -= new ElapsedEventHandler(timer_Elapsed);
        }

        public void Startup()
        {
            SystemEvents.TimeChanged += new EventHandler(systemEvents_TimeChanged);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

    }
}