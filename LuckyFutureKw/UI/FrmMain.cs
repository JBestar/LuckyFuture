using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using LuckyFuture.Site;
using LuckyFuture.Models.ValueObjects;
using LuckyFutureLib.Include;
using LuckyFuture.Properties;
using LuckyFuture.Logic;
using ChartCtrl;

namespace LuckyFuture.UI
{
	public partial class FrmMain : Form
	{
		public FrmMain()
		{
            
//             if (LoginForm.ShowDialog() != DialogResult.OK)
// 			{
// 				Environment.Exit(0);
// 			}
// 
//             if (UpdateForm.CheckUpdate())
//             {
//                 if (UpdateForm.ShowDialog() != DialogResult.OK)
//                 {
//                     AppAuthor.Default.Logout();
//                     Thread.Sleep(1000);
//                     Environment.Exit(0);
//                 }
//             }

            InitializeComponent();
			CenterToScreen();
			InitializeComponentEx();
// 			AppAuthor.Default.Start();
// 			AppAuthor.Default.NoticeEvent += OnAuthorNoticeReceive;
// 			AppAuthor.Default.UploadConfig();
			
		}

		// Sub Forms
		private FrmLogin LoginForm { get => FrmLogin.Default; }
		private FrmUpdate UpdateForm { get => FrmUpdate.Default; }
		private FrmChart ChartForm { get => FrmChart.Default; }
		private FrmSetting SettingForm { get => FrmSetting.Default; }

		/// <summary>
		/// Initialize components additionally
		/// </summary>
		void InitializeComponentEx()
		{
			// supported site list
			string[] site_names = { "키움증권" };
			foreach (string site_name in site_names)
				cmbSiteList.Items.Add(site_name);

			this.hopeForm1.Text = AppAuthor.Default.GetAppName() +"(Kiwoom) "+ AppAuthor.Default.GetAppVersion();
			LogPath = AppAuthor.Default.GetAppLogPath();
			WriteLog("<============= 게임시작 =============>");
			// double buffered
			this.dgvCurrentInfo.DoubleBuffered(true);
			this.dgvQuoteInfo.DoubleBuffered(true);
			this.dgvTotalQuoteInfo.DoubleBuffered(true);

			this.QuoteInfo = new List<QuoteInfo>();
			this.CurrentInfo = new List<CurrentInfo>();
			this.ValuationInfo = new List<ValuationInfo>();
			this.ItemPriceInfo = new List<ItemPriceInfo>();
			this.TotalQuoteInfo = new List<TotalQuoteInfo>();
			this.OrderInfo = new BindingList<OrderInfo>();

			//Connection Event Handler
			axKFOpenAPI.OnEventConnect += KF_OnEventConnect;

			int iConnect = 0;

			do
			{
				iConnect = ShowKFOpenLogin();
				if (iConnect < 0)
					Environment.Exit(0);
				else if (iConnect > 0)
                {
					CloseKFLoginDlg();
					Thread.Sleep(1000);
				}
				
			} while (iConnect > 0);

            this.ChartForm.Visible = false;
			txtPassword.Text = "0000";

			ChartForm.SetChartEventHandler(this.OnChartNoticeReceive);
			SettingForm.SetChartEventHandler(this.OnChartNoticeReceive);
			SetDChartInfo();

			

        }

		private FutureSite CurrentSite { get => LogicAuto.Default.CurrentSite; }

		// 호가고정
		public bool IsFixed => this.chkFixed.Checked;
		private string LogPath;
		// 현재 오브젝트
		public List<CurrentInfo> CurrentInfo
		{
			get => (List<CurrentInfo>)this.bsCurrentInfo.DataSource;
			set => this.bsCurrentInfo.DataSource = value;
		}

		// item price info
		public List<ItemPriceInfo> ItemPriceInfo
		{
			get => (List<ItemPriceInfo>)this.bsItemPriceInfo.DataSource;
			set => this.bsItemPriceInfo.DataSource = value;
		}

