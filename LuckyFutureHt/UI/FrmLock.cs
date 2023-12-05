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
    public partial class FrmLock : Form
    {
        public static readonly FrmLock Default = new FrmLock();
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;

        public FrmLock()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
           
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
            //             string pwd = LockEx.DecryptString(LockEx.key, "7kWqr0Doi2YdYmxqEFoX3g==");//9285
            //             pwd = LockEx.EncryptString(LockEx.key, "9285");//1234  
            //             Trace.TraceError("<FrmLock> encrypt(9285) = {0} ", pwd);
            LockEx.locked = true;
            txtPwd.Text = "";
        }
        protected void CheckKey()
        {
            if (txtPwd.Text.Length < 1)
            {
                MessageBox.Show("비밀번호를 입력해주세요.", "경고");
                return;
            }

            string pwdEnc = LockEx.EncryptString(LockEx.key, txtPwd.Text);
            if (pwdEnc != Settings.Default.LockPwd)
            {
                MessageBox.Show("비밀번호가 틀림니다.", "경고");
            }
            else
            {
                LockEx.locked = false;
            }
            OnChartNoticeEvent(CHART_EVENTTYPE.BETTING_CHANGED);

            Hide();
        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnChartNoticeEvent(CHART_EVENTTYPE.BETTING_CHANGED);

            Hide();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CheckKey();
        }

        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CheckKey();
            }
        }
    }
}
