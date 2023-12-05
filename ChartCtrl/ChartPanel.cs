using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ChartCtrl.Properties;

namespace ChartCtrl
{

    public partial class ChartPanel : Control
    {
        public ChartPanel()
        {
            InitializeComponent();
            InitializeComponentEx();
            initCtrl();
            
        }

        void InitializeComponentEx()
        {
            SettingForm.SetChartEventHandler(this.OnChartNoticeReceive);
        }

        private LineSetting SettingForm { get => LineSetting.Default; }

        public event EventHandler<ChartEventArgs> ChartNoticeEvent;

        //Color colorSel = Color.FromArgb(255, 30, 144, 224);
        //[Category("UserProperty"), Description("UnitTimeType")]
        public TIMETYPE UnitTimeType
        {
            get
            {
                return rChartCtrl.UnitTimeType;
            }
            set
            {
                rChartCtrl.UnitTimeType = value;
            }
        }
        public TIMEUNIT UnitTimeCount
        {
            get
            {
                return rChartCtrl.UnitTimeCount;
            }
            set
            {
                rChartCtrl.UnitTimeCount = value;
            }
        }
        /*
        //[Category("UserProperty"), Description("UnitValue")]
        public int UnitValue
        {
            get
            {
                return rChartCtrl.UnitValue;
            }
            set
            {
                rChartCtrl.UnitValue = value;

            }
        }
        */
        //[Category("UserProperty"), Description("GridRuler")]
        public bool GridRuler
        {
            get
            {
                return rChartCtrl.GridRuler;
            }
            set
            {
                rChartCtrl.GridRuler = value;

            }
        }
        //[Category("UserProperty"), Description("ChartStyle")]
        public int ChartStyle
        {
            get
            {
                return rChartCtrl.ChartStyle;
            }
            set
            {
                rChartCtrl.ChartStyle = value;

            }
        }
        public bool SaveRtVal
        {
            get
            {
                return rChartCtrl.SaveRtVal;
            }
            set
            {
                rChartCtrl.SaveRtVal = value;

            }
        }
        public void initCtrl()
        {
            InitBtnTimeType();
            initBtnTmUnit();
            initBtnTkUnit();
            SetToolTip();

            btnTimeMin.BackgroundImage = Resources.button_tm_on;

            btnTkUnit1.BackgroundImage = Resources.button_tm_on;

            txtSpec.Text = "";

            hScrollBar.Minimum = 0;
            hScrollBar.Maximum = 100;
            CtrlProperty._nBoLineAdjust = Settings.Default.BoLineAdjustR;
            btnAvg.BackColor = Settings.Default.AvgLine || Settings.Default.BoLine ? Color.White : Color.LightGray;

            for (int i=1; i<6; i++)
                cmbAvgLineWidth.Items.Add(i);

            cmbAvgLineWidth.SelectedItem = Settings.Default.AvgLineWidth;

            btnTimeTick_Click(this, new EventArgs());
            
            rChartCtrl.SetScrollBar(hScrollBar);
            rChartCtrl.InitCtrl();
        }

        public void InitDraw()
        {
            txtSpec.Text = "";
            rChartCtrl.InitCtrl();
        }
        public void InitChart()
        {
            txtSpec.Text = "";
            rChartCtrl.InitChart();
        }
        public void Redraw()
        {
            rChartCtrl.Redraw();
        }
        public void SetToolTip()
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            //toolTip.IsBalloon = true;

            toolTip.SetToolTip(this.btnZoomIn, "확대");
            toolTip.SetToolTip(this.btnZoomOut, "축소");
            toolTip.SetToolTip(this.btnAvg, "특성선 설정");
            toolTip.SetToolTip(this.cmbAvgLineWidth, "특성선 너비");
        }

        public void SaveSetting()
        {
            Settings.Default.Save();
        }

