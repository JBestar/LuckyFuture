using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using ChartCtrl.Properties;

namespace ChartCtrl
{

    public partial class RChartCtrl: Control
    {
        public RChartCtrl()
        {
            
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            SetAvgLineBrush();
        }

        public TIMETYPE UnitTimeType
        {
            get
            {
                return CtrlProperty._RTimeType;
            }
            set
            {
                CtrlProperty.SetTimeType(value);
                // Trace.TraceError("<RChartCtrl> UnitTimeType = {0} ", value);
                // InitChart();
            }
        }

        public TIMEUNIT UnitTimeCount
        {
            get
            {
                return CtrlProperty._RTimeUnitAmt;
            }
            set
            {
                CtrlProperty._RTimeUnitAmt = value;
                // Trace.TraceError("<RChartCtrl> UnitTimeCount = {0} ", value);
                InitChart();
            }
        }
        
        public bool GridRuler
        {
            get
            {
                return CtrlProperty._bGridRuler;
            }
            set
            {
                CtrlProperty._bGridRuler = value;

            }
        }
        public int ChartStyle
        {
            get
            {
                return CtrlProperty._iChartStyle;
            }
            set
            {
                CtrlProperty._iChartStyle = value;
                Invalidate();
            }
        }
        public bool SaveRtVal
        {
            get
            {
                return CtrlProperty._bSaveRtVal;
            }
            set
            {
                CtrlProperty._bSaveRtVal = value;

            }
        }
        private TimeAxis m_timeAxis = new TimeAxis();
        private ValueAxis m_valueAxis = new ValueAxis();
        private RItemGroup m_itemGroup = new RItemGroup();

        private HScrollBar m_hScrollBar = null;
        //List to Save Current Values
        private List<RealVal> mlistRealVal = new List<RealVal>();
        private readonly object _objLock = new object();


        private RectangleF mRectWorld ;     ///World View Region
        private Point mMousePt;             ///Curent MouseHover Or MouseClick Point
        private Point mCursorPt;
        private RItem mHoverItem = null;    ///Curent MouseHover Item
        private RItem mTmHoverItem = null;  ///Curent MouseHover Item
        private float mRealEndVal = -1;     ///RealTime End-Value
        private long m_tickCurrent = 0;

        private SolidBrush mBrushOpaque = new SolidBrush(Color.FromArgb(200, 194, 194, 194));
        Font m_fontStr = new Font(new FontFamily("돋움"), 9, FontStyle.Regular);

        Brush[] m_arrAvgBrush = new Brush[] {new SolidBrush(Color.FromArgb(255, 255, 0, 255)), new SolidBrush(Color.FromArgb(255, 0, 200, 255)),
            new SolidBrush(Color.Orange), new SolidBrush(Color.FromArgb(255, 0, 255, 0)), new SolidBrush(Color.DarkGray)};
        
        private float mCandleW = 0;
        public void InitCtrl()
        {
            Trace.TraceInformation("<RChartCtrl> InitCtrl");
            mlistRealVal.Clear();
            CtrlProperty._OrderList.Clear();
            SetClientRect();
            InitChart();
            CtrlProperty._DItemList.Clear();
            CtrlProperty._CItemList.Clear();

        }
        public void SetAvgLineBrush()
        {
            m_arrAvgBrush = new Brush[] {new SolidBrush(Settings.Default.AvgLineColor1), new SolidBrush(Settings.Default.AvgLineColor2),
            new SolidBrush(Settings.Default.AvgLineColor3), new SolidBrush(Settings.Default.AvgLineColor4), 
                new SolidBrush(Settings.Default.AvgLineColor5), new SolidBrush(Settings.Default.AvgLineColor6)};
        }
        public void Redraw()
        {
            if (CtrlProperty._nItemTotal < m_itemGroup.Count())
                CtrlProperty._nItemTotal = m_itemGroup.Count();

            int iStartItem = 0;
            if (m_itemGroup.Count() > CtrlProperty._nItemView)
                iStartItem = m_itemGroup.Count() - CtrlProperty._nItemView;

            CtrlProperty._OrderList.ForEach(o =>
            {
                o.Layouted = false;
            });
            if(CtrlProperty._OrderList.Count > 0)
            {
                RItem item = null;
                for (int i = m_itemGroup.listRItem.Count - 1; i >= 0; i--)
                {
                    item = m_itemGroup.listRItem[i];
                    // item.Orders.Clear();
                    for (int j = 0; j < CtrlProperty._OrderList.Count; j++)
                    {
                        OrderVal o = CtrlProperty._OrderList[j];
                        if (CtrlProperty._RTimeType == o.TimeType && CtrlProperty._RTimeUnitAmt == o.TimeUnit
                        && item.IsValidTime(o.OrderTm, 0) == 1 && o.Layouted == false)
                        {
                            o.Layouted = true;
                            if (!item.Orders.Contains(j))
                                item.Orders.Add(j);
                        }
                    }

                }
            }

            SetTimeAxis(iStartItem);
            if (CtrlProperty._ClientW > 0 && CtrlProperty._nItemView > 0)
                mCandleW = (float)CtrlProperty._ClientW / CtrlProperty._nItemView;

            RChartCtrl_SizeChanged(this, new EventArgs());
        }

