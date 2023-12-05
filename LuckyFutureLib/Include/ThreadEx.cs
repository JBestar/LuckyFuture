using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuckyFutureLib.Include
{
    public class ThreadEx: Object
    {
        protected Thread _thread_obj;
        bool _stop_flag = false;
        public Thread Handle { get => _thread_obj; }

        public int Id { get => _thread_obj != null ? _thread_obj.ManagedThreadId : 0; }

		public bool IsRunning { get => _thread_obj != null && _thread_obj.IsAlive; }


		public virtual bool Start()
        {
            if (IsRunning)
                return false;

            _thread_obj = new Thread(ThreadProc);
            _stop_flag = false;
            _thread_obj.Start(this);

            return true;
        }
        
        public virtual void Stop(UInt32 dwTimeOut = UInt32.MaxValue)
        {
            _stop_flag = true;
            int nStart = Environment.TickCount;
            while (this.IsRunning)
            {
                if (dwTimeOut != UInt32.MaxValue && (UInt32)(Environment.TickCount - nStart) >= dwTimeOut)
                    break;
                Thread.Sleep(5);
            }
			_thread_obj = null;
			_stop_flag = false;
		}

        protected virtual bool Run()
        {
            return true;
        }

        protected virtual void OnStarted()
        {

        }

        protected virtual void OnStopped(bool bAutoStop)
        {

        }

        static void ThreadProc(Object obj)
        {
            ThreadEx pThis = (ThreadEx) obj;
            pThis.OnStarted();

            bool bAutoStop = false;
            while(!pThis._stop_flag && !bAutoStop)
                bAutoStop = !pThis.Run();

            if (bAutoStop)
            {
                pThis._thread_obj = null;
                pThis._stop_flag = false;
            }

			pThis.OnStopped(bAutoStop);
		}
	}
}
