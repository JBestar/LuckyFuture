using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LuckyFuture.Properties;

namespace LuckyFuture.UI
{
    public partial class FrmNotice : Form
    {
        public static readonly FrmNotice Default = new FrmNotice();
        public FrmNotice()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            
        }

        public void SetNotice(string notice)
        {
            txtNotice.Text = notice;

        }
        private void SetNoticeView()
        {
            Settings.Default.ChkNoticeDay = chkDayNotice.Checked;
            if (chkDayNotice.Checked)
            {
                Settings.Default.NoticeViewTime = DateTime.Now;
            }
            Settings.Default.Save();
        }
        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void chkDayNotice_CheckedChanged(object sender, EventArgs e)
        {
            SetNoticeView();
            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetNoticeView();
            this.Hide();
        }
    }
}
