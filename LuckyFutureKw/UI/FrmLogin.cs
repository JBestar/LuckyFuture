using LuckyFuture.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyFuture.UI
{
	public partial class FrmLogin : Form
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int WM_NCHITTEST = 0x84;
		public const int HTCLIENT = 0x1;
		public const int HTCAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		public static readonly FrmLogin Default = new FrmLogin();

		private FrmLogin()
		{
			InitializeComponent();
			CenterToScreen();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (txtId.Text == "")
			{
				txtId.Focus();
				return;
			}
			if (txtPwd.Text == "")
			{
				txtPwd.Focus();
				return;
			}
			var res = AppAuthor.Default.Login(txtId.Text, txtPwd.Text, false);
//			var res = APPLOGINRESULT.SUCCESS;
			string err_msg = "";
			switch (res)
			{
				case APPLOGINRESULT.SUCCESS:
					if (Settings.Default.IsSaveLoginId)
						Settings.Default.LoginId = txtId.Text;
					Settings.Default.Save();
					this.DialogResult = DialogResult.OK;
					break;
				case APPLOGINRESULT.INVALID_ID:
				case APPLOGINRESULT.INVALID_PWD:
					err_msg = "아이디 비번이 정확하지 않습니다.";
					break;
				case APPLOGINRESULT.USAGE_EXPIRED:
					err_msg = "기간이 만기되었습니다.";
					break;
				case APPLOGINRESULT.USER_BLOCKED:
					err_msg = "차단된 계정입니다.";
					break;
				case APPLOGINRESULT.MULTI_USE:
					err_msg = "이미 사용중인 계정입니다.";
					break;
				case APPLOGINRESULT.SESSION_OUT:
					err_msg = "세션이 만료되었습니다.";
					break;
				default:
					err_msg = "알수없는 오류가 발생하였습니다.";
					break;
			}
			if (!string.IsNullOrEmpty(err_msg))
			{
				this.DialogResult = DialogResult.None;
				MessageBox.Show(err_msg, "경고");
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

// 		protected override void WndProc(ref Message m)
// 		{
// 			switch (m.Msg)
// 			{
// 				case WM_NCHITTEST:
// 					base.WndProc(ref m);
// 					if ((int)m.Result == HTCLIENT)
// 						m.Result = (IntPtr)HTCAPTION;
// 					return;
// 			}
// 			base.WndProc(ref m);
// 		}

		private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
			}
		}

		private void FrmLogin_Load(object sender, EventArgs e)
		{
			if(Settings.Default.IsSaveLoginId)
				txtId.Text = Settings.Default.LoginId;
			togSaveId.Checked = Settings.Default.IsSaveLoginId;
		}

		private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					btnLogin_Click(this, new EventArgs());
					break;
				case Keys.Escape:
					btnCancel_Click(this, new EventArgs());
					break;
			}

		}
		private void togSaveId_CheckedChanged(object sender)
		{
			Settings.Default.IsSaveLoginId = togSaveId.Checked;
		}
	}
}
