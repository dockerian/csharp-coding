using System;
using System.Runtime.InteropServices;

namespace Common.Extensions
{
    public sealed class WeakReference<T> : IDisposable
    {
        private GCHandle _handle;
        private bool _trackResurrection;

        public WeakReference(T target) : this(target, false) { }
        public WeakReference(T target, bool trackResurrection)
        {
            _trackResurrection = trackResurrection;            
            _handle = GCHandle.Alloc(target, _trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
        }

        ~WeakReference()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            _handle.Free();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsAlive
        {
            get { return (_handle.Target != null); }
        }

        public bool TrackResurrection
        {
            get { return this._trackResurrection; }
        }

        public T Target
        {
            get
            {
                object o = _handle.Target;
                if ((o == null) || (!(o is T)))
                {
                    return default(T);
                }

                return (T) o;
            }            
        }
    }
}