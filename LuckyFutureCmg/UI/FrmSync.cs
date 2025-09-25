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
using ChartCtrl;

namespace LuckyFuture.UI
{
    public partial class FrmSync : Form
    {
        public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        List<MemberInfo> CurMemberList = new List<MemberInfo>();
        public FrmSync()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();
        }
        private void InitializeComponentEx()
        {
            dgvMemberInfo.DoubleBuffered(true);
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
        public List<MemberInfo> MemberList
        {
            get => (List<MemberInfo>)this.bsMemInfo.DataSource;
            set => this.bsMemInfo.DataSource = value;
        }

        public MemberInfo SelectedMemberInfo
        {
            get
            {
                if (this.dgvMemberInfo.CurrentCell.RowIndex < 0)
                {
                    return null;
                }
                return this.MemberList[this.dgvMemberInfo.CurrentCell.RowIndex];
            }
        }


        public void loadControls()
        {
            txtUser.Text = "";
            chkSync.Checked = Settings.Default.SyncChart;
            lock (AppConfig._lockObj)
            {
                
                CurMemberList.Clear();
                MemberInfo memberInfo = null;
                foreach (MemberInfo memInfo in AppConfig.SyncMemInfos)
                {
                    memberInfo = new MemberInfo
                    {
                        Id = memInfo.Id,
                        Delete = memInfo.Delete,
                    };

                    CurMemberList.Add(memberInfo);
                }
            }

            UpdateInfo();

        }
        private void UpdateInfo()
        {

            lock (AppConfig._lockObj)
            {
                Settings.Default.SyncMembers.Clear();
                AppConfig.SyncMemInfos.Clear();

                MemberInfo memberInfo = null;
                foreach (MemberInfo memInfo in CurMemberList)
                {

                    memberInfo = new MemberInfo
                    {
                        Id = memInfo.Id,
                        Delete = memInfo.Delete
                    };
                    Settings.Default.SyncMembers.Add(memInfo.Id);
                    AppConfig.SyncMemInfos.Add(memberInfo);
                }

            }

            if (this.dgvMemberInfo.RowCount >= 0)
            {
                MemberList = null;
                MemberList = CurMemberList;
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

            Settings.Default.Save();

            if (MessageBox.Show("회원들의 프로그램에서 차트를 동기화하시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                     == DialogResult.Yes)
            {
                OnChartNoticeEvent(CHART_EVENTTYPE.SYNCCHART_CHANGED);
            }
            Hide();
        }

        private void deleteMember(MemberInfo lossInfo)
        {
            CurMemberList.Remove(lossInfo);
            UpdateInfo();
        }
        private void dgvRangeInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && this.MemberList[e.RowIndex] == this.SelectedMemberInfo)
                {
                    if (e.ColumnIndex == 1)
                    {
                        if (this.SelectedMemberInfo != null)
                        {
                            deleteMember(this.SelectedMemberInfo);
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
            if(txtUser.TextLength < 1)
            {
                txtUser.Focus();
                return;
            }

            foreach(MemberInfo memberInfo in CurMemberList)
            {
                if(memberInfo.Id == txtUser.Text.Trim())
                {
                    MessageBox.Show("존재하는 아이디입니다.", "경고");
                    txtUser.Text = "";
                    return;
                } else if(AppAuthor.Default.Uid == txtUser.Text.Trim())
                {
                    MessageBox.Show("본인 아이디입니다.", "경고");
                    txtUser.Text = "";
                    return;
                }
            }


            MemberInfo memInfo = new MemberInfo()
            {
                Id = txtUser.Text,
                Delete = "삭제",
            };
            CurMemberList.Add(memInfo);

            UpdateInfo();
            txtUser.Text = "";
        }

        private void dgvRangeInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvMemberInfo.CurrentCell.ColumnIndex == 0) 
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

        private void chkSync_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.SyncChart = chkSync.Checked;
        }
    }
}
