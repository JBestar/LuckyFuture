using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;
using LuckyFuture.Models.ValueObjects;

namespace LuckyFuture.UI
{
    public enum RANGETYPE
    {
        PayoffLoss,
        SmartLoss,
        CrossLoss,
        CciLoss
    }
    public partial class FrmRange : Form
    {
        RANGETYPE _RangeType = RANGETYPE.PayoffLoss;
        List<PayoffLossInfo> CurRangeList = new List<PayoffLossInfo>();
        public FrmRange(RANGETYPE type)
        {
            RangeType = type;
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            dgvRangeInfo.DoubleBuffered(true);
            //lbAmount.Text = "금액";
            //lbLoss.Text = "하락";
            //lbLossUnit.Text = "%";
            //lblRsi.Text = "RSI";
            //this.amountDataGridViewTextBoxColumn.HeaderText = "금액";
            //this.rateDataGridViewTextBoxColumn.HeaderText = "손실";
            if (RangeType == RANGETYPE.SmartLoss)
            {
                this.Text = "스마트청산 영역설정";
            }
            else if (RangeType == RANGETYPE.CrossLoss)
            {
                this.Text = "교차청산 영역설정";
            }
            else if (RangeType == RANGETYPE.CciLoss)
            {
                this.Text = "CCI청산 영역설정";
                amountDataGridViewTextBoxColumn.HeaderText = "CCI";
                rateDataGridViewTextBoxColumn.HeaderText = "하락";
                lbAmount.Text = "CCI";
                lbAmoutUnit.Text = "이상";
            }
            else
            {
                this.Text = "손실 영역설정";
                lbLoss.Text = "손실";
                lbLossUnit.Text = "틱";
                lbParaUnit.Text = "틱";
                rateDataGridViewTextBoxColumn.HeaderText = "손실";
                paramDataGridViewTextBoxColumn.HeaderText = "이익";
                lblRsi.Text = "이익";
                txtRsi.Text = "";

            }
        }
       
        public RANGETYPE RangeType
        {
            get => (RANGETYPE)this._RangeType;
            set => _RangeType = value;
        }

        public List<PayoffLossInfo> RangeInfo
        {
            get => (List<PayoffLossInfo>)this.bsRangeInfo.DataSource;
            set => this.bsRangeInfo.DataSource = value;
        }

        public PayoffLossInfo SelectedRangeInfo
        {
            get
            {
                if (this.dgvRangeInfo.CurrentCell.RowIndex < 0)
                {
                    return null;
                }
                return this.RangeInfo[this.dgvRangeInfo.CurrentCell.RowIndex];
            }
        }

        public void loadControls()
        {
            lock (AppConfig._lockObj)
            {
                List<PayoffLossInfo> lossConfs = null;
                if (RangeType == RANGETYPE.SmartLoss)
                {
                    lossConfs = AppConfig.SmartLossConfs;
                }
                else if (RangeType == RANGETYPE.CrossLoss)
                {
                    lossConfs = AppConfig.CrossLossConfs;
                }
                else if (RangeType == RANGETYPE.CciLoss)
                {
                    lossConfs = AppConfig.CciLossConfs;
                }
                else
                {
                    lossConfs = AppConfig.PayoffLossConfs;
                }
                CurRangeList.Clear();
                PayoffLossInfo lossInfo = null;
                foreach(PayoffLossInfo lossConf in lossConfs)
                {
                    lossInfo = new PayoffLossInfo
                    {
                        Stage = lossConf.Stage,
                        StageName = lossConf.StageName,
                        Amount = lossConf.Amount,
                        AmountUnit = lossConf.AmountUnit,
                        Rate = lossConf.Rate,
                        RateUnit = lossConf.RateUnit,
                        Enabled = lossConf.Enabled,
                        Param = lossConf.Param,
                        ActionDelete = lossConf.ActionDelete
                    };

                    CurRangeList.Add(lossInfo);
                }
            }

            UpdateRangeInfo();

        }
        private void UpdateRangeInfo()
        {
            lock (AppConfig._lockObj)
            {
                StringCollection rangeCollection = new StringCollection();
                List<PayoffLossInfo> lossConfs = null;
                if (RangeType == RANGETYPE.SmartLoss)
                {
                    lossConfs = AppConfig.SmartLossConfs;
                    rangeCollection = Settings.Default.SmartLossRange;
                }
                else if (RangeType == RANGETYPE.CrossLoss)
                {
                    lossConfs = AppConfig.CrossLossConfs;
                    rangeCollection = Settings.Default.CrossLossRange;
                }
                else if (RangeType == RANGETYPE.CciLoss)
                {
                    lossConfs = AppConfig.CciLossConfs;
                    rangeCollection = Settings.Default.CciLossRange;
                }
                else
                {
                    lossConfs = AppConfig.PayoffLossConfs;
                    rangeCollection = Settings.Default.PayoffLossRange;
                }
                lossConfs.Clear();
                rangeCollection.Clear();
                int iStage = 0;

                CurRangeList.Sort(new Comparison<PayoffLossInfo>((info1, info2) => info1.Amount.CompareTo(info2.Amount)));

                PayoffLossInfo lossConf = null;
                foreach (PayoffLossInfo lossInfo in CurRangeList)
                {
                    lossInfo.Stage = ++iStage;
                    lossInfo.StageName = iStage.ToString() + "단계";

                    lossConf = new PayoffLossInfo
                    {
                        Stage = iStage,
                        StageName = iStage.ToString() + "단계",
                        Amount = lossInfo.Amount,
                        AmountUnit = lossInfo.AmountUnit,
                        Rate = lossInfo.Rate,
                        RateUnit = lossInfo.RateUnit,
                        Enabled = lossInfo.Enabled,
                        Param = lossInfo.Param,
                        ActionDelete = lossInfo.ActionDelete
                    };
                    rangeCollection.Add(lossConf.Enabled.ToString() + "#" + lossConf.Amount.ToString() + "#" + lossConf.Rate.ToString() + "#" + lossConf.Param);
                    lossConfs.Add(lossConf);
                }
            }

            if (this.dgvRangeInfo.RowCount >= 0)
            {
                RangeInfo = null;
                RangeInfo = CurRangeList;
            }

        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        
        private void btnOk_Click(object sender, EventArgs e)
        {
            
            UpdateRangeInfo();

            Settings.Default.Save();
            MessageBox.Show("설정이 저장되었습니다.", "저장성공");
            Hide();
        }

        private void deleteLossItem(PayoffLossInfo lossInfo)
        {
            CurRangeList.Remove(lossInfo);
            UpdateRangeInfo();
        }
        private void dgvRangeInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && this.RangeInfo[e.RowIndex] == this.SelectedRangeInfo)
                {
                    if (e.ColumnIndex == 8)
                    {
                        if (this.SelectedRangeInfo != null)
                        {
                            deleteLossItem(this.SelectedRangeInfo);
                        }
                    }
                }


            }
            catch (Exception)
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int iStage = CurRangeList.Count, nAmount = 0, nRate = 0 ;
//             try
//             {
//                 int nTemp = Int32.Parse(txtStage.Text);
//                 if (nTemp < 1)
//                 {
//                     txtStage.SelectAll();
//                     txtStage.Focus();
//                     return;
//                 }
//                 iStage = nTemp;
//             }
//             catch
//             {
//                 txtStage.SelectAll();
//                 txtStage.Focus();
//                 return;
//             }

