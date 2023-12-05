using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LuckyFuture.UI
{
    public partial class FrmLog : Form
    {
        public static readonly FrmLog Default = new FrmLog();
        public FrmLog()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            
        }
        public void AddLog(string log)
        {
            if (listLog.Items.Count > 1000)
                listLog.Items.RemoveAt(1000);
            listLog.Items.Add(log);
            AutoScrollList();
        }
        public void AutoScrollList()
        {
            if (!chkScroll.Checked)
                return;
            int cnt = listLog.Items.Count;
            if (cnt>= 30)
            {
                listLog.Items[cnt-1].Selected = true;
                listLog.EnsureVisible(cnt-1);
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

        private void chkScroll_CheckedChanged(object sender, EventArgs e)
        {
            AutoScrollList();
        }
    }
}
