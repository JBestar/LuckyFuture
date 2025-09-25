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
    public enum ORDTYPE
    {
        Order,
        Earn,
        Loss
    }
    public partial class FrmOrdCnt : Form
    {
        ORDTYPE _OrdType = ORDTYPE.Order;
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;

        public FrmOrdCnt(ORDTYPE type)
        {
            OrdType = type;
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            int max = 10000;
            if (OrdType == ORDTYPE.Earn)
            {
                this.Text = "수익틱설정";
                this.groupAvgLine.Text = "수익틱 버튼설정";
                this.label1.Text = "틱수";
            }
            else if (OrdType == ORDTYPE.Loss)
            {
                this.Text = "손실틱설정";
                this.groupAvgLine.Text = "손실틱 버튼설정";
                this.label1.Text = "틱수";
            }
            else
            {
                max = 10000;
                spin1.DecimalPlaces = 2;     // Show 2 decimal places
                spin1.Increment = 0.01M;      // Step size
                spin1.Minimum = 0.01M;

                spin2.DecimalPlaces = 2;     // Show 2 decimal places
                spin2.Increment = 0.01M;      // Step size
                spin2.Minimum = 0.01M;

                spin3.DecimalPlaces = 2;     // Show 2 decimal places
                spin3.Increment = 0.01M;      // Step size
                spin3.Minimum = 0.01M;

                spin4.DecimalPlaces = 2;     // Show 2 decimal places
                spin4.Increment = 0.01M;      // Step size
                spin4.Minimum = 0.01M;
            }
            spin1.Maximum = max;
            spin2.Maximum = max;
            spin3.Maximum = max;
            spin4.Maximum = max;
            loadControls();
        }
        public ORDTYPE OrdType
        {
            get => (ORDTYPE)this._OrdType;
            set => _OrdType = value;
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
        public void loadControls()
        {
            decimal[] spins = { 1, 1, 1, 1 };
            string[] cnts = null;
            if (OrdType == ORDTYPE.Earn)
            {
                cnts = Settings.Default.EarnTicks.Split('#');
            }
            else if (OrdType == ORDTYPE.Loss)
            {
                cnts = Settings.Default.LossTicks.Split('#');
            }
            else 
            {
                cnts = Settings.Default.OrdCnts.Split('#');
            }

            if (cnts == null && cnts.Length < 4)
                return;
            for (int i = 0; i<4; i++)
            {
                 if(!decimal.TryParse(cnts[i], out spins[i]))
                {
                    spins[i] = 1;
                }
            }

            spin1.Value = spins[0];
            spin2.Value = spins[1];
            spin3.Value = spins[2];
            spin4.Value = spins[3];

        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            string sCnts = string.Format("{0}#{1}#{2}#{3}", spin1.Value, spin2.Value, spin3.Value, spin4.Value);
            if (OrdType == ORDTYPE.Earn)
            {
                Settings.Default.EarnTicks = sCnts;
                OnChartNoticeEvent(CHART_EVENTTYPE.EARNTICK_CHANGED);
            }
            else if (OrdType == ORDTYPE.Loss)
            {
                Settings.Default.LossTicks = sCnts;
                OnChartNoticeEvent(CHART_EVENTTYPE.LOSSTICK_CHANGED);
            }
            else
            {
                Settings.Default.OrdCnts = sCnts;
                OnChartNoticeEvent(CHART_EVENTTYPE.ORDERCNT_CHANGED);
            }

            Settings.Default.Save();
            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
