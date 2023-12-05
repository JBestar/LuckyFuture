using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;
using ChartCtrl;

namespace LuckyFuture.UI
{
    public partial class FrmOrdCnt : Form
    {
        public static readonly FrmOrdCnt Default = new FrmOrdCnt();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;

        public FrmOrdCnt()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            InitComponent();
        }

        public void SetChartEventHandler(EventHandler<ChartEventArgs> chartEvent)
        {
            this.ChartNoticeEvent += chartEvent;
        }

        protected virtual void OnChartNoticeEvent(Object obj)
        {
            if (ChartNoticeEvent != null)
                ChartNoticeEvent(this, new ChartEventArgs(obj));
        }
        public void InitComponent()
        {
            spin1.Value = Settings.Default.OrdCnt1;
            spin2.Value = Settings.Default.OrdCnt2;
            spin3.Value = Settings.Default.OrdCnt3;
            spin4.Value = Settings.Default.OrdCnt4;
        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnChartNoticeEvent(CHART_EVENTTYPE.BETTING_CHANGED);

            Hide();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.Default.OrdCnt1 = (int)spin1.Value ;
            Settings.Default.OrdCnt2 = (int)spin2.Value;
            Settings.Default.OrdCnt3 = (int)spin3.Value;
            Settings.Default.OrdCnt4 = (int)spin4.Value;
            Settings.Default.Save();
            OnChartNoticeEvent(CHART_EVENTTYPE.SETTING_CHANGED);
            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
