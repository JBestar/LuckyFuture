using ChartCtrl;
using LuckyFuture.Logic;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFuture.Site;
using LuckyFutureLib.Include;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyFuture.UI
{
    public partial class FrmMain : Form
	{
        // 1. 클래스 최상단 멤버 변수 영역에 추가
        public static FrmMain Default; // ★ 여기 추가
        public FrmMain()
		{
            // 2. 생성자 가장 첫 줄에서 자기 자신을 대입
            Default = this; // ★ 여기 추가
            DeleteBeforeFiles();
			if (LoginForm.ShowDialog() != DialogResult.OK)
			{
				Environment.Exit(0);
			}

            if (UpdateForm.CheckUpdate())
            {
                if (UpdateForm.ShowDialog() != DialogResult.OK)
                {
					AppAuthor.Default.Logout();
					Thread.Sleep(1000);
					Environment.Exit(0);
                }
            }

            InitializeComponent();
            CenterToScreen();
            InitializeComponentEx();
            AppAuthor.Default.Start();
            AppAuthor.Default.NoticeEvent += OnAuthorNoticeReceive;
            ChartForm.SetChartEventHandler(OnChartNoticeReceive);
            LockForm.SetChartEventHandler(OnChartNoticeReceive);
            SettingBand.SetChartEventHandler(OnChartNoticeReceive);
            OrdCntForm.SetChartEventHandler(OnChartNoticeReceive);
            EarnTickForm.SetChartEventHandler(OnChartNoticeReceive);
            LossTickForm.SetChartEventHandler(OnChartNoticeReceive);
            SyncForm.SetChartEventHandler(OnChartNoticeReceive);
        }

        // Sub Forms
        private FrmLogin LoginForm { get => FrmLogin.Default; }
		private FrmUpdate UpdateForm { get => FrmUpdate.Default; }
		private FrmChart ChartForm { get => FrmChart.Default; }
        private FrmCurrent CurrentForm { get => FrmCurrent.Default; }
        private FrmCurrent2 CurrentForm2 { get => FrmCurrent2.Default; }
        private FrmLog LogForm { get => FrmLog.Default; }
        // private FrmSetting SettingForm { get => FrmSetting.Default; }
        private BandSetting SettingBand { get => BandSetting.Default; }
        private FrmLock LockForm { get => FrmLock.Default; }
        private FrmNotice NoticeForm { get => FrmNotice.Default; }
        private FrmOrdCnt OrdCntForm = new FrmOrdCnt(ORDTYPE.Order);
        private FrmOrdCnt EarnTickForm = new FrmOrdCnt(ORDTYPE.Earn);
        private FrmOrdCnt LossTickForm = new FrmOrdCnt(ORDTYPE.Loss);
        public FrmRange PayoffLossForm = new FrmRange(RANGETYPE.PayoffLoss);
        public FrmRange2 SmartLossForm = new FrmRange2(RANGETYPE.SmartLoss);
        public FrmRange2 CrossLossForm = new FrmRange2(RANGETYPE.CrossLoss);
        public FrmRange2 CciLossForm = new FrmRange2(RANGETYPE.CciLoss);
        public FrmSync SyncForm = new FrmSync();

        private bool ItemChanged{ get ; set; }
        //AppWebSocket _appSocket;
        //private Thread _CheckThread = null;
        /// <summary>
        /// Initialize components additionally
        /// </summary>
        void InitializeComponentEx()
		{
            AppConfig.ReadLossConfig();
            AppConfig.SetNetworkInterfaces();
            // supported site list
            string[] site_names = {"CMG"}; //"더드림", "몬스타", "키움증권", "미래", //"레안텍", "나눔"
            foreach (string site_name in site_names) 
				cmbSiteList.Items.Add(site_name);

			this.hopeForm1.Text = AppAuthor.Default.GetAppName() + " " + AppAuthor.Default.GetAppVersion();
            LogPath = AppAuthor.Default.CreatePathFolder("Log") + "/" + DateTime.Now.ToString("yyyyMMdd") + "_MT";
            WriteLog("<============= 시작 =============>");
			// double buffered
            this.dgvOrderInfo.DoubleBuffered(true);
            this.dgvValuationInfo.DoubleBuffered(true);
            this.dgvItemPriceInfo.DoubleBuffered(true);

			this.ValuationInfo = new List<ValuationInfo>();
			this.ItemPriceInfo = new List<ItemPriceInfo>();
			this.OrderInfo = new List<OrderInfo>();

            InitializeSetting();

            btnHide.Text = "<<";
            formHeight = 750;
            this.ClientSize = new Size(890, formHeight);

			this.ChartForm.Visible = false;
            this.CurrentForm.Visible = false;
            this.CurrentForm2.Visible = false;
            this.LogForm.Visible = false;

            ShowNotice();
            _tickLogout = 0;

            //ConectWebSocket();

        }

        private void DeleteBeforeFiles()
        {
            try
            {
                string path = Environment.CurrentDirectory;
                string[] files = Directory.GetFiles(path);
                foreach (string f in files)
                {
                    if (f.Contains("Temple.exe"))
                        File.Delete(f);
                }
            }   catch { }
            
        }
        
        private AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI;
        private bool createKFOpenApi()
        {
            if (this.axKFOpenAPI != null)
                return true;

            if (!CheckSetupKFOpenAPI())
            {
                if (MessageBox.Show(new Form { TopMost = true }, "키움OpenAPI프로그램이 설치되지 않았습니다.\n 키움OpenAPI를 다운하시겠습니까.", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                     == DialogResult.Yes)
                {
                    Process.Start("https://download.kiwoom.com/web/openapi/OpenApiGSetup.exe"); //
                }
                return false;
            }

            this.axKFOpenAPI = new AxKFOpenAPILib.AxKFOpenAPI();
            this.axKFOpenAPI.Enabled = true;
            this.axKFOpenAPI.Location = new System.Drawing.Point(231, 134);
            this.axKFOpenAPI.Name = "axKFOpenAPI";
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.axKFOpenAPI.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKFOpenAPI.OcxState")));
            this.axKFOpenAPI.Size = new System.Drawing.Size(88, 32);
            this.axKFOpenAPI.TabIndex = 20;
            this.axKFOpenAPI.Visible = false;
            this.Controls.Add(this.axKFOpenAPI);
            ((System.ComponentModel.ISupportInitialize)(this.axKFOpenAPI)).EndInit();
            axKFOpenAPI.OnEventConnect += KF_OnEventConnect;

            return true;
        }

        private void InitializeSetting()
        {

            BETTYPE[] betTypeList = Common.GetAllBetType();
            foreach (BETTYPE betType in betTypeList)
                cmbBettingType.Items.Add(Common.GetBetTypeFullStr(betType));

            for (int i = 0; i < 10; i++)
            {
                cmbBettingCandle1.Items.Add(i + 1);
                cmbBettingCandle2.Items.Add(i + 1);
                cmbCandlePayoff.Items.Add(i + 1);
            }

            cmbOrderCnt.Items.Add(0.01);
            cmbOrderCnt.Items.Add(0.02);
            cmbOrderCnt.Items.Add(0.05);
            cmbOrderCnt.Items.Add(0.1);
            cmbOrderCnt.Items.Add(0.2);
            cmbOrderCnt.Items.Add(0.5);
            cmbOrderCnt.Items.Add(1);
            cmbOrderCnt.Items.Add(2);
            cmbOrderCnt.Items.Add(5);
            cmbOrderCnt.Items.Add(10);
            cmbOrderCnt.Items.Add(20);
            cmbOrderCnt.Items.Add(50);

            //이평선교차
            cmbBettingCandle3.Items.Add("미완성");
            cmbBettingCandle3.Items.Add("완성");

            cmbReorder3.Items.Add("무시");
            cmbReorder3.Items.Add("주문");
            cmbReorder3.Items.Add("청산");

            cmbReorder4.Items.Add("주문");
            cmbReorder4.Items.Add("청산");
            //주하선
            cmbBettingCross6.Items.Add("대기");
            cmbBettingCross6.Items.Add("재진입");
            //이평-주하
            cmbBettingCandle5.Items.Add("미완성");
            cmbBettingCandle5.Items.Add("완성");


            string[] chartTypeList = { "1분", "60틱", "90틱", "120틱",
                    "240틱", "350틱", "400틱", "600틱", "750틱", "990틱", "5분", "15분", "30분"};
            foreach (string s in chartTypeList)
            {
                cmbChartType1.Items.Add(s);
                cmbChartType2.Items.Add(s);
                cmbChartType3.Items.Add(s);
                cmbChartType4.Items.Add(s);
                cmbChartType5.Items.Add(s);
                cmbChartType6.Items.Add(s);
            }

            string[] orderTypeList = { "시장가", "지정가" };
            foreach (string s in orderTypeList)
            {
                cmbOrderType1.Items.Add(s);
                cmbOrderType2.Items.Add(s);
                cmbOrderType3.Items.Add(s);
                cmbOrderType4.Items.Add(s);
                cmbOrderType5.Items.Add(s);
                cmbOrderType6.Items.Add(s);
            }

            int[] avgTypeList = { 5, 10, 20, 60, 120 };
            foreach (int i in avgTypeList)
            {
                cmbAvgType2.Items.Add(i);
                cmbOrderLine1.Items.Add(string.Format("{0}평선", i));
                cmbOrderLine2.Items.Add(string.Format("{0}평선", i));
                cmbOrderLine5_1.Items.Add(string.Format("{0}평선", i));
                cmbOrderLine5_2.Items.Add(string.Format("{0}평선", i));
            }

            cmbSmartUnit.Items.Add("%");
            cmbSmartUnit.Items.Add("틱");

            cmbCrossUnit.Items.Add("%");
            cmbCrossUnit.Items.Add("틱");

            // cmbOrderSelect.Items.Add("전체");
            string[] ordeSides = { "매수", "매도" };
            foreach (string s in ordeSides)
            {
                // cmbOrderSelect.Items.Add(s);
                cmbCciSide1.Items.Add(s);
                cmbCciSide2.Items.Add(s);
                cmbRsiSide1.Items.Add(s);
                cmbRsiSide2.Items.Add(s);
                cmbAvgsSide1.Items.Add(s);
                cmbAvgsSide2.Items.Add(s);

                cmbCciSide1_3.Items.Add(s);
                cmbCciSide2_3.Items.Add(s);
                cmbRsiSide1_3.Items.Add(s);
                cmbRsiSide2_3.Items.Add(s);
                cmbAvgsSide1_3.Items.Add(s);
                cmbAvgsSide2_3.Items.Add(s);
            }

            string[] reverseOrds = { "정배", "역배" };
            foreach(string s in reverseOrds)
            {
                cmbReverseOrd1.Items.Add(s);
                cmbReverseOrd2.Items.Add(s);
            }

            cmbPayoffLoss.Items.Add("15");
            cmbPayoffLoss.Items.Add("30");

            dtAutoReserve.CustomFormat = "HH:mm:ss";
            dtAutoReserve.Format = DateTimePickerFormat.Custom;

            cmbOrderCnt.SelectedIndex = 0;
            LoadSettingControls();
        }


        private SITETYPE CurrentSiteType { get => (SITETYPE)cmbSiteList.SelectedIndex; }
		private FutureSite CurrentSite { get => LogicAuto.Default.CurrentSite; }
        private FutureSite SignalSite { get => LogicAuto.Default.SignalSite; }
        private readonly object _objLock = new object();
		private int _tickLogout;
        private bool _bLoadConfig;
        private int _tickBand;
        private int _tickBandCheck;
        // 호가고정
        public bool IsFixed => this.chkFixed.Checked;
		private string LogPath;
        private int formHeight = 0;
		
		// item price info
		public List<ItemPriceInfo> ItemPriceInfo
		{
			get => (List<ItemPriceInfo>)this.bsItemPriceInfo.DataSource;
			set => this.bsItemPriceInfo.DataSource = value;
		}

		// valuation info
		public List<ValuationInfo> ValuationInfo
		{
			get => (List<ValuationInfo>)this.bsValuationInfo.DataSource;
			set => this.bsValuationInfo.DataSource = value;
		}

		public List<OrderInfo> OrderInfo
		{
			get => (List<OrderInfo>)this.bsOrderInfo.DataSource;
			set => this.bsOrderInfo.DataSource = value;
		}

		// selected user account
		public UserAccountInfo SelectedUserAccount
		{
			get
			{
				if (CurrentSite == null || this.cmbUserAccounts.SelectedIndex < 0)
					return null;
				if (CurrentSite.UserAccounts == null || this.cmbUserAccounts.SelectedIndex >= CurrentSite.UserAccounts.Count)
					return null;
				return CurrentSite.UserAccounts[this.cmbUserAccounts.SelectedIndex];
			}
		}
		public OrderInfo SelectedOrderInfoItem
		{
			get
			{
                if (this.dgvOrderInfo.CurrentCell.RowIndex < 0)
                {
                    return null;
                }
                return this.OrderInfo[this.dgvOrderInfo.CurrentCell.RowIndex];
			}
		}
		private void InitListView(bool bAll = true)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    InitListView(bAll);
                }));
            }
            else
            {
                ValuationInfo = null;
                ItemPriceInfo = null;
                OrderInfo = null;
                if (bAll)
                {
					cmbPrdList.Items.Clear();
					cmbItemList.Items.Clear();
                }
                CurrentForm.InitListView();
                CurrentForm2.InitListView();

            }
        }
        private void UpdateValuationInfo()
		{
			if (CurrentSite != null)
			{
				if (this.dgvValuationInfo.RowCount >= 0)
				{
					ValuationInfo = null;
					ValuationInfo = CurrentSite.ValuationList;
				}
			}
		}

        private void UpdateCurrentInfo()
		{
			if (CurrentSite != null && CurrentSite.Current != null)
			{
                CurrentInfo current = CurrentSite.Current;

                 if ((Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.CROSS) && 
                    Settings.Default.BandChart && Math.Abs(Environment.TickCount - _tickBand ) > 30000 && Math.Abs(Environment.TickCount - _tickBandCheck) > 3000)
                 {
                    _tickBandCheck = Environment.TickCount;
                    if (Settings.Default.BettingType == (int)BETTYPE.BOLINE && !Settings.Default.BothOrder &&
                         CurrentSite.OrderList != null && CurrentSite.OrderList.FirstOrDefault(o => o.OrderType == "체결") != null)
                    {

                    } else
                    {

                        int nConc = GetConcPerMin();
                        if (Settings.Default.BandVal1 > 0 && nConc < Settings.Default.BandVal1)
                        {
                            if ((CHARTTYPE)Settings.Default.BandChartType1 != FrmChart.Default._ChartType)
                            {
                                _tickBand = Environment.TickCount;
                                AddLog("차트타입변경(거래량:" + Settings.Default.BandVal1.ToString() + "미만 " + Common.GetChartTypeStr((CHARTTYPE)Settings.Default.BandChartType1) + "봉)");
                                SetDChartType((CHARTTYPE)Settings.Default.BandChartType1);
                                SetChartType((CHARTTYPE)Settings.Default.BandChartType1);
                            }

                        } else if (nConc >= Settings.Default.BandVal1 && nConc < Settings.Default.BandVal2)
                        {
                            if ((CHARTTYPE)Settings.Default.BandChartType2 != FrmChart.Default._ChartType)
                            {
                                _tickBand = Environment.TickCount;
                                AddLog("차트타입변경(거래량:" + Settings.Default.BandVal1.ToString() + "이상 " + Common.GetChartTypeStr((CHARTTYPE)Settings.Default.BandChartType2) + "봉)");
                                SetDChartType((CHARTTYPE)Settings.Default.BandChartType2);
                                SetChartType((CHARTTYPE)Settings.Default.BandChartType2);
                            }
                        } else if (nConc >= Settings.Default.BandVal2 && nConc < Settings.Default.BandVal3)
                        {
                            if ((CHARTTYPE)Settings.Default.BandChartType3 != FrmChart.Default._ChartType)
                            {
                                _tickBand = Environment.TickCount;
                                AddLog("차트타입변경(거래량:" + Settings.Default.BandVal2.ToString() + "이상 " + Common.GetChartTypeStr((CHARTTYPE)Settings.Default.BandChartType3) + "봉)");
                                SetDChartType((CHARTTYPE)Settings.Default.BandChartType3);
                                SetChartType((CHARTTYPE)Settings.Default.BandChartType3);
                            }
                        }
                        else if (nConc >= Settings.Default.BandVal3)
                        {
                            if ((CHARTTYPE)Settings.Default.BandChartType4 != FrmChart.Default._ChartType)
                            {
                                _tickBand = Environment.TickCount;
                                AddLog("차트타입변경(거래량:" + Settings.Default.BandVal3.ToString() + "이상 " + Common.GetChartTypeStr((CHARTTYPE)Settings.Default.BandChartType4) + "봉)");
                                SetDChartType((CHARTTYPE)Settings.Default.BandChartType4);
                                SetChartType((CHARTTYPE)Settings.Default.BandChartType4);
                            }
                        }
                    }
                 }
                if(!Settings.Default.SignalSiteOn)
                    ChartForm.SetRTValue(current.CurrentPrice, current.Time, 1, current.ConclusionQty);
				LogicAuto.Default.OnLogicNoticeReceive();

                if (this.CurrentSiteType == SITETYPE.CMG)
                   CurrentForm2.UpdateCurrentInfo();
                else CurrentForm.UpdateCurrentInfo();
            }
        }

        private void UpdateCurrentInfo2()
        {
            if (SignalSite != null && SignalSite.Current != null)
            {
                CurrentInfo current = SignalSite.Current;
                ChartForm.SetRTValue(current.CurrentPrice, current.Time, 1, current.ConclusionQty);
            }
        }

        private void UpdateItemPriceInfo()
		{
			if (CurrentSite != null)
			{
                CurrentForm.UpdateItemPriceInfo();

                if (this.dgvItemPriceInfo.RowCount >= 0)
				{
					ItemPriceInfo = null;
					this.ItemPriceInfo = CurrentSite.ItemPriceList;					
				}
			}
		}

		private void UpdateQuoteInfo()
		{
			if (CurrentSite != null)
            {
                CurrentForm.UpdateQuoteInfo();
            }
		}

		private void UpdateTotalQuoteInfo()
		{
            if (Settings.Default.SignalSiteOn)
            {
                if (CurrentSite != null)
                {
                    CurrentForm.UpdateTotalQuoteInfo();
                }
            } else
            {
                if (SignalSite != null)
                {
                    CurrentForm.UpdateTotalQuoteInfo();
                }
            }
			
        }

		private int mOrderCnt = 0;
		private void UpdateOrderInfo()
		{
			if (CurrentSite != null)
			{
                lock (_objLock)
                {
					if (this.dgvOrderInfo.RowCount >= 0)
					{
                        
                        int nOrderListCnt = CurrentSite.OrderList.Count;
						if (nOrderListCnt > 0 || mOrderCnt != nOrderListCnt)
						{
                            int firstDisplayedScrollingRowIndex = this.dgvOrderInfo.FirstDisplayedScrollingRowIndex;
							OrderInfo = null;
                            OrderInfo = new List<OrderInfo>(CurrentSite.OrderList);
                            if (firstDisplayedScrollingRowIndex < this.dgvOrderInfo.RowCount && firstDisplayedScrollingRowIndex >= 0)
                            {
                                this.dgvOrderInfo.FirstDisplayedScrollingRowIndex = ((firstDisplayedScrollingRowIndex >= 0) ? firstDisplayedScrollingRowIndex : 0);
                            }
                            mOrderCnt = nOrderListCnt;
						}
					}
				}
			}
		}
        public void OnChangeOrder()
        {
            if (CurrentSite != null && CurrentSite.LiquidOrder != null)
            {
                ChartForm.SetOrderInfo(CurrentSite.LiquidOrder, (CHARTTYPE)Settings.Default.ChartType);
            }
        }
        public void SetQuoteInfoTopRow(int topRow)
		{
            CurrentForm.SetQuoteInfoTopRow(topRow);
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
				catch(Exception ex)
				{
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
                    AddLog(ex.Message);
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
							if (Math.Abs(Environment.TickCount - _tickLogout) >= 180000)
                            {
                                // Trace.TraceInformation("<FrmMain> OnSiteNoticeReceive.Login");
                                if(!Settings.Default.SignalSiteOn && Settings.Default.SiteType != (int)SITETYPE.KIWOOM)
                                {
                                    ChartForm.ResetChart();
                                    // Trace.TraceInformation("<FrmMain> OnSiteNoticeReceive.Login ResetChart");
                                }
                            }
                            EnableControls();
							ItemChanged = false;
							break;
						case SITE_NOTICEEVENTTYPE.PREPAREITEM:
							ShowPrdInfo();
							ShowItemInfo();
                            break;
						case SITE_NOTICEEVENTTYPE.PREPARE:

							ShowUserInfo();
							UpdateValuationInfo();
							UpdateOrderInfo();
							ShowBalance(noticeType);
                            UpdateCurrentInfo();
                            UpdateItemPriceInfo();
                            UpdateQuoteInfo();
                            break;
						// current info
						case SITE_NOTICEEVENTTYPE.CURRENT:
							if (!ItemChanged)
							{
                                UpdateCurrentInfo();
								UpdateItemPriceInfo();
								UpdateOrderInfo();
								UpdateValuationInfo();
								ShowBalance(noticeType);
							}

							break;
                        case SITE_NOTICEEVENTTYPE.CURRENT_SIGNAL:
                            if (!ItemChanged)
                            {
                                UpdateCurrentInfo2();
                            }
                            break;
                        // quote info
                        case SITE_NOTICEEVENTTYPE.QUOTE:
							if (!ItemChanged)
							{
                                UpdateQuoteInfo();
                                if (this.IsFixed && CurrentSite != null && CurrentSite.Ask1Row != null)
                                    SetQuoteInfoTopRow(CurrentSite.Ask1Row.QuoteInfoId);
                                UpdateTotalQuoteInfo();
							}
							break;
                        case SITE_NOTICEEVENTTYPE.QUOTE_SIGNAL:
//                             if (!ItemChanged)
//                             {
//                                 UpdateQuoteInfo();
//                                 if (this.IsFixed && CurrentSite != null && CurrentSite.Ask1Row != null)
//                                     SetQuoteInfoTopRow(CurrentSite.Ask1Row.QuoteInfoId);
//                                 UpdateTotalQuoteInfo();
//                             }
                            break;
                        // order info
                        case SITE_NOTICEEVENTTYPE.ORDER:
							if (!ItemChanged) 
							{
								UpdateOrderInfo();
								ShowBalance(noticeType);
							}
							break;
                        case SITE_NOTICEEVENTTYPE.LIQUID:
                            if (!ItemChanged)
                            {
                                UpdateOrderInfo();
                                ShowBalance(noticeType);
                            }
                            OnChangeOrder();
                            break;
                        case SITE_NOTICEEVENTTYPE.VALUATION:
                            UpdateValuationInfo();
                            ShowBalance(noticeType);
                            break;
                        case SITE_NOTICEEVENTTYPE.DISCONNECT:
                            _tickLogout = Environment.TickCount;
                            break;
                        case SITE_NOTICEEVENTTYPE.LOGOUT:
							
							break;
                        case SITE_NOTICEEVENTTYPE.NOLOGIN:
                            ShowKFOpenLogin();
                            break;
                        case SITE_NOTICEEVENTTYPE.REDRAW:
                            ChartForm.Redraw();
                            break;
                        case SITE_NOTICEEVENTTYPE.MANUAL:
                            OnLogicStop();
                            break;
                        case SITE_NOTICEEVENTTYPE.AUTO:
                            OnLogicStart();
                            break;
                        case SITE_NOTICEEVENTTYPE.STOP:
                            if (!ItemChanged)
                            {
                                EnableControls();
                                AddLog("로그아웃 되었습니다.");
                            }
                            ShowKiwoomUserInfo();
                            break;
					}
				}
				catch(Exception ex)
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
				// string mark = log.Substring(0, 2);
				DateTime dtCurrent = DateTime.Now;
				if(AppConfig._DtDelay != 0)
                {
                    dtCurrent = dtCurrent.AddSeconds(AppConfig._DtDelay);
				}
				log = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", dtCurrent.Hour, dtCurrent.Minute, dtCurrent.Second) + log;


                if (listLog.Items.Count > 2000)
                    listLog.Items.RemoveAt(0);
                listLog.Items.Add(log);
                //LogForm.AddLog(log);
                AutoScrollLog();
                WriteLog(log);
			}
		}
        public void AddStateLog(string fmt, params Object[] param_list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.AddStateLog(fmt, param_list);
                }));
            }
            else
            {
                string log = String.Format(fmt, param_list);
                DateTime dtCurrent = DateTime.Now;
                if (AppConfig._DtDelay != 0)
                {
                    dtCurrent = dtCurrent.AddSeconds(AppConfig._DtDelay);
                }
                log = string.Format("[{0:D2}:{1:D2}:{2:D2}]", dtCurrent.Hour, dtCurrent.Minute, dtCurrent.Second) + log;
                txtStateLog.Text = log;
                LogForm.AddLog(log);
            }
        }
        public void AddValueLog(string fmt, params Object[] param_list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.AddValueLog(fmt, param_list);
                }));
            }
            else
            {
                string log = String.Format(fmt, param_list);
                DateTime dtCurrent = DateTime.Now;
                if (AppConfig._DtDelay != 0)
                {
                    dtCurrent = dtCurrent.AddSeconds(AppConfig._DtDelay);
                }
                log = string.Format("[{0:D2}:{1:D2}:{2:D2}]", dtCurrent.Hour, dtCurrent.Minute, dtCurrent.Second) + log;

                LogForm.AddLog(log);
            }
        }
        public void AppendValue(string value, params Object[] param_list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.AppendValue(value, param_list);
                }));
            }
            else
            {
                Color color = Color.Black;
                if (value.Length > 0)
                {
                    if (param_list.Length > 1)
                        color = Color.Red;
                    else if (param_list.Length > 0)
                        color = Color.Blue;
                    AppendText(txtValueLog, value, color);
                    ChartForm.SetChartValue(value, color);
                }
                else
                {
                    txtValueLog.Text = "";
                    ChartForm.SetChartValue("", color);
                }
            }
        }

        public void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        public void OnLogicStop()
        {
            AddLog("정지되었습니다.");

            chkAutoMode.Checked = false;
            // Trace.TraceError("<OnLogicStop> AlarmStop = {0} InformStop = {1}", Settings.Default.AlarmStop, Settings.Default.InformStop);
            if (Settings.Default.AlarmStop)
            {
                try
                {
                    
                    SoundPlayer alarmSound = new SoundPlayer(Properties.Resources.Alarm01);
                    alarmSound.Play();
                } catch (Exception e)
                {
                    Trace.TraceError("<OnLogicStop> alarm ex = {0}", e.Message);
                }
           }
            if (Settings.Default.InformStop)
            {
                MessageBox.Show("정지되었습니다.\n수동모드로 전환합니다.", "경고");
            }
        }
        public void OnLogicStart()
        {
            chkAutoMode.Checked = true;
        }
        public void AutoScrollLog()
        {
            if (!chkScroll.Checked)
                return;
            int cnt = listLog.Items.Count;
            if (cnt >= 10)
            {
                listLog.Items[cnt - 1].Selected = true;
                listLog.EnsureVisible(cnt - 1);
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
                        case CHART_EVENTTYPE.BETTING_CHANGED:
							if(LockEx.locked)
                                cmbBettingType.SelectedIndex = Settings.Default.BettingType;
                            else ChangeSettingControls();
                            break;
                        case CHART_EVENTTYPE.SETTING_CHANGED:
                            // AddLog("등락설정이 저장되었습니다.");
                            // ChartForm.SetChartFrom(Settings.Default.StartChartDt);
                            break;
                        case CHART_EVENTTYPE.DRAWING_CHANGED:
                            if (!Settings.Default.SignalSiteOn)
                            {
                                if (CurrentSite != null)
                                    CurrentSite.RequestRChart();
                            } else
                            {
                                if (SignalSite != null)
                                    SignalSite.RequestRChart();
                            }
                            break;
                        case CHART_EVENTTYPE.ORDERCNT_CHANGED:
                            ChangeOrdCnt();
                            break;
                        case CHART_EVENTTYPE.EARNTICK_CHANGED:
                            ChangeEarnTick();
                            break;
                        case CHART_EVENTTYPE.LOSSTICK_CHANGED:
                            ChangeLossTick();
                            break;
                        case CHART_EVENTTYPE.SYNCCHART_CHANGED:
                            //SyncMembersChart();
                            break;
                        default:
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
                            Thread.Sleep(1000);
                            Environment.Exit(0);

                            break;
                    }

                }
                catch (Exception) { }

            }
            else
            {
                JsonDocument doc = JsonDocument.Parse(e.Data.ToString());
                string command = "";
                try
                {
                    command = doc.RootElement.GetProperty("Command").GetString();
                    if (command == "connect")
                    {
                        if (doc.RootElement.GetProperty("Result").GetString() == "OK")
                        {
                            WriteLog("서비스접속 성공!");
                        }
                    } else if (command == "sync_chart")
                    {
                        if (doc.RootElement.GetProperty("Value").GetString().Length > 0)
                        {
                            if (!Settings.Default.SyncChart)
                                return;
                            string value = doc.RootElement.GetProperty("Value").GetString();
                            string[] infos = value.Split('#');

                            if(infos.Length > 2)
                            {
                                string symbol = infos[0]; 
                                DateTime dt = DateTime.ParseExact(infos[1], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                double dPrice = double.Parse(infos[2]);

                                if(CurrentSite != null && CurrentSite.Current != null 
                                    && ( CurrentSite.Type == SITETYPE.TOPASSET || CurrentSite.Type == SITETYPE.DREAM || CurrentSite.Type == SITETYPE.MIRAE2) 
                                    && CurrentSite.ItemSymbol.ToLower() == symbol)
                                {
                                    ChartForm.SetChartFrom(dt, dPrice);
                                    AddLog("차트가 동기화되었습니다.");
                                }

                            }
                        }
                    }
                }
                catch (Exception) { }
            }
        }
        
        //public void SyncMembersChart()
        //{
        //    if (_appSocket.ConnectState.Length == 0 || _appSocket.ConnectState == "Closed")
        //    {
        //        AddLog("서비스접속 실패!");

        //        MessageBox.Show("서비스에 접속할수 없습니다.\n 잠시후 다시 시도해주세요.", "경고");
        //        return;
        //    }

        //    if (Settings.Default.SyncMembers.Count < 1)
        //        return;

        //    string value = "";
        //    CurrentInfo current = null;

        //    if (CurrentSite != null && CurrentSite.CurrentList != null && CurrentSite.Current != null)
        //    {
        //        if (CurrentSite.CurrentList.Count > 50)
        //            current = CurrentSite.CurrentList[CurrentSite.CurrentList.Count - 50];
        //        else if (CurrentSite.Current != null)
        //            current = CurrentSite.Current;
        //        if(current != null)
        //            value = CurrentSite.ItemSymbol + "#" + current.Time.ToString("yyyy-MM-dd HH:mm:ss") + "#" + current.CurrentPriceStr;
        //    }
        //    else return;
            
        //    string users = "";
        //    foreach(string member in Settings.Default.SyncMembers)
        //    {
        //        users += member + "#";
        //    }
        //    JsonObj jsonObj = new JsonObj
        //    {
        //        Command = "sync_chart",
        //        Users = users,
        //        Value = value,
        //    };

        //    string msg = JsonSerializer.Serialize(jsonObj);

        //    _appSocket.SendMsg(msg);

        //    ChartForm.SetChartFrom(current.Time, current.CurrentPrice);
        //    AddLog("회원 차트동기화가 적용었습니다.");
        //    WriteLog("동기화:" + value);

        //}
        public void SetChartType(CHARTTYPE chartType)
        {
            if(Settings.Default.BettingType == (int)BETTYPE.CROSS)
            {
                cmbChartType3.SelectedIndex = (int)chartType;
            } else if(Settings.Default.BettingType == (int)BETTYPE.BOLINE)
            {
                cmbChartType4.SelectedIndex = (int)chartType;
            }
        }

        public void SetDChartType(CHARTTYPE chartType)
        {
            SetDChartInfo(chartType);
            SetDChart2Info(chartType);
        }

        public void SetDChartInfo(CHARTTYPE chartType)
        {
            ChartForm.SetDChartType(chartType);
            ChartForm.SetRChartType(chartType);

            if(CurrentSite != null)
                CurrentSite.RequestDChart(chartType);
            if (SignalSite != null)
                SignalSite.RequestDChart(chartType);
        }
        public void SetDChart2Info(CHARTTYPE chartType)
        {
            //if(CurrentSite != null)
            //    CurrentSite.RequestDChart(false);
            //if (SignalSite != null)
            //    SignalSite.RequestDChart(false);

            ChartForm.SetDChart2Type(chartType);
        }
        public List<DItem> GetCandleList(int nCandles = 0, bool bNeedLast = false)
		{
			return ChartForm.GetCandleList(nCandles, bNeedLast);
		}
        public int GetConcPerMin(int nMin = 1)
        {
            return ChartForm.GetConcPerMin(nMin);
        }
        public List<CItem> GetCandleList2(int nCandles = 0, bool bNeedLast = false)
        {
            return ChartForm.GetCandleList2(nCandles, bNeedLast);
        }
        private void ShowUserInfo()
		{
			this.cmbUserAccounts.Items.Clear();
			if (CurrentSite != null && CurrentSite.UserAccounts != null)
			{
				// accounts
				this.cmbUserAccounts.Items.AddRange(
					(from u in CurrentSite.UserAccounts
					 select u.UserAccountStr).ToArray<string>()
				);
				this.cmbUserAccounts.SelectedIndex = 0;
				// user name
				string siteName = "";
				if (cmbSiteList.SelectedItem != null)
					siteName = cmbSiteList.SelectedItem.ToString();
				//this.txtUserName.Text = CurrentSite.User.BankUserName;
				AppAuthor.Default.SetUserAccount(txtId.Text, siteName);
			}
		}

        private void ShowKiwoomUserInfo()
        {
            if (this.CurrentSiteType == SITETYPE.KIWOOM)
            {
                this.cmbUserAccounts.Items.Clear();

                string sUserId = axKFOpenAPI.GetLoginInfo("USER_ID");
                string sUserName = axKFOpenAPI.GetLoginInfo("USER_NAME");

                if (String.IsNullOrEmpty(sUserId))
                {
                    return;
                }
                string siteName = "";
                if (cmbSiteList.SelectedItem != null)
                    siteName = cmbSiteList.SelectedItem.ToString();
                // txtId.Text = sUserName;
                txtId.Text = sUserId;
                AppAuthor.Default.SetUserAccount(sUserName, siteName);

                string sAccList = axKFOpenAPI.GetLoginInfo("ACCNO");

                string[] accounts = sAccList.Split(';');
                for (int i = 0; i < accounts.Length; i++)
                {
                    if (accounts[i].Trim().Length > 0)
                        cmbUserAccounts.Items.Add(accounts[i].Trim());
                }
                if (cmbUserAccounts.Items.Count > 0)
                {
                    cmbUserAccounts.SelectedIndex = 0;
                }
            }

        }

        public List<string> GetKiwoomAccount()
        {
            List<string> listAcc = new List<string>();
            if(axKFOpenAPI != null)
            {
                string sAccList = axKFOpenAPI.GetLoginInfo("ACCNO");

                string[] accounts = sAccList.Split(';');
                for (int i = 0; i < accounts.Length; i++)
                {
                    if (accounts[i].Trim().Length > 0)
                    {
                        listAcc.Add(accounts[i].Trim());
                    }
                }
            }
            return listAcc;
        }

        private void ShowBalance(SITE_NOTICEEVENTTYPE noticeType)
		{
			if (this.SelectedUserAccount != null && this.ValuationInfo != null && this.ValuationInfo.Count > 0)
			{
				double lBalance = this.SelectedUserAccount.Balance + this.ValuationInfo[0].TotalValuation;
                txtBalance.Text = lBalance.ToString("N2");
                if (noticeType == SITE_NOTICEEVENTTYPE.PREPARE)
                {
                    AppAuthor.Default.SetUserAccount("", "", SelectedUserAccount.Balance - ValuationInfo[0].CurrentProfit, SelectedUserAccount.Balance);
                }
                else
                {
                    AppAuthor.Default.SetUserAccount("", "", SelectedUserAccount.Balance - ValuationInfo[0].CurrentProfit, SelectedUserAccount.Balance);
                }
            }
		}
        public void ShowPrdInfo()
        {
            if (CurrentSite != null && CurrentSite.PrdList != null)
            {
                cmbPrdList.Items.Clear();
                int selIndex = -1;
                for (int i = 0; i < CurrentSite.PrdList.Count; i++)
                {
                    cmbPrdList.Items.Add(CurrentSite.PrdList[i].Name);

                    if (CurrentSite.CurPrd.Code == CurrentSite.PrdList[i].Code)
                        selIndex = i;
                }

                if (selIndex >= 0)
                {
                    cmbPrdList.SelectedIndex = selIndex;
                }
            }
        }
        public void ShowItemInfo()
        {
            if (CurrentSite != null && CurrentSite.ItemList != null)
            {
                cmbItemList.Items.Clear();
                int selIndex = -1;
                for (int i = 0; i < CurrentSite.ItemList.Count; i++)
                {
					cmbItemList.Items.Add(CurrentSite.ItemList[i].ItemName);

                    if (CurrentSite.ItemSymbol == CurrentSite.ItemList[i].Symbol)
                        selIndex = i;
                }

                if (selIndex >= 0)
                {
                    CtrlProperty.SetValueRate(Common.GetPrecisionRate(CurrentSite.ItemSymbol, CurrentSite.CurItemSymbol.Precision), Common.GetValueFormat(CurrentSite.ItemPrecision+1), (float)CurrentSite.CurItemSymbol.OverTick);
					cmbItemList.SelectedIndex = selIndex;
                    AddLog(CurrentSite.CurItemSymbol.ItemName);
                }
            }
        }
        private void EnableControls()
		{
            //chkSignal.Visible = !((SITETYPE)cmbSiteList.SelectedIndex == SITETYPE.KIWOOM || (SITETYPE)cmbSiteList.SelectedIndex == SITETYPE.CMG); 

            bool running  = LogicAuto.Default.IsRunning;
			cmbSiteList.Enabled = !running;
			txtId.Enabled = !running;
			txtPassword.Enabled = !running;
			btnLogin.Enabled = !running;
			btnLogout.Enabled = running;
            chkSignal.Enabled = !running;
            {
				this.btnLogin.ForeColor = !running ? System.Drawing.SystemColors.ControlText : System.Drawing.SystemColors.ControlDark;
				this.btnLogout.ForeColor = running? System.Drawing.SystemColors.ControlText : System.Drawing.SystemColors.ControlDark;

			}
		}

        public void ShowNotice()
        {


            if (Settings.Default.ChkNoticeDay)
            {
                if (Settings.Default.NoticeViewTime.Date.AddDays(1) > DateTime.Now)
                    return;
            }

            NoticeInfo noticeInfo = AppAuthor.Default.GetNotice();

            if (noticeInfo == null || noticeInfo.Content.Length < 1)
                return;


            if (noticeInfo.UpdateTime.AddDays(6) < DateTime.Now)
                return;

            noticeInfo.Content = noticeInfo.Content.Replace("@@", "\r\n");

            if (!NoticeForm.Visible)
            {

                NoticeForm.SetNotice(noticeInfo.Content);
                NoticeForm.Show(this);
            }

        }

        // Event handlers
        private void FrmMain_Load(object sender, EventArgs e)
		{
			// site type
			cmbSiteList.SelectedIndex = Settings.Default.SiteType;
            Settings.Default.SignalSiteOn = false;
            chkSignal.Checked = Settings.Default.SignalSiteOn;
            // site account (id & password)
            txtId.Text = Settings.Default.SiteId;
			txtPassword.Text = Settings.Default.SitePassword;
			// auto mode (auto / manual)
			this.chkAutoMode.Checked = Settings.Default.IsAutoMode;
			cmbPrdList.Enabled = !chkAutoMode.Checked;
			cmbItemList.Enabled = !chkAutoMode.Checked;
            EnableControls();
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{

            
            if (LogicAuto.Default.IsRunning && MessageBox.Show("프로그램을 종료하시겠습니까? 종료전 접속을 해제하세요", "종료", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            Settings.Default.Save();
			AppAuthor.Default.Stop();
			LogicAuto.Default.Stop();
			
			InitListView();
			AppAuthor.Default.Logout();
            CloseKFLoginDlg();
            DisconnectKFOpenAPI();
            if (!ChartForm.IsDisposed)
            {
				ChartForm.SaveSetting();
				ChartForm.Close();
			}
            if (!CurrentForm.IsDisposed)
            {
                CurrentForm.Close();
            }
            if (!CurrentForm2.IsDisposed)
            {
                CurrentForm2.Close();
            }
            if (!LogForm.IsDisposed)
            {
                LogForm.Close();
            }
            e.Cancel = false;
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (LogicAuto.Default.IsRunning)
				return;

			if(this.CurrentSiteType == SITETYPE.NONE)
			{
				cmbSiteList.Focus();
				cmbSiteList.DroppedDown = true;
				return;
			}

			else
			{
				if (string.IsNullOrEmpty(txtId.Text))
				{
					txtId.Focus();
					return;
				}

				if (string.IsNullOrEmpty(txtPassword.Text))
				{
					txtPassword.Focus();
					return;
				}

                string id = txtId.Text;
                string acc = "";
                if (this.CurrentSiteType == SITETYPE.KIWOOM || Settings.Default.SignalSiteOn)
                {
                    if (!createKFOpenApi())
                        return;
                    if(this.CurrentSiteType == SITETYPE.KIWOOM)
                    {
                        List<string> listAcc = GetKiwoomAccount();
                        if(listAcc.Count < 1)
                        {
                            AddLog("키움에 로그인해주세요");
                            ShowKFOpenLogin();
                            return;
                        } else
                        {
                            if (cmbUserAccounts.Items.Count > 0 && cmbUserAccounts.SelectedItem != null)
                                acc = cmbUserAccounts.SelectedItem.ToString();
                            if(acc.Length < 1 || !listAcc.Contains(acc))
                            {
                                ShowKiwoomUserInfo();
                                acc = cmbUserAccounts.SelectedItem.ToString();
                            }
                        }
                        
                        chkSignal.Checked = false;
                        Settings.Default.SignalSiteOn = chkSignal.Checked;
                    }
                    else if(Settings.Default.SignalSiteOn)
                    {
                        List<string> listAcc = GetKiwoomAccount();
                        if (listAcc.Count < 1)
                        {
                            AddLog("키움에 로그인해주세요");
                            ShowKFOpenLogin();
                            return;
                        }
                        else acc = listAcc[0];
                    }
                }
                SetDChartType((CHARTTYPE)Settings.Default.ChartType);

                ItemChanged = false;
				_tickLogout = Environment.TickCount - 180000;
				if (LogicAuto.Default.Start(
					this.CurrentSiteType,
					id, 
					txtPassword.Text, 
                    acc,
					"",
					this.OnSiteLogReceive, 
					this.OnSiteNoticeReceive,
					this, axKFOpenAPI)
				)
				{
					Settings.Default.SiteType = (int)this.CurrentSiteType;
					Settings.Default.SiteId = txtId.Text;
					Settings.Default.SitePassword = txtPassword.Text;
					Settings.Default.Save();
					AddLog("시작 중입니다.");
				}
				else
					AddLog("잠시 후에 다시 시도해주세요.");
			}
			EnableControls();
		}
		private void btnLogout_Click(object sender, EventArgs e)
		{
			if (LogicAuto.Default.IsRunning)
			{
				ItemChanged = false;
				LogicAuto.Default.Stop();
				ChartForm.ResetChart();
				
                InitListView();
                Invalidate();
            }
			EnableControls();
		}
        private void ChangePrd(int prdIndex)
        {
            if (CurrentSite == null || CurrentSite.PrdList == null || CurrentSite.PrdList.Count < prdIndex + 1)
                return;

            if (CurrentSite.CurPrd.Code != CurrentSite.PrdList[prdIndex].Code)
            {
                string prdSymbol = CurrentSite.PrdList[prdIndex].Code;

                if (CurrentSite.ChangePrd(prdSymbol))
                {
                    ChangeItem(0, false);
                    ShowItemInfo();
                }

            }

        }
        //      private void ChangeItem(int itemIndex, bool bLog=true)
        //{
        //          if (CurrentSite == null || CurrentSite.ItemList == null || CurrentSite.ItemList.Count < itemIndex + 1)
        //              return;

        //          if (CurrentSite.ItemSymbol != CurrentSite.ItemList[itemIndex].Symbol)
        //          {
        //		string itemSymbol = CurrentSite.ItemList[itemIndex].Symbol;

        //		if(this.CurrentSiteType == SITETYPE.DREAM || this.CurrentSiteType == SITETYPE.TOPASSET
        //                  || this.CurrentSiteType == SITETYPE.KIWOOM || this.CurrentSiteType == SITETYPE.MIRAE2
        //                  || this.CurrentSiteType == SITETYPE.CMG)
        //              {
        //                  ItemChanged = true;
        //                  if (CurrentSite.ChangeItem(itemSymbol))
        //                  {
        //                      if(bLog)
        //                          AddLog(CurrentSite.ItemList[itemIndex].ItemName);

        //                      if (SignalSite != null)
        //                          SignalSite.ChangeItem(itemSymbol);
        //                      Thread.Sleep(1000);
        //                  }
        //                  ItemChanged = false;

        //              }
        //              ChartForm.ResetChart();
        //              InitListView(false);
        //              Invalidate();

        //          }

        //      }
        // 함수 선언에 async를 추가합니다.
        private async void ChangeItem(int itemIndex, bool bLog = true)
        {
            if (CurrentSite == null || CurrentSite.ItemList == null || CurrentSite.ItemList.Count < itemIndex + 1)
                return;

            if (CurrentSite.ItemSymbol != CurrentSite.ItemList[itemIndex].Symbol)
            {
                string itemSymbol = CurrentSite.ItemList[itemIndex].Symbol;

                if (this.CurrentSiteType == SITETYPE.DREAM || this.CurrentSiteType == SITETYPE.TOPASSET
                    || this.CurrentSiteType == SITETYPE.KIWOOM || this.CurrentSiteType == SITETYPE.MIRAE2
                    || this.CurrentSiteType == SITETYPE.CMG)
                {
                    ItemChanged = true;
                    if (CurrentSite.ChangeItem(itemSymbol))
                    {
                        if (bLog)
                            AddLog(CurrentSite.ItemList[itemIndex].ItemName);

                        if (SignalSite != null)
                            SignalSite.ChangeItem(itemSymbol);

                        // 핵심 수정: Thread.Sleep(1000) 대신 비동기 대기 사용
                        // 1초 동안 기다리지만 UI 쓰레드는 멈추지 않고 계속 현재가를 그립니다.
                        await Task.Delay(1000);
                    }
                    ItemChanged = false;
                }
                ChartForm.ResetChart();
                InitListView(false);
                Invalidate();
            }
        }
        private void dgvCurrentInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
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
							e.CellStyle.BackColor = Color.FromArgb(218, 232, 254);
							break;
						case 1:
						case 2:
							if (e.RowIndex < CurrentSite.Bid1Row.QuoteInfoId || !(CurrentSite.Bid1Row.BidQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(240, 240, 253) : Color.White);
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
							e.CellStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
							e.CellStyle.ForeColor = Color.Red;
							if (e.RowIndex == CurrentSite.CurrentPriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(128, 255, 128);
								
								return;
							}
							if (e.RowIndex >= CurrentSite.HighPriceRow.QuoteInfoId && e.RowIndex <= CurrentSite.LowPriceRow.QuoteInfoId && e.RowIndex < CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(255, 227, 227);
								return;
							}
							if (e.RowIndex <= CurrentSite.LowPriceRow.QuoteInfoId && e.RowIndex >= CurrentSite.HighPriceRow.QuoteInfoId && e.RowIndex > CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(220, 241, 252);
								return;
							}
							if (e.RowIndex == CurrentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.LightGray;
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
            try { 
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

		}

		private void chkFixed_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void btnChat_Click(object sender, EventArgs e)
		{
            if (!ChartForm.Visible)
            {
                ChartForm.Show(this);
                Thread.Sleep(500);
                ChartForm.Redraw();
            }
        }

        private void btnCurrent_Click(object sender, EventArgs e)
        {
            if (!LogicAuto.Default.IsRunning)
                return;
            if(this.CurrentSiteType == SITETYPE.CMG)
            {
                if (!CurrentForm2.Visible)
                {
                    CurrentForm2.Show(this);
                }
            } else
            {
                if (!CurrentForm.Visible)
                {
                    CurrentForm.Show(this);
                }
            }
            
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (!LogForm.Visible)
            {
                LogForm.Show(this);
            }
        }
        private void dgvQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvQuoteInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvCurrentInfo_SelectionChanged(object sender, EventArgs e)
		{
            try { 
				dgvCurrentInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvTotalQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvTotalQuoteInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvItemPriceInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvItemPriceInfo.ClearSelection();
			}
			catch (Exception) { }
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
			if(CurrentSite != null && this.SelectedUserAccount != null)
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
                lock (_objLock)
                {
                    if (e.Value != null && e.RowIndex >= 0)
					{
                    
                        if (e.ColumnIndex == 2)
                        {
							if (e.Value.ToString().StartsWith("매도"))
							{
								e.CellStyle.ForeColor = Color.Blue;
								return;
							}
							else if (e.Value.ToString().StartsWith("매수"))
							{
								e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
								return;
							}
                            else { 
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
						}
                        else if (e.ColumnIndex == 5)
                        {
							if (Convert.ToInt64(e.Value) < 0L)
							{
								e.CellStyle.ForeColor = Color.Blue;
								return;
							}
							else if (Convert.ToInt64(e.Value) > 0L)
							{
								e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
								return;
							}
							else
							{
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
                        }
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
				else if ((e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4) && e.Value != null)
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
        private void dgvValuationInfo_SelectionChanged(object sender, EventArgs e)
        {
			try
			{
				dgvValuationInfo.ClearSelection();
			}
			catch (Exception) { }
        }
        private void dgvOrderInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			//if (Settings.Default.IsAutoMode)
			//	return;
			try
			{
                lock (_objLock)
                {
                    if (e.RowIndex >= 0 && this.OrderInfo[e.RowIndex] == this.SelectedOrderInfoItem)
                    {
                        if (e.ColumnIndex == 6 && CurrentSite != null && this.SelectedUserAccount != null)
                        {
                            if (this.SelectedOrderInfoItem != null && this.SelectedOrderInfoItem.OrderType != string.Empty)
                            {
                                if (this.SelectedOrderInfoItem.OrderType == "체결")
                                {
                                    LogicAuto.Default.m_tickCancel = Environment.TickCount;
                                    CurrentSite.LiquidateOrder(this.SelectedOrderInfoItem);
                                }
                                else if (this.SelectedOrderInfoItem.OrderType == "미체결")
                                {
                                    LogicAuto.Default.m_tickCancel = Environment.TickCount;
                                    CurrentSite.CancelOrder(this.SelectedOrderInfoItem);
                                }
                            }
                        }
                    }
                }
			}
			catch(Exception)
			{

			}
		}

		private void dgvQuoteInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
		}

		private void chkAutoMode_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.IsAutoMode = chkAutoMode.Checked;

			chkAutoMode.BackColor = chkAutoMode.Checked ? Color.FromArgb(0, 142, 71) : Color.FromArgb(227, 123, 117);
			chkAutoMode.Text = chkAutoMode.Checked ? "자 동" : "수 동";
			cmbPrdList.Enabled = !chkAutoMode.Checked;
            cmbItemList.Enabled = !chkAutoMode.Checked;

            AddLog(chkAutoMode.Checked ? "자동모드입니다." : "수동모드입니다.");

        }

        private void cmbItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
			int index = cmbItemList.SelectedIndex;
            if (index >= 0)
            {
                ChangeItem(index);
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
			if(btnHide.Text == "<<")
            {
				btnHide.Text = ">>";
				this.ClientSize = new Size(530, formHeight);
			} else
            {
				btnHide.Text = "<<";
				this.ClientSize = new Size(890, formHeight);
				LoadSettingControls();
			}
        }

        public void LoadSettingControls()
        {
            _bLoadConfig = true;
            
            cmbBettingType.SelectedIndex = Settings.Default.BettingType;
            try
            {
                ChangeSettingControls(true);

                ChangeOrdCnt();
                ChangeEarnTick();
                ChangeLossTick();
            }
            catch (Exception) { }
            _bLoadConfig = false;
        }
        private void ChangeBoOrdBtn(int index=0)
        {
            Settings.Default.BoOrdType = index;
            if (index == 0)
            {
                ChangeOrdSelBtnColor(btnSbOrd4, true);
                ChangeOrdSelBtnColor(btnCciOrd4, false);

                label80.Text = "이상";
                label47.Text = "이하";

                cmbCciSide1.Location = new Point(144, 177);
                cmbCciSide2.Location = new Point(144, 200);

            }
            else
            {
                ChangeOrdSelBtnColor(btnSbOrd4, false);
                ChangeOrdSelBtnColor(btnCciOrd4, true);

                label80.Text = "상승";
                label47.Text = "하락";
                label77.Text = "이상";
                label78.Text = "이하";
                cmbCciSide1.Location = new Point(210, 177);
                cmbCciSide2.Location = new Point(210, 200);

            }
            txtCci11.Visible = index == 1;
            txtCci21.Visible = index == 1;
            label77.Visible = index == 1;
            label78.Visible = index == 1;

            txtBoAdjust4.Visible = index == 0;
            label45.Visible = index == 0;
            label46.Visible = index == 0;
            chkBoAdjust4.Visible = index == 0;
            chkBoSec4.Visible = index == 0;
            txtBoSec4.Visible = index == 0;
            label103.Visible = index == 0;

            btnSbOrd4.Invalidate();
            btnCciOrd4.Invalidate();
        }
        
        private void ChangeOrdSelBtn(int index = 0)
        {
            Settings.Default.OrderSelectType = index;
            if(index == 0)
            {
                ChangeOrdSelBtnColor(btnSelOrderAll, true);
                ChangeOrdSelBtnColor(btnSelOrderBuy, false);
                ChangeOrdSelBtnColor(btnSelOrderSell, false);
            }
            else if(index == 1)
            {
                ChangeOrdSelBtnColor(btnSelOrderAll, false);
                ChangeOrdSelBtnColor(btnSelOrderBuy, true);
                ChangeOrdSelBtnColor(btnSelOrderSell, false);
            }
            else
            {
                ChangeOrdSelBtnColor(btnSelOrderAll, false);
                ChangeOrdSelBtnColor(btnSelOrderBuy, false);
                ChangeOrdSelBtnColor(btnSelOrderSell, true);
            }
            btnSelOrderAll.Invalidate();
            btnSelOrderBuy.Invalidate();
            btnSelOrderSell.Invalidate();
        }
        private void ChangeOrdSelBtnColor(ReaLTaiizor.Controls.DreamButton btn, bool bChecked)
        {
            Color btnColor = bChecked ? Color.LimeGreen : Color.White;

            btn.ColorA = btnColor;
            btn.ColorB = btnColor;
            btn.ColorC = btnColor;
            btn.ColorD = btnColor;
        }
        private void ChangeOrdCnt(int index=0)
        {
            
            if (index == 0)
            {
                float[] ordCnts = { 1, 1, 1, 1 };
                string[] sCnts = Settings.Default.OrdCnts.Split('#');

                if (sCnts == null && sCnts.Length < 4)
                    return;
                for (int i = 0; i < 4; i++)
                {
                    if (!float.TryParse(sCnts[i], out ordCnts[i]))
                    {
                        ordCnts[i] = 1;
                    }
                }

                chkOrd11.Text = ordCnts[0].ToString();
                chkOrd12.Text = ordCnts[1].ToString();
                chkOrd13.Text = ordCnts[2].ToString();
                chkOrd14.Text = ordCnts[3].ToString();
                chkOrd21.Text = ordCnts[0].ToString();
                chkOrd22.Text = ordCnts[1].ToString();
                chkOrd23.Text = ordCnts[2].ToString();
                chkOrd24.Text = ordCnts[3].ToString();
                chkOrd31.Text = ordCnts[0].ToString();
                chkOrd32.Text = ordCnts[1].ToString();
                chkOrd33.Text = ordCnts[2].ToString();
                chkOrd34.Text = ordCnts[3].ToString();
                chkOrd41.Text = ordCnts[0].ToString();
                chkOrd42.Text = ordCnts[1].ToString();
                chkOrd43.Text = ordCnts[2].ToString();
                chkOrd44.Text = ordCnts[3].ToString();
                chkOrd51.Text = ordCnts[0].ToString();
                chkOrd52.Text = ordCnts[1].ToString();
                chkOrd53.Text = ordCnts[2].ToString();
                chkOrd54.Text = ordCnts[3].ToString();
            }
            else
            {
                switch (index)
                {
                    case 1:
                        if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
                        {
                            txtOrderCount1.Text = chkOrd11.Text;
                            ChangeOrdBtnColor(chkOrd11, true);
                            ChangeOrdBtnColor(chkOrd12, false);
                            ChangeOrdBtnColor(chkOrd13, false);
                            ChangeOrdBtnColor(chkOrd14, false);
                        } else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
                        {
                            txtOrderCount2.Text = chkOrd21.Text;
                            ChangeOrdBtnColor(chkOrd21, true);
                            ChangeOrdBtnColor(chkOrd22, false);
                            ChangeOrdBtnColor(chkOrd23, false);
                            ChangeOrdBtnColor(chkOrd24, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
                        {
                            txtOrderCount3.Text = chkOrd31.Text;
                            ChangeOrdBtnColor(chkOrd31, true);
                            ChangeOrdBtnColor(chkOrd32, false);
                            ChangeOrdBtnColor(chkOrd33, false);
                            ChangeOrdBtnColor(chkOrd34, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)
                        {
                            txtOrderCount4.Text = chkOrd41.Text;
                            ChangeOrdBtnColor(chkOrd41, true);
                            ChangeOrdBtnColor(chkOrd42, false);
                            ChangeOrdBtnColor(chkOrd43, false);
                            ChangeOrdBtnColor(chkOrd44, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
                        {
                            txtOrderCount5.Text = chkOrd51.Text;
                            ChangeOrdBtnColor(chkOrd51, true);
                            ChangeOrdBtnColor(chkOrd52, false);
                            ChangeOrdBtnColor(chkOrd53, false);
                            ChangeOrdBtnColor(chkOrd54, false);
                        }
                        break;
                    case 2:
                        if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
                        {
                            txtOrderCount1.Text = chkOrd12.Text;
                            ChangeOrdBtnColor(chkOrd11, false);
                            ChangeOrdBtnColor(chkOrd12, true);
                            ChangeOrdBtnColor(chkOrd13, false);
                            ChangeOrdBtnColor(chkOrd14, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
                        {
                            txtOrderCount2.Text = chkOrd22.Text;
                            ChangeOrdBtnColor(chkOrd21, false);
                            ChangeOrdBtnColor(chkOrd22, true);
                            ChangeOrdBtnColor(chkOrd23, false);
                            ChangeOrdBtnColor(chkOrd24, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
                        {
                            txtOrderCount3.Text = chkOrd32.Text;
                            ChangeOrdBtnColor(chkOrd31, false);
                            ChangeOrdBtnColor(chkOrd32, true);
                            ChangeOrdBtnColor(chkOrd33, false);
                            ChangeOrdBtnColor(chkOrd34, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)
                        {
                            txtOrderCount4.Text = chkOrd42.Text;
                            ChangeOrdBtnColor(chkOrd41, false);
                            ChangeOrdBtnColor(chkOrd42, true);
                            ChangeOrdBtnColor(chkOrd43, false);
                            ChangeOrdBtnColor(chkOrd44, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
                        {
                            txtOrderCount5.Text = chkOrd52.Text;
                            ChangeOrdBtnColor(chkOrd51, false);
                            ChangeOrdBtnColor(chkOrd52, true);
                            ChangeOrdBtnColor(chkOrd53, false);
                            ChangeOrdBtnColor(chkOrd54, false);
                        }
                        break;
                    case 3:
                        if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
                        {
                            txtOrderCount1.Text = chkOrd13.Text;
                            ChangeOrdBtnColor(chkOrd11, false);
                            ChangeOrdBtnColor(chkOrd12, false);
                            ChangeOrdBtnColor(chkOrd13, true);
                            ChangeOrdBtnColor(chkOrd14, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
                        {
                            txtOrderCount2.Text = chkOrd23.Text;
                            ChangeOrdBtnColor(chkOrd21, false);
                            ChangeOrdBtnColor(chkOrd22, false);
                            ChangeOrdBtnColor(chkOrd23, true);
                            ChangeOrdBtnColor(chkOrd24, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
                        {
                            txtOrderCount3.Text = chkOrd33.Text;
                            ChangeOrdBtnColor(chkOrd31, false);
                            ChangeOrdBtnColor(chkOrd32, false);
                            ChangeOrdBtnColor(chkOrd33, true);
                            ChangeOrdBtnColor(chkOrd34, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)
                        {
                            txtOrderCount4.Text = chkOrd43.Text ;
                            ChangeOrdBtnColor(chkOrd41, false);
                            ChangeOrdBtnColor(chkOrd42, false);
                            ChangeOrdBtnColor(chkOrd43, true);
                            ChangeOrdBtnColor(chkOrd44, false);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
                        {
                            txtOrderCount5.Text = chkOrd53.Text ;
                            ChangeOrdBtnColor(chkOrd51, false);
                            ChangeOrdBtnColor(chkOrd52, false);
                            ChangeOrdBtnColor(chkOrd53, true);
                            ChangeOrdBtnColor(chkOrd54, false);
                        }
                       
                        break;
                    case 4:
                        if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
                        {
                            txtOrderCount1.Text = chkOrd14.Text;
                            ChangeOrdBtnColor(chkOrd11, false);
                            ChangeOrdBtnColor(chkOrd12, false);
                            ChangeOrdBtnColor(chkOrd13, false);
                            ChangeOrdBtnColor(chkOrd14, true);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
                        {
                            txtOrderCount2.Text = chkOrd24.Text;
                            ChangeOrdBtnColor(chkOrd21, false);
                            ChangeOrdBtnColor(chkOrd22, false);
                            ChangeOrdBtnColor(chkOrd23, false);
                            ChangeOrdBtnColor(chkOrd24, true);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
                        {
                            txtOrderCount3.Text = chkOrd34.Text;
                            ChangeOrdBtnColor(chkOrd31, false);
                            ChangeOrdBtnColor(chkOrd32, false);
                            ChangeOrdBtnColor(chkOrd33, false);
                            ChangeOrdBtnColor(chkOrd34, true);
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)
                        {
                            txtOrderCount4.Text = chkOrd44.Text;
                            ChangeOrdBtnColor(chkOrd41, false);
                            ChangeOrdBtnColor(chkOrd42, false);
                            ChangeOrdBtnColor(chkOrd43, false);
                            ChangeOrdBtnColor(chkOrd44, true);
                            
                        }
                        else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
                        {
                            txtOrderCount5.Text = chkOrd54.Text;
                            ChangeOrdBtnColor(chkOrd51, false);
                            ChangeOrdBtnColor(chkOrd52, false);
                            ChangeOrdBtnColor(chkOrd53, false);
                            ChangeOrdBtnColor(chkOrd54, true);
                            
                        }
                        break;
                    default:
                        break;
                }

            }

        }
        private void ChangeOrdBtnColor(CheckBox chkOrd, bool bChecked)
        {
            chkOrd.Checked = bChecked;
            chkOrd.BackColor = chkOrd.Checked ? Color.FromArgb(105, 170, 142) : Color.FromArgb(100, 150, 250);
        }
        private void ChangeEarnTick(int index = 0)
        {

            if (index == 0)
            {
                int[] ordCnts = { 1, 1, 1, 1 };
                string[] sCnts = Settings.Default.EarnTicks.Split('#');

                if (sCnts == null && sCnts.Length < 4)
                    return;
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(sCnts[i], out ordCnts[i]))
                    {
                        ordCnts[i] = 1;
                    }
                }

                btnEarnTick1.Text = ordCnts[0].ToString();
                btnEarnTick2.Text = ordCnts[1].ToString();
                btnEarnTick3.Text = ordCnts[2].ToString();
                btnEarnTick4.Text = ordCnts[3].ToString();
            }
            else
            {
                switch (index)
                {
                    case 1:
                        txtPayoffEarn.Text = btnEarnTick1.Text;
                        ChangeTickBtnColor(btnEarnTick1, true);
                        ChangeTickBtnColor(btnEarnTick2, false);
                        ChangeTickBtnColor(btnEarnTick3, false);
                        ChangeTickBtnColor(btnEarnTick4, false);
                        break;
                    case 2:
                        txtPayoffEarn.Text = btnEarnTick2.Text;
                        ChangeTickBtnColor(btnEarnTick1, false);
                        ChangeTickBtnColor(btnEarnTick2, true);
                        ChangeTickBtnColor(btnEarnTick3, false);
                        ChangeTickBtnColor(btnEarnTick4, false);
                        break;
                    case 3:
                        txtPayoffEarn.Text = btnEarnTick3.Text;
                        ChangeTickBtnColor(btnEarnTick1, false);
                        ChangeTickBtnColor(btnEarnTick2, false);
                        ChangeTickBtnColor(btnEarnTick3, true);
                        ChangeTickBtnColor(btnEarnTick4, false);
                        break;
                    case 4:
                        txtPayoffEarn.Text = btnEarnTick4.Text;
                        ChangeTickBtnColor(btnEarnTick1, false);
                        ChangeTickBtnColor(btnEarnTick2, false);
                        ChangeTickBtnColor(btnEarnTick3, false);
                        ChangeTickBtnColor(btnEarnTick4, true);
                        break;
                    default: break;
                }
                btnEarnTick1.Invalidate();
                btnEarnTick2.Invalidate();
                btnEarnTick3.Invalidate();
                btnEarnTick4.Invalidate();
            }
        }
        private void ChangeLossTick(int index = 0)
        {

            if (index == 0)
            {
                int[] ordCnts = { 1, 1, 1, 1 };
                string[] sCnts = Settings.Default.LossTicks.Split('#');

                if (sCnts == null && sCnts.Length < 4)
                    return;
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(sCnts[i], out ordCnts[i]))
                    {
                        ordCnts[i] = 1;
                    }
                }

                btnLossTick1.Text = ordCnts[0].ToString();
                btnLossTick2.Text = ordCnts[1].ToString();
                btnLossTick3.Text = ordCnts[2].ToString();
                btnLossTick4.Text = ordCnts[3].ToString();
            }
            else
            {
                switch (index)
                {
                    case 1:
                        cmbPayoffLoss.Text = btnLossTick1.Text;
                        ChangeTickBtnColor(btnLossTick1, true);
                        ChangeTickBtnColor(btnLossTick2, false);
                        ChangeTickBtnColor(btnLossTick3, false);
                        ChangeTickBtnColor(btnLossTick4, false);
                        break;
                    case 2:
                        cmbPayoffLoss.Text = btnLossTick2.Text;
                        ChangeTickBtnColor(btnLossTick1, false);
                        ChangeTickBtnColor(btnLossTick2, true);
                        ChangeTickBtnColor(btnLossTick3, false);
                        ChangeTickBtnColor(btnLossTick4, false);
                        break;
                    case 3:
                        cmbPayoffLoss.Text = btnLossTick3.Text;
                        ChangeTickBtnColor(btnLossTick1, false);
                        ChangeTickBtnColor(btnLossTick2, false);
                        ChangeTickBtnColor(btnLossTick3, true);
                        ChangeTickBtnColor(btnLossTick4, false);
                        break;
                    case 4:
                        cmbPayoffLoss.Text = btnLossTick4.Text;
                        ChangeTickBtnColor(btnLossTick1, false);
                        ChangeTickBtnColor(btnLossTick2, false);
                        ChangeTickBtnColor(btnLossTick3, false);
                        ChangeTickBtnColor(btnLossTick4, true);
                        break;
                    default: break;
                }
                btnLossTick1.Invalidate();
                btnLossTick2.Invalidate();
                btnLossTick3.Invalidate();
                btnLossTick4.Invalidate();
            }
        }
        private void ChangeTickBtnColor(ReaLTaiizor.Controls.DreamButton btn, bool bChecked)
        {

            Color btnColor = bChecked? Color.FromArgb(105, 170, 142) : Color.FromArgb(100, 150, 250);

            btn.ColorA = btnColor;
            btn.ColorB = btnColor;
            btn.ColorC = btnColor;
            btn.ColorD = btnColor;
        }
        private void ChangeSettingControls(bool bForce = false)
        {

            if (_bLoadConfig && !bForce)
                return;

            _bLoadConfig = true;
            groupBetting1.Visible = false;
            groupBetting2.Visible = false;
            groupBetting3.Visible = false;
            groupBetting4.Visible = false;
            groupBetting5.Visible = false;
            groupBetting6.Visible = false;

            groupPayoff1.Visible = false;
            groupPayoff2.Visible = false;

            bool enableSmart = false;
            bool enableCross = false;
            bool enableCci = false;
            bool enableBoll = false;

            // string strCom = "개(최대 " + Settings.Default.OrderMax.ToString() + "개)";
            string strCom = "개";
            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT)
            {
                groupBetting1.Visible = true;
                groupPayoff1.Visible = true;

				cmbChartType1.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType1.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount1.Text = Settings.Default.OrderCount.ToString();
                label19.Text = strCom;
                cmbBettingCandle1.SelectedItem = Settings.Default.BettingCandleCount;

                enableSmart = true;
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

                cmbChartType3.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType3.SelectedIndex = Settings.Default.OrderType;
                chkBandChart_3.Checked = Settings.Default.BandChart;
                txtOrderCount3.Text = Settings.Default.OrderCount.ToString();
                label23.Text = strCom;
                cmbBettingCandle3.SelectedIndex = Settings.Default.BettingCandleComplete;
                cmbReorder3.SelectedIndex = Settings.Default.ReturnOption;
                cmbOrderLine1.SelectedIndex = Settings.Default.CrossAvgLine1;
                cmbOrderLine2.SelectedIndex = Settings.Default.CrossAvgLine2;

                //Group A
                chkConc1_3.Checked = Settings.Default.Conc1On;
                txtConc1Min_3.Text = Settings.Default.Conc1Min.ToString();
                txtConc1Cnt_3.Text = Settings.Default.Conc1Cnt.ToString();
                chkConc2_3.Checked = Settings.Default.Conc2On;
                txtConc2Cand_3.Text = Settings.Default.Conc2Candle.ToString();
                txtConc2Cnt_3.Text = Settings.Default.Conc2Cnt.ToString();
                chkAdx_3.Checked = Settings.Default.AdxOn;
                txtAdx_3.Text = Settings.Default.AdxCnt.ToString();
                //Group B
                chkCci_3.Checked = Settings.Default.CciOn;
                txtCci1_3.Text = Settings.Default.CciRange1.ToString();
                txtCci11_3.Text = Settings.Default.CciRange11.ToString();
                txtCci2_3.Text = Settings.Default.CciRange2.ToString();
                txtCci21_3.Text = Settings.Default.CciRange21.ToString();
                cmbCciSide1_3.SelectedIndex = Settings.Default.CciSide1;
                cmbCciSide2_3.SelectedIndex = Settings.Default.CciSide2;

                chkRsi_3.Checked = Settings.Default.RsiOn;
                txtRsi1_3.Text = Settings.Default.RsiRange1.ToString();
                txtRsi2_3.Text = Settings.Default.RsiRange2.ToString();
                cmbRsiSide1_3.SelectedIndex = Settings.Default.RsiSide1;
                cmbRsiSide2_3.SelectedIndex = Settings.Default.RsiSide2;

                chkAvgs_3.Checked = Settings.Default.AvgsOn;
                txtAvgsCandle_3.Text = Settings.Default.AvgsCandle.ToString();
                cmbAvgsSide1_3.SelectedIndex = Settings.Default.AvgsSide1;
                cmbAvgsSide2_3.SelectedIndex = Settings.Default.AvgsSide2;

                chkCrossSec3.Checked = Settings.Default.BoAdjustSecOn;
                txtCrossSec3.Text = Settings.Default.BoAdjustSec.ToString();

                cmbCciSide1_3.Location = new Point(144, 176);
                cmbCciSide2_3.Location = new Point(144, 199);
                txtCci11_3.Visible = false;
                txtCci21_3.Visible = false;
                label82.Visible = false;
                label81.Visible = false;

                enableSmart = true;
                enableCross = true;
                enableCci = true;
                enableBoll = true;
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE)
            {
                groupBetting4.Visible = true;
                groupPayoff1.Visible = true;

                cmbChartType4.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType4.SelectedIndex = Settings.Default.OrderType;
                chkBandChart_4.Checked = Settings.Default.BandChart;
                txtOrderCount4.Text = Settings.Default.OrderCount.ToString();
                label29.Text = strCom;
                cmbReorder4.SelectedIndex = Settings.Default.ReturnSBOption;
                //Group A
                chkConc1.Checked = Settings.Default.Conc1On;
                txtConc1Min.Text = Settings.Default.Conc1Min.ToString();
                txtConc1Cnt.Text = Settings.Default.Conc1Cnt.ToString();
                chkConc2.Checked = Settings.Default.Conc2On;
                txtConc2Cand.Text = Settings.Default.Conc2Candle.ToString();
                txtConc2Cnt.Text = Settings.Default.Conc2Cnt.ToString();
                chkAdx.Checked = Settings.Default.AdxOn;
                txtAdx.Text = Settings.Default.AdxCnt.ToString();
                chkBoAdjust4.Checked = Settings.Default.BoAdjustPerOn;
                txtBoAdjust4.Text = Settings.Default.BoLineAdjust.ToString();
                chkBoSec4.Checked = Settings.Default.BoAdjustSecOn;
                txtBoSec4.Text = Settings.Default.BoAdjustSec.ToString();

                ChangeBoOrdBtn(Settings.Default.BoOrdType);
                //Group B
                chkCci.Checked = Settings.Default.CciOn;
                txtCci1.Text = Settings.Default.CciRange1.ToString();
                txtCci11.Text = Settings.Default.CciRange11.ToString();
                txtCci2.Text = Settings.Default.CciRange2.ToString();
                txtCci21.Text = Settings.Default.CciRange21.ToString();
                cmbCciSide1.SelectedIndex = Settings.Default.CciSide1;
                cmbCciSide2.SelectedIndex = Settings.Default.CciSide2;

                chkRsi.Checked = Settings.Default.RsiOn;
                txtRsi1.Text = Settings.Default.RsiRange1.ToString();
                txtRsi2.Text = Settings.Default.RsiRange2.ToString();
                cmbRsiSide1.SelectedIndex = Settings.Default.RsiSide1;
                cmbRsiSide2.SelectedIndex = Settings.Default.RsiSide2;

                chkAvgs.Checked = Settings.Default.AvgsOn;
                txtAvgsCandle.Text = Settings.Default.AvgsCandle.ToString();
                cmbAvgsSide1.SelectedIndex = Settings.Default.AvgsSide1;
                cmbAvgsSide2.SelectedIndex = Settings.Default.AvgsSide2;

                enableSmart = true;
                enableCross = true;
                enableCci = true;
                enableBoll = true;
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID)
            {
                groupBetting5.Visible = true;
                groupPayoff1.Visible = true;                

                cmbChartType5.SelectedIndex = Settings.Default.ChartType;
                cmbOrderType5.SelectedIndex = Settings.Default.OrderType;
                txtOrderCount5.Text = Settings.Default.OrderCount.ToString();
                label39.Text = strCom;
                cmbOrderLine5_1.SelectedIndex = Settings.Default.CrossAvgLine1;
                cmbOrderLine5_2.SelectedIndex = Settings.Default.CrossAvgLine2;
                cmbBettingCandle5.SelectedIndex = Settings.Default.BettingCandleComplete;
                txtBoAdjust5.Text = Settings.Default.BoLineAdjust.ToString();
            }
            
            chkSmartLossPayoff.Visible = enableSmart;
            txtSmartEarn.Visible = enableSmart;
            txtSmartLoss.Visible = enableSmart;
            cmbSmartUnit.Visible = enableSmart;
            label54.Visible = enableSmart;
            label55.Visible = enableSmart;
            chkSmartRange.Visible = enableSmart;
            btnSmartRange.Visible = enableSmart;

            chkCrossLossPayoff.Visible = enableCross;
            txtCrossLoss.Visible = enableCross;
            cmbCrossUnit.Visible = enableCross;
            label61.Visible = enableCross;
            chkCrossRange.Visible = enableCross;
            btnCrossLossRange.Visible = enableCross;

            chkCciPayoff.Visible = enableCci;
            txtPayoffCci1.Visible = enableCci;
            txtPayoffCci2.Visible = enableCci;
            label73.Visible = enableCci;
            label75.Visible = enableCci;
            label76.Visible = enableCci;
            txtPayoffRsi.Visible = enableCci;
            chkCciRange.Visible = enableCci;
            btnCciLossRange.Visible = enableCci;

            chkPerbPayoff.Visible = enableBoll;
            txtPayoffPerbDown.Visible = enableBoll;
            txtPayoffPerbUp.Visible = enableBoll;
            chkPayoffMacd.Visible = enableBoll;
            chkWithEarn.Visible = enableBoll;
            label95.Visible = enableBoll;
            label96.Visible = enableBoll;

            chkEarnPayoff.Checked = Settings.Default.EarnPayoff;
			chkForceEarnPayoff.Checked = Settings.Default.ForceEarnPayoff;
            chkSmartLossPayoff.Checked = Settings.Default.SmartLossPayoff;
            txtSmartEarn.Text = Settings.Default.SmartEarnTick.ToString();
            txtSmartLoss.Text = Settings.Default.SmartLossTick.ToString();
            cmbSmartUnit.SelectedIndex = Settings.Default.SmartLossUnit;
            chkSmartRange.Checked = Settings.Default.SmartRangePayoff;

            chkCrossLossPayoff.Checked = Settings.Default.CrossLossPayoff;
            txtCrossLoss.Text = Settings.Default.CrossLossTick.ToString();
            cmbCrossUnit.SelectedIndex = Settings.Default.CrossLossUnit;
            chkCrossRange.Checked = Settings.Default.CrossRangePayoff;

            chkLossPayoff.Checked = Settings.Default.LossPayoff;
            chkCandlePayoff.Checked = Settings.Default.CandlePayoff;
            txtPayoffEarn.Text = Settings.Default.EarnPayoffMoney.ToString();
            cmbPayoffLoss.Text = Settings.Default.LossPayoffMoney.ToString();
            cmbCandlePayoff.SelectedItem = Settings.Default.CandlePayoffCount;
            txtTickPayoff.Text = Settings.Default.TickPayoffCount.ToString();
            chkLossRange.Checked = Settings.Default.LossRangePayoff;

            chkCciPayoff.Checked = Settings.Default.CciPayoff;
            chkCciRange.Checked = Settings.Default.CciRangePayoff;
            txtPayoffCci1.Text = Settings.Default.CciPayoffValue1.ToString();
            txtPayoffCci2.Text = Settings.Default.CciPayoffValue2.ToString();
            txtPayoffRsi.Text = Settings.Default.CciPayoffValue3.ToString();

            chkPerbPayoff.Checked = Settings.Default.BollPayoff;
            txtPayoffPerbDown.Text = Settings.Default.BollPayoffDown.ToString();
            txtPayoffPerbUp.Text = Settings.Default.BollPayoffUp.ToString();
            chkPayoffMacd.Checked = Settings.Default.MacdPayoff;
            chkWithEarn.Checked = Settings.Default.PayoffWithEarn;

            chkPerbPayoff2.Checked = Settings.Default.BollPayoff;
            txtPayoffPerbDown2.Text = Settings.Default.BollPayoffDown.ToString();
            txtPayoffPerbUp2.Text = Settings.Default.BollPayoffUp.ToString();
            chkPayoffMacd2.Checked = Settings.Default.MacdPayoff;
            chkWithEarn2.Checked = Settings.Default.PayoffWithEarn;

            chkLiqStop.Checked = Settings.Default.LiquidStop;
            chkEarnStop.Checked = Settings.Default.EarnStop;
            chkLossStop.Checked = Settings.Default.LossStop;
            chkOrderStop.Checked = Settings.Default.OrderStop;
            chkProfitStop.Checked = Settings.Default.ProfitStop;
            txtStopEarn.Text = Settings.Default.EarnStopMoney.ToString();
            txtStopLoss.Text = Settings.Default.LossStopMoney.ToString();
            txtStopOrder.Text = Settings.Default.OrderStopDelay.ToString();
            txtStopProfit.Text = Settings.Default.ProfitStopRate.ToString();
            // chkAlarmStop.Checked = Settings.Default.AlarmStop;
            // chkInformStop.Checked = Settings.Default.InformStop;
            //chkEarnPayoffN.Checked = Settings.Default.EarnPayoffN;
            //txtPayoffEarnN.Text = Settings.Default.EarnPayoffMoneyN.ToString();
            //chkLossPayoffN.Checked = Settings.Default.LossPayoffN;
            //txtPayoffLossN.Text = Settings.Default.LossPayoffMoneyN.ToString();
            chkOrderSelect.Checked = Settings.Default.OrderSelectOn;
            ChangeOrdSelBtn(Settings.Default.OrderSelectType);
            chkBothOrder.Checked = Settings.Default.BothOrder;
            chkGapLiquid.Checked = Settings.Default.GapLiquid;
            txtGapVal.Text = Settings.Default.GapValue.ToString();

            chkReverseOrder.Checked = Settings.Default.ReverseOrder;
            txtReverseOrd1.Text = Settings.Default.ReverseOrdCnt1.ToString();
            cmbReverseOrd1.SelectedIndex = Settings.Default.ReverseOrdSel1;
            txtReverseOrd2.Text = Settings.Default.ReverseOrdCnt2.ToString();
            cmbReverseOrd2.SelectedIndex = Settings.Default.ReverseOrdSel2;

            dtAutoReserve.Value = Settings.Default.AutoReserveTime;
            chkAutoReserve.Checked = Settings.Default.AutoReserveOn;

            _bLoadConfig = false;
            // Trace.TraceInformation("<FrmMain> ChangeSettingControls() End  _bLoadConfig:{0}", _bLoadConfig);
            EnableSettingControls();
            
        }

        private void EnableSettingControls()
        {
            //수익
            txtPayoffEarn.Enabled = chkEarnPayoff.Checked;
			//강제청산
            chkForceEarnPayoff.Visible = (chkEarnPayoff.Checked && (cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS || cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE)) ? true:false ;
            //스마트청산
            txtSmartEarn.Enabled = chkSmartLossPayoff.Checked && !chkSmartRange.Checked;
            txtSmartLoss.Enabled = chkSmartLossPayoff.Checked && !chkSmartRange.Checked;
            cmbSmartUnit.Enabled = chkSmartLossPayoff.Checked && !chkSmartRange.Checked;
            chkSmartRange.Enabled = chkSmartLossPayoff.Checked;
            //교차점 하락청산
            txtCrossLoss.Enabled = chkCrossLossPayoff.Checked && !chkCrossRange.Checked;
            cmbCrossUnit.Enabled = chkCrossLossPayoff.Checked && !chkCrossRange.Checked;
            chkCrossRange.Enabled = chkCrossLossPayoff.Checked;
            //손실
            cmbPayoffLoss.Enabled = chkLossPayoff.Checked;
            chkLossRange.Enabled = chkLossPayoff.Checked;
            //CCi 청산
            chkCciRange.Enabled = chkCciPayoff.Checked;
            txtPayoffCci1.Enabled = chkCciPayoff.Checked && !chkCciRange.Checked;
            txtPayoffCci2.Enabled = chkCciPayoff.Checked && !chkCciRange.Checked;
            txtPayoffRsi.Enabled = chkCciPayoff.Checked && !chkCciRange.Checked;
            //익절
            txtStopEarn.Enabled = chkEarnStop.Checked;
            //손절
            txtStopLoss.Enabled = chkLossStop.Checked;
            //실시간수익
            txtStopProfit.Enabled = chkProfitStop.Checked;
            //상승/하락
            cmbCandlePayoff.Enabled = chkCandlePayoff.Checked;
            txtTickPayoff.Enabled = chkCandlePayoff.Checked;

            //미체결취소
            txtStopOrder.Enabled = chkOrderStop.Checked;
            //기타설정
            //txtPayoffEarnN.Enabled = chkEarnPayoffN.Checked;
            //txtPayoffLossN.Enabled = chkLossPayoffN.Checked;
            // cmbOrderSelect.Enabled = chkOrderSelect.Checked;

            dtAutoReserve.Enabled = chkAutoReserve.Checked;
            txtSelVal.Enabled = chkSelVal.Checked;

            txtConc1Cnt.Enabled = chkConc1.Checked;
            txtConc1Min.Enabled = chkConc1.Checked;
            txtConc2Cand.Enabled = chkConc2.Checked;
            txtConc2Cnt.Enabled = chkConc2.Checked;
            txtAdx.Enabled = chkAdx.Checked;

            txtCci1.Enabled = chkCci.Checked;
            txtCci2.Enabled = chkCci.Checked;
            cmbCciSide1.Enabled = chkCci.Checked;
            cmbCciSide2.Enabled = chkCci.Checked;

            txtRsi1.Enabled = chkRsi.Checked;
            txtRsi2.Enabled = chkRsi.Checked;
            cmbRsiSide1.Enabled = chkRsi.Checked;
            cmbRsiSide2.Enabled = chkRsi.Checked;

            txtAvgsCandle_3.Enabled = chkAvgs_3.Checked;
            cmbAvgsSide1_3.Enabled = chkAvgs_3.Checked;
            cmbAvgsSide2_3.Enabled = chkAvgs_3.Checked;

            txtConc1Cnt_3.Enabled = chkConc1_3.Checked;
            txtConc1Min_3.Enabled = chkConc1_3.Checked;
            txtConc2Cand_3.Enabled = chkConc2_3.Checked;
            txtConc2Cnt_3.Enabled = chkConc2_3.Checked;
            txtAdx_3.Enabled = chkAdx_3.Checked;

            txtCci1_3.Enabled = chkCci_3.Checked;
            txtCci2_3.Enabled = chkCci_3.Checked;
            cmbCciSide1_3.Enabled = chkCci_3.Checked;
            cmbCciSide2_3.Enabled = chkCci_3.Checked;

            txtRsi1_3.Enabled = chkRsi_3.Checked;
            txtRsi2_3.Enabled = chkRsi_3.Checked;
            cmbRsiSide1_3.Enabled = chkRsi_3.Checked;
            cmbRsiSide2_3.Enabled = chkRsi_3.Checked;

            txtAvgsCandle_3.Enabled = chkAvgs_3.Checked;
            cmbAvgsSide1_3.Enabled = chkAvgs_3.Checked;
            cmbAvgsSide2_3.Enabled = chkAvgs_3.Checked;

            txtPayoffPerbDown.Enabled = chkPerbPayoff.Checked;
            txtPayoffPerbUp.Enabled = chkPerbPayoff.Checked;

            txtPayoffPerbDown2.Enabled = chkPerbPayoff2.Checked;
            txtPayoffPerbUp2.Enabled = chkPerbPayoff2.Checked;

            txtReverseOrd1.Enabled = chkReverseOrder.Checked;
            cmbReverseOrd1.Enabled = chkReverseOrder.Checked;
            txtReverseOrd2.Enabled = chkReverseOrder.Checked;
            cmbReverseOrd2.Enabled = chkReverseOrder.Checked;

            chkGapLiquid.Enabled = chkBothOrder.Checked;
            txtGapVal.Enabled = chkBothOrder.Checked;

            txtBoSec4.Enabled = chkBoSec4.Checked;
            txtBoAdjust4.Enabled = chkBoAdjust4.Checked;

            txtCrossSec3.Enabled = chkCrossSec3.Checked;
        }

        private void chkEarnPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }
        private void chkLossPayoff_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
            saveSetting();
        }

        private void chkEarnStop_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
            saveSetting();
        }
        private void chkLossStop_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
            saveSetting();
        }
        private void chkProfitStop_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }
        private void chkOrderStop_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
            saveSetting();
        }
        private void chkCandlePayoff4_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
        }
        private void chkCandlePayoff_CheckedChanged(object sender, EventArgs e)
        {
			EnableSettingControls();
            saveSetting();
        }
        private void chkSmartLossPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }
        private void chkCrossLossPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkEarnPayoffN_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
        }

        private void chkLossPayoffN_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
        }

        private void chkOrderSelect_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }
        private void cmbBettingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Trace.TraceError("<cmbBettingType_SelectedIndexChanged> cmbBettingType.SelectedIndex = {0} ", cmbBettingType.SelectedIndex);
            if (cmbBettingType.SelectedIndex == (int)BETTYPE.BOT1 && LockEx.locked) {
                if (!LockForm.Visible)
                {
                    LockForm.InitComponent();
                    LockForm.ShowDialog(this); //Modal Dialog
                    return;
                }

            }
            // Trace.TraceInformation("<FrmMain> cmbBettingType_SelectedIndexChanged()  _bLoadConfig:{0}", _bLoadConfig);
            if (!_bLoadConfig)
                AddLog("설정이 저장되었습니다.");
            ChangeSettingControls();
            Settings.Default.BettingType = (int)cmbBettingType.SelectedIndex;
            // Trace.TraceInformation("<FrmMain> cmbBettingType_SelectedIndexChanged() End  _bLoadConfig:{0}", _bLoadConfig);
            //saveSetting();
        }

        private void btnSettingSave_Click(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void saveSetting() 
        {
            // Trace.TraceInformation("<FrmMain> saveSetting() _bLoadConfig:{0}", _bLoadConfig);

            if (_bLoadConfig)
                return;
            // Trace.TraceInformation("<FrmMain> saveSetting()");

            string strWarning = "주문수량이 최대 " + Settings.Default.OrderMax.ToString() + "개를 초과할수 없습니다.";
            if (Settings.Default.OrderMax < 10)
            {
                strWarning += "\r\n고객센터에 문의해주세요";
            }
            string log = "";

            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT)       //동일색캔들
            {
                Settings.Default.BettingType = (int)BETTYPE.EQUIVALENT;
                Settings.Default.ChartType = cmbChartType1.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType1.SelectedIndex;
                try
                {
                    float nOrderCnt = float.Parse(txtOrderCount1.Text);
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

                log += "주문(방식:동일";
                log += ", 차트타입:" + cmbChartType1.SelectedItem.ToString();
                log += ", 주문타입:" + (Settings.Default.OrderType == 0 ? "시장가" : "지정가");
                log += ", 주문수량:" + Settings.Default.OrderCount;
                log += ", 동일색캔들:" + cmbBettingCandle1.SelectedItem.ToString();
                log += ") ";

            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.UPDOWN)     //이평선상하
            {
                Settings.Default.BettingType = (int)BETTYPE.UPDOWN;
                Settings.Default.ChartType = cmbChartType2.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType2.SelectedIndex;

                try
                {
                    float nOrderCnt = float.Parse(txtOrderCount2.Text);
                    if (nOrderCnt < 0)
                    {
                        txtOrderCount2.SelectAll();
                        txtOrderCount2.Focus();
                        return;
                    }
                    else if (nOrderCnt > Settings.Default.OrderMax)
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

                Settings.Default.BollPayoff = chkPerbPayoff2.Checked;
                try
                {
                    Settings.Default.BollPayoffDown = float.Parse(txtPayoffPerbDown2.Text);
                    Settings.Default.BollPayoffUp = float.Parse(txtPayoffPerbUp2.Text);
                }
                catch
                {
                    txtPayoffPerbDown2.Focus();
                    txtPayoffPerbUp2.Focus();
                    return;
                }
                Settings.Default.MacdPayoff = chkPayoffMacd2.Checked;
                Settings.Default.PayoffWithEarn = chkWithEarn2.Checked;


                log += "주문(방식:이평언오버";
                log += ", 차트타입:" + cmbChartType2.SelectedItem.ToString();
                log += ", 주문타입:" + (Settings.Default.OrderType == 0 ? "시장가" : "지정가");
                log += ", 주문수량:" + Settings.Default.OrderCount;
                log += ", 이동평균선:" + cmbAvgType2.SelectedItem.ToString();
                log += ", 상승/하락:" + cmbBettingCandle2.SelectedItem.ToString() + "개 "+ Settings.Default.BettingTickCount + "틱이상";
                log += ") ";

                if (chkCandlePayoff.Checked && txtTickPayoff.Visible)
                    log += "청산( 상승/하락:" + cmbCandlePayoff.SelectedItem.ToString() + "개 " + Settings.Default.TickPayoffCount + "틱";

                if (chkPerbPayoff2.Checked)
                {
                    log += ", 볼린저밴드 %B:" + Settings.Default.BollPayoffDown + "이하" + Settings.Default.BollPayoffUp + "이상";
                }
                if (chkPayoffMacd2.Checked)
                {
                    log += ", Macd전환";
                }
                if (chkWithEarn2.Checked)
                {
                    log += ", 수익조건";
                }
                log += ")";
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS)       //이평선교차
            {
                Settings.Default.BettingType = (int)BETTYPE.CROSS;
                Settings.Default.ChartType = cmbChartType3.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType3.SelectedIndex;
                Settings.Default.BandChart = chkBandChart_3.Checked;
                if(cmbOrderLine1.SelectedIndex == cmbOrderLine2.SelectedIndex)
                {
                    cmbOrderLine2.SelectAll();
                    cmbOrderLine2.Focus();
                    return;
                }
                Settings.Default.CrossAvgLine1 = cmbOrderLine1.SelectedIndex;
                Settings.Default.CrossAvgLine2 = cmbOrderLine2.SelectedIndex;
                try
                {
                    float nOrderCnt = float.Parse(txtOrderCount3.Text);
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
                    Settings.Default.ReturnOption = (byte)cmbReorder3.SelectedIndex;
                else
                    Settings.Default.ReturnOption = 0;

                Settings.Default.BoAdjustSecOn = chkCrossSec3.Checked;
                if (chkCrossSec3.Checked)
                {
                    try
                    {
                        int nAdjust = Int32.Parse(txtCrossSec3.Text);
                        if (nAdjust < 0 || nAdjust > 10000)
                        {
                            txtCrossSec3.SelectAll();
                            txtCrossSec3.Focus();
                            return;
                        }

                        Settings.Default.BoAdjustSec = nAdjust;
                    }
                    catch
                    {
                        txtCrossSec3.SelectAll();
                        txtCrossSec3.Focus();
                        return;
                    }
                }
                Settings.Default.Conc1On = chkConc1_3.Checked;
                if (chkConc1_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtConc1Min_3.Text);
                        if (nTemp < 1 || nTemp > 60)
                        {
                            txtConc1Min_3.SelectAll();
                            txtConc1Min_3.Focus();
                            return;
                        }

                        Settings.Default.Conc1Min = nTemp;
                    }
                    catch
                    {
                        txtConc1Min_3.SelectAll();
                        txtConc1Min_3.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtConc1Cnt_3.Text);
                        if (nTemp < 0)
                        {
                            txtConc1Cnt_3.SelectAll();
                            txtConc1Cnt_3.Focus();
                            return;
                        }

                        Settings.Default.Conc1Cnt = nTemp;
                    }
                    catch
                    {
                        txtConc1Cnt_3.SelectAll();
                        txtConc1Cnt_3.Focus();
                        return;
                    }
                }

                Settings.Default.Conc2On = chkConc2_3.Checked;
                if (chkConc2_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtConc2Cand_3.Text);
                        if (nTemp < 0)
                        {
                            txtConc2Cand_3.SelectAll();
                            txtConc2Cand_3.Focus();
                            return;
                        }

                        Settings.Default.Conc2Candle = nTemp;
                    }
                    catch
                    {
                        txtConc2Cand_3.SelectAll();
                        txtConc2Cand_3.Focus();
                        return;
                    }
                    try
                    {
                        int nTemp = Int32.Parse(txtConc2Cnt_3.Text);
                        if (nTemp < 0)
                        {
                            txtConc2Cnt_3.SelectAll();
                            txtConc2Cnt_3.Focus();
                            return;
                        }

                        Settings.Default.Conc2Cnt = nTemp;
                    }
                    catch
                    {
                        txtConc2Cnt_3.SelectAll();
                        txtConc2Cnt_3.Focus();
                        return;
                    }
                }
                Settings.Default.AdxOn = chkAdx_3.Checked;
                if (chkAdx_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtAdx_3.Text);
                        if (nTemp < 0)
                        {
                            txtAdx_3.SelectAll();
                            txtAdx_3.Focus();
                            return;
                        }

                        Settings.Default.AdxCnt = nTemp;
                    }
                    catch
                    {
                        txtAdx_3.SelectAll();
                        txtAdx_3.Focus();
                        return;
                    }
                }

                Settings.Default.CciOn = chkCci_3.Checked;
                if (chkCci_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtCci1_3.Text);
                        Settings.Default.CciRange1 = nTemp;
                    }
                    catch
                    {
                        // txtCci1_3.SelectAll();
                        txtCci1_3.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtCci2_3.Text);
                        Settings.Default.CciRange2 = nTemp;
                    }
                    catch
                    {
                        // txtCci2_3.SelectAll();
                        txtCci2_3.Focus();
                        return;
                    }
                    
                    Settings.Default.CciSide1 = cmbCciSide1_3.SelectedIndex;
                    Settings.Default.CciSide2 = cmbCciSide2_3.SelectedIndex;
                }

                Settings.Default.RsiOn = chkRsi_3.Checked;
                if (chkRsi_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtRsi1_3.Text);
                        Settings.Default.RsiRange1 = nTemp;
                    }
                    catch
                    {
                        txtRsi1_3.SelectAll();
                        txtRsi1_3.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtRsi2_3.Text);
                        Settings.Default.RsiRange2 = nTemp;
                    }
                    catch
                    {
                        txtRsi2_3.SelectAll();
                        txtRsi2_3.Focus();
                        return;
                    }
                    Settings.Default.RsiSide1 = cmbRsiSide1_3.SelectedIndex;
                    Settings.Default.RsiSide2 = cmbRsiSide2_3.SelectedIndex;
                }
                Settings.Default.AvgsOn = chkAvgs_3.Checked;
                if (chkAvgs_3.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtAvgsCandle_3.Text);
                        Settings.Default.AvgsCandle = nTemp;
                    }
                    catch
                    {
                        txtAvgsCandle_3.SelectAll();
                        txtAvgsCandle_3.Focus();
                        return;
                    }
                    Settings.Default.AvgsSide1 = cmbAvgsSide1_3.SelectedIndex;
                    Settings.Default.AvgsSide2 = cmbAvgsSide2_3.SelectedIndex;
                }

                log += "주문(방식:이평크로스";
                log += ", 차트타입:" + cmbChartType3.SelectedItem.ToString();
                log += ", 주문타입:" + (Settings.Default.OrderType == 0 ? "시장가" : "지정가");
                log += ", 크로스선:" + Common.GetAvgTypeStr(Settings.Default.CrossAvgLine1) + "&" + Common.GetAvgTypeStr(Settings.Default.CrossAvgLine2);
                if (Settings.Default.BoAdjustSecOn)
                {
                    log += " " + Settings.Default.BoAdjustSec + "초";
                }
                log += ", 주문수량:" + Settings.Default.OrderCount;
                log += ", 교차시:" + cmbBettingCandle3.SelectedItem.ToString();
                log += ", 되돌림:" + cmbReorder3.SelectedItem.ToString();

                if (chkConc1_3.Checked)
                    log += string.Format(", {0}분당 거래량 {1}이상", Settings.Default.Conc1Min, Settings.Default.Conc1Cnt);
                if (chkConc2_3.Checked)
                    log += string.Format(", {0}봉내 거래량 {1}%이상", Settings.Default.Conc2Candle, Settings.Default.Conc2Cnt);
                if (chkAdx_3.Checked)
                    log += string.Format(", ADX: {0}이상", Settings.Default.AdxCnt);

                if (chkCci_3.Checked)
                {
                    log += string.Format(", CCI: {0}이상 {1}, {2}이하 {3}",
                    Settings.Default.CciRange1, Settings.Default.CciSide1 == 0 ? "매수" : "매도",
                    Settings.Default.CciRange2, Settings.Default.CciSide2 == 0 ? "매수" : "매도");
                }
                if (chkRsi_3.Checked)
                {
                    log += string.Format(", RSI: {0}이상 {1}, {2}이하 {3}",
                        Settings.Default.RsiRange1, Settings.Default.RsiSide1 == 0 ? "매수" : "매도",
                        Settings.Default.RsiRange2, Settings.Default.RsiSide2 == 0 ? "매수" : "매도");
                }
                if (chkAvgs_3.Checked)
                {
                    log += string.Format(", 200일선: {0}봉 위{1}, 아래{2}", Settings.Default.AvgsCandle,
                            Settings.Default.AvgsSide1 == 0 ? "매수" : "매도",
                            Settings.Default.AvgsSide2 == 0 ? "매수" : "매도");
                }
                log += ") ";

            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE)       //하늘-주황라인
            {
                Settings.Default.BettingType = (int)BETTYPE.BOLINE;
                Settings.Default.ChartType = cmbChartType4.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType4.SelectedIndex;
                Settings.Default.BandChart = chkBandChart_4.Checked;
                try
                {
                    float nOrderCnt = float.Parse(txtOrderCount4.Text);
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

                Settings.Default.BettingCandleCount = 1;
                Settings.Default.BettingEnter = false;

                if (cmbReorder4.SelectedItem != null)
                    Settings.Default.ReturnSBOption = (byte)cmbReorder4.SelectedIndex;
                else
                    Settings.Default.ReturnSBOption = 0;
                //주-하선조종
                Settings.Default.BoAdjustPerOn = chkBoAdjust4.Checked;
                if (chkBoAdjust4.Checked)
                {
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
                Settings.Default.BoAdjustSecOn = chkBoSec4.Checked;
                if (chkBoSec4.Checked)
                {
                    try
                    {
                        int nAdjust = Int32.Parse(txtBoSec4.Text);
                        if (nAdjust < 0 || nAdjust > 10000)
                        {
                            txtBoSec4.SelectAll();
                            txtBoSec4.Focus();
                            return;
                        }

                        Settings.Default.BoAdjustSec = nAdjust;
                    }
                    catch
                    {
                        txtBoSec4.SelectAll();
                        txtBoSec4.Focus();
                        return;
                    }
                }

                Settings.Default.Conc1On = chkConc1.Checked;
                if (chkConc1.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtConc1Min.Text);
                        if (nTemp < 1 || nTemp > 60)
                        {
                            txtConc1Min.SelectAll();
                            txtConc1Min.Focus();
                            return;
                        }

                        Settings.Default.Conc1Min = nTemp;
                    }
                    catch
                    {
                        txtConc1Min.SelectAll();
                        txtConc1Min.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtConc1Cnt.Text);
                        if (nTemp < 0)
                        {
                            txtConc1Cnt.SelectAll();
                            txtConc1Cnt.Focus();
                            return;
                        }

                        Settings.Default.Conc1Cnt = nTemp;
                    }
                    catch
                    {
                        txtConc1Cnt.SelectAll();
                        txtConc1Cnt.Focus();
                        return;
                    }
                }

                Settings.Default.Conc2On = chkConc2.Checked;
                if (chkConc2.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtConc2Cand.Text);
                        if (nTemp < 0)
                        {
                            txtConc2Cand.SelectAll();
                            txtConc2Cand.Focus();
                            return;
                        }

                        Settings.Default.Conc2Candle = nTemp;
                    }
                    catch
                    {
                        txtConc2Cand.SelectAll();
                        txtConc2Cand.Focus();
                        return;
                    }
                    try
                    {
                        int nTemp = Int32.Parse(txtConc2Cnt.Text);
                        if (nTemp < 0)
                        {
                            txtConc2Cnt.SelectAll();
                            txtConc2Cnt.Focus();
                            return;
                        }

                        Settings.Default.Conc2Cnt = nTemp;
                    }
                    catch
                    {
                        txtConc2Cnt.SelectAll();
                        txtConc2Cnt.Focus();
                        return;
                    }
                }
                Settings.Default.AdxOn = chkAdx.Checked;
                if (chkAdx.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtAdx.Text);
                        if (nTemp < 0)
                        {
                            txtAdx.SelectAll();
                            txtAdx.Focus();
                            return;
                        }

                        Settings.Default.AdxCnt = nTemp;
                    }
                    catch
                    {
                        txtAdx.SelectAll();
                        txtAdx.Focus();
                        return;
                    }
                }
                
                Settings.Default.CciOn = chkCci.Checked;
                if (chkCci.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtCci1.Text);
                        Settings.Default.CciRange1 = nTemp;
                    }
                    catch
                    {
                        // txtCci1.SelectAll();
                        txtCci1.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtCci2.Text);
                        Settings.Default.CciRange2 = nTemp;
                    }
                    catch
                    {
                        // txtCci2.SelectAll();
                        txtCci2.Focus();
                        return;
                    }
                    if(Settings.Default.BoOrdType == 1)
                    {
                        try
                        {
                            int nTemp = Int32.Parse(txtCci11.Text);
                            Settings.Default.CciRange11 = nTemp;
                        }
                        catch
                        {
                            // txtCci11.SelectAll();
                            txtCci11.Focus();
                            return;
                        }

                        try
                        {
                            int nTemp = Int32.Parse(txtCci21.Text);
                            Settings.Default.CciRange21 = nTemp;
                        }
                        catch
                        {
                            // txtCci2s1.SelectAll();
                            txtCci21.Focus();
                            return;
                        }
                    }
                    Settings.Default.CciSide1 = cmbCciSide1.SelectedIndex;
                    Settings.Default.CciSide2 = cmbCciSide2.SelectedIndex;
                }

                Settings.Default.RsiOn = chkRsi.Checked;
                if (chkRsi.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtRsi1.Text);
                        Settings.Default.RsiRange1 = nTemp;
                    }
                    catch
                    {
                        txtRsi1.SelectAll();
                        txtRsi1.Focus();
                        return;
                    }

                    try
                    {
                        int nTemp = Int32.Parse(txtRsi2.Text);
                        Settings.Default.RsiRange2 = nTemp;
                    }
                    catch
                    {
                        txtRsi2.SelectAll();
                        txtRsi2.Focus();
                        return;
                    }
                    Settings.Default.RsiSide1 = cmbRsiSide1.SelectedIndex;
                    Settings.Default.RsiSide2 = cmbRsiSide2.SelectedIndex;
                }
                
                Settings.Default.AvgsOn = chkAvgs.Checked;
                if (chkAvgs.Checked)
                {
                    try
                    {
                        int nTemp = Int32.Parse(txtAvgsCandle.Text);
                        Settings.Default.AvgsCandle = nTemp;
                    }
                    catch
                    {
                        txtAvgsCandle.SelectAll();
                        txtAvgsCandle.Focus();
                        return;
                    }
                    Settings.Default.AvgsSide1 = cmbAvgsSide1.SelectedIndex;
                    Settings.Default.AvgsSide2 = cmbAvgsSide2.SelectedIndex;
                }

                log += "주문(방식:S-B선";
                log += ", 차트타입:" + cmbChartType4.SelectedItem.ToString();
                log += ", 주문타입:" + (Settings.Default.OrderType == 0 ? "시장가" : "지정가");
                log += ", 주문수량:" + Settings.Default.OrderCount;
                log += ", 되돌림:" + cmbReorder4.SelectedItem.ToString();
                log += ", 진입체결:" + (Settings.Default.BoOrdType == 0 ? "S-B선" : "CCI");
                if (Settings.Default.BoOrdType == 0)
                {
                    if(Settings.Default.BoAdjustPerOn || Settings.Default.BoAdjustSecOn)
                    {
                        log += ", S-B선조정:";
                        if (Settings.Default.BoAdjustPerOn)
                        {
                            log += Settings.Default.BoLineAdjust + "% ";
                        }
                        if (Settings.Default.BoAdjustSecOn)
                        {
                            log += Settings.Default.BoAdjustSec + "초";
                        }
                    }
                }

                if (chkConc1.Checked)
                    log += string.Format(", {0}분당 거래량 {1}이상", Settings.Default.Conc1Min, Settings.Default.Conc1Cnt);
                if(chkConc2.Checked)
                    log += string.Format(", {0}봉내 거래량 {1}%이상", Settings.Default.Conc2Candle, Settings.Default.Conc2Cnt);
                if (chkAdx.Checked)
                    log += string.Format(", ADX: {0}이상", Settings.Default.AdxCnt);

                if (chkCci.Checked)
                {
                    if(Settings.Default.BoOrdType == 0)
                    {
                        log += string.Format(", CCI: {0}이상 {1}, {2}이하 {3}",
                        Settings.Default.CciRange1, Settings.Default.CciSide1 == 0 ? "매수" : "매도",
                        Settings.Default.CciRange2, Settings.Default.CciSide2 == 0 ? "매수" : "매도");
                    } else
                    {
                        log += string.Format(", CCI: {0}상승 {1}이상 {2}, {3}하락 {4}이하 {5}",
                        Settings.Default.CciRange1, Settings.Default.CciRange11, Settings.Default.CciSide1 == 0 ? "매수" : "매도",
                        Settings.Default.CciRange2, Settings.Default.CciRange21, Settings.Default.CciSide2 == 0 ? "매수" : "매도");
                    }
                    
                }
                if (chkRsi.Checked)
                {
                    log += string.Format(", RSI: {0}이상 {1}, {2}이하 {3}",
                        Settings.Default.RsiRange1, Settings.Default.RsiSide1 == 0 ? "매수" : "매도",
                        Settings.Default.RsiRange2, Settings.Default.RsiSide2 == 0 ? "매수" : "매도");
                }
                if (chkAvgs.Checked)
                {
                    log += string.Format(", 200일선: {0}봉 위{1}, 아래{2}", Settings.Default.AvgsCandle,
                            Settings.Default.AvgsSide1 == 0 ? "매수" : "매도",
                            Settings.Default.AvgsSide2 == 0 ? "매수" : "매도");
                }
                log += ") ";
            }
            else if (cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID)       //이평주하
            {
                Settings.Default.BettingType = (int)BETTYPE.HYBRID;
                Settings.Default.ChartType = cmbChartType5.SelectedIndex;
                Settings.Default.OrderType = cmbOrderType5.SelectedIndex;
                if (cmbOrderLine5_1.SelectedIndex == cmbOrderLine5_2.SelectedIndex)
                {
                    cmbOrderLine5_2.SelectAll();
                    cmbOrderLine5_2.Focus();
                    return;
                }
                Settings.Default.CrossAvgLine1 = cmbOrderLine5_1.SelectedIndex;
                Settings.Default.CrossAvgLine2 = cmbOrderLine5_2.SelectedIndex;

                //주문가능수량
                try
                {
                    float nOrderCnt = float.Parse(txtOrderCount5.Text);
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

                log += "주문(방식:이평-SB";
                log += ", 차트타입:" + cmbChartType5.SelectedItem.ToString();
                log += ", 주문타입:" + (Settings.Default.OrderType == 0 ? "시장가" : "지정가");
                log += ", 주문수량:" + Settings.Default.OrderCount;
                log += ", 크로스선:" + Common.GetAvgTypeStr(Settings.Default.CrossAvgLine1) + "&" + Common.GetAvgTypeStr(Settings.Default.CrossAvgLine2); 
                log += ", 상승/하락:" + cmbBettingCandle5.SelectedItem.ToString();
                log += ", S-B선조정:" + Settings.Default.BoLineAdjust + "틱";
                log += ") ";
            }
            
            log = "[설정저장]" + log;
            AddLog(log);
            log = "";
            if (cmbBettingType.SelectedIndex == (int)BETTYPE.EQUIVALENT ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.CROSS ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.BOLINE ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.HYBRID ||
                cmbBettingType.SelectedIndex == (int)BETTYPE.BOT1)
            {
                log += "청산(";

                Settings.Default.EarnPayoff = chkEarnPayoff.Checked;
                if (chkEarnPayoff.Checked && txtPayoffEarn.Visible)
                {
                    try
                    {
						Settings.Default.ForceEarnPayoff = chkForceEarnPayoff.Checked;
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
                    log += "수익:"+ Settings.Default.EarnPayoffMoney+"틱";
                    if (Settings.Default.ForceEarnPayoff)
                        log += "-강제";
                }

                Settings.Default.LossPayoff = chkLossPayoff.Checked;
                if (chkLossPayoff.Checked && cmbPayoffLoss.Visible)
                {
                    try
                    {
                        
                        Settings.Default.LossPayoffMoney = Int32.Parse(cmbPayoffLoss.Text);
                        Settings.Default.LossRangePayoff = chkLossRange.Checked;

                        if (Settings.Default.LossPayoffMoney < 0)
                        {
                            cmbPayoffLoss.SelectAll();
                            cmbPayoffLoss.Focus();
                            return;
                        }
                    }
                    catch
                    {
                        cmbPayoffLoss.SelectAll();
                        cmbPayoffLoss.Focus();
                        return;
                    }
                    log += ", 손실:" + Settings.Default.LossPayoffMoney + "틱";
                    if(Settings.Default.LossRangePayoff)
                        log += ", 손실영역 ";

                }
                Settings.Default.SmartLossPayoff = chkSmartLossPayoff.Checked;
                if (chkSmartLossPayoff.Checked && chkSmartLossPayoff.Visible)
                {
                    Settings.Default.SmartRangePayoff = chkSmartRange.Checked;
                    Settings.Default.SmartLossUnit = cmbSmartUnit.SelectedIndex;
                    try
                    {
                        Settings.Default.SmartEarnTick = Int32.Parse(txtSmartEarn.Text);
                        if (Settings.Default.SmartEarnTick < 0)
                        {
                            txtSmartEarn.SelectAll();
                            txtSmartEarn.Focus();
                            return;
                        }
                    }
                    catch
                    {
                        txtSmartEarn.SelectAll();
                        txtSmartEarn.Focus();
                        return;
                    }

                    try
                    {
                        Settings.Default.SmartLossTick = Int32.Parse(txtSmartLoss.Text);
                        if (Settings.Default.SmartLossTick < 0)
                        {
                            txtSmartLoss.SelectAll();
                            txtSmartLoss.Focus();
                            return;
                        }
                    }
                    catch
                    {
                        txtSmartLoss.SelectAll();
                        txtSmartLoss.Focus();
                        return;
                    }
                    
                    log += ", 최대수익:";
                    if (chkSmartRange.Checked)
                        log += "영역청산";
                    else
                        log += Settings.Default.SmartEarnTick + "틱 " + Settings.Default.SmartLossTick + cmbSmartUnit.SelectedItem.ToString();
                }
                Settings.Default.CrossLossPayoff = chkCrossLossPayoff.Checked;
                if (chkCrossLossPayoff.Checked && chkCrossLossPayoff.Visible)
                {
                    Settings.Default.CrossLossUnit = cmbCrossUnit.SelectedIndex;
                    Settings.Default.CrossRangePayoff = chkCrossRange.Checked;
                    try
                    {
                        Settings.Default.CrossLossTick = Int32.Parse(txtCrossLoss.Text);
                        if (Settings.Default.CrossLossTick < 0)
                        {
                            txtCrossLoss.SelectAll();
                            txtCrossLoss.Focus();
                            return;
                        }
                    }
                    catch
                    {
                        txtCrossLoss.SelectAll();
                        txtCrossLoss.Focus();
                        return;
                    }
                    log += ", 교차점:";
                    if(chkCrossRange.Checked)
                        log += "영역청산";
                    else
                        log += Settings.Default.CrossLossTick + cmbCrossUnit.SelectedItem.ToString();
                }

                Settings.Default.CciPayoff = chkCciPayoff.Checked;
                if (chkCciPayoff.Checked && chkCciPayoff.Visible)
                {
                    Settings.Default.CciRangePayoff = chkCciRange.Checked;
                    try
                    {
                        Settings.Default.CciPayoffValue1 = Int32.Parse(txtPayoffCci1.Text);
                    }
                    catch
                    {
                        // txtPayoffCci1.SelectAll();
                        txtPayoffCci1.Focus();
                        return;
                    }
                    try
                    {
                        Settings.Default.CciPayoffValue2 = Int32.Parse(txtPayoffCci2.Text);
                    }
                    catch
                    {
                        // txtPayoffCci2.SelectAll();
                        txtPayoffCci2.Focus();
                        return;
                    }
                    try
                    {
                        Settings.Default.CciPayoffValue3 = Int32.Parse(txtPayoffRsi.Text);
                    }
                    catch
                    {
                        txtPayoffRsi.SelectAll();
                        txtPayoffRsi.Focus();
                        return;
                    }
                    log += ", CCI:";
                    if (chkCciRange.Checked)
                        log += "영역청산";
                    else
                        log += Settings.Default.CciPayoffValue1 + "이상" + Settings.Default.CciPayoffValue2+"%하락 RSI:"+ Settings.Default.CciPayoffValue3;
                }

                if (chkPerbPayoff.Visible)
                {
                    Settings.Default.BollPayoff = chkPerbPayoff.Checked;
                    try
                    {
                        Settings.Default.BollPayoffDown = float.Parse(txtPayoffPerbDown.Text);
                        Settings.Default.BollPayoffUp = float.Parse(txtPayoffPerbUp.Text);
                    }
                    catch
                    {
                        // txtPayoffPerbDown.SelectAll();
                        txtPayoffPerbDown.Focus();
                        txtPayoffPerbUp.Focus();
                        return;
                    }
                    if (chkPerbPayoff.Checked)
                    {
                        log += ", 볼린저밴드 %B:" + Settings.Default.BollPayoffDown + "이하" + Settings.Default.BollPayoffUp + "이상";
                    }
                    Settings.Default.MacdPayoff = chkPayoffMacd.Checked;
                    if (chkPayoffMacd.Checked)
                    {
                        log += ", Macd전환";
                    }
                    Settings.Default.PayoffWithEarn = chkWithEarn.Checked;
                    if (chkWithEarn.Checked)
                    {
                        log += ", 수익조건";
                    }
                }
                log += ") ";
            }

            log += "정지(";
            Settings.Default.ProfitStop = chkProfitStop.Checked;
            if (chkProfitStop.Checked)
            {
                try
                {
                    Settings.Default.ProfitStopRate = Int32.Parse(txtStopProfit.Text);
                    if (Settings.Default.ProfitStopRate < 0)
                    {
                        txtStopProfit.SelectAll();
                        txtStopProfit.Focus();
                        return;
                    }
                }
                catch
                {
                    txtStopProfit.SelectAll();
                    txtStopProfit.Focus();
                    return;
                }
                log += "실시간수익:" + Settings.Default.ProfitStopRate + "% 하락시 강제정지 ";

            }

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
                log += "익절:" + Settings.Default.EarnStopMoney + " ";

                Settings.Default.LiquidStop = chkLiqStop.Checked;
                if (Settings.Default.LiquidStop)
                    log += "강제 ";
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
                log += "손절:" + Settings.Default.LossStopMoney + " ";

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
            log += ") ";

            // Settings.Default.AlarmStop = chkAlarmStop.Checked ;
            // Settings.Default.InformStop = chkInformStop.Checked;
            //기타
            //Settings.Default.EarnPayoffN = chkEarnPayoffN.Checked;
            //if (chkEarnPayoffN.Checked)
            //{
            //    try
            //    {
            //        Settings.Default.EarnPayoffMoneyN = Int32.Parse(txtPayoffEarnN.Text);
            //        if (Settings.Default.EarnPayoffMoneyN < 0)
            //        {
            //            txtPayoffEarnN.SelectAll();
            //            txtPayoffEarnN.Focus();
            //            return;
            //        }
            //    }
            //    catch
            //    {
            //        txtPayoffEarnN.SelectAll();
            //        txtPayoffEarnN.Focus();
            //        return;
            //    }
            //}
            //Settings.Default.LossPayoffN = chkLossPayoffN.Checked;
            //if (chkLossPayoffN.Checked)
            //{
            //    try
            //    {
            //        Settings.Default.LossPayoffMoneyN = Int32.Parse(txtPayoffLossN.Text);
            //        if (Settings.Default.LossPayoffMoneyN < 0)
            //        {
            //            txtPayoffLossN.SelectAll();
            //            txtPayoffLossN.Focus();
            //            return;
            //        }
            //    }
            //    catch
            //    {
            //        txtPayoffLossN.SelectAll();
            //        txtPayoffLossN.Focus();
            //        return;
            //    }
            //}
            Settings.Default.OrderSelectOn = chkOrderSelect.Checked;
            Settings.Default.BothOrder = chkBothOrder.Checked;
            Settings.Default.GapLiquid = chkGapLiquid.Checked;
            if (chkGapLiquid.Checked)
            {
                try
                {
                    Settings.Default.GapValue = int.Parse(txtGapVal.Text);
                    if (Settings.Default.GapValue < 0)
                    {
                        txtGapVal.SelectAll();
                        txtGapVal.Focus();
                        return;
                    }
                }
                catch
                {
                    txtGapVal.SelectAll();
                    txtGapVal.Focus();
                    return;
                }
            }
            Settings.Default.ReverseOrder = chkReverseOrder.Checked;
            if (chkReverseOrder.Checked)
            {
                try
                {
                    Settings.Default.ReverseOrdCnt1 = Int32.Parse(txtReverseOrd1.Text);
                    if (Settings.Default.ReverseOrdCnt1 < 0)
                    {
                        txtReverseOrd1.SelectAll();
                        txtReverseOrd1.Focus();
                        return;
                    }
                }
                catch
                {
                    txtReverseOrd1.SelectAll();
                    txtReverseOrd1.Focus();
                    return;
                }
                Settings.Default.ReverseOrdSel1 = cmbReverseOrd1.SelectedIndex;

                try
                {
                    Settings.Default.ReverseOrdCnt2 = Int32.Parse(txtReverseOrd2.Text);
                    if (Settings.Default.ReverseOrdCnt2 < 0)
                    {
                        txtReverseOrd2.SelectAll();
                        txtReverseOrd2.Focus();
                        return;
                    }
                }
                catch
                {
                    txtReverseOrd2.SelectAll();
                    txtReverseOrd2.Focus();
                    return;
                }
                Settings.Default.ReverseOrdSel2 = cmbReverseOrd2.SelectedIndex;
            }

            log += "기타(";
            if (chkOrderSelect.Checked)
            {
                log += "선택주문:"+(Settings.Default.OrderSelectType==0?"전체":(Settings.Default.OrderSelectType==1)?"매수":"매도");
                log += ",";
            }
            if (chkBothOrder.Checked)
            {
                log += " 양방향주문";
                if (chkGapLiquid.Checked)
                {
                    log += ":Gap "+Settings.Default.GapValue+"USD";
                }
                log += ",";
            }
            if (chkOrderStop.Checked)
            {
                log += " 미체결취소:"+Settings.Default.OrderStopDelay+"초,";
            }
            if(chkReverseOrder.Checked)
            {
                log += string.Format(" 단계배팅:{0}회 {1}/{2}회 {3}",Settings.Default.ReverseOrdCnt1, Settings.Default.ReverseOrdSel1==0?"정배":"역배",
                                Settings.Default.ReverseOrdCnt2, Settings.Default.ReverseOrdSel2 == 0?"정배":"역배");
            }
            log += ")";

            Settings.Default.Save();
            AppAuthor.Default.UploadConfig();

            log = "[설정저장]" + log;
            AddLog(log);
            AddLog("설정이 저장되었습니다.");

            SetDChartType((CHARTTYPE)Settings.Default.ChartType);

        }
        private void btnSettingLoad_Click(object sender, EventArgs e)
		{

            if (MessageBox.Show("새로고침 하시겠습니까? ", "새로고침", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                LoadSettingControls();
            }
            
		}

        private void btnBandSetting_Click(object sender, EventArgs e)
        {
            if (!SettingBand.Visible)
            {
                SettingBand.loadControls();
                SettingBand.Show(this);
            }
        }

        private void btnSettingFetch_Click(object sender, EventArgs e)
        {
            if (LogicAuto.Default.IsRunning && Settings.Default.IsAutoMode)
                return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "설정파일 (*.dat)|*.dat;|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (AppConfig.ReadConfig(openFileDialog.FileName))
                    {
                        LoadSettingControls();
                        AddLog("불러오기되었습니다.");
                        SetDChartType((CHARTTYPE)Settings.Default.ChartType);
                    }
                }
            }
        }

        private void btnSettingExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog.Filter = "설정파일 (*.dat)|*.dat";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    if (AppConfig.SaveConfig(saveFileDialog.FileName))
                    {
                        MessageBox.Show(new Form { TopMost = true }, " 내보내기가 완료되었습니다.", "설정 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void cmbSiteList_DrawItem(object sender, DrawItemEventArgs e)
        {
			e.DrawBackground();
			if (e.Index >= 0)
			{
				e.Graphics.DrawString(cmbSiteList.Items[e.Index].ToString(), e.Font,
				 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
			}
        }

        private void cmbUserAccounts_DrawItem(object sender, DrawItemEventArgs e)
        {
			e.DrawBackground();
			if (e.Index >= 0)
			{
				e.Graphics.DrawString(cmbUserAccounts.Items[e.Index].ToString(), e.Font,
				 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
			}
        }

        private void cmbItemList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
			if (e.Index >= 0)
			{
				e.Graphics.DrawString(cmbItemList.Items[e.Index].ToString(), e.Font,
				 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
			}
        }

        private void cmbCandlePayoff_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbCandlePayoff.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderType4_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType4.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbChartType4_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType4.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderType5_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType5.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbChartType5_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType5.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbBettingCandle5_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingCandle5.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbReorder3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbReorder3.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }


        private void cmbOrderType3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType3.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbChartType3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType3.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbBettingCandle3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingCandle3.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderType1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbChartType1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbBettingCandle1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingCandle1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }


        private void cmbOrderType2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbChartType2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbBettingCandle2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingCandle2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
        private void cmbAvgType2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbAvgType2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }


        private void cmbBettingType_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingType.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkScroll_CheckedChanged(object sender, EventArgs e)
        {
            AutoScrollLog();
        }

        private void dtAutoReserve_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoReserveTime = dtAutoReserve.Value;
        }

        private void chkAutoReserve_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoReserveOn = chkAutoReserve.Checked;
            Settings.Default.AutoReserveTime = dtAutoReserve.Value;
            EnableSettingControls();
        }
        private void cmbSmartUnit_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbSmartUnit.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbCrossUnit_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbCrossUnit.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbChartType6_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbChartType6.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderType6_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderType6.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbBettingCross6_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbBettingCross6.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private bool CheckSetupKFOpenAPI()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            string sDrive = "";
            string sFile = "";
            foreach (DriveInfo d in allDrives)
            {

                if (d.IsReady == true && d.DriveType == DriveType.Fixed)
                {
                    sDrive = d.VolumeLabel;
                    sDrive = d.Name;
                    sFile = sDrive + "OpenAPIG/kfopenapi.ocx";
                    FileInfo file = new FileInfo(sFile);
                    if (file.Exists)
                        return true;
                }
            }

            return false;
        }

        private int ShowKFOpenLogin()
        {
            if (axKFOpenAPI == null)
                return 0;
            Process procKFLogin = Common.GetKFOpenLoginProc();
            if (procKFLogin != null)
            {
                procKFLogin.Kill();
            }
            return axKFOpenAPI.CommConnect(1);
        }
        private void CloseKFLoginDlg()
        {
            if (axKFOpenAPI == null)
                return ;
            Process procKFLogin = Common.GetKFOpenLoginProc();
            if (procKFLogin != null)
                procKFLogin.Kill();
        }
        private void DisconnectKFOpenAPI()
        {
            if (axKFOpenAPI == null)
                return;
            if(!axKFOpenAPI.IsDisposed)
                axKFOpenAPI.Dispose();
        }
        private void KF_OnEventConnect(object sender, AxKFOpenAPILib._DKFOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0)
            {
                AddLog("키움에 로그인되었습니다.");
                ShowKiwoomUserInfo();
            }
            else
            {
                AddLog("키움로그인에 실패하였습니다.");
            }
        }

        private void txtStopEarn_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtStopLoss_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtStopProfit_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtStopOrder_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkLiqStop_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCandlePayoff_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtTickPayoff_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtPayoffEarn_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkForceEarnPayoff_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtSmartEarn_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtSmartLoss_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCrossLoss_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbSmartUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCrossUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbChartType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtOrderCount1_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbBettingCandle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbChartType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtOrderCount2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbAvgType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbBettingCandle2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtBettingTick2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbChartType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtOrderCount3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbBettingCandle3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbReorder3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbChartType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtOrderCount4_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtBoAdjust4_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbChartType5_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderType5_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtOrderCount5_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbBettingCandle5_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtBoAdjust5_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbPayoffLoss_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPayoffLoss_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbPayoffLoss_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbPayoffLoss.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkConc1_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtConc1Min_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtConc1Cnt_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkConc2_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtConc2Cand_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtConc2Cnt_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkAdx_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtAdx_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnOrderBuy_Click(object sender, EventArgs e)
        {
            try
            {

                if (CurrentSite == null)
                    return;

                QuoteInfo quoteInfo = null;
                if (chkSelVal.Checked)
                {
                    double price = 0;

                    try
                    {
                        price = double.Parse(txtSelVal.Text);
                    }
                    catch
                    {
                        txtSelVal.SelectAll();
                        txtSelVal.Focus();
                        return;
                    }

                    quoteInfo = new QuoteInfo
                    {
                        Price = price
                    };
                    AddLog("[매수주문] 주문가:" + quoteInfo.Price);
                }

                double ordCnt = 0;
                try
                {
                    ordCnt = double.Parse(cmbOrderCnt.Text);
                }
                catch
                {
                    cmbOrderCnt.Focus();
                    AddLog("[매수주문] 주문수량 오류");
                    return;
                }
                //CurrentSite.DoBuyOrder(quoteInfo, ordCnt, quoteInfo==null);
                // 백그라운드 쓰레드에서 주문을 처리하게 함
                Task.Run(() => {
                    try
                    {
                        // 기존의 주문 실행 로직
                        CurrentSite.DoBuyOrder(quoteInfo, ordCnt, quoteInfo == null);
                    }
                    catch (Exception ex)
                    {
                        AddLog(ex.Message);
                    }
                });

            }
            catch (Exception) { }
        }

        private void btnOrderSell_Click(object sender, EventArgs e)
        {
            if (CurrentSite == null)
                return;

            QuoteInfo quoteInfo = null;
            if (chkSelVal.Checked)
            {
                double price = 0;

                try
                {
                    price = double.Parse(txtSelVal.Text);
                }
                catch
                {
                    txtSelVal.SelectAll();
                    txtSelVal.Focus();
                    return;
                }

                quoteInfo = new QuoteInfo
                {
                    Price = price
                };
                // AddLog("[매도주문] 주문가:" + quoteInfo.Price);
            }
            double ordCnt = 0;
            try
            {
                ordCnt = double.Parse(cmbOrderCnt.Text);
            }
            catch
            {
                cmbOrderCnt.Focus();
                AddLog("[매도주문] 주문수량 오류");
                return;
            }
            //CurrentSite.DoSellOrder(quoteInfo, ordCnt, quoteInfo==null);
            // 백그라운드 쓰레드에서 주문을 처리하게 함
            Task.Run(() => {
                try
                {
                    // 기존의 주문 실행 로직
                    CurrentSite.DoSellOrder(quoteInfo, ordCnt, quoteInfo == null);
                }
                catch (Exception ex)
                {
                    AddLog(ex.Message);
                }
            });
        }

        private void cmbOrderCnt_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderCnt.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkCrossRange_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkSmartRange_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void cmbSiteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void chkSignal_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.SignalSiteOn = chkSignal.Checked;
        }

        private void chkCci_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtCci1_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCciSide1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                } else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbCciSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnOrdSel4_Click(object sender, EventArgs e)
        {
            if (!OrdCntForm.Visible)
            {
                OrdCntForm.loadControls();
                OrdCntForm.Show(this);
            }
        }

        private void chkOrd41_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOrd41.Checked)
                ChangeOrdCnt(1);
        }

        private void chkOrd42_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOrd42.Checked)
                ChangeOrdCnt(2);
        }

        private void chkOrd43_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOrd43.Checked)
                ChangeOrdCnt(3);
        }

        private void chkOrd44_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOrd44.Checked)
                ChangeOrdCnt(4);
        }

        private void chkOrd11_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd11.Checked)
                ChangeOrdCnt(1);
        }

        private void chkOrd12_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd12.Checked)
                ChangeOrdCnt(2);
        }

        private void chkOrd13_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd13.Checked)
                ChangeOrdCnt(3);
        }

        private void chkOrd14_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd14.Checked)
                ChangeOrdCnt(4);
        }

        private void btnOrdSel1_Click(object sender, EventArgs e)
        {
            if (!OrdCntForm.Visible)
            {
                OrdCntForm.loadControls();
                OrdCntForm.Show(this);
            }
        }

        private void chkOrd21_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd21.Checked)
                ChangeOrdCnt(1);
        }

        private void chkOrd22_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd22.Checked)
                ChangeOrdCnt(2);
        }

        private void chkOrd23_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd23.Checked)
                ChangeOrdCnt(3);
        }

        private void chkOrd24_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd24.Checked)
                ChangeOrdCnt(4);
        }

        private void btnOrdSel2_Click(object sender, EventArgs e)
        {
            if (!OrdCntForm.Visible)
            {
                OrdCntForm.loadControls();
                OrdCntForm.Show(this);
            }
        }

        private void btnOrdSel5_Click(object sender, EventArgs e)
        {
            if (!OrdCntForm.Visible)
            {
                OrdCntForm.loadControls();
                OrdCntForm.Show(this);
            }
        }

        private void chkOrd52_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd52.Checked)
                ChangeOrdCnt(2);
        }

        private void chkOrd51_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd51.Checked)
                ChangeOrdCnt(1);
        }

        private void chkOrd53_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd53.Checked)
                ChangeOrdCnt(3);
        }

        private void chkOrd54_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd54.Checked)
                ChangeOrdCnt(4);
        }

        private void btnOrdSel3_Click(object sender, EventArgs e)
        {
            if (!OrdCntForm.Visible)
            {
                OrdCntForm.loadControls();
                OrdCntForm.Show(this);
            }
        }

        private void chkOrd31_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd31.Checked)
                ChangeOrdCnt(1);
        }

        private void chkOrd32_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd32.Checked)
                ChangeOrdCnt(2);
        }

        private void chkOrd33_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd33.Checked)
                ChangeOrdCnt(3);
        }

        private void chkOrd34_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd34.Checked)
                ChangeOrdCnt(4);
        }

        private void chkRsi_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void cmbCciSide1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCciSide2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbCciSide2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtRsi1_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtRsi2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbRsiSide1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbRsiSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbRsiSide1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbRsiSide2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbRsiSide2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbRsiSide2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkAvgs_CheckedChanged(object sender, EventArgs e)
        {

            EnableSettingControls();
            saveSetting();
        }


        private void txtAvgsCandle_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbAvgsSide1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbAvgsSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbAvgsSide1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbAvgsSide2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbAvgsSide2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbAvgsSide2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnSmartRange_Click(object sender, EventArgs e)
        {
            if (!SmartLossForm.Visible)
            {
                SmartLossForm.loadControls();
                SmartLossForm.Show(this);
            }
        }

        private void chkLossRange_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void btnLossRange_Click(object sender, EventArgs e)
        {
            if (!PayoffLossForm.Visible)
            {
                PayoffLossForm.loadControls();
                PayoffLossForm.Show(this);
            }
        }

        private void btnCrossLossRange_Click(object sender, EventArgs e)
        {
            if (!CrossLossForm.Visible)
            {
                CrossLossForm.loadControls();
                CrossLossForm.Show(this);
            }
        }

        private void chkCciPayoff_CheckedChanged(object sender, EventArgs e)
        {

            EnableSettingControls();
            saveSetting();
        }

        private void chkCciRange_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void btnCciLossRange_Click(object sender, EventArgs e)
        {
            if (!CciLossForm.Visible)
            {
                CciLossForm.loadControls();
                CciLossForm.Show(this);
            }
        }

        private void txtPayoffCci2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtPayoffCci1_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnEarnTick1_Click(object sender, EventArgs e)
        {
            ChangeEarnTick(1);
        }

        private void btnEarnTick2_Click(object sender, EventArgs e)
        {
            ChangeEarnTick(2);
        }

        private void btnEarnTick3_Click(object sender, EventArgs e)
        {
            ChangeEarnTick(3);
        }

        private void btnEarnTick4_Click(object sender, EventArgs e)
        {
            ChangeEarnTick(4);
        }

        private void btnEarnTickSet_Click(object sender, EventArgs e)
        {
            if (!EarnTickForm.Visible)
            {
                EarnTickForm.loadControls();
                EarnTickForm.Show(this);
            }
        }

        private void btnLossTick1_Click(object sender, EventArgs e)
        {
            ChangeLossTick(1);
        }

        private void btnLossTick2_Click(object sender, EventArgs e)
        {
            ChangeLossTick(2);
        }

        private void btnLossTick3_Click(object sender, EventArgs e)
        {
            ChangeLossTick(3);
        }

        private void btnLossTick4_Click(object sender, EventArgs e)
        {
            ChangeLossTick(4);
        }

        private void btnLossTickSet_Click(object sender, EventArgs e)
        {
            if (!LossTickForm.Visible)
            {
                LossTickForm.loadControls();
                LossTickForm.Show(this);
            }
        }

        private void btnSelOrderAll_Click(object sender, EventArgs e)
        {
            ChangeOrdSelBtn(0); //선택주문-전체
        }

        private void btnSelOrderBuy_Click(object sender, EventArgs e)
        {
            ChangeOrdSelBtn(1); //선택주문-매수
        }

        private void btnSelOrderSell_Click(object sender, EventArgs e)
        {
            ChangeOrdSelBtn(2); //선택주문-매도
        }
        private void btnSbOrd4_Click(object sender, EventArgs e)
        {
            ChangeBoOrdBtn(0); //진입체결-S-B선
            saveSetting();
        }
        private void btnCciOrd4_Click(object sender, EventArgs e)
        {
            ChangeBoOrdBtn(1); //진입체결-CCI
            saveSetting();
        }

        private void chkSelVal_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
        }

        private void txtPayoffRsi_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci11_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci21_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            if (!SyncForm.Visible)
            {
                SyncForm.loadControls();
                SyncForm.Show(this);
            }
        }

        private void chkConc1_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkConc2_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();

        }

        private void chkAdx_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();

        }

        private void chkCci_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();

        }

        private void chkRsi_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();

        }

        private void chkAvgs_3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();

        }

        private void txtConc1Min_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtConc1Cnt_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void txtConc2Cand_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtConc2Cnt_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtAdx_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci1_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci2_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci11_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCci21_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCciSide1_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbCciSide1_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbCciSide2_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbCciSide2_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtRsi1_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbRsiSide1_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbRsiSide1_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtRsi2_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbRsiSide2_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbRsiSide2_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtAvgsCandle_3_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbAvgsSide1_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbAvgsSide1_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbAvgsSide2_3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color itemColor = Color.Black;
                if (e.Index == 0)
                {
                    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                }
                else if (e.Index == 1)
                {
                    e.Graphics.FillRectangle(Brushes.DodgerBlue, e.Bounds);
                }
                else
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);

                e.Graphics.DrawString(cmbCciSide1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(itemColor/*e.ForeColor*/), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbAvgsSide2_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void btnBandSetting_4_Click(object sender, EventArgs e)
        {
            if (!SettingBand.Visible)
            {
                SettingBand.loadControls();
                SettingBand.Show(this);
            }
        }

        private void chkBandChart_4_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkBandChart_3_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbPrdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbPrdList.SelectedIndex;
            if (index >= 0)
            {
                ChangePrd(index);
            }
        }

        private void cmbPrdList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbPrdList.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkPerbPayoff_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtPayoffPerbDown_TextChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void txtPayoffPerbUp_TextChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void chkPayoffMacd_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkPerbPayoff2_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();

        }

        private void txtPayoffPerbDown2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void txtPayoffPerbUp2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void chkPayoffMacd2_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();

        }

        private void chkBothOrder_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkWithEarn_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }
        private void chkWithEarn2_CheckedChanged(object sender, EventArgs e)
        {
            saveSetting();
        }
        private void dgvValuationInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
        }

        private void dgvOrderInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
        }

        private void dgvItemPriceInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
        }

        private void cmbOrderLine1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderLine1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderLine1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderLine2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderLine2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderLine2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkReverseOrder_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtReverseOrd1_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtReverseOrd2_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbReverseOrd1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbReverseOrd1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbReverseOrd1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbReverseOrd2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbReverseOrd2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbReverseOrd2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderLine5_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderLine5_1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderLine5_1.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void cmbOrderLine5_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbOrderLine5_2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbOrderLine5_2.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void chkGapLiquid_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtGapVal_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void chkBoAdjust4_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkBoSec4_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void txtBoSec4_TextChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void txtCrossSec3_TextChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void chkCrossSec3_CheckedChanged(object sender, EventArgs e)
        {
            EnableSettingControls();
            saveSetting();
        }

        private void cmbReorder4_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSetting();
        }

        private void cmbReorder4_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(cmbReorder4.Items[e.Index].ToString(), e.Font,
                 new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }
    }
}
