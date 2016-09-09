using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library
{
    public class DoubleStackQueue<T>
    {
        #region Fields
        private Stack<T> _stack1 = new Stack<T>();
        private Stack<T> _stack2 = new Stack<T>();
        private T _peekLast;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public bool IsEmpty
        {
            get { return Length <= 0; }
        }

        public T First
        {
            get { return Peek(); }
        }

        public T Last
        {
            get { return _peekLast = PeekLast(); }
        }

        public int Length
        {
            get { return _stack1.Count + _stack2.Count; }
        }

        #endregion

        #region Functions

        private void Shift()
        {
            if (_stack2.Count == 0)
            {
                while(_stack1.Count != 0)
                {
                    T data = _stack1.Pop();
                    _stack2.Push(data);
                }
            }
        }

        #endregion

        #region Methods

        public T Dequeue()
        {
            Shift();
            return _stack2.Pop();
        }

        public void Enqueue(T data)
        {
            _stack1.Push(data);
        }

        public T Peek()
        {
            Shift();
            return _stack2.Peek();
        }

        public T PeekLast()
        {
            T peekLast = (_stack1.Count > 0) ? _stack1.Peek(): _stack2.ElementAt(_stack2.Count - 1);
            return peekLast;
        }

        public override string ToString()
        {
            string stringValue = "Queue [" + Length + "]";

            int n = 0;

            for(int i = 0; i < _stack2.Count; i++)
            {
                var element = _stack2.ElementAt(i) == null ? "null" : _stack2.ElementAt(i).ToString();
                stringValue += "\r\n[" + (n++) + "]" + element;
            }
            for(int i = _stack1.Count - 1; i >= 0; i--)
            {
                var element = _stack1.ElementAt(i) == null ? "null" : _stack1.ElementAt(i).ToString();
                stringValue += "\r\n[" + (n++) + "]" + element;
            }

            return stringValue;
        }

        #endregion

    }

    public class QueueOperationException : InvalidOperationException
    {
        public QueueOperationException(string message) : base(message)
        {

        }
    }

    public class QueueComparable<T> : QueueGeneric<T> where T : IComparable
    {
        #region QueueComparable :: Fields

        protected T _min = default(T);

        #endregion

        #region QueueComparable :: Constructor

        public QueueComparable() : base()
        {
            
        }

        #endregion

        #region QueueComparable :: Properties

        public T Minimum
        {
            get { return _min; }
        }

        #endregion

        #region QueueComparable :: Functions

        #endregion

        #region QueueComparable :: Methods

        public T GetMinimum()
        {
            T minimum = default(T);

            for(int i = 0; i < _queue.Count; i++)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;

                if (minimum == null)
                {
                    minimum = _queue[i];
                }
                else if (!nullable || _queue[i] != null)
                {
                    if (_queue[i].CompareTo(minimum) < 0)
                    {
                        minimum = _queue[i];
                    }
                }
            }

            return minimum;
        }

        public override T Dequeue()
        {
            T dequeueData = base.Dequeue();

            _min = GetMinimum();

            return dequeueData;
        }

        public override void Enqueue(T data)
        {
            base.Enqueue(data);

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
        }

        #endregion

    }

    public class QueueGeneric<T>
    {
        #region Queue :: Fields

        protected List<T> _queue = new List<T>();

        #endregion

        #region Queue :: Constructor

        public QueueGeneric()
        {

        }

        #endregion

        #region Queue :: Properties

        public bool IsEmpty
        {
            get { return _queue.Count <= 0; }
        }

        public T First
        {
            get { return Peek(); }
        }

        public T Last
        {
            get { return PeekLast(); }
        }

        public int Length
        {
            get { return _queue.Count; }
        }

        #endregion

        #region Queue :: Functions

        #endregion

        #region Queue :: Methods

        public virtual T Dequeue()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new QueueOperationException("Cannot operate on an empty queue");
                }
                else // object type nullable
                {
                    return default(T);
                }
            }

            T data = _queue[0];
            _queue.RemoveAt(0);

            return data;
        }

        public virtual void Enqueue(T data)
        {
            _queue.Add(data);
        }

        public virtual T Peek()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new QueueOperationException("Cannot peek an empty queue");
                }
                else // object type nullable
                { 
                    return default(T);
                }
            }
            return _queue[0];
        }

        public virtual T PeekLast()
        {
            if (IsEmpty)
            {
                Type t1 = typeof(T);
                Type t2 = Nullable.GetUnderlyingType(t1);
                bool nullable = !t1.IsValueType || t2 != null;
                if (!nullable)
                {
                    throw new QueueOperationException("Cannot peek an empty queue");
                }
                else // object type nullable
                { 
                    return default(T);
                }
            }
            return _queue[_queue.Count - 1];
        }

        public override string ToString()
        {
            string stringValue = "Queue [" + _queue.Count + "]";

            for(int i = 0; i < _queue.Count; i++)
            {
                stringValue += "\r\n[" + i + "]" + (_queue[i] == null ? "null" : _queue[i].ToString());
            }

            return stringValue;
        }

        #endregion


    }

}
