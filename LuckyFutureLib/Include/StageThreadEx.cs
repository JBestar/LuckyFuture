using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuckyFutureLib.Include
{
	public class StageThreadEx: ThreadEx
	{	
		protected enum STAGE : int
		{
			NONE,
			WAIT,
			LAST,
		};

		protected int _stage = (int) STAGE.NONE;
		int _callback_stage = (int) STAGE.NONE;

		int _start_tick = 0;
		int _wait_time = 0;

		protected void ResetStageState()
		{
			_stage = (int) STAGE.NONE;
			_callback_stage = (int) STAGE.NONE;
			_start_tick = 0;
			_wait_time = 0;
		}

		protected void SetStage(int iStage)
		{
			_stage = iStage;
			_start_tick = Environment.TickCount;
		}

		protected void SetStageWait(int iStage, int nWaitTime)
		{
			_callback_stage = iStage;
			_wait_time = nWaitTime;
			SetStage((int) STAGE.WAIT);
		}

		protected void WaitStage(int nSleepCycle = 10)
		{
			if (Environment.TickCount - _start_tick >= _wait_time)
				SetStage(_callback_stage);
			else
				Thread.Sleep(nSleepCycle);
		}
		protected override void OnStarted()
		{
			ResetStageState();
		}
		protected override bool Run()
		{
			if (_stage == (int)STAGE.WAIT)
				WaitStage();
			return true;
		}
		protected override void OnStopped(bool bAutoStop)
		{
			ResetStageState();
		}
	}
}
