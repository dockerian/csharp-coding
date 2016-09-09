using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Wpf.Services
{
    public class Validator : VMElement
    {
        private VMElement _e;
        List<ValidatorFault> _faults = new List<ValidatorFault>();
        bool _valid = true;

        public Validator(VMElement elem) 
        {
            _e = elem;
        }

        public VMElement Element 
        { 
            get { return _e; }
        }

        public bool Valid
        {
            get { return _valid; }
            set 
            {
                if (_valid != value)
                {
                    _valid = value;
                    OnPropertyChanged("Valid");
                }
            }
        }

        public List<ValidatorFault> Faults { get { return _faults; } }
        public virtual void Run(){}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ValidatorFault fault in _faults)
            {
                sb.AppendFormat("- {0}\n", fault.Reason);
            }
            return sb.ToString();
        }
    }
}
