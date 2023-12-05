
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
using ChartCtrl;

namespace LuckyFuture.UI
{

    public partial class FrmSetting : Form
    {
        public static readonly FrmSetting Default = new FrmSetting();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        private FrmSetting()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }

        private void InitializeComponentEx()
        {
            for (int i = 0; i < 10; i++)
            {
                cmbBettingCandle1.Items.Add(i + 1);
                cmbBettingCandle2.Items.Add(i + 1);
                cmbOrderCount1.Items.Add(i + 1);
                cmbOrderCount2.Items.Add(i + 1);
                cmbOrderCount3.Items.Add(i + 1);
                cmbCandlePayoff.Items.Add(i + 1);
            }

            cmbBettingCandle3.Items.Add("미완성");
            cmbBettingCandle3.Items.Add("완성");

            string[] chartTypeList = { "1분", "60틱", "120틱" };
            foreach (string s in chartTypeList)
            {
                cmbChartType1.Items.Add(s);
                cmbChartType2.Items.Add(s);
                cmbChartType3.Items.Add(s);
            }

            string[] orderTypeList = { "시장가", "지정가" };
            foreach (string s in orderTypeList)
            {
                cmbOrderType1.Items.Add(s);
                cmbOrderType2.Items.Add(s);
                cmbOrderType3.Items.Add(s);
            }

            int[] avgTypeList = { 5, 10, 20, 60, 120 };
            foreach (int i in avgTypeList)
                cmbAvgType2.Items.Add(i);


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



        private void ChangeControls()
        {
            groupBetting1.Visible = false;
            groupBetting2.Visible = false;
            groupBetting3.Visible = false;

            groupPayoff1.Visible = false;
            groupPayoff2.Visible = false;
            if (radCandleBet.Checked)
            {
                groupBetting1.Visible = true;
                groupPayoff1.Visible = true;

                cmbChartType1.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType1.SelectedIndex = Settings.Default.OrderType;
                cmbOrderCount1.SelectedItem = Settings.Default.OrderCount;
                cmbBettingCandle1.SelectedItem = Settings.Default.BettingCandleCount;
            }
            else if (radAvgBet.Checked)
            {
                groupBetting2.Visible = true;
                groupPayoff2.Visible = true;

                cmbChartType2.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType2.SelectedIndex = Settings.Default.OrderType;
                cmbOrderCount2.SelectedItem = Settings.Default.OrderCount;
                cmbBettingCandle2.SelectedItem = Settings.Default.BettingCandleCount;

                cmbAvgType2.SelectedIndex = Settings.Default.AvgType;
                txtBettingTick2.Text = Settings.Default.BettingTickCount.ToString();
            }
            else if (radCrossBet.Checked)
            {
                groupBetting3.Visible = true;
                groupPayoff1.Visible = true;

                cmbChartType3.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType3.SelectedIndex = Settings.Default.OrderType;
                cmbOrderCount3.SelectedItem = Settings.Default.OrderCount;
                cmbBettingCandle3.SelectedIndex = Settings.Default.BettingCandleNext;
            }


        }

        private void EnableControls()
        {
            //수익
            txtPayoffEarn.Enabled = chkEarnPayoff.Checked;
            //손실
            txtPayoffLoss.Enabled = chkLossPayoff.Checked;
            //익절
            txtStopEarn.Enabled = chkEarnStop.Checked;
            //손절
            txtStopLoss.Enabled = chkLossStop.Checked;
            //상승/하락
            cmbCandlePayoff.Enabled = chkCandlePayoff.Checked;
            txtTickPayoff.Enabled = chkCandlePayoff.Checked;
            //미체결취소
            txtStopOrder.Enabled = chkOrderStop.Checked;
        }

        public void loadControls()
        {
            if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
            {
                radCandleBet.Checked = true;
                //cmbChartType1.SelectedIndex = Settings.Default.ChartType;
                //cmbOrderCount1.SelectedItem = Settings.Default.OrderCount;
                //cmbBettingCandle1.SelectedItem = Settings.Default.BettingCandleCount;

            }
            else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
            {
                radAvgBet.Checked = true;
                //cmbChartType2.SelectedIndex = Settings.Default.ChartType;
                //cmbOrderCount2.SelectedItem = Settings.Default.OrderCount;
                //cmbBettingCandle2.SelectedItem = Settings.Default.BettingCandleCount;
                //cmbAvgType2.SelectedIndex = Settings.Default.AvgType;
                //txtBettingTick2.Text = Settings.Default.BettingTickCount.ToString();

            }
            else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
            {
                radCrossBet.Checked = true;
                //cmbChartType3.SelectedIndex = Settings.Default.ChartType;
                //cmbOrderCount3.SelectedItem = Settings.Default.OrderCount;
                //cmbBettingCandle3.SelectedIndex = Settings.Default.BettingCandleNext;
            }

            chkEarnPayoff.Checked = Settings.Default.EarnPayoff;
            chkLossPayoff.Checked = Settings.Default.LossPayoff;
            chkCandlePayoff.Checked = Settings.Default.CandlePayoff;
            txtPayoffEarn.Text = Settings.Default.EarnPayoffMoney.ToString();
            txtPayoffLoss.Text = Settings.Default.LossPayoffMoney.ToString();
            cmbCandlePayoff.SelectedItem = Settings.Default.CandlePayoffCount;
            txtTickPayoff.Text = Settings.Default.TickPayoffCount.ToString();

            chkEarnStop.Checked = Settings.Default.EarnStop;
            chkLossStop.Checked = Settings.Default.LossStop;
            chkOrderStop.Checked = Settings.Default.OrderStop;
            txtStopEarn.Text = Settings.Default.EarnStopMoney.ToString();
            txtStopLoss.Text = Settings.Default.LossStopMoney.ToString();
            txtStopOrder.Text = Settings.Default.OrderStopDelay.ToString();

            ChangeControls();
            EnableControls();

            OnChartNoticeEvent(CHART_EVENTTYPE.BETTING_CHANGED);
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            //loadControls();
        }

        private void FrmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radCandleBet.Checked)
            {
                Settings.Default.BettingType = (int)BETTYPE.EQUIVALENT;
                Settings.Default.ChartType = cmbChartType1.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType1.SelectedIndex;
                if (cmbOrderCount1.SelectedItem != null)
                    Settings.Default.OrderCount = (int)cmbOrderCount1.SelectedItem;
                else Settings.Default.OrderCount = 1;

                if (cmbBettingCandle1.SelectedItem != null)
                    Settings.Default.BettingCandleCount = (int)cmbBettingCandle1.SelectedItem;
                else
                    Settings.Default.BettingCandleCount = 0;

            }
            else if (radAvgBet.Checked)
            {
                Settings.Default.BettingType = (int)BETTYPE.UPDOWN;
                Settings.Default.ChartType = cmbChartType2.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType2.SelectedIndex;
                if (cmbOrderCount2.SelectedItem != null)
                    Settings.Default.OrderCount = (int)cmbOrderCount2.SelectedItem;
                else Settings.Default.OrderCount = 1;

                if (cmbBettingCandle2.SelectedItem != null)
                    Settings.Default.BettingCandleCount = (int)cmbBettingCandle2.SelectedItem;
                else
                    Settings.Default.BettingCandleCount = 0;
                Settings.Default.AvgType = cmbAvgType2.SelectedIndex;

                try
                {
                    Settings.Default.BettingTickCount = Int32.Parse(txtBettingTick2.Text);
                    if (Settings.Default.BettingTickCount < 0)
                    {
                        txtBettingTick2.SelectAll();
                        txtBettingTick2.Focus();
                        return;
                    }
                }
                catch
                {
                    txtBettingTick2.SelectAll();
                    txtBettingTick2.Focus();
                    return;
                }
            }
            else if (radCrossBet.Checked)
            {
                Settings.Default.BettingType = (int)BETTYPE.CROSS;
                Settings.Default.ChartType = cmbChartType3.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType3.SelectedIndex;
                if (cmbOrderCount3.SelectedItem != null)
                    Settings.Default.OrderCount = (int)cmbOrderCount3.SelectedItem;
                else Settings.Default.OrderCount = 1;

                if (cmbBettingCandle3.SelectedItem != null)
                    Settings.Default.BettingCandleNext = (int)cmbBettingCandle3.SelectedIndex;
                else
                    Settings.Default.BettingCandleNext = 0;

            }


