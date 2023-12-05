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
using LuckyFutureLib.Include;
using LuckyFuture.Properties;

namespace LuckyFuture.UI
{
	public partial class FrmChart : Form
	{
		public static readonly FrmChart Default = new FrmChart();
        public CHARTTYPE _ChartType;
		private FrmChart()
		{
			InitializeComponent();
			CenterToParent();
			InitializeComponentEx();
		}
        private void InitializeComponentEx()
        {
            this.chartFuture.UnitTimeCount = ChartCtrl.TIMEUNIT.TIMEUNIT_60;
            this.chartFuture.UnitTimeType = ChartCtrl.TIMETYPE.TIMETYPE_TICK;

            chartFuture.SaveRtVal = true;
            ResetDChartType();
        }
        
        private void FrmChat_Load(object sender, EventArgs e)
		{

		}
        private void ResetDChartType()
        {

            SetDChartType((CHARTTYPE)Settings.Default.ChartType);
            SetDChart2Type((CHARTTYPE)Settings.Default.Conc2Chart);
        }
        public void SetChartEventHandler(EventHandler<ChartEventArgs> chartEvent)
        {
            chartFuture.ChartNoticeEvent += chartEvent;
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
        public void SaveSetting()
        {
            chartFuture.SaveSetting();

        }
        public void SetRTValue(double rt_value, DateTime time, int nQuantity, int nConclusion)
		{
			

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(delegate ()
                    {
                        SetRTValue(rt_value, time, nQuantity, nConclusion);
                    }));
                }
                catch (Exception ex)
                {
                    //TraceEx.TraceException(ex);
                    string exMessage = ex.Message;
                    return;
                }
            }
            else
                chartFuture.SetRealTimeVal((float)rt_value, time, nQuantity, nConclusion>0? nConclusion:1);


        }
        
        public bool SetDChartType(CHARTTYPE chartType)
        {
            _ChartType = chartType;
            TIMETYPE tt;
            TIMEUNIT tu;
            int[] arrAvgCnt = { 5, 10, 20, 60, 120 };
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_1;
                    break;
                case CHARTTYPE.MIN_5:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_5;
                    break;
                case CHARTTYPE.MIN_15:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_15;
                    break;
                case CHARTTYPE.MIN_30:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_30;
                    break;
                case CHARTTYPE.TICK_60:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
                case CHARTTYPE.TICK_90:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_90;
                    break;
                case CHARTTYPE.TICK_120:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_120;
                    break;
                case CHARTTYPE.TICK_240:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_240;
                    break;
                case CHARTTYPE.TICK_350:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_350;
                    break;
                case CHARTTYPE.TICK_400:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_400;
                    break;
                case CHARTTYPE.TICK_600:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_600;
                    break;
                case CHARTTYPE.TICK_750:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_750;
                    break;
                case CHARTTYPE.TICK_990:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_990;
                    break;
                default:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
            }

            return chartFuture.SetDChartType(tt, tu, arrAvgCnt, Settings.Default.BoLineAdjust);

        }

        public bool SetDChart2Type(CHARTTYPE chartType)
        {
            _ChartType = chartType;
            TIMETYPE tt;
            TIMEUNIT tu;
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_1;
                    break;
                case CHARTTYPE.MIN_5:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_5;
                    break;
                case CHARTTYPE.MIN_15:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_15;
                    break;
                case CHARTTYPE.MIN_30:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_30;
                    break;
                case CHARTTYPE.TICK_60:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
                case CHARTTYPE.TICK_90:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_90;
                    break;
                case CHARTTYPE.TICK_120:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_120;
                    break;
                case CHARTTYPE.TICK_240:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_240;
                    break;
                case CHARTTYPE.TICK_350:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_350;
                    break;
                case CHARTTYPE.TICK_400:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_400;
                    break;
                case CHARTTYPE.TICK_600:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_600;
                    break;
                case CHARTTYPE.TICK_750:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_750;
                    break;
                case CHARTTYPE.TICK_990:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_990;
                    break;
                default:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
            }

            return chartFuture.SetDChart2Type(tt, tu);

        }
        public void SetOrderInfo(OrderVal orderInfo, CHARTTYPE chartType)
        {
            TIMETYPE tt;
            TIMEUNIT tu;
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_1;
                    break;
                case CHARTTYPE.MIN_5:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_5;
                    break;
                case CHARTTYPE.MIN_15:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_15;
                    break;
                case CHARTTYPE.MIN_30:
                    tt = TIMETYPE.TIMETYPE_MIN;
                    tu = TIMEUNIT.TIMEUNIT_30;
                    break;
                case CHARTTYPE.TICK_60:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
                case CHARTTYPE.TICK_90:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_90;
                    break;
                case CHARTTYPE.TICK_120:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_120;
                    break;
                case CHARTTYPE.TICK_240:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_240;
                    break;
                case CHARTTYPE.TICK_350:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_350;
                    break;
                case CHARTTYPE.TICK_400:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_400;
                    break;
                case CHARTTYPE.TICK_600:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_600;
                    break;
                case CHARTTYPE.TICK_750:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_750;
                    break;
                case CHARTTYPE.TICK_990:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_990;
                    break;
                default:
                    tt = TIMETYPE.TIMETYPE_TICK;
                    tu = TIMEUNIT.TIMEUNIT_60;
                    break;
            }
            orderInfo.TimeType = tt;
            orderInfo.TimeUnit = tu;

            chartFuture.SetOrderInfo(orderInfo);
        }
        public List<DItem> GetCandleList(int nCandles = 0, bool bNeedLast = false)
		{
            return chartFuture.GetDChartList(nCandles, bNeedLast);
		}
        public List<CItem> GetCandleList2(int nCandles = 0, bool bNeedLast = false)
        {
            return chartFuture.GetCChartList(nCandles, bNeedLast);
        }
        public int GetConcPerMin(int nMin = 0)
        {
            return chartFuture.GetConcPerMin(nMin);
        }
        public void SetChartFrom(DateTime dtFrom)
        {
            chartFuture.SetChartFrom(dtFrom);
        }
        private void FrmChat_FormClosing(object sender, FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
	}
}