        public void SetRealTimeVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            rChartCtrl.SetRealTimeVal(fVal, dtReceive, nTick, nConc);
            showSpec();
        }
        private int m_tickLog = 0;
        public void showSpec()
        {
            if (Math.Abs(Environment.TickCount - m_tickLog) < 500)
                return ;
            m_tickLog = Environment.TickCount;

            if (CtrlProperty._RItemList.Count < 1)
                return;
            RItem rItem = CtrlProperty._RItemList.Last();
            string log = "";
            if (rItem.Adx > 0)
            {
                log += string.Format(" ADX:{0:N2}  | ", rItem.Adx);
            }

            if (rItem.Cci != 0)
            {
                log += string.Format(" CCI:{0:N2}  | ", rItem.Cci);
            }

            if (rItem.Rsi > 0)
            {
                log += string.Format(" RSI:{0:N2} ", rItem.Rsi);
            }

            if (log.Length > 0)
                txtSpec.Text = log;

        }
        private void InitBtnTimeType()
        {
            setBackGradient(btnTimeSec);
            setBackGradient(btnTimeMin);
            setBackGradient(btnTimeDay);
            setBackGradient(btnTimeMonth);
            setBackGradient(btnTimeWeek);
            setBackGradient(btnTimeYear);
            setBackGradient(btnTimeTick);
        }

        private void initBtnTmUnit()
        {

            setBackGradient(btnTmUnit1);
            setBackGradient(btnTmUnit2);
            setBackGradient(btnTmUnit3);
            setBackGradient(btnTmUnit4);
            setBackGradient(btnTmUnit5);
            setBackGradient(btnTmUnit6);
            setBackGradient(btnTmUnit7);
        }

        private void initBtnTkUnit()
        {

            setBackGradient(btnTkUnit01);
            setBackGradient(btnTkUnit02);
            setBackGradient(btnTkUnit03);
            setBackGradient(btnTkUnit1);
            setBackGradient(btnTkUnit2);
            setBackGradient(btnTkUnit3);
            setBackGradient(btnTkUnit4);
            setBackGradient(btnTkUnit5);
            setBackGradient(btnTkUnit6);
            setBackGradient(btnTkUnit7);
            setBackGradient(btnTkUnit8);
            setBackGradient(btnTkUnit9);
        }

        private void setBackGradient(ReaLTaiizor.Controls.DreamButton dreamBut, bool bActive = false)
        {
            if (bActive)
            {
                dreamBut.ColorA = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
                dreamBut.ColorB = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
                dreamBut.ColorC = Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(208)))), ((int)(((byte)(232)))));
                dreamBut.ColorD = Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(208)))), ((int)(((byte)(232)))));
                dreamBut.ColorE = Color.White;
                
            }
            else
            {
                dreamBut.ColorA = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
                dreamBut.ColorB = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
                dreamBut.ColorC = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
                dreamBut.ColorD = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
                dreamBut.ColorE = Color.White;

            }
            dreamBut.Invalidate();
        }
        

        private void enablebtnTkUnit(bool bEnable)
        {
            btnTmUnit1.Enabled = bEnable;
            btnTmUnit2.Enabled = bEnable;
            btnTmUnit3.Enabled = bEnable;
            btnTmUnit4.Enabled = bEnable;
            btnTmUnit5.Enabled = bEnable;
            btnTmUnit6.Enabled = bEnable;
            btnTmUnit7.Enabled = bEnable;

            btnTmUnit1.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit2.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit3.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit4.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit5.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit6.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;
            btnTmUnit7.ForeColor = bEnable ? SystemColors.ControlText : SystemColors.GrayText;

            if (rChartCtrl.UnitTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                panelTimeUnit1.Visible = false;
                panelTimeUnit2.Visible = true;
            }
            else
            {
                panelTimeUnit1.Visible = true;
                panelTimeUnit2.Visible = false;
            }
        }

        protected virtual void OnChartNoticeEvent(Object obj)
        {
            if (ChartNoticeEvent != null)
                ChartNoticeEvent(this, new ChartEventArgs(obj));
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
                        case CHART_EVENTTYPE.SETTING_CHANGED:
                            rChartCtrl.ChangeAvgLineStyle();
                            break;

                    }

                }
                catch (Exception) { }

            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            rChartCtrl.SetZoom(1);
        }
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            rChartCtrl.SetZoom(-1);
        }
        private void btnStyleCandle_Click(object sender, EventArgs e)
        {
            rChartCtrl.ChartStyle = 0;
            
        }
        private void btnStyleLine_Click(object sender, EventArgs e)
        {
            rChartCtrl.ChartStyle = 1;
            
        }
        private void btnTimeSec_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_SEC;
            InitBtnTimeType();
            enablebtnTkUnit(true);
            setBackGradient(btnTimeSec, true);
        }
        private void btnTimeSec_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_SEC)
            {
                btnTimeSec_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeMin_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_MIN;
            InitBtnTimeType();
            enablebtnTkUnit(true);
            setBackGradient(btnTimeMin, true);
        }
        private void btnTimeMin_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_MIN)
            {
                btnTimeMin_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeDay_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_DAY;
            InitBtnTimeType();
            enablebtnTkUnit(false);
            setBackGradient(btnTimeDay, true);
        }
        private void btnTimeDay_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_DAY)
            {
                btnTimeDay_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeWeek_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_WEEK;
            InitBtnTimeType();
            enablebtnTkUnit(false);
            setBackGradient(btnTimeWeek, true);
        }
        private void btnTimeWeek_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_WEEK)
            {
                btnTimeWeek_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeMonth_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_MONTH;
            InitBtnTimeType();
            enablebtnTkUnit(false);
            setBackGradient(btnTimeMonth, true);
        }
        private void btnTimeMonth_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_MONTH)
            {
                btnTimeMonth_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeYear_Click()
        {
            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_YEAR;
            InitBtnTimeType();
            enablebtnTkUnit(false);
            setBackGradient(btnTimeYear, true);
        }
        private void btnTimeYear_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_YEAR)
            {
                btnTimeYear_Click();
                btnTmUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTimeTick_Click()
        {
            // Trace.TraceError("<btnTimeTick_Click> ");

            rChartCtrl.UnitTimeType = TIMETYPE.TIMETYPE_TICK;
            InitBtnTimeType();
            enablebtnTkUnit(true);
            setBackGradient(btnTimeTick, true);
        }
        private void btnTimeTick_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeType != TIMETYPE.TIMETYPE_TICK)
            {
                btnTimeTick_Click();
                btnTkUnit1_Click(false);
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
            }
        }
        private void btnTkUnit01_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit01, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_1;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit01_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_1)
                btnTkUnit01_Click(true);
        }
        private void btnTkUnit02_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit02, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_15;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit02_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_15)
                btnTkUnit02_Click(true);
        }
        private void btnTkUnit03_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit03, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_30;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit03_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_30)
                btnTkUnit03_Click(true);
        }
        private void btnTkUnit1_Click(bool bRedraw)
        {
            // Trace.TraceError("<btnTkUnit1_Click> ");

            initBtnTkUnit();
            setBackGradient(btnTkUnit1, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_60;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit1_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_60)
                btnTkUnit1_Click(true);
        }
        private void btnTkUnit2_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit2, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_90;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit2_Click(object sender, EventArgs e)
        {
            if(rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_90)
                btnTkUnit2_Click(true);
        }
        private void btnTkUnit3_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit3, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_120;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit3_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_120)
                btnTkUnit3_Click(true);
        }
        private void btnTkUnit4_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit4, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_240;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit4_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_240)
                btnTkUnit4_Click(true);
        }
        private void btnTkUnit5_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit5, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_350;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit5_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_350)
                btnTkUnit5_Click(true);
        }
        private void btnTkUnit6_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit6, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_400;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit6_Click(object sender, EventArgs e)
        {
            if(rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_400)
                btnTkUnit6_Click(true);
        }
        private void btnTkUnit7_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit7, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_600;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit7_Click(object sender, EventArgs e)
        {
            if(rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_600)
                btnTkUnit7_Click(true);
        }
        private void btnTkUnit8_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit8, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_750;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit8_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_750)
                btnTkUnit8_Click(true);
        }
        private void btnTkUnit9_Click(bool bRedraw)
        {
            initBtnTkUnit();
            setBackGradient(btnTkUnit9, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_990;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTkUnit9_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_990)
                btnTkUnit9_Click(true);
        }

        private void btnAvg_Click(object sender, EventArgs e)
        {

            if (!SettingForm.Visible)
            {
                SettingForm.loadControls();
                SettingForm.Show(this);
            }

        }


        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {

            if(hScrollBar.Maximum > 1)
            {

                int iStartItem = hScrollBar.Value * CtrlProperty._nItemView / 10;
                if (iStartItem > CtrlProperty._nItemTotal - CtrlProperty._nItemView)
                    iStartItem = CtrlProperty._nItemTotal - CtrlProperty._nItemView;
                
                rChartCtrl.SetTimeAxis(iStartItem);
                rChartCtrl.Review();
                
            }
        }

        public bool SetRChartType(TIMETYPE iTimeType, TIMEUNIT nTimeUnit, int[] arrAvgCnt, int boLineAdjust)
        {
            bool bResult = false;
            if (CtrlProperty._RTimeType != iTimeType || CtrlProperty._RTimeUnitAmt != nTimeUnit)
            {
                Settings.Default.BoLineAdjustR = boLineAdjust;
                Trace.TraceError("<RChartCtrl> TimeType = {0}, TimeUnit = {1} ", iTimeType, nTimeUnit);

                switch (iTimeType)
                {
                    case TIMETYPE.TIMETYPE_SEC: btnTimeSec_Click(); break;
                    case TIMETYPE.TIMETYPE_MIN: btnTimeMin_Click(); break;
                    case TIMETYPE.TIMETYPE_DAY: btnTimeDay_Click(); break;
                    case TIMETYPE.TIMETYPE_WEEK: btnTimeWeek_Click(); break;
                    case TIMETYPE.TIMETYPE_MONTH: btnTimeMonth_Click(); break;
                    case TIMETYPE.TIMETYPE_YEAR: btnTimeYear_Click(); break;
                    case TIMETYPE.TIMETYPE_TICK: btnTimeTick_Click(); break;
                    default: break;
                }

                if (iTimeType == TIMETYPE.TIMETYPE_TICK)
                {
                    switch (nTimeUnit)
                    {
                        case TIMEUNIT.TIMEUNIT_1: btnTkUnit01_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_5: btnTkUnit02_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_30: btnTkUnit03_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_60: btnTkUnit1_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_90: btnTkUnit2_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_120: btnTkUnit3_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_240: btnTkUnit4_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_350: btnTkUnit5_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_400: btnTkUnit6_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_600: btnTkUnit7_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_750: btnTkUnit8_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_990: btnTkUnit9_Click(false); break;
                        default: break;
                    }
                } else if (iTimeType == TIMETYPE.TIMETYPE_SEC || iTimeType == TIMETYPE.TIMETYPE_MIN  )
                {
                    switch (nTimeUnit)
                    {
                        case TIMEUNIT.TIMEUNIT_1: btnTmUnit1_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_3: btnTmUnit2_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_5: btnTmUnit3_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_15: btnTmUnit4_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_30: btnTmUnit5_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_60: btnTmUnit6_Click(false); break;
                        case TIMEUNIT.TIMEUNIT_120: btnTmUnit7_Click(false); break;
                        default: break;
                    }
                } else
                {
                    btnTmUnit1_Click(false);
                }

                bResult = true;
            }

            if (!bResult && Settings.Default.BoLineAdjustR != boLineAdjust)
            {
                // Trace.TraceError("<RChartCtrl> LineAdjust = {0} => {1} ", Settings.Default.BoLineAdjustR, boLineAdjust);

                int nCount = CtrlProperty._RItemList.Count;
                Settings.Default.BoLineAdjustR = boLineAdjust;
                for (int i = 0; i < nCount; i++)
                {
                    CtrlProperty._RItemList[i].SetBoLineVal(false);
                }

            }
            return true;
        }
        public bool SetDChartType(TIMETYPE iTimeType, TIMEUNIT nTimeUnit, int[] arrAvgCnt, int boLineAdjust)
        {
            return rChartCtrl.SetDChartType(iTimeType, nTimeUnit, arrAvgCnt, boLineAdjust);
        }
        public bool SetDChart2Type(TIMETYPE iTimeType, TIMEUNIT nTimeUnit)
        {
            return rChartCtrl.SetDChart2Type(iTimeType, nTimeUnit);
        }

        public void SetChartFrom(DateTime dtFrom)
        {
            rChartCtrl.SetChartFrom(dtFrom);
        }

        public bool SetOrderInfo(OrderVal orderInfo)
        {
            return rChartCtrl.SetOrderInfo(orderInfo);
        }
        public List<DItem> GetDChartList(int nCount, bool bNeedLast)
        {
            return rChartCtrl.GetDChartList(nCount, bNeedLast);
        }
        public List<CItem> GetCChartList(int nCount, bool bNeedLast)
        {
            return rChartCtrl.GetCChartList(nCount, bNeedLast);
        }
        public int GetConcPerMin(int min)
        {
            return rChartCtrl.GetConcPerMin(min);
        }
        private void cmbAvgLineWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.AvgLineWidth = (int)cmbAvgLineWidth.SelectedItem;
            rChartCtrl.ChangeAvgLineStyle();
        }

        private void btnTmUnit1_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit1, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_1;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit1_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_1)
                btnTmUnit1_Click(true);
        }
        private void btnTmUnit2_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit2, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_3;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit2_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_3)
                btnTmUnit2_Click(true);
        }
        private void btnTmUnit3_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit3, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_5;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit3_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_5)
                btnTmUnit3_Click(true);
        }
        private void btnTmUnit4_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit4, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_15;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit4_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_15)
                btnTmUnit4_Click(true);
        }
        private void btnTmUnit5_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit5, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_30;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit5_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_30)
                btnTmUnit5_Click(true);
        }
        private void btnTmUnit6_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit6, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_60;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit6_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_60)
                btnTmUnit6_Click(true);
        }
        private void btnTmUnit7_Click(bool bRedraw)
        {
            initBtnTmUnit();
            setBackGradient(btnTmUnit7, true);
            rChartCtrl.UnitTimeCount = TIMEUNIT.TIMEUNIT_240;
            if (bRedraw)
                OnChartNoticeEvent(CHART_EVENTTYPE.DRAWING_CHANGED);
        }
        private void btnTmUnit7_Click(object sender, EventArgs e)
        {
            if (rChartCtrl.UnitTimeCount != TIMEUNIT.TIMEUNIT_240)
                btnTmUnit7_Click(true);
        }

        
    }
}