            Settings.Default.EarnPayoff = chkEarnPayoff.Checked;
            if (chkEarnPayoff.Checked && txtPayoffEarn.Visible)
            {
                try
                {
                    Settings.Default.EarnPayoffMoney = Int32.Parse(txtPayoffEarn.Text);
                    if (Settings.Default.EarnPayoffMoney <= 0)
                    {
                        txtPayoffEarn.SelectAll();
                        txtPayoffEarn.Focus();
                        return;
                    }
                }
                catch
                {
                    txtPayoffEarn.SelectAll();
                    txtPayoffEarn.Focus();
                    return;
                }
            }

            Settings.Default.LossPayoff = chkLossPayoff.Checked;
            if (chkLossPayoff.Checked && txtPayoffLoss.Visible)
            {
                try
                {
                    Settings.Default.LossPayoffMoney = Int32.Parse(txtPayoffLoss.Text);
                    if (Settings.Default.LossPayoffMoney <= 0)
                    {
                        txtPayoffLoss.SelectAll();
                        txtPayoffLoss.Focus();
                        return;
                    }
                }
                catch
                {
                    txtPayoffLoss.SelectAll();
                    txtPayoffLoss.Focus();
                    return;
                }
            }
            Settings.Default.CandlePayoff = chkCandlePayoff.Checked;
            if (cmbCandlePayoff.SelectedItem != null)
                Settings.Default.CandlePayoffCount = (int)cmbCandlePayoff.SelectedItem;
            else
                Settings.Default.CandlePayoffCount = 0;

