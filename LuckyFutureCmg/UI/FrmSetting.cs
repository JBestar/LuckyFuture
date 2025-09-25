
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
        // public static readonly FrmSetting Default = new FrmSetting();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        private FrmSetting()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }

        private void InitializeComponentEx()
        {

            BETTYPE[] betTypeList = Common.GetAllBetType();
            foreach(BETTYPE betType in betTypeList)
                cmbBettingType.Items.Add(Common.GetBetTypeFullStr(betType));

            for (int i = 0; i < 10; i++)
            {
                cmbBettingCandle1.Items.Add(i + 1);
                cmbBettingCandle2.Items.Add(i + 1);
                cmbCandlePayoff.Items.Add(i + 1);
                cmbCandlePayoff4.Items.Add(i + 1);
                cmbBettingCandle4.Items.Add(i);
            }
            //이평선교차
            cmbBettingCandle3.Items.Add("미완성");
            cmbBettingCandle3.Items.Add("완성");

            cmbReorder3.Items.Add("아니");
            cmbReorder3.Items.Add("예");
            //주하선
            cmbBettingCross4.Items.Add("대기");
            cmbBettingCross4.Items.Add("재진입");
            //이평-주하
            cmbBettingCandle5.Items.Add("미완성");
            cmbBettingCandle5.Items.Add("완성");

            
            string[] chartTypeList = { "1분", "60틱", "90틱", "120틱",
                    "240틱", "350틱", "400틱", "600틱", "750틱", "990틱"};
            foreach (string s in chartTypeList)
            {
                cmbChartType1.Items.Add(s);
                cmbChartType2.Items.Add(s);
                cmbChartType3.Items.Add(s);
                cmbChartType4.Items.Add(s);
                cmbChartType5.Items.Add(s);
            }

            string[] orderTypeList = { "시장가", "지정가" };
            foreach (string s in orderTypeList)
            {
                cmbOrderType1.Items.Add(s);
                cmbOrderType2.Items.Add(s);
                cmbOrderType3.Items.Add(s);
                cmbOrderType4.Items.Add(s);
                cmbOrderType5.Items.Add(s);
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
            groupBetting4.Visible = false;
            groupBetting5.Visible = false;

            groupPayoff1.Visible = false;
            groupPayoff2.Visible = false;
            groupPayoff4.Visible = false;
            string strCom = "개 (최대 " + Settings.Default.OrderMax.ToString() + "개)";
            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT)
            {
                groupBetting1.Visible = true;
                groupPayoff1.Visible = true;
                groupPayoff1.Text = "청산설정";

                cmbChartType1.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType1.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount1.Text = Settings.Default.OrderCount.ToString();
                label19.Text = strCom;
                cmbBettingCandle1.SelectedItem = Settings.Default.BettingCandleCount;
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.UPDOWN)
            {
                groupBetting2.Visible = true;
                groupPayoff2.Visible = true;

                cmbChartType2.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType2.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount2.Text = Settings.Default.OrderCount.ToString();
                label18.Text = strCom;
                cmbBettingCandle2.SelectedItem = Settings.Default.BettingCandleCount;

                cmbAvgType2.SelectedIndex = Settings.Default.AvgType;
                txtBettingTick2.Text = Settings.Default.BettingTickCount.ToString();
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS)
            {
                groupBetting3.Visible = true;
                groupPayoff1.Visible = true;
                groupPayoff1.Text = "청산설정";

                cmbChartType3.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType3.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount3.Text = Settings.Default.OrderCount.ToString();
                label23.Text = strCom;
                cmbBettingCandle3.SelectedIndex = Settings.Default.BettingCandleComplete;
                cmbReorder3.SelectedIndex = Settings.Default.Reorder ? 1 : 0;
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE)
            {
                groupBetting4.Visible = true;
                groupPayoff1.Visible = true;
                groupPayoff1.Text = "박스권설정";

                cmbChartType4.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType4.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount4.Text = Settings.Default.OrderCount.ToString();
                label29.Text = strCom;
                cmbBettingCandle4.SelectedItem = Settings.Default.BettingCandleCount-1;
                cmbBettingCross4.SelectedIndex = Settings.Default.BettingEnter?1:0;
                txtBoAdjust4.Text = Settings.Default.BoLineAdjust.ToString();
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID)
            {
                groupBetting5.Visible = true;
                groupPayoff1.Visible = true;
                groupPayoff1.Text = "청산설정";

                cmbChartType5.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType5.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount5.Text = Settings.Default.OrderCount.ToString();
                label39.Text = strCom;
                cmbBettingCandle5.SelectedIndex = Settings.Default.BettingCandleComplete;
                txtBoAdjust5.Text = Settings.Default.BoLineAdjust.ToString();
            }

            chkEarnPayoff.Checked = Settings.Default.EarnPayoff;
            chkLossPayoff.Checked = Settings.Default.LossPayoff;
            chkCandlePayoff.Checked = Settings.Default.CandlePayoff;
            chkCandlePayoff4.Checked = Settings.Default.CandlePayoff;
            txtPayoffEarn.Text = Settings.Default.EarnPayoffMoney.ToString();
            txtPayoffLoss.Text = Settings.Default.LossPayoffMoney.ToString();
            cmbCandlePayoff.SelectedItem = Settings.Default.CandlePayoffCount;
            cmbCandlePayoff4.SelectedItem = Settings.Default.CandlePayoffCount;
            txtTickPayoff.Text = Settings.Default.TickPayoffCount.ToString();

            chkLiqStop.Checked = Settings.Default.LiquidStop;
            chkEarnStop.Checked = Settings.Default.EarnStop;
            chkLossStop.Checked = Settings.Default.LossStop;
            chkOrderStop.Checked = Settings.Default.OrderStop;
            txtStopEarn.Text = Settings.Default.EarnStopMoney.ToString();
            txtStopLoss.Text = Settings.Default.LossStopMoney.ToString();
            txtStopOrder.Text = Settings.Default.OrderStopDelay.ToString();

            EnableControls();
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

            cmbCandlePayoff4.Enabled = chkCandlePayoff4.Checked;
            //미체결취소
            txtStopOrder.Enabled = chkOrderStop.Checked;
        }

        public void loadControls()
        {
            cmbBettingType.SelectedIndex = Settings.Default.BettingType;
            ChangeControls();

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
            string strWarning = "주문수량이 최대 " + Settings.Default.OrderMax.ToString() + "개를 초과할수 없습니다.";
            if (Settings.Default.OrderMax < 10)
            {
                strWarning += "\r\n고객센터에 문의해주세요";
            }
            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT)       //동일색캔들
            {
                Settings.Default.BettingType = (int)BETTYPE.EQUIVALENT;
                Settings.Default.ChartType = cmbChartType1.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType1.SelectedIndex;
                try
                {
                    int nOrderCnt = Int32.Parse(txtOrderCount1.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount1.SelectAll();
                        txtOrderCount1.Focus();
                        return;
                    }
                    else if (nOrderCnt > Settings.Default.OrderMax)
                    {
                        MessageBox.Show(strWarning, "경고");

                        txtOrderCount1.SelectAll();
                        txtOrderCount1.Focus();
                        return;
                    }
                    Settings.Default.OrderCount = nOrderCnt;
                }
                catch
                {
                    txtOrderCount1.SelectAll();
                    txtOrderCount1.Focus();
                    return;
                }

                if (cmbBettingCandle1.SelectedItem != null)
                    Settings.Default.BettingCandleCount = (int)cmbBettingCandle1.SelectedItem;
                else
                    Settings.Default.BettingCandleCount = 0;

            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.UPDOWN)     //이평선상하
            {
                Settings.Default.BettingType = (int)BETTYPE.UPDOWN;
                Settings.Default.ChartType = cmbChartType2.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType2.SelectedIndex;
                
                try
                {
                    int nOrderCnt = Int32.Parse(txtOrderCount2.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount2.SelectAll();
                        txtOrderCount2.Focus();
                        return;
                    } else if(nOrderCnt > Settings.Default.OrderMax)
                    {
                        MessageBox.Show(strWarning, "경고");
                        txtOrderCount2.SelectAll();
                        txtOrderCount2.Focus();
                        return;
                    }
                    Settings.Default.OrderCount = nOrderCnt;
                }
                catch
                {
                    txtOrderCount2.SelectAll();
                    txtOrderCount2.Focus();
                    return;
                }

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
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS)       //이평선교차
            {
                Settings.Default.BettingType = (int)BETTYPE.CROSS;
                Settings.Default.ChartType = cmbChartType3.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType3.SelectedIndex;
                try
                {
                    int nOrderCnt = Int32.Parse(txtOrderCount3.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount3.SelectAll();
                        txtOrderCount3.Focus();
                        return;
                    }
                    else if (nOrderCnt > Settings.Default.OrderMax)
                    {
                        MessageBox.Show(strWarning, "경고");
                        txtOrderCount3.SelectAll();
                        txtOrderCount3.Focus();
                        return;
                    }
                    Settings.Default.OrderCount = nOrderCnt;
                }
                catch
                {
                    txtOrderCount3.SelectAll();
                    txtOrderCount3.Focus();
                    return;
                }

                if (cmbBettingCandle3.SelectedItem != null)
                    Settings.Default.BettingCandleComplete = (int)cmbBettingCandle3.SelectedIndex;
                else
                    Settings.Default.BettingCandleComplete = 0;

                if (cmbReorder3.SelectedItem != null)
                    Settings.Default.Reorder = cmbReorder3.SelectedIndex == 1 ? true:false;
                else
                    Settings.Default.Reorder = false;

            } else if (cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE)       //하늘-주황라인
            {
                Settings.Default.BettingType = (int)BETTYPE.BOLINE;
                Settings.Default.ChartType = cmbChartType4.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType4.SelectedIndex;
                try
                {
                    int nOrderCnt = Int32.Parse(txtOrderCount4.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount4.SelectAll();
                        txtOrderCount4.Focus();
                        return;
                    }
                    else if (nOrderCnt > Settings.Default.OrderMax)
                    {
                        MessageBox.Show(strWarning, "경고");

                        txtOrderCount4.SelectAll();
                        txtOrderCount4.Focus();
                        return;
                    }
                    Settings.Default.OrderCount = nOrderCnt;
                }
                catch
                {
                    txtOrderCount4.SelectAll();
                    txtOrderCount4.Focus();
                    return;
                }

                if (cmbBettingCandle4.SelectedItem != null)
                    Settings.Default.BettingCandleCount = (int)cmbBettingCandle4.SelectedItem+1;
                else
                    Settings.Default.BettingCandleCount = 0;

                Settings.Default.BettingEnter = cmbBettingCross4.SelectedIndex == 1 ? true:false;

                //주-하선조종
                try
                {
                    int nAdjust = Int32.Parse(txtBoAdjust4.Text);
                    if (nAdjust < 0 || nAdjust > 100)
                    {
                        txtBoAdjust4.SelectAll();
                        txtBoAdjust4.Focus();
                        return;
                    }

                    Settings.Default.BoLineAdjust = nAdjust;
                }
                catch
                {
                    txtBoAdjust4.SelectAll();
                    txtBoAdjust4.Focus();
                    return;
                }

            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID)       //이평주하
            {
                Settings.Default.BettingType = (int)BETTYPE.HYBRID;
                Settings.Default.ChartType = cmbChartType5.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType5.SelectedIndex;
                //주문가능수량
                try
                {
                    int nOrderCnt = Int32.Parse(txtOrderCount5.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount5.SelectAll();
                        txtOrderCount5.Focus();
                        return;
                    }
                    else if (nOrderCnt > Settings.Default.OrderMax)
                    {
                        MessageBox.Show(strWarning, "경고");
                        txtOrderCount5.SelectAll();
                        txtOrderCount5.Focus();
                        return;
                    }
                    Settings.Default.OrderCount = nOrderCnt;
                }
                catch
                {
                    txtOrderCount5.SelectAll();
                    txtOrderCount5.Focus();
                    return;
                }

                if (cmbBettingCandle5.SelectedItem != null)
                    Settings.Default.BettingCandleComplete = (int)cmbBettingCandle5.SelectedIndex;
                else
                    Settings.Default.BettingCandleComplete = 0;
                //주-하선조종
                try
                {
                    int nAdjust = Int32.Parse(txtBoAdjust5.Text);
                    if (nAdjust < 0 || nAdjust > 100)
                    {
                        txtBoAdjust5.SelectAll();
                        txtBoAdjust5.Focus();
                        return;
                    }

                    Settings.Default.BoLineAdjust = nAdjust;
                }
                catch
                {
                    txtBoAdjust5.SelectAll();
                    txtBoAdjust5.Focus();
                    return;
                }
            }

            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID)
            {
                Settings.Default.EarnPayoff = chkEarnPayoff.Checked;
                if (chkEarnPayoff.Checked && txtPayoffEarn.Visible)
                {
                    try
                    {
                        Settings.Default.EarnPayoffMoney = Int32.Parse(txtPayoffEarn.Text);
                        if (Settings.Default.EarnPayoffMoney < 0)
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
                        if (Settings.Default.LossPayoffMoney < 0)
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
            }

            Settings.Default.LiquidStop = chkLiqStop.Checked;
            Settings.Default.EarnStop = chkEarnStop.Checked;
            if (chkEarnStop.Checked)
            {
                try
                {
                    Settings.Default.EarnStopMoney = Int32.Parse(txtStopEarn.Text);
                    if (Settings.Default.EarnStopMoney < 0)
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
                    if (Settings.Default.LossStopMoney < 0)
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
                    if (Settings.Default.OrderStopDelay < 0)
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
        
        private void chkEarnStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void chkLossStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkOrderStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkCandlePayoff4_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void chkCandlePayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void cmbBettingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeControls();
        }
    }
}
