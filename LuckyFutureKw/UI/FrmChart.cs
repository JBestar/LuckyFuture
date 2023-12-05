using ChartCtrl;
using LuckyFuture.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;

namespace LuckyFuture.UI
{
	public partial class FrmChart : Form
	{
		public static readonly FrmChart Default = new FrmChart();
		private FrmChart()
		{
			InitializeComponent();
			CenterToParent();
			InitializeComponentEx();
		}

		private void InitializeComponentEx()
		{
			chartFuture.SaveRtVal = false;
			SetDChartType((CHARTTYPE)Settings.Default.ChartType);

		}

        private void FrmChat_Load(object sender, EventArgs e)
		{

		}

		public void ResetChart()
		{
			if(InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(delegate ()
				{
					ResetChart();
				}));
			}
			else
				chartFuture.InitDraw();
		}

        public void Redraw()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
					Redraw();
                }));
            }
            else
                chartFuture.Redraw();
        }

        public void SetChartEventHandler(EventHandler<ChartEventArgs> chartEvent)
        {
			chartFuture.ChartNoticeEvent += chartEvent;
		}

		public void SaveSetting()
        {
			chartFuture.SaveSetting();

		}

		public void SetRTValue(double rt_value, DateTime time, int nQuantity)
		{
			chartFuture.SetRealTimeVal((float)rt_value, time, nQuantity);
		}
		/*
		public void SetRTValueList(double rt_value, DateTime time, int nQuantity)
		{
			chartFuture.SetRealTimeVal((float)rt_value, time, nQuantity);
		}
		*/
		public bool SetDChartType(CHARTTYPE chartType)
        {
            TIMETYPE tt;
            TIMEUNIT tu;
            int[] arrAvgCnt = { 5, 10, 20, 60, 120 };
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_1;
                    break;
                case CHARTTYPE.TICK_60:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
                case CHARTTYPE.TICK_120:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_120;
                    break;

                default:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
            }

			return chartFuture.SetDChartType(tt, tu, arrAvgCnt, 0);

        }

        public List<DItem> GetCandleList(BETTYPE betType, int nCandles = 0)
		{
			bool bNeedLast = false;
 			if (betType == BETTYPE.CROSS && Settings.Default.BettingCandleNext == 0)
				bNeedLast = true;

			return chartFuture.GetDChartList(nCandles, bNeedLast);
		}

		private void FrmChat_FormClosing(object sender, FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
	}
}
