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
using ChartCtrl;

namespace LuckyFuture.UI
{
    public partial class BandSetting : Form
    {
        public static readonly BandSetting Default = new BandSetting();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        public BandSetting()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            dtClear.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtClear.Format = DateTimePickerFormat.Custom;
            dtClear.Visible = false;

            string[] chartTypeList = { "1분", "60틱", "90틱", "120틱",
                    "240틱", "350틱", "400틱", "600틱", "750틱", "990틱"};
            foreach (string s in chartTypeList)
            {
                cmbBandChart1.Items.Add(s);
                cmbBandChart2.Items.Add(s);
                cmbBandChart3.Items.Add(s);
                cmbBandChart4.Items.Add(s);

            }
        }

        public void loadControls()
        {
            dtClear.Value = DateTime.Now;
            ChangeControls();
        }

        private void ChangeControls()
        {
            chkBandChart.Checked = Settings.Default.BandChart;
            txtBandVal1.Text = Settings.Default.BandVal1.ToString();
            txtBandVal2.Text = Settings.Default.BandVal2.ToString();
            txtBandVal3.Text = Settings.Default.BandVal3.ToString();
            cmbBandChart1.SelectedIndex = Settings.Default.BandChartType1;
            cmbBandChart2.SelectedIndex = Settings.Default.BandChartType2;
            cmbBandChart3.SelectedIndex = Settings.Default.BandChartType3;
            cmbBandChart4.SelectedIndex = Settings.Default.BandChartType4;
            EnableControls();
        }
        private void EnableControls()
        {
            cmbBandChart1.Enabled = chkBandChart.Checked;
            cmbBandChart2.Enabled = chkBandChart.Checked;
            cmbBandChart3.Enabled = chkBandChart.Checked;
            cmbBandChart4.Enabled = chkBandChart.Checked;
            txtBandVal1.Enabled = chkBandChart.Checked;
            txtBandVal2.Enabled = chkBandChart.Checked;
            txtBandVal3.Enabled = chkBandChart.Checked;
        }

        private void Setting_Load(object sender, EventArgs e)
        {

        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.Default.BandChart = chkBandChart.Checked;
            if (chkBandChart.Checked)
            {
                int nVal = 0;
                try
                {
                    nVal = Int32.Parse(txtBandVal1.Text);
                    if (nVal < 0)
                    {
                        txtBandVal1.SelectAll();
                        txtBandVal1.Focus();
                        return;
                    }
                    Settings.Default.BandVal1 = nVal;

                }
                catch
                {
                    txtBandVal1.SelectAll();
                    txtBandVal1.Focus();
                    return;
                }

                try
                {
                    nVal = Int32.Parse(txtBandVal2.Text);
                    if (nVal < 0)
                    {
                        txtBandVal2.SelectAll();
                        txtBandVal2.Focus();
                        return;
                    }
                    Settings.Default.BandVal2 = nVal;
                }
                catch
                {
                    txtBandVal2.SelectAll();
                    txtBandVal2.Focus();
                    return;
                }

                if(Settings.Default.BandVal1 > Settings.Default.BandVal2)
                {
                    MessageBox.Show("등락폭구간을 정확히 입력해주세요", "경고");
                    txtBandVal2.SelectAll();
                    txtBandVal2.Focus();
                    return;
                }

                try
                {
                    nVal = Int32.Parse(txtBandVal3.Text);
                    if (nVal < 0)
                    {
                        txtBandVal3.SelectAll();
                        txtBandVal3.Focus();
                        return;
                    }
                    Settings.Default.BandVal3 = nVal;
                }
                catch
                {
                    txtBandVal3.SelectAll();
                    txtBandVal3.Focus();
                    return;
                }

                if (Settings.Default.BandVal2 > Settings.Default.BandVal3)
                {
                    MessageBox.Show("등락폭구간을 정확히 입력해주세요", "경고");
                    txtBandVal3.SelectAll();
                    txtBandVal3.Focus();
                    return;
                }


                Settings.Default.BandChartType1 = cmbBandChart1.SelectedIndex;
                Settings.Default.BandChartType2 = cmbBandChart2.SelectedIndex;
                Settings.Default.BandChartType3 = cmbBandChart3.SelectedIndex;
                Settings.Default.BandChartType4 = cmbBandChart4.SelectedIndex;

            }
            // Settings.Default.StartChartDt = dtClear.Value;

            Settings.Default.Save();
            OnChartNoticeEvent(CHART_EVENTTYPE.SETTING_CHANGED);

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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void chkBandChart_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
    }
}
