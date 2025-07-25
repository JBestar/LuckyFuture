using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChartCtrl.Properties;

namespace ChartCtrl
{
    public partial class LineSetting : Form
    {
        public static readonly LineSetting Default = new LineSetting();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        public LineSetting()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            for (int i = 1; i < 6; i++)
                cmbLineWidth.Items.Add(i);
        }

        public void loadControls()
        {
            cmbLineWidth.SelectedItem = Settings.Default.AvgLineWidth;
            chkAvgShow.Checked = Settings.Default.AvgLine;
            chkAvg5.Checked = Settings.Default.Avg5On;
            chkAvg10.Checked = Settings.Default.Avg10On;
            chkAvg20.Checked = Settings.Default.Avg20On;
            chkAvg60.Checked = Settings.Default.Avg60On;
            chkAvg120.Checked = Settings.Default.Avg120On;
            chkAvg200.Checked = Settings.Default.Avg200On;
            textABox1.BackColor = Settings.Default.AvgLineColor1;
            textABox2.BackColor = Settings.Default.AvgLineColor2;
            textABox3.BackColor = Settings.Default.AvgLineColor3;
            textABox4.BackColor = Settings.Default.AvgLineColor4;
            textABox5.BackColor = Settings.Default.AvgLineColor5;
            textABox6.BackColor = Settings.Default.AvgLineColor6;

            chkBoShow.Checked = Settings.Default.BoLine;
            textBBox1.BackColor = Settings.Default.BoLineColor1;
            textBBox2.BackColor = Settings.Default.BoLineColor2;
            textBBox3.Text = Settings.Default.BoLineAdjustR.ToString();

            chkBollShow.Checked = Settings.Default.BollBand;
            textBollMid.BackColor = Settings.Default.BollMidColor;
            textBollUp.BackColor = Settings.Default.BollUpColor;
            textBollDown.BackColor = Settings.Default.BollDownColor;

            enableControls();
        }

        public void enableControls()
        {
            chkAvg5.Enabled = chkAvgShow.Checked;
            chkAvg10.Enabled = chkAvgShow.Checked;
            chkAvg20.Enabled = chkAvgShow.Checked;
            chkAvg60.Enabled = chkAvgShow.Checked;
            chkAvg120.Enabled = chkAvgShow.Checked;
            chkAvg200.Enabled = chkAvgShow.Checked;
            if (!chkAvgShow.Checked)
            {
                chkAvg5.Checked = false;
                chkAvg10.Checked = false;
                chkAvg20.Checked = false;
                chkAvg60.Checked = false;
                chkAvg120.Checked = false;
                chkAvg200.Checked = false;
            }
        }

        public void SetChartEventHandler(EventHandler<ChartEventArgs> chartEvent)
        {
            ChartNoticeEvent += chartEvent;
        }

        private void AvgLineSetting_Load(object sender, EventArgs e)
        {

        }

        private void AvgLineSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int adjust = 0;
            try
            {
                adjust = Int32.Parse(textBBox3.Text);
                if (adjust < 0 || adjust > 100)
                {
                    textBBox3.SelectAll();
                    textBBox3.Focus();
                    return;
                }
            }
            catch
            {
                textBBox3.SelectAll();
                textBBox3.Focus();
                return;
            }
            CtrlProperty._nBoLineAdjust = adjust;
            Settings.Default.AvgLine = chkAvgShow.Checked;
            Settings.Default.Avg5On = chkAvg5.Checked;
            Settings.Default.Avg10On = chkAvg10.Checked;
            Settings.Default.Avg20On = chkAvg20.Checked;
            Settings.Default.Avg60On = chkAvg60.Checked;
            Settings.Default.Avg120On = chkAvg120.Checked;
            Settings.Default.Avg200On = chkAvg200.Checked;

            Settings.Default.AvgLineColor1 = textABox1.BackColor;
            Settings.Default.AvgLineColor2 = textABox2.BackColor;
            Settings.Default.AvgLineColor3 = textABox3.BackColor;
            Settings.Default.AvgLineColor4 = textABox4.BackColor;
            Settings.Default.AvgLineColor5 = textABox5.BackColor;
            Settings.Default.AvgLineColor6 = textABox6.BackColor;

            Settings.Default.AvgLineWidth = (int)cmbLineWidth.SelectedItem;
            Settings.Default.BoLine = chkBoShow.Checked;
            Settings.Default.BoLineColor1 = textBBox1.BackColor;
            Settings.Default.BoLineColor2 = textBBox2.BackColor;

            Settings.Default.BollBand = chkBollShow.Checked;
            Settings.Default.BollMidColor = textBollMid.BackColor;
            Settings.Default.BollUpColor = textBollUp.BackColor;
            Settings.Default.BollDownColor = textBollDown.BackColor;

            Settings.Default.Save();
            OnChartNoticeEvent(CHART_EVENTTYPE.SETTING_CHANGED);
            Hide();
        }

        protected virtual void OnChartNoticeEvent(Object obj)
        {
            if (ChartNoticeEvent != null)
                ChartNoticeEvent(this, new ChartEventArgs(obj));
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox1.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox1.BackColor = colorPickDlg.Color;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox2.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox2.BackColor = colorPickDlg.Color;
            }
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox3.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox3.BackColor = colorPickDlg.Color;
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox4.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox4.BackColor = colorPickDlg.Color;
            }
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox5.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox5.BackColor = colorPickDlg.Color;
            }
        }

        private void textBBox1_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textBBox1.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textBBox1.BackColor = colorPickDlg.Color;
            }
        }

        private void textBBox2_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textBBox2.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textBBox2.BackColor = colorPickDlg.Color;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void chkAvgShow_CheckedChanged(object sender, EventArgs e)
        {
            enableControls();
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textABox6.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textABox6.BackColor = colorPickDlg.Color;
            }
        }

        private void textBollMid_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textBollMid.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textBollMid.BackColor = colorPickDlg.Color;
            }
        }

        private void textBollUp_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textBollUp.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textBollUp.BackColor = colorPickDlg.Color;
            }
        }

        private void textBollDown_Click(object sender, EventArgs e)
        {
            colorPickDlg.Color = textBollDown.BackColor;
            if (colorPickDlg.ShowDialog() == DialogResult.OK)
            {
                textBollDown.BackColor = colorPickDlg.Color;
            }
        }
    }
}