        public void Review()
        {
            Invalidate();
        }
        /// <summary>
        /// inIt Chart
        /// </summary>
        public void InitChart()
        {
            m_tickCurrent = 0;
            CtrlProperty._nItemTotal = 50;
            CtrlProperty._nItemView = 50;

            SetAxis(0);

            m_itemGroup.Clear();

            if (SaveRtVal)
            {
                for (int i = 0; i < mlistRealVal.Count; i++)
                {
                    SetRItemVal(mlistRealVal[i]._fVal, mlistRealVal[i]._dtRec, mlistRealVal[i]._nTick, mlistRealVal[i]._nConc);
                }
            }
            CtrlProperty._OrderList.ForEach(o =>
            {
                o.Layouted = false;
            });


            if(CtrlProperty._OrderList.Count > 0)
            {
                RItem item = null;
                for (int i = m_itemGroup.listRItem.Count - 1; i >= 0; i--)
                {
                    item = m_itemGroup.listRItem[i];

                    for (int j = 0; j < CtrlProperty._OrderList.Count; j++)
                    {
                        OrderVal o = CtrlProperty._OrderList[j];
                        if (CtrlProperty._RTimeType == o.TimeType && CtrlProperty._RTimeUnitAmt == o.TimeUnit
                        && item.IsValidTime(o.OrderTm, 0) == 1 && o.Layouted == false)
                        {
                            o.Layouted = true;
                            if (!item.Orders.Contains(j))
                                item.Orders.Add(j);
                        }
                    }

                }
            }
            

            if (CtrlProperty._nItemTotal < m_itemGroup.Count())
                CtrlProperty._nItemTotal = m_itemGroup.Count();

            int iStartItem = 0;
            if (m_itemGroup.Count() > CtrlProperty._nItemView)
                iStartItem = m_itemGroup.Count() - CtrlProperty._nItemView;

            SetTimeAxis(iStartItem);
            if (CtrlProperty._ClientW > 0)
                mCandleW = (float)CtrlProperty._ClientW / CtrlProperty._nItemView;

            Invalidate();
        }
        ///Setting Time-Axis
        private void SetAxis(int iStartItem)
        {

            ///Fix Origin Time
            float fValMin = 1001000;
            float fValMax = 1002000;
            ///Set Time-Axis Value
            CtrlProperty._nTimeOrigin = iStartItem * CtrlProperty._nItemWidth;
            CtrlProperty._nTimeMax = (iStartItem + CtrlProperty._nItemView) * CtrlProperty._nItemWidth;
            CtrlProperty._fValueOrigin = fValMin;
            CtrlProperty._fValueMax = fValMax;

            SetScrollValue(iStartItem);

            ///Set World Region
            SetWorldArea();

        }
        public void SetScrollBar(HScrollBar hScrollBar)
        {
            m_hScrollBar = hScrollBar;
        }

        private void SetClientRect()
        {
            
            CtrlProperty._ClientW = ClientSize.Width > CtrlProperty._nValueAxisBand ? ClientSize.Width- CtrlProperty._nValueAxisBand : ClientSize.Width;
            CtrlProperty._ClientH = ClientSize.Height > 80 ? ClientSize.Height - 80: ClientSize.Height;
        }

        public void ChangeAvgLineStyle()
        {
            SetAvgLineBrush();
            m_itemGroup.SetAvgLinePen();

            if(Settings.Default.BoLineAdjustR != CtrlProperty._nBoLineAdjust)
            {
                Settings.Default.BoLineAdjustR = CtrlProperty._nBoLineAdjust;

                Settings.Default.Save();
                int nCount = CtrlProperty._RItemList.Count;
                for (int i = 0; i < nCount; i++)
                {
                    CtrlProperty._RItemList[i].SetBoLineVal(false);
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (m_itemGroup.Count() < 1)
            {
                CtrlProperty._nItemTotal = 50;
                CtrlProperty._nItemView = 50;

                SetAxis(0);
                SetTimeAxis(0);
            }

            if (CtrlProperty._ClientW == 0 || CtrlProperty._ClientH == 0)
            {
                SetClientRect();
            }
            ///Background
            g.FillRectangle(CtrlProperty._DrawPaneBrush, new RectangleF(0, 0, CtrlProperty._ClientW+CtrlProperty._nValueAxisBand, CtrlProperty._ClientH));

            ///Setting Clent Area
            if (mRectWorld.Width <= 0 || mRectWorld.Height <= 0)
            {
                mRectWorld.Width = CtrlProperty._ClientW;
                mRectWorld.Height = CtrlProperty._ClientH;
                
            }
            ///Draw Axis
            DrawAxis(g);


            ///Border
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, CtrlProperty._ClientW + CtrlProperty._nValueAxisBand - 1, CtrlProperty._ClientH- 1));

            ///Transform Coordiante
            PointF[] arrDevicePoints = 
                {
                new PointF(0, CtrlProperty._ClientH),
                new PointF(CtrlProperty._ClientW, CtrlProperty._ClientH),
                new PointF(0, 0),
            };
            Matrix mxNextTrans = new Matrix(mRectWorld, arrDevicePoints);
            Matrix mxPrevTrans = g.Transform;
            g.Transform = mxNextTrans;
            
            ///Draw Chart
            DrawRChart(g);

            ///Transform Previous Coordiante 
            g.Transform = mxPrevTrans;
            ///Draw Label
            DrawLabel(g);

            ///Draw Comment
            DrawComment(g);

            ///Draw Realtime Value
            DrawRealValue(g);

            ///Draw Current Cursor Value
            DrawCursorValue(g);
        }