            try
            {
                int nTemp = Int32.Parse(txtAmout.Text);
                if (nTemp < 1)
                {
                    txtAmout.SelectAll();
                    txtAmout.Focus();
                    return;
                }
                nAmount = nTemp;
            }
            catch
            {
                txtAmout.SelectAll();
                txtAmout.Focus();
                return;
            }

            try
            {
                int nTemp = Int32.Parse(txtPercent.Text);
                if (nTemp < 1)
                {
                    txtPercent.SelectAll();
                    txtPercent.Focus();
                    return;
                }
                nRate = nTemp;
            }
            catch
            {
                txtPercent.SelectAll();
                txtPercent.Focus();
                return;
            }

            string amountUnit = "USD";
            string rateUnit = "%";

           if (RangeType == RANGETYPE.CciLoss)
            {
                amountUnit = "이상";
            }
            else if(RangeType == RANGETYPE.PayoffLoss)
            {
                rateUnit = "틱";
            }

            string param = "";

            try
            {
                int nTemp = Int32.Parse(txtRsi.Text);
                if (nTemp < 1)
                {
                    txtRsi.SelectAll();
                    txtRsi.Focus();
                    return;
                }
                param = nTemp.ToString();
            }
            catch
            {
                txtRsi.SelectAll();
                txtRsi.Focus();
                return;
            }

            bool bInserted = false;
            PayoffLossInfo insertInfo = new PayoffLossInfo()
            {
                Stage = iStage,
                StageName = iStage.ToString() + "단계",
                Amount = nAmount,
                AmountUnit = amountUnit,
                Rate = nRate,
                RateUnit = rateUnit,
                Enabled = 1,
                Param = param,
                ActionDelete = "삭제"
            };
            for (int i = 0; i < CurRangeList.Count; i++)
            {
                if (iStage <= CurRangeList[i].Stage)
                {
                    bInserted = true;
                    CurRangeList.Insert(i, insertInfo);
                    break;
                }
            }
            if (!bInserted)
            {
                CurRangeList.Add(insertInfo);
            }

            iStage = 0;
            foreach (PayoffLossInfo lossInfo in CurRangeList)
            {
                lossInfo.Stage = ++iStage;
                lossInfo.StageName = iStage.ToString() + "단계";
            }
            UpdateRangeInfo();

            // txtStage.Text = "";
            txtAmout.Text = "";
            txtPercent.Text = "";
            txtRsi.Text = "";

        }

        private void dgvRangeInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvRangeInfo.CurrentCell.ColumnIndex == 2 || dgvRangeInfo.CurrentCell.ColumnIndex == 4 || (_RangeType != RANGETYPE.PayoffLoss && dgvRangeInfo.CurrentCell.ColumnIndex == 6) ) //Amout Or Rate Or Param
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
