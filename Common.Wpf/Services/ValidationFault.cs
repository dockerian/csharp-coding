using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Wpf.Services
{
    public class ValidatorFault : VMElement
    {
        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set 
            {
                if (_reason != value)
                {
                    _reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }
    }
}