        private void DrawAxis(Graphics g)
        {
            ///Draw value-Axis
            try
            {
                if (m_itemGroup.Count() > 0)
                    m_valueAxis.Draw(g);

                ///Draw Time-Axis
                m_itemGroup.DrawAxis(g);
            }
            catch (Exception) { }


        }

        private void DrawLabel(Graphics g)
        {
            int iX = 20, iY = 10;

            if (Settings.Default.AvgLine)
            {

                string strLabel = "종가단순";
                g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[0], new PointF(iX, iY));
                iX += 50;
                if (Settings.Default.Avg5On)
                {
                    strLabel = "5";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[0], new PointF(iX += 20, iY));
                }
                if (Settings.Default.Avg10On)
                {
                    strLabel = "10";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[1], new PointF(iX += 20, iY));
                }
                if (Settings.Default.Avg20On)
                {
                    strLabel = "20";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[2], new PointF(iX += 20, iY));
                }
                if (Settings.Default.Avg60On)
                {
                    strLabel = "60";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[3], new PointF(iX += 20, iY));
                }
                if (Settings.Default.Avg120On)
                {
                    strLabel = "120";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[4], new PointF(iX += 20, iY));
                }
                if (Settings.Default.Avg200On)
                {
                    strLabel = "200";
                    g.DrawString(strLabel, m_fontStr, m_arrAvgBrush[5], new PointF(iX += 30, iY));
                }
            }
        }

        private void DrawComment(Graphics g)
        {
            ///Draw Selected Item Comment
            if (mHoverItem != null)
            {
                int iY = mMousePt.Y;
                int iX = mMousePt.X;
                int nWidth = 200;
                int nHeight = 130; //160

                if (Settings.Default.AvgLine)
                {
                    nHeight += 15;
                    int avgCnt = 0;
                    if (Settings.Default.Avg5On)
                        avgCnt++;
                    if (Settings.Default.Avg10On)
                        avgCnt++;
                    if (Settings.Default.Avg20On)
                        avgCnt++;
                    if (Settings.Default.Avg60On)
                        avgCnt++;
                    if (Settings.Default.Avg120On)
                        avgCnt++;
                    if (Settings.Default.Avg200On)
                        avgCnt++;
                    nHeight += (int)(Math.Ceiling((double)avgCnt / 2) * 15);
                }

                if (Settings.Default.BoLine)
                    nHeight += 30;

                if(mHoverItem.Orders != null && mHoverItem.Orders.Count > 0)
                    nHeight += 30;

                if (iY + nHeight > CtrlProperty._ClientH - CtrlProperty._nTimeAxisBand)
                    iY = CtrlProperty._ClientH - nHeight - CtrlProperty._nTimeAxisBand;

                if (iX + nWidth > CtrlProperty._ClientW)
                    iX = CtrlProperty._ClientW - nWidth;


                g.FillRectangle(mBrushOpaque, new RectangleF(iX, iY, nWidth, nHeight));
                g.DrawRectangle(Pens.Black, new Rectangle(iX, iY, nWidth, nHeight));

                string strComment = "시간:" + string.Format(CtrlProperty._tDayPattern, CtrlProperty.GetTime(mHoverItem.TmStart)) + " " + string.Format(CtrlProperty._tTmPattern, CtrlProperty.GetTime(mHoverItem.TmStart));
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 5));

                strComment = "시가:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.Start / CtrlProperty._nValueRate);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 20));

                strComment = "고가:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.Max / CtrlProperty._nValueRate);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));

                strComment = "저가:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.Min / CtrlProperty._nValueRate);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                strComment = "종가:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.End / CtrlProperty._nValueRate);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));

                strComment = "ADX:" + string.Format("{0:N2}", mHoverItem.Adx);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                strComment = "CCI:" + string.Format("{0:N2}", mHoverItem.Cci);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));

                strComment = "RSI:" + string.Format("{0:N2}", mHoverItem.Rsi);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                strComment = "거래량:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.Conc);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));

                strComment = "[MACD]:" + string.Format("{0:N2}", mHoverItem.MacdVal);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                strComment = "시그널:" + string.Format(CtrlProperty._tValueFormat, mHoverItem.MacdSig);
                g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));

                if (mHoverItem.BollAvg != 0)
                {
                    strComment = "[볼린저밴드]";
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));
                    strComment = "%B:" +string.Format(CtrlProperty._tValueFormat, mHoverItem.BollPerb);
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));
                    strComment = "상한:" + string.Format(CtrlProperty._tValueFormat, (mHoverItem.BollAvg + 2 * mHoverItem.BollDev) / CtrlProperty._nValueRate);
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));
                    strComment = "하한:" + string.Format(CtrlProperty._tValueFormat, (mHoverItem.BollAvg - 2 * mHoverItem.BollDev) / CtrlProperty._nValueRate);
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY));
                }

                if (Settings.Default.AvgLine)
                {
                    strComment = "[가격 이동평균]";
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));
                    int j = 0;
                    for (int i = 0; i < mHoverItem.Avgs.Length; i++)
                    {
                        if (i == 0 && !Settings.Default.Avg5On)
                            continue;
                        if (i == 1 && !Settings.Default.Avg10On)
                            continue;
                        if (i == 2 && !Settings.Default.Avg20On)
                            continue;
                        if (i == 3 && !Settings.Default.Avg60On)
                            continue;
                        if (i == 4 && !Settings.Default.Avg120On)
                            continue;
                        if (i == 5 && !Settings.Default.Avg200On)
                            continue;

                        if (mHoverItem.Avgs[i] > 0)
                        {
                            strComment = CtrlProperty._arrAvgItems[i].ToString();
                            if (CtrlProperty._arrAvgItems[i].ToString().Length > 2)
                                strComment += "  :";
                            else if (CtrlProperty._arrAvgItems[i].ToString().Length > 1)
                                strComment += "   :";
                            else strComment += "     :";
                            strComment += string.Format(CtrlProperty._tValueFormat, mHoverItem.Avgs[i] / CtrlProperty._nValueRate);
                            g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + (j%2==0?5:105), iY += (j % 2 == 0 ? 15 : 0)));
                            j++;
                        }

                    }
                }
                if (Settings.Default.BoLine)
                {
                    strComment = "[하늘-주황선]";
                    g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));
                    if (mHoverItem.Est_Crossed)
                        g.DrawString("교차:" + mHoverItem.Est_Cross.ToString(), m_fontStr, Brushes.Black, new PointF(iX + 105, iY ));
                    //else g.DrawString("유지", m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));
                    for (int i = 0; i < mHoverItem.Bos.Length; i++)
                    {
                        if (mHoverItem.Bos[i] > 0)
                        {
                            strComment = i == 0 ? "하늘:" : "주황:";
                            strComment += string.Format(CtrlProperty._tValueFormat, mHoverItem.Bos[i] / CtrlProperty._nValueRate);
                            g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + (i==0?5:105), iY += (i == 0 ? 15 : 0)));
                        }

                    }
                    
                }
                
                

                if (mHoverItem.Orders != null && mHoverItem.Orders.Count > 0 )
                {
                    
                    OrderVal orderInfo = CtrlProperty._OrderList[mHoverItem.Orders.Last()];
                    if(orderInfo != null)
                    {
                        strComment = "[" + orderInfo.OrderType + "-";
                        strComment += (orderInfo.ResultState == RESULTSTATE.BUY ? "매수" : "매도") + "]";
                        g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                        strComment = "시간:" + string.Format(CtrlProperty._tTmPattern, CtrlProperty.GetTime(orderInfo.OrderTm));
                        g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 5, iY += 15));

                        strComment = "체결:" + string.Format(CtrlProperty._tValueFormat, orderInfo.AveragePrice);
                        g.DrawString(strComment, m_fontStr, Brushes.Black, new PointF(iX + 105, iY ));
                    }
                    
                }
            }

        }

        public void SetScrollValue(int iStart)
        {
            if (m_hScrollBar == null)
                return;

            if (iStart > CtrlProperty._nItemTotal)
                return;

            int nScrMax = 0;
            if (CtrlProperty._nItemView == CtrlProperty._nItemTotal) { 
                nScrMax = 1;
                if (m_hScrollBar.Height > 0)
                    m_hScrollBar.Height = 0;
            }
            else { 
                nScrMax = (int)Math.Round(CtrlProperty._nItemTotal*10 / (float)CtrlProperty._nItemView);
                if (m_hScrollBar.Height < 1)
                    m_hScrollBar.Height = CtrlProperty._nHScrollbarHeight;
            }
            int nScrVal = 0;
            if (iStart == CtrlProperty._nItemTotal - CtrlProperty._nItemView)
                nScrVal = nScrMax;
            else nScrVal = (int)Math.Round(iStart * 10 / (float)CtrlProperty._nItemView); 

            if (m_hScrollBar.Value != nScrVal || m_hScrollBar.Maximum != nScrMax)
            {
                m_hScrollBar.Minimum = 0;
                m_hScrollBar.Maximum = nScrMax;
                m_hScrollBar.Value = nScrVal;
            }
            
        }
        private void DrawRealValue(Graphics g)
        {
            try { 
                if (mRealEndVal > 0)
                {
                    m_valueAxis.DrawEndValue(g, mRealEndVal);
                }
            }
            catch (Exception) { }
        }
        private void DrawCursorValue(Graphics g)
        {
            if (mCursorPt.X > 0 && mCursorPt.Y > 0 && mTmHoverItem != null)
            {
                m_timeAxis.DrawCursorValue(g, mCursorPt.X, mTmHoverItem.TmStart);
                m_valueAxis.DrawCursorValue(g, mCursorPt.Y);
            }
        }

        private void DrawRChart(Graphics g)
        {
            ///Draw Chart
            m_itemGroup.Draw(g);

        }

        

        public void SetTimeAxis(int iStartIdx)
        {
            if (iStartIdx < 0)
                iStartIdx = 0;

            if (iStartIdx > CtrlProperty._nItemTotal - CtrlProperty._nItemView)
                iStartIdx = CtrlProperty._nItemTotal - CtrlProperty._nItemView;
            if (iStartIdx < 0)
                iStartIdx = 0;
            ///Fix Origin Time
            CtrlProperty._nTimeOrigin = iStartIdx * CtrlProperty._nItemWidth;
            CtrlProperty._nTimeMax = (iStartIdx + CtrlProperty._nItemView) * CtrlProperty._nItemWidth;

            SetScrollValue(iStartIdx);
            ChangeValueAxis(iStartIdx);

        }

        private void ChangeValueAxis(int iStartIdx)
        {
            if (iStartIdx > m_itemGroup.Count()-1)
                return;

            float fMaxVal = 0;
            float fMinVal = (float)9e8;

            float fItemMax = 0;
            float fItemMin = 0;
            for (int i= iStartIdx; i < iStartIdx + CtrlProperty._nItemView; i++)
            {
                if (i > m_itemGroup.listRItem.Count - 1)
                    break;

                fItemMax = m_itemGroup.listRItem[i].GetMax();
                if (fMaxVal < fItemMax)
                    fMaxVal = fItemMax;

                fItemMin = m_itemGroup.listRItem[i].GetMin();
                if (fMinVal > fItemMin)
                    fMinVal = fItemMin;

            }
            
            if (fMaxVal > 0 && fMinVal < 9e8)
            {
                CtrlProperty._nValueMargin = ((fMaxVal - fMinVal) / 5);
                CtrlProperty._nValueMargin = CtrlProperty._nValueMargin > 300 ? CtrlProperty._nValueMargin : 300;
                CtrlProperty._nValueUnit = CtrlProperty._nValueMargin ;
                CtrlProperty._fValueOrigin = fMinVal- (CtrlProperty._nValueMargin+100);
                CtrlProperty._fValueMax = fMaxVal + CtrlProperty._nValueMargin;
            }
            SetWorldArea();
        }

        private void SetWorldArea()
        {
            int nTmWidth = (int)(CtrlProperty._nTimeMax - CtrlProperty._nTimeOrigin);
            int nValHeight = (int)(CtrlProperty._fValueMax - CtrlProperty._fValueOrigin);

            mRectWorld = new RectangleF(CtrlProperty._nTimeOrigin, CtrlProperty._fValueOrigin, nTmWidth, nValHeight);
        }

        private bool SetRItemVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {

            mRealEndVal = fVal;

            long lTmStamp = CtrlProperty.GetTimeStamp(dtReceive);

            RItem itemLast = m_itemGroup.Last();
            bool bAdd = false;

            int iValid = 0;
            if (itemLast != null)
            {
                iValid = itemLast.IsValidTime(lTmStamp, nTick);
            } else if(CtrlProperty._RTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                iValid = 0;//2;
            }

            if (iValid == 1)
            {
                itemLast.SetEndValue(fVal, nTick, nConc);
            }
            else if (iValid == 2)
            {
                int nRemainTick = nTick;
                while (nRemainTick > 0)
                {
                    if(itemLast != null) {
                        if (itemLast.Tick < (int)CtrlProperty._RTimeUnitAmt)
                            nRemainTick = itemLast.SetEndValue(fVal, nRemainTick, nConc);
                    }
                    if (nRemainTick > 0)
                    {
                        nRemainTick = CreateRItem(fVal, dtReceive, nRemainTick, nConc);
                        itemLast = m_itemGroup.Last();
                        bAdd = true;
                    }

                }
            }
            else
            {
                CreateRItem(fVal, dtReceive, nTick, nConc);
                bAdd = true;
            }
            return bAdd;
        }


        /// <summary>
        /// Set realtime-changing value 
        /// </summary>
        /// <param name="fVal"></param>
        public void SetRealTimeVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            fVal *= CtrlProperty._nValueRate;

            if (SaveRtVal) { 
                RealVal stReal = new RealVal(fVal, dtReceive, nTick, nConc);
                lock (_objLock)
                {
                    if (mlistRealVal.Count > int.MaxValue - 3000) //2147480000
                    {
                        mlistRealVal.RemoveAt(0);
                    }
                    mlistRealVal.Add(stReal);
                }
            }
            //베팅차트설정
            SetDItemVal(fVal, dtReceive, nTick, nConc);
            SetCItemVal(fVal, dtReceive, nTick, nConc);

            //현시차트설정
            if (SetRItemVal(fVal, dtReceive, nTick, nConc))
            {
                int iStartItem = 0;
                if (CtrlProperty._nItemTotal < m_itemGroup.Count())
                    CtrlProperty._nItemTotal = m_itemGroup.Count();

                if (m_itemGroup.Count() > CtrlProperty._nItemView)
                    iStartItem = CtrlProperty._nTimeOrigin / CtrlProperty._nItemWidth + 1;

                SetTimeAxis(iStartItem);
            } else
            {
                int iStartIdx = CtrlProperty._nTimeOrigin / CtrlProperty._nItemWidth;
                ChangeValueAxis(iStartIdx);

            }
            if (Math.Abs(Environment.TickCount - m_tickCurrent) > 50)
            {
                m_tickCurrent = Environment.TickCount;
                Invalidate();
            }
        }

        private int CreateRItem(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            DateTime dtStart = CtrlProperty.GetStartTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtReceive);
            DateTime dtEnd = CtrlProperty.GetEndTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtStart, true);
            
            RItem itemNew = new RItem(fVal, fVal, fVal, fVal, CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), m_itemGroup.Count(), nTick, nConc);
            m_itemGroup.Add(itemNew);
            
            return nTick - itemNew.Tick ;
        }
        public bool SetOrderInfo(OrderVal orderInfo)
        {
            long lTmStamp = CtrlProperty.GetTimeStamp(orderInfo.OrderTime);

            OrderVal newOrder = new OrderVal
            {
                OrderType = orderInfo.OrderType,
                Symbol = orderInfo.Symbol,
                AveragePrice = orderInfo.AveragePrice,
                OrderTime = orderInfo.OrderTime,
                ResultState = orderInfo.ResultState,
                ConcState = orderInfo.ConcState,
                OrderQty = orderInfo.OrderQty,
                TimeType = orderInfo.TimeType,
                TimeUnit = orderInfo.TimeUnit,
                OrderTm = lTmStamp,
            };

            CtrlProperty._OrderList.Add(newOrder);

            int cnt = m_itemGroup.Count();
            
            if(CtrlProperty._RTimeType == orderInfo.TimeType && CtrlProperty._RTimeUnitAmt == orderInfo.TimeUnit)
            {
                int i = 0, iValid = 0;
                RItem item = null;

                while (i < 10)
                {
                    i++;
                    if (i > cnt)
                        break;

                    item = m_itemGroup.listRItem[cnt - i];
                    iValid = item.IsValidTime(lTmStamp, 0);
                    if (iValid == 1)
                    {
                        break;
                    }
                }
                if(iValid == 1)
                {
                    if(!item.Orders.Contains(CtrlProperty._OrderList.Count - 1))
                        item.Orders.Add(CtrlProperty._OrderList.Count - 1);
                }
            }

            if (CtrlProperty._DTimeType == orderInfo.TimeType && CtrlProperty._DTimeUnitAmt == orderInfo.TimeUnit)
            {
                cnt = CtrlProperty._DItemList.Count;
                int i = 0, iValid = 0;
                DItem item = null;

                while (i < 10)
                {
                    i++;
                    if (i > cnt)
                        break;

                    item = CtrlProperty._DItemList[cnt - i];
                    iValid = item.IsValidTime(lTmStamp, 0);
                    if (iValid == 1)
                    {
                        break;
                    }
                }
                if (iValid == 1)
                {
                    if (!item.Orders.Contains(CtrlProperty._OrderList.Count - 1))
                        item.Orders.Add(CtrlProperty._OrderList.Count - 1);
                }
            }

            return true;
        }
        private void ClearCursorProperty()
        {
            if (mCursorPt.X != 0 || mCursorPt.Y != 0)
            {
                mCursorPt.X = 0;
                mCursorPt.Y = 0;
                mHoverItem = null;
                mTmHoverItem = null;
                mMousePt.X = 0;
                mMousePt.Y = 0;
                Invalidate();
            }
        }

        public int GetConcPerMin(int min)
        {
            int nQty = 0;
            if (mlistRealVal.Count > 0)
            {
                // min이 과대하면 AddMinutes(-min)에서 DateTime 범위 초과 → 최대 1일(1440분)로 제한
                if (min < 0) min = 0;
                if (min > 1440) min = 1440;
                DateTime dtLast;
                try
                {
                    dtLast = mlistRealVal[mlistRealVal.Count - 1]._dtRec.AddMinutes(-min);
                }
                catch
                {
                    return 0; // DateTime 범위 초과 시(비정상 _dtRec 등) 0 반환
                }
                for (int i = mlistRealVal.Count - 1; i >= 0; i--)
                {
                    if (mlistRealVal[i]._dtRec > dtLast)
                        nQty += mlistRealVal[i]._nConc;
                    else break;
                }
            }
            return nQty;
        }

        public void SetZoom(int nZoomVal)
        {

            if (m_itemGroup.Count() < 1)
                return;

            if (nZoomVal > 0)
            {
                float fZoomVal = (float)(nZoomVal + 0.5);

                float fView = CtrlProperty._nItemView / fZoomVal;

                int nItemView = (int)Math.Ceiling(fView);

                if (nItemView > 0)
                {
                    CtrlProperty._nItemView = nItemView;
                    mCandleW = (float)CtrlProperty._ClientW / CtrlProperty._nItemView;
                    int iStartItem = CtrlProperty._nTimeMax / CtrlProperty._nItemWidth - nItemView;
                    SetTimeAxis(iStartItem);
                    Invalidate();
                }

            }
            else if (nZoomVal < 0)
            {
                float fZoomVal = (float)(-nZoomVal + 0.5);

                float fView = CtrlProperty._nItemView * fZoomVal;

                int nItemView = (int)Math.Ceiling(fView);


                if (nItemView > CtrlProperty._nItemTotal)
                    nItemView = CtrlProperty._nItemTotal;

                if (nItemView <= CtrlProperty._nItemTotal)
                {
                    CtrlProperty._nItemView = nItemView;
                    mCandleW = (float)CtrlProperty._ClientW / CtrlProperty._nItemView;
                    int iStartItem = CtrlProperty._nTimeMax / CtrlProperty._nItemWidth - nItemView;
                    if (iStartItem < 0)
                        iStartItem = 0;
                    SetTimeAxis(iStartItem);
                    Invalidate();
                }
                

            }
            
        }
        private bool isInDrawRect(Point ptPos)
        {
            if (ptPos.X > CtrlProperty._ClientW  || ptPos.Y > CtrlProperty._ClientH-CtrlProperty._nTimeAxisBand )
                return true;
            else return false;
        }
        private void RChartCtrl_MouseMove(object sender, MouseEventArgs e)
        {

            bool bRefresh = false;

            if (isInDrawRect(e.Location))
            {
                ClearCursorProperty();
                return;
            }

            if (m_itemGroup.Count() < 1)
                return;

            if (mCursorPt.X != e.X || mCursorPt.Y != e.Y)
            {
                mCursorPt.X = e.X;
                mCursorPt.Y = e.Y;
                bRefresh = true;
            }
                

            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;
                
                if (Math.Abs(e.X - mMousePt.X) > 30)
                {
                    int nTmWidth = (int)(CtrlProperty._nTimeMax - CtrlProperty._nTimeOrigin);

                    float fTmDiff = (mMousePt.X - e.X ) * nTmWidth / CtrlProperty._ClientW;

                    
                    int nDiffItem = (int)Math.Round( fTmDiff / CtrlProperty._nItemWidth);

                    if (Math.Abs(nDiffItem) < 1)
                        nDiffItem = mMousePt.X > e.X ? 1 : -1;
                    int iStartItem = CtrlProperty._nTimeOrigin / CtrlProperty._nItemWidth + nDiffItem;

                    SetTimeAxis(iStartItem);
                    mMousePt.X = e.X;

                    bRefresh = true;

                }
                
            } else
            {
                this.Cursor = Cursors.Cross;

                PointF ptWorld = CtrlProperty.GetWorldPt(mCursorPt);

                RItem itemTmHover = m_itemGroup.GetValidTm(ptWorld.X);
                
                if(mTmHoverItem != itemTmHover) {
                    mTmHoverItem = itemTmHover;
                }

                RItem itemSel = m_itemGroup.GetValidItem(ptWorld.X, ptWorld.Y);
                if (mHoverItem != itemSel)
                {
                    
                    mHoverItem = itemSel;
                    mMousePt = mCursorPt;
                    bRefresh = true;
                }   

            }

            if (bRefresh)
            {
                Invalidate();
            }

        }

        private void RChartCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;
                mMousePt.X = e.X;
                mMousePt.Y = e.Y;
            }

        }

        private void RChartCtrl_MouseWheel(object sender, MouseEventArgs e)
        {
            
            int nZoomVal = e.Delta / 120;
            SetZoom(nZoomVal);

        }
        private void RChartCtrl_MouseLeave(object sender, System.EventArgs e)
        {

            if (mCursorPt.X != 0 || mCursorPt.Y != 0)
            {
                ClearCursorProperty();
            }
        }

        private void RChartCtrl_SizeChanged(object sender, System.EventArgs e)
        {

            if (m_itemGroup.Count() < 1)
                return;

            SetClientRect();

            int prvClientW = CtrlProperty._ClientW;

            if (prvClientW == 0 && mCandleW == 0)
            {
                if (CtrlProperty._ClientW > 0)
                    mCandleW = (float)CtrlProperty._ClientW / CtrlProperty._nItemView;
            }

            if (mCandleW == 0)
                return;

            int prvItemView = CtrlProperty._nItemView;

            if (prvClientW > 0 && prvClientW != CtrlProperty._ClientW)
            {
                CtrlProperty._nItemView = (int)Math.Round(((float)CtrlProperty._ClientW / mCandleW));
            }
            if (CtrlProperty._nItemView < 1)
                CtrlProperty._nItemView = 1;
            if (prvItemView != CtrlProperty._nItemView)
            {
                int iStartItem = 0;
                if (m_itemGroup.Count() > CtrlProperty._nItemView)
                    iStartItem = CtrlProperty._nTimeOrigin / CtrlProperty._nItemWidth + 1;

                SetTimeAxis(iStartItem);
            }

            Invalidate();
        }

        public void SetChartFrom(DateTime dtFrom, double price = 0)
        {
            lock (_objLock)
            {
                if(price == 0)
                    mlistRealVal.RemoveAll(v => v._dtRec < dtFrom);
                else
                    mlistRealVal.RemoveAll(v => v._dtRec < dtFrom || (v._dtRec == dtFrom && v._fVal == price) );

            }
            InitChart();
            int[] arrAvgCnt = { 5, 10, 20, 60, 120 };

            SetDChartType(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, arrAvgCnt, Settings.Default.BoLineAdjustD, true);
        }

        public bool SetDChartType(TIMETYPE timeType, TIMEUNIT timeUnit, int[] arrAvgCnt, int boLineAdjust, bool bFresh = false) {
            bool bResult = false;
            if (CtrlProperty._DTimeType != timeType || CtrlProperty._DTimeUnitAmt != timeUnit || bFresh)
            {
                CtrlProperty._DTimeType = timeType;
                CtrlProperty._DTimeUnitAmt = timeUnit;
                // CtrlProperty.SetAvgCnts(arrAvgCnt);
                Settings.Default.BoLineAdjustD = boLineAdjust;
                CtrlProperty._DItemList.Clear();
                
                if (SaveRtVal)
                {
                    for (int i = 0; i < mlistRealVal.Count; i++)
                    {
                        SetDItemVal(mlistRealVal[i]._fVal, mlistRealVal[i]._dtRec, mlistRealVal[i]._nTick, mlistRealVal[i]._nConc);
                    }
                }
                bResult = true;
            }

            if(!bResult && Settings.Default.BoLineAdjustD != boLineAdjust)
            {
                int nCount = CtrlProperty._DItemList.Count;
                Settings.Default.BoLineAdjustD = boLineAdjust;
                for (int i = 0; i < nCount; i++)
                {
                    CtrlProperty._DItemList[i].SetBoLineVal();
                }
            }
            return true;
        }

        public bool SetDChart2Type(TIMETYPE timeType, TIMEUNIT timeUnit)
        {
            if (CtrlProperty._CTimeType != timeType || CtrlProperty._CTimeUnitAmt != timeUnit)
            {
                // Trace.TraceInformation(string.Format("<RChartCtrl> _CTimeType={0}, _CTimeUnitAmt={1}", timeType, timeUnit));
                CtrlProperty._CTimeType = timeType;
                CtrlProperty._CTimeUnitAmt = timeUnit;
                CtrlProperty._CItemList.Clear();

                if (SaveRtVal)
                {
                    for (int i = 0; i < mlistRealVal.Count; i++)
                    {
                        SetCItemVal(mlistRealVal[i]._fVal, mlistRealVal[i]._dtRec, mlistRealVal[i]._nTick, mlistRealVal[i]._nConc);
                    }
                }
            }

            return true;
        }

        public List<DItem> GetDChartList(int nCount, bool bNeedLast)
        {
            
            bool bRemove = true;

            if (CtrlProperty._DItemList.Count > 0) {
                DItem itemLast = CtrlProperty._DItemList.Last();
                if (bNeedLast || itemLast.IsCompleted())
                    bRemove = false;
            }

            int iStart = CtrlProperty._DItemList.Count - nCount;
            if (bRemove)
            {
                iStart --;
            }
            
            List<DItem> listDItem = null; ;
            if(nCount <= 0 || iStart <= 0)
            {
                listDItem = new List<DItem>(CtrlProperty._DItemList);
            } else {

                listDItem = new List<DItem>(CtrlProperty._DItemList.GetRange(iStart, nCount));                
            }
            return listDItem;
        }

        public List<CItem> GetCChartList(int nCount, bool bNeedLast)
        {

            bool bRemove = true;

            if (CtrlProperty._CItemList.Count > 0)
            {
                CItem itemLast = CtrlProperty._CItemList.Last();
                if (bNeedLast || itemLast.IsCompleted())
                    bRemove = false;
            }

            int iStart = CtrlProperty._CItemList.Count - nCount;
            if (bRemove)
            {
                iStart--;
            }

            List<CItem> listDItem = null; ;
            if (nCount <= 0 || iStart <= 0)
            {
                listDItem = new List<CItem>(CtrlProperty._CItemList);
            }
            else
            {
                listDItem = new List<CItem>(CtrlProperty._CItemList.GetRange(iStart, nCount));
            }
            return listDItem;
        }
        private void SetDItemVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {            
            long lTmStamp = CtrlProperty.GetTimeStamp(dtReceive);

            DItem itemLast = null;
            if (CtrlProperty._DItemList.Count > 0)
                itemLast = CtrlProperty._DItemList.Last();

            int iValid = 0;
            if (itemLast != null)
            {
                iValid = itemLast.IsValidTime(lTmStamp, nTick);
            }
            else if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                iValid = 0;
            }

            if (iValid == 1)
            {
                itemLast.SetEndValue(fVal, nTick, nConc);
            }
            else if (iValid == 2)
            {
                int nRemainTick = nTick;
                while (nRemainTick > 0)
                {
                    if (itemLast != null)
                    {
                        if (itemLast.Tick < (int)CtrlProperty._DTimeUnitAmt)
                            nRemainTick = itemLast.SetEndValue(fVal, nRemainTick, nConc);
                    }
                    if (nRemainTick > 0)
                    {
                        nRemainTick = CreateDItem(fVal, dtReceive, nRemainTick, nConc);
                        itemLast = CtrlProperty._DItemList.Last();
                    }
                }
            }
            else
            {
                CreateDItem(fVal, dtReceive, nTick, nConc);
            }

        }

        private int CreateDItem(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            DateTime dtStart = CtrlProperty.GetStartTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtReceive);
            DateTime dtEnd = CtrlProperty.GetEndTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtStart, true);

            DItem dItem = new DItem(fVal, fVal, fVal, fVal, CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._DItemList.Count, nTick, nConc);
            CtrlProperty._DItemList.Add(dItem);

            return nTick - dItem.Tick;
        }

        private void SetCItemVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            long lTmStamp = CtrlProperty.GetTimeStamp(dtReceive);

            CItem itemLast = null;
            if (CtrlProperty._CItemList.Count > 0)
                itemLast = CtrlProperty._CItemList.Last();

            int iValid = 0;
            if (itemLast != null)
            {
                iValid = itemLast.IsValidTime(lTmStamp, nTick);
            }
            else if (CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                iValid = 0;
            }

            if (iValid == 1)
            {
                itemLast.SetEndValue(fVal, nTick, nConc);
            }
            else if (iValid == 2)
            {
                int nRemainTick = nTick;
                while (nRemainTick > 0)
                {
                    if (itemLast != null)
                    {
                        if (itemLast.Tick < (int)CtrlProperty._CTimeUnitAmt)
                            nRemainTick = itemLast.SetEndValue(fVal, nRemainTick, nConc);
                    }
                    if (nRemainTick > 0)
                    {
                        nRemainTick = CreateCItem(fVal, dtReceive, nRemainTick, nConc);
                        itemLast = CtrlProperty._CItemList.Last();
                    }
                }
            }
            else
            {
                CreateCItem(fVal, dtReceive, nTick, nConc);
            }

        }

        private int CreateCItem(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            DateTime dtStart = CtrlProperty.GetStartTime(CtrlProperty._CTimeType, CtrlProperty._CTimeUnitAmt, dtReceive);
            DateTime dtEnd = CtrlProperty.GetEndTime(CtrlProperty._CTimeType, CtrlProperty._CTimeUnitAmt, dtStart, true);

            CItem cItem = new CItem(CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), nTick, nConc);
            if (CtrlProperty._CItemList.Count > 100)
                CtrlProperty._CItemList.RemoveAt(0);
            CtrlProperty._CItemList.Add(cItem);

            return nTick - cItem.Tick;
        }


    }
}