            if (chkCandlePayoff.Checked && txtTickPayoff.Visible)
            {
                try
                {
                    Settings.Default.TickPayoffCount = Int32.Parse(txtTickPayoff.Text);
                    if (Settings.Default.TickPayoffCount < 0)
                    {
                        txtTickPayoff.SelectAll();
                        txtTickPayoff.Focus();
                        return;
                    }
                }
                catch
                {
                    txtTickPayoff.SelectAll();
                    txtTickPayoff.Focus();
                    return;
                }
            }


            Settings.Default.EarnStop = chkEarnStop.Checked;
            if (chkEarnStop.Checked)
            {
                try
                {
                    Settings.Default.EarnStopMoney = Int32.Parse(txtStopEarn.Text);
                    if (Settings.Default.EarnStopMoney <= 0)
                    {
                        txtStopEarn.SelectAll();
                        txtStopEarn.Focus();
                        return;
                    }
                }
                catch
                {
                    txtStopEarn.SelectAll();
                    txtStopEarn.Focus();
                    return;
                }
            }

            Settings.Default.LossStop = chkLossStop.Checked;
            if (chkLossStop.Checked)
            {
                try
                {
                    Settings.Default.LossStopMoney = Int32.Parse(txtStopLoss.Text);
                    if (Settings.Default.LossStopMoney <= 0)
                    {
                        txtStopLoss.SelectAll();
                        txtStopLoss.Focus();
                        return;
                    }
                }
                catch
                {
                    txtStopLoss.SelectAll();
                    txtStopLoss.Focus();
                    return;
                }
            }

            Settings.Default.OrderStop = chkOrderStop.Checked;
            if (chkOrderStop.Checked)
            {
                try
                {
                    Settings.Default.OrderStopDelay = Int32.Parse(txtStopOrder.Text);
                    if (Settings.Default.OrderStopDelay <= 0)
                    {
                        txtStopOrder.SelectAll();
                        txtStopOrder.Focus();
                        return;
                    }
                }
                catch
                {
                    txtStopOrder.SelectAll();
                    txtStopOrder.Focus();
                    return;
                }
            }

            Settings.Default.Save();
            AppAuthor.Default.UploadConfig();
            OnChartNoticeEvent(CHART_EVENTTYPE.BETTING_CHANGED);

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void chkEarnPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkLossPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkCandlePayOff_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkEarnStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void chkLossStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void radCandleBet_Click(object sender, EventArgs e)
        {
            ChangeControls();
        }
        private void radAvgBet_Click(object sender, EventArgs e)
        {
            ChangeControls();
        }

        private void radCrossBet_Click(object sender, EventArgs e)
        {
            ChangeControls();
        }


        private void chkOrderStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }


    }
}
