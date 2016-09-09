using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library
{
    public class StackOperationException : InvalidOperationException
    {
        public StackOperationException(string message) : base(message)
        {

        }
    }

    public class StackComparable<T> : StackGeneric<T> where T : IComparable
    {
        #region StackComparable :: Fields

        protected T _min = default(T);
        protected List<T> _stackMinList = new List<T>();

        #endregion

        #region StackComparable :: Constructor

        public StackComparable() : base()
        {
            
        }

        #endregion

        #region StackComparable :: Properties

        public T Minimum
        {
            get { return _min; }
        }

        #endregion

        #region StackComparable :: Functions

        #endregion

        #region StackComparable :: Methods

        public T GetMinimum()
        {
            T minimum = default(T);

            for(int i = 0; i < _stack.Count; i++)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;

                if (minimum == null)
                {
                    minimum = _stack[i];
                }
                else if (!nullable || _stack[i] != null)
                {
                    if (_stack[i].CompareTo(minimum) < 0)
                    {
                        minimum = _stack[i];
                    }
                }
            }

            return minimum;
        }

        public override T Pop()
        {
            T nextMinimum = default(T);

            if (Length > 0)
            {
                if (Length > 1)
                {
                    nextMinimum = _stackMinList[1];
                }
                _stackMinList.RemoveAt(0);
            }
            _min = nextMinimum;

            return base.Pop();
        }

        public override void Push(T data)
        {
            base.Push(data);

            Type t1 = typeof(T);
            Type t2 = Nullable.GetUnderlyingType(t1);
            bool nullable = !t1.IsValueType || t2 != null;

            if (nullable && _min == null)
            {
                _min = data;
            }
            else if (data.CompareTo(_min) < 0)
            {
                _min = data;
            }
            _stackMinList.Insert(0, _min);
        }

        #endregion

    }

    public class StackGeneric<T>
    {
        #region Stack :: Fields

        protected List<T> _stack = new List<T>();

        #endregion

        #region Stack :: Constructor

        public StackGeneric()
        {

        }

        #endregion

        #region Stack :: Properties

        public bool IsEmpty
        {
            get { return _stack.Count <= 0; }
        }

        public int Length
        {
            get { return _stack.Count; }
        }

        public T Bottom
        {
            get { return PeekBottom(); }
        }

        public T Top
        {
            get { return Peek(); }
        }

        #endregion

        #region Stack :: Functions

        #endregion

        #region Stack :: Methods

        public virtual T Peek()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new StackOperationException("Cannot peek an empty stack");
                }
                else // object type nullable
                { 
                    return default(T);
                }
            }
            return _stack[0];
        }

        public virtual T PeekBottom()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new StackOperationException("Cannot peek an empty stack");
                }
                else // object type nullable
                { 
                    return default(T);
                }
            }
            return _stack[_stack.Count - 1];
        }

        public virtual T Pop()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new StackOperationException("Cannot pop an empty stack");
                }
                else // object type nullable
                {
                    return default(T);
                }
            }

            T data = _stack[0];
            _stack.RemoveAt(0);

            return data;
        }

        public virtual void Push(T data)
        {
            _stack.Insert(0, data);
        }

        #endregion


    }

}