		// quote info
		public List<QuoteInfo> QuoteInfo
		{
			get => (List<QuoteInfo>)this.bsQuoteInfo.DataSource;
			set => this.bsQuoteInfo.DataSource = value;
		}

		// total quote info
		public List<TotalQuoteInfo> TotalQuoteInfo
		{
			get => (List<TotalQuoteInfo>)this.bsTotalQuoteInfo.DataSource;
			set => this.bsTotalQuoteInfo.DataSource = value;
		}

		// valuation info
		public List<ValuationInfo> ValuationInfo
		{
			get => (List<ValuationInfo>)this.bsValuationInfo.DataSource;
			set => this.bsValuationInfo.DataSource = value;
		}

		public BindingList<OrderInfo> OrderInfo
		{
			get => (BindingList<OrderInfo>)this.bsOrderInfo.DataSource;
			set => this.bsOrderInfo.DataSource = value;
		}

		// selected user account
		public UserAccountInfo SelectedUserAccount
		{
			get
			{
                
                if (CurrentSite == null || this.cmbUserAccounts.SelectedIndex < 0)
                    return null;
                return CurrentSite.CurrentUserAccount;
			}
		}
		public OrderInfo SelectedOrderInfoItem
		{
			get
			{
				DataGridViewRow currentRow = this.dgvOrderInfo.CurrentRow;
				return ((currentRow != null) ? currentRow.DataBoundItem : null) as OrderInfo;
			}
		}

		private void InitListView()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
					InitListView();
                }));
            }
            else
            {
                ValuationInfo = null;
                ItemPriceInfo = null;
                QuoteInfo = null;
                TotalQuoteInfo = null;
                //OrderInfo = null;
				dgvOrderInfo.DataSource = null;
				CurrentInfo = null;
				//Invalidate();
            }
            
		}

		private void UpdateValuationInfo()
		{
			if (CurrentSite != null)
			{
				ValuationInfo = null;
				ValuationInfo = CurrentSite.ValuationList;

			}
		}

		private void UpdateCurrentInfo()
		{
			if (CurrentSite != null)
			{
				this.CurrentInfo = null;
				this.CurrentInfo = CurrentSite.CurrentList;
				
				CurrentInfo current = CurrentSite.Current;
				if(current != null)
					ChartForm.SetRTValue(current.CurrentPrice, current.Time, 1/*current.ConclusionQty*/);

			}
		}

        private void InitialCurrentInfo()
        {
            if (CurrentSite != null)
            {
                this.CurrentInfo = null;
                this.CurrentInfo = CurrentSite.CurrentList;
                for(int i= CurrentSite.CurrentList.Count -1 ; i>= 0; i--)
                    ChartForm.SetRTValue(CurrentSite.CurrentList[i].CurrentPrice, CurrentSite.CurrentList[i].Time, CurrentSite.CurrentList[i].ConclusionQty);

            }
        }

        private void UpdateItemPriceInfo()
		{
			if (CurrentSite != null)
			{
				ItemPriceInfo = null;
				this.ItemPriceInfo = CurrentSite.ItemPriceList;

			}
		}

		private void UpdateQuoteInfo()
		{
			if (CurrentSite != null)
			{
				this.QuoteInfo = CurrentSite.QuoteList;

				dgvQuoteInfo.Invalidate();
			}


		}

		private void UpdateTotalQuoteInfo()
		{
			if (CurrentSite != null)
			{
				this.TotalQuoteInfo = CurrentSite.TotalQuoteList;

				dgvTotalQuoteInfo.Invalidate();
			}


		}

		private int mOrderCnt = 0;
		private void UpdateOrderInfo()
		{
			if (CurrentSite != null)
			{
				int nOrderListCnt = CurrentSite.OrderList.Count;
				if (nOrderListCnt > 0 || mOrderCnt != nOrderListCnt)
				{
					this.OrderInfo = new BindingList<OrderInfo>(CurrentSite.OrderList);
					dgvOrderInfo.DataSource = this.OrderInfo;
					mOrderCnt = nOrderListCnt;
				
				}

			}
		}

		public void SetQuoteInfoTopRow(int topRow)
		{
			if (this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex >= 0)
			{
				int num = this.dgvQuoteInfo.DisplayedRowCount(true);
				int num2 = (num > 1) ? (topRow - (num / 2 - 1)) : 0;
				this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex = ((num2 >= 0) ? num2 : 0);
			}
		}

		private void OnSiteLogReceive(object sender, FutureSiteLogArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					BeginInvoke(new MethodInvoker(delegate ()
					{
						this.OnSiteLogReceive(sender, e);
					}));
				}
				catch (Exception ex)
				{
					//TraceEx.TraceException(ex);
					string exMessage = ex.Message;
					return;
				}
			}
			else
				AddLog(e.Log);
		}

		private void OnSiteNoticeReceive(object sender, FutureSiteEventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					BeginInvoke(new MethodInvoker(delegate ()
					{
						this.OnSiteNoticeReceive(sender, e);
					}));
				}
				catch (Exception ex)
				{
					string exMessage = ex.Message;
					return;
				}
			}
			else if (e.Data is SITE_NOTICEEVENTTYPE)
			{
				try
				{
					SITE_NOTICEEVENTTYPE noticeType = (SITE_NOTICEEVENTTYPE)e.Data;
					switch (noticeType)
					{
						case SITE_NOTICEEVENTTYPE.LOGIN:
							EnableControls();
							break;
						case SITE_NOTICEEVENTTYPE.PREPARE:							
							UpdateValuationInfo();
							UpdateOrderInfo();
							ShowBalance(noticeType);
							break;
                        case SITE_NOTICEEVENTTYPE.VALUATION:
                            UpdateValuationInfo();
                            ShowBalance(noticeType);
							break;
						// current info
						case SITE_NOTICEEVENTTYPE.INITCURRENT:
							InitialCurrentInfo();
							break;
						case SITE_NOTICEEVENTTYPE.CURRENT:
							UpdateCurrentInfo();
							UpdateItemPriceInfo();
							UpdateOrderInfo();
							UpdateValuationInfo();
							ShowBalance(noticeType);
							break;
						// quote info
						case SITE_NOTICEEVENTTYPE.QUOTE:
							UpdateQuoteInfo();
							if (this.IsFixed && CurrentSite != null && CurrentSite.Ask1Row != null)
								SetQuoteInfoTopRow(CurrentSite.Ask1Row.QuoteInfoId);
							UpdateTotalQuoteInfo();
							//UpdateValuationInfo();
							//ShowBalance();
							break;
						// order info
						case SITE_NOTICEEVENTTYPE.ORDER:
							//UpdateCurrentInfo();
							//UpdateValuationInfo();
							UpdateOrderInfo();
							//ShowBalance();
							break;
						case SITE_NOTICEEVENTTYPE.NOLOGIN:
							ShowKFOpenLogin();
							break;
                        case SITE_NOTICEEVENTTYPE.REDRAW:
							ChartForm.Redraw();
                            break;
                        case SITE_NOTICEEVENTTYPE.LOGOUT:
							ChartForm.ResetChart();
							
							break;
						case SITE_NOTICEEVENTTYPE.STOP:
							EnableControls();
							break;
					}
				}
				catch (Exception ex)
				{
					AddLog(ex.Message);
				}
			}
		}

		// 로그 현시
		public void AddLog(string fmt, params Object[] param_list)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(delegate ()
				{
					this.AddLog(fmt, param_list);
				}));
			}
			else
			{
				string log = String.Format(fmt, param_list);
				log = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) + log;
				listLog.Items.Add(log);
				WriteLog(log);


			}
		}

		public void WriteLog(string strLog)
        {
            try
            {
                if (LogPath.Length > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(LogPath, true))
                    {
                        outputFile.WriteLine(strLog);
                    }
                }
            }
            catch (Exception ex)
            { string error = ex.Message; }

        }

        private void OnChartNoticeReceive(object sender, ChartEventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					BeginInvoke(new MethodInvoker(delegate ()
					{
						this.OnChartNoticeReceive(sender, e);
					}));
				}
				catch (Exception ex)
				{
					string exMessage = ex.Message;
					return;
				}
			}
			else if (e.Data is CHART_EVENTTYPE)
			{
				try
				{
					CHART_EVENTTYPE noticeType = (CHART_EVENTTYPE)e.Data;
					switch (noticeType)
					{
						case CHART_EVENTTYPE.DRAWING_CHANGED:
							if (CurrentSite != null)
								CurrentSite.RequestRChart();
							break;
                        case CHART_EVENTTYPE.BETTING_CHANGED:
							SetDChartInfo();
                            
                            break;
                    }

                }
				catch (Exception) { }

			}
		}

        private void OnAuthorNoticeReceive(object sender, AuthorEventArgs e)
        {
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(delegate ()
                    {
                        this.OnAuthorNoticeReceive(sender, e);
                    }));
                }
                catch (Exception ex)
                {
                    string exMessage = ex.Message;
                    return;
                }
            }
            else if (e.Data is AUTHOR_EVENTTYPE)
            {
                try
                {
                    AUTHOR_EVENTTYPE noticeType = (AUTHOR_EVENTTYPE)e.Data;
                    switch (noticeType)
                    {
                        case AUTHOR_EVENTTYPE.LOGOUT:

                            btnLogout_Click(this, new EventArgs());
                            AppAuthor.Default.Logout();
                            MessageBox.Show("사용이 중지되었습니다.\n프로그램을 종료합니다.", "경고");
							CloseKFLoginDlg();
							Thread.Sleep(1000);
                            Environment.Exit(0);

                            break;
                    }

                }
                catch (Exception) { }

            }
        }
        public void SetDChartInfo()
        {
            if (ChartForm.SetDChartType((CHARTTYPE)Settings.Default.ChartType))
            {
                if (CurrentSite != null)
                    CurrentSite.RequestDChart();
            }
            

        }

        public List<DItem> GetCandleList(BETTYPE betType, int nCandles = 0)
		{
			return ChartForm.GetCandleList(betType, nCandles);
		}

		private int ShowKFOpenLogin()
        {
            Process procKFLogin = Common.GetKFOpenLoginProc();
            if (procKFLogin == null)
                return axKFOpenAPI.CommConnect(1);
			return 1;
		}

		private void ShowUserInfo()
		{
			this.cmbUserAccounts.Items.Clear();
			this.txtUserName.Text = "";

            string sUserId = axKFOpenAPI.GetLoginInfo("USER_ID");
            string sUserName = axKFOpenAPI.GetLoginInfo("USER_NAME");

            if (String.IsNullOrEmpty(sUserId))
            {
				return;
            }

			txtUserName.Text = sUserName;
			txtId.Text = sUserId;
			AppAuthor.Default.SetUserAccount(sUserName);
			string sAccList = axKFOpenAPI.GetLoginInfo("ACCNO");

            string[] accounts = sAccList.Split(';');
			for (int i = 0; i < accounts.Length; i++)
            {
				if (accounts[i].Trim().Length > 0)					
					cmbUserAccounts.Items.Add(accounts[i].Trim());
            }
			if(cmbUserAccounts.Items.Count > 0)
            {
				cmbUserAccounts.SelectedIndex = 0;
			}

        }

		private void ShowBalance(SITE_NOTICEEVENTTYPE noticeType)
		{
			UserAccountInfo userAccount = this.SelectedUserAccount;
			if (userAccount != null)
			{
				long lBalance = userAccount.Balance + this.ValuationInfo[0].TotalValuation;
				txtBalance.Text = lBalance.ToString("N0");
				txtSave.Text = userAccount.Balance.ToString("N0");
				if (noticeType == SITE_NOTICEEVENTTYPE.PREPARE)
                {
					AppAuthor.Default.SetUserAccount("", userAccount.Balance - ValuationInfo[0].TotalProfit, userAccount.Balance);
				} else
                {
					AppAuthor.Default.SetUserAccount("", userAccount.Balance - ValuationInfo[0].TotalProfit, userAccount.Balance);
				}

			}
		}

		private void EnableControls()
		{
			bool running = LogicAuto.Default.IsRunning;
			//txtId.Enabled = !running;
			//txtPassword.Enabled = !running;
			btnLogin.Enabled = !running;
			btnLogout.Enabled = running;
		}

		// Event handlers
		private void FrmMain_Load(object sender, EventArgs e)
		{
			// auto mode (auto / manual)
			this.chkAutoMode.Checked = Settings.Default.IsAutoMode;
			EnableControls();

		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.Save();
			AppAuthor.Default.Stop();
			LogicAuto.Default.Stop();
			
			InitListView();
			AppAuthor.Default.Logout();

			if (!ChartForm.IsDisposed)
            {
				ChartForm.SaveSetting();
				ChartForm.Close();
			}
				
			if (!SettingForm.IsDisposed)
				SettingForm.Close();

			//Close KFLogin Dialog
			CloseKFLoginDlg();
			e.Cancel = false;

		}

		private void CloseKFLoginDlg()
        {
            Process procKFLogin = Common.GetKFOpenLoginProc();
            if (procKFLogin != null)
                procKFLogin.Kill();

        }

        private void btnLogin_Click(object sender, EventArgs e)
		{
			if (LogicAuto.Default.IsRunning)
				return;
							
			if (string.IsNullOrEmpty(txtPassword.Text))
			{
				txtPassword.Focus();
				return;
			}

			string txtAccountId = "";
			if(cmbUserAccounts.Items.Count > 0)
            {
				txtAccountId = cmbUserAccounts.SelectedItem.ToString();
			} 
				
			if (LogicAuto.Default.Start(
				txtAccountId,
				txtPassword.Text,
				this.OnSiteLogReceive, 
				this.OnSiteNoticeReceive,
				this, axKFOpenAPI)
			)
			{
				AddLog("시작 중입니다.");
			}
			else
				AddLog("잠시 후에 다시 접속해주세요.");
				
			
			EnableControls();
		}
		private void btnLogout_Click(object sender, EventArgs e)
		{
			
			if (LogicAuto.Default.IsRunning)
			{
				LogicAuto.Default.Stop();
				EnableControls();
				AddLog("접속해제됨");
				InitListView();
				Invalidate();
			}
			
		}

		private void dgvCurrentInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (this.CurrentInfo != null && this.CurrentInfo.Count > e.RowIndex)
				{
					if (e.ColumnIndex == 1)
					{
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.SELL)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.BUY)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
							return;
						}
					}
					else if (e.ColumnIndex == 2)
					{
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.SELL)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.BUY)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						}
					}
				}
			}
			catch (Exception) { }
		}

		private void dgvQuoteInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (CurrentSite != null)
				{
					switch (e.ColumnIndex)
					{
						case 0:
							e.CellStyle.BackColor = Color.FromArgb(192, 217, 241);
							break;
						case 1:
						case 2:
							if (e.RowIndex < CurrentSite.Bid1Row.QuoteInfoId || !(CurrentSite.Bid1Row.BidQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(215, 233, 255) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 3:
							if (e.Value != null)
							{
								string a = e.Value.ToString();
								e.CellStyle.ForeColor = a == "시" ? Color.Green :
									(a == "고" ? Color.Red :
									(a == "저") ? Color.Blue : Color.Black);
							}
							if (CurrentSite.PositionRow != null && e.RowIndex == CurrentSite.PositionRow.QuoteInfoId && CurrentSite.PositionTradeType == TRADETYPE.SELL)
							{
								e.CellStyle.BackColor = Color.FromArgb(0, 0, 192);
							}
							else if (CurrentSite.PositionRow != null && e.RowIndex == CurrentSite.PositionRow.QuoteInfoId && CurrentSite.PositionTradeType == TRADETYPE.BUY)
							{
								e.CellStyle.BackColor = Color.FromArgb(192, 0, 0);
							}
							break;
						case 4:
							if (e.RowIndex == CurrentSite.CurrentPriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(255, 255, 128);
								e.CellStyle.ForeColor = Color.Red;
								return;
							}
							if (e.RowIndex >= CurrentSite.HighPriceRow.QuoteInfoId && e.RowIndex <= CurrentSite.LowPriceRow.QuoteInfoId && e.RowIndex < CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(255, 227, 227);
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							if (e.RowIndex <= CurrentSite.LowPriceRow.QuoteInfoId && e.RowIndex >= CurrentSite.HighPriceRow.QuoteInfoId && e.RowIndex > CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(215, 233, 255);
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							if (e.RowIndex == CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.LightGray;
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 5:
							if (e.RowIndex > CurrentSite.Ask1Row.QuoteInfoId || !(CurrentSite.Ask1Row.AskQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(255, 227, 227) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 6:
							if (e.RowIndex > CurrentSite.Ask1Row.QuoteInfoId || !(CurrentSite.Ask1Row.AskQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(255, 227, 227) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 7:
							e.CellStyle.BackColor = Color.FromArgb(252, 213, 181);
							break;
					}
				}
			}
			catch (Exception) { }
		}

		private void dgvQuoteInfo_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				int num = this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex + this.dgvQuoteInfo.DisplayedRowCount(true) / 2;
				if (e.RowIndex == num && this.IsFixed)
				{
					using (Pen pen = new Pen(Color.Black, 1f))
					{
						e.Graphics.DrawLine(pen, e.CellBounds.X, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Top - 1);
					}
				}
			}
			catch (Exception) { }
			// 			if (e.RowIndex == this._mouseOverQuoteInfoRowIndex && e.ColumnIndex >= 0 && e.RowIndex >= 0 && e.ColumnIndex == 5)
			// 			{
			// 				Rectangle rectangle = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 2, e.CellBounds.Height - 2);
			// 				using (Brush brush = new SolidBrush(this.dgvQuoteInfo.GridColor))
			// 				{
			// 					using (Brush brush2 = new SolidBrush(e.CellStyle.BackColor))
			// 					{
			// 						using (Pen pen2 = new Pen(brush))
			// 						{
			// 							e.Graphics.FillRectangle(brush2, e.CellBounds);
			// 							e.Graphics.DrawLine(pen2, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			// 							e.Graphics.DrawLine(pen2, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
			// 							e.Graphics.DrawRectangle(Pens.Red, rectangle);
			// 							if (e.Value != null)
			// 							{
			// 								string text = "{0:0.";
			// 								for (int i = 0; i < this.Item.PricePrecision; i++)
			// 								{
			// 									text += "0";
			// 								}
			// 								text += "}";
			// 								if (e.RowIndex == this.CurrentPriceRow.QuoteInfoId)
			// 								{
			// 									e.Graphics.DrawString(string.Format(text, e.Value), e.CellStyle.Font, Brushes.Red, rectangle, new StringFormat
			// 									{
			// 										Alignment = StringAlignment.Center,
			// 										LineAlignment = StringAlignment.Far
			// 									});
			// 								}
			// 								else
			// 								{
			// 									e.Graphics.DrawString(string.Format(text, e.Value), e.CellStyle.Font, Brushes.Black, rectangle, new StringFormat
			// 									{
			// 										Alignment = StringAlignment.Center,
			// 										LineAlignment = StringAlignment.Far
			// 									});
			// 								}
			// 							}
			// 							e.Handled = true;
			// 						}
			// 					}
			// 				}
			// 			}
			// 			if (e.ColumnIndex == this._mouseOverQuoteInfoColumnIndex && e.RowIndex == this._mouseOverQuoteInfoRowIndex && e.ColumnIndex >= 0 && e.RowIndex >= 0 && (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 8 || e.ColumnIndex == 9))
			// 			{
			// 				Rectangle rectangle2 = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 2, e.CellBounds.Height - 2);
			// 				using (Brush brush3 = new SolidBrush(this.dgvQuoteInfo.GridColor))
			// 				{
			// 					using (Brush brush4 = new SolidBrush(e.CellStyle.BackColor))
			// 					{
			// 						using (Pen pen3 = new Pen(brush3))
			// 						{
			// 							e.Graphics.FillRectangle(brush4, e.CellBounds);
			// 							e.Graphics.DrawLine(pen3, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			// 							e.Graphics.DrawLine(pen3, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
			// 							e.Graphics.DrawRectangle(Pens.Red, rectangle2);
			// 							if (e.Value != null)
			// 							{
			// 								if (e.ColumnIndex == 0)
			// 								{
			// 									e.Graphics.DrawString(Convert.ToString(e.Value), e.CellStyle.Font, (Convert.ToInt32(this.TotalQuoteInfo[0].TotalSellMit) > this.SellAcceptable) ? Brushes.Red : Brushes.Black, rectangle2, new StringFormat
			// 									{
			// 										Alignment = StringAlignment.Center,
			// 										LineAlignment = StringAlignment.Far
			// 									});
			// 								}
			// 								else if (e.ColumnIndex == 9)
			// 								{
			// 									e.Graphics.DrawString(Convert.ToString(e.Value), e.CellStyle.Font, (Convert.ToInt32(this.TotalQuoteInfo[0].TotalBuyMit) > this.BuyAcceptable) ? Brushes.Red : Brushes.Black, rectangle2, new StringFormat
			// 									{
			// 										Alignment = StringAlignment.Center,
			// 										LineAlignment = StringAlignment.Far
			// 									});
			// 								}
			// 								else
			// 								{
			// 									e.Graphics.DrawString(Convert.ToString(e.Value), e.CellStyle.Font, Brushes.Black, rectangle2, new StringFormat
			// 									{
			// 										Alignment = StringAlignment.Center,
			// 										LineAlignment = StringAlignment.Far
			// 									});
			// 								}
			// 							}
			// 							e.Handled = true;
			// 						}
			// 					}
			// 				}
			// 			}
		}

		private void chkFixed_CheckedChanged(object sender, EventArgs e)
		{
			if (CurrentSite != null && this.QuoteInfo != null && this.QuoteInfo.Any<QuoteInfo>() && this.IsFixed)
			{
				if (CurrentSite.CurrentPriceRow != null)
				{
					int num = (from q in this.QuoteInfo
							   where q.AskQty != null
							   orderby q.QuoteInfoId descending
							   select q.QuoteInfoId).FirstOrDefault<int>();
					this.SetQuoteInfoTopRow(
						(num != 0) ? num : (CurrentSite != null ? CurrentSite.CurrentPriceRow.QuoteInfoId : 0)
					);
				}
			}
			dgvQuoteInfo.Invalidate();
		}

		private void btnChat_Click(object sender, EventArgs e)
		{
			if (!ChartForm.Visible)
				ChartForm.Show(this);

		}

		private void btnSetting_Click(object sender, EventArgs e)
		{
			if (!SettingForm.Visible)
			{
				SettingForm.loadControls();
				SettingForm.Show(this);
			}

		}

		private void dgvQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			dgvQuoteInfo.ClearSelection();
		}

		private void dgvCurrentInfo_SelectionChanged(object sender, EventArgs e)
		{
			dgvCurrentInfo.ClearSelection();
		}

		private void dgvTotalQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			dgvTotalQuoteInfo.ClearSelection();
		}

		private void dgvItemPriceInfo_SelectionChanged(object sender, EventArgs e)
		{
			dgvItemPriceInfo.ClearSelection();
		}

		private void dgvItemPriceInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == 0)
				{
					e.CellStyle.BackColor = SystemColors.Control;
					return;
				}
				if (e.ColumnIndex == 4)
				{
					e.CellStyle.BackColor = SystemColors.Control;
					return;
				}
				if (e.ColumnIndex == 2)
				{
					if (e.Value != null)
					{
						if (Convert.ToDouble(e.Value) < 0.0)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (Convert.ToDouble(e.Value) > 0.0)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
							return;
						}
						e.CellStyle.ForeColor = Color.Black;
						return;
					}
				}
				else if (e.ColumnIndex == 3 && e.Value != null)
				{
					if (Convert.ToDouble(e.Value) < 0.0)
					{
						e.CellStyle.ForeColor = Color.Blue;
						return;
					}
					if (Convert.ToDouble(e.Value) > 0.0)
					{
						e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						return;
					}
					e.CellStyle.ForeColor = Color.Black;
				}
			}
			catch (Exception) { }
		}

		private void cmbUserAccounts_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CurrentSite != null && this.SelectedUserAccount != null)
				CurrentSite.CurrentUserAccount = this.SelectedUserAccount;
		}

		private void dgvOrderInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvOrderInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvOrderInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.Value != null)
				{
					if (e.ColumnIndex == 2)
					{
						if (e.Value.ToString().StartsWith("매도"))
							e.CellStyle.ForeColor = Color.Blue;
						else if (e.Value.ToString().StartsWith("매수"))
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						else
							e.CellStyle.ForeColor = Color.Black;
					}
					else if (e.ColumnIndex == 5)
					{
						if (Convert.ToInt64(e.Value) < 0L)
							e.CellStyle.ForeColor = Color.Blue;
						else if (Convert.ToInt64(e.Value) > 0L)
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						else
							e.CellStyle.ForeColor = Color.Black;
					}
				}
			}
			catch (Exception) { }
		}

		private void dgvValuationInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == 0)
				{
					if (e.Value != null)
					{
						if (e.Value.ToString().StartsWith("매도"))
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (e.Value.ToString().StartsWith("매수"))
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
							return;
						}
						e.CellStyle.ForeColor = Color.Black;
						return;
					}
				}
				else if ((e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4) && e.Value != null)
				{
					if (Convert.ToInt64(e.Value) < 0L)
					{
						e.CellStyle.ForeColor = Color.Blue;
						return;
					}
					if (Convert.ToInt64(e.Value) > 0L)
					{
						e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						return;
					}
					e.CellStyle.ForeColor = Color.Black;
				}
			}
			catch (Exception) { }
		}

		private void dgvOrderInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (Settings.Default.IsAutoMode)
				return;

			try
			{
				if (e.RowIndex >= 0 && this.OrderInfo[e.RowIndex] == this.SelectedOrderInfoItem)
				{
					if (e.ColumnIndex == 6 && CurrentSite != null && this.SelectedUserAccount != null)
					{
						if (this.SelectedOrderInfoItem != null && this.SelectedOrderInfoItem.OrderType != string.Empty)
						{
							if (this.SelectedOrderInfoItem.OrderType == "체결")
							{
								CurrentSite.LiquidateOrder(this.SelectedOrderInfoItem);
							}
							else if (this.SelectedOrderInfoItem.OrderType == "미체결")
							{
								CurrentSite.CancelOrder(this.SelectedOrderInfoItem);
							}
						}
					}
				}
			}
			catch
			{

			}
		}

		private void dgvQuoteInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.RowIndex < 0)
					return;

				if (Settings.Default.IsAutoMode || CurrentSite == null)
					return;

				QuoteInfo quoteInfo = this.QuoteInfo[e.RowIndex];

				switch (e.ColumnIndex)
				{
					case 0:
						CurrentSite.DoSellOrder(quoteInfo, Settings.Default.OrderCount, false);
						break;
					case 7:
						CurrentSite.DoBuyOrder(quoteInfo, Settings.Default.OrderCount, false);
						break;
				}
			}
			catch (Exception) { }
		}

		private void chkAutoMode_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.IsAutoMode = chkAutoMode.Checked;

			chkAutoMode.BackColor = chkAutoMode.Checked ? Color.FromArgb(0, 142, 71) : Color.FromArgb(228, 94, 86);
			chkAutoMode.Text = chkAutoMode.Checked ? "자 동" : "수 동";
			
        }

        private void dgvValuationInfo_SelectionChanged(object sender, EventArgs e)
		{
			dgvValuationInfo.ClearSelection();
		}

        private void KF_OnEventConnect(object sender, AxKFOpenAPILib._DKFOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0)
            {
				AddLog("로그인 성공");
				ShowUserInfo();
			}
            else
            {
				//if(e.nErrCode == )
				AddLog("로그인 실패");

            }
        }


    


    }
}
