using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using ChartCtrl.Properties;

namespace ChartCtrl
{
    public class RItem
    {
        public RItem(float fStart, float fEnd, float fMin, float fMax, long tmStart, long tmEnd, int itemIndex, int nTick, int nConc)
        {
            m_fStartVal = fStart;
            m_fEndVal = fEnd;
            m_fMinVal = fMin;
            m_fMaxVal = fMax;
            m_lStartTm = tmStart;
            m_lEndTm = tmEnd;
            m_iIndex = itemIndex;
            Orders = new List<int>();
            if (CtrlProperty._RTimeType == TIMETYPE.TIMETYPE_TICK)
                m_iTick = nTick <= (int)CtrlProperty._RTimeUnitAmt? nTick: (int)CtrlProperty._RTimeUnitAmt;

            SetAverageVal();
            SetBoLineVal(false);
            SetAdx();
            SetCci(true);
            SetRsi();
            m_estType2 = m_estType;
            m_nConc = nConc;
        }

        /// Declare Member Variables
        private float m_fStartVal;      //시가
        private float m_fEndVal;        //종가
        private float m_fMaxVal;        //고가
        private float m_fMinVal;        //저가
        private long m_lStartTm;        //시작시간
        private long m_lEndTm;          //마감시간
        private int  m_iIndex;          //인뎃스
        private int  m_iTick;           //틱수
        private int m_nConc;            //거래량

        private float m_fPdm;           //PDM
        private float m_fMdm;           //MDM
        private float m_fTr;            //TR
        private float m_fDx;           //DX
        private float m_fAdx;           //ADX

        private float m_fSMn;          //M1+M2+..+Mn-1;
        private float m_fDn;           //|M-SM|(1)...(N-1)/N;
        private float m_fCci;            //CCI

        private float m_fAum;           //AU
        private float m_fAdm;           //AD
        private float m_fRsi;           //RSI

        private float[] m_arrAvg = new float[CtrlProperty._arrAvgItems.Length];         //이동평균값1~5
        private float[] m_arrBo = new float[2];         //하늘-주황색
        private RESULTSTATE m_estType = RESULTSTATE.IGNORE; //추세타입
        private RESULTSTATE m_estType2 = RESULTSTATE.IGNORE;
        private bool m_estCrossed;        //하늘-주황 교차
        private float m_estCross;         //교차시가격

        Pen m_PenRed = Pens.Red;
        Pen m_PenBlue = Pens.Blue;
        Brush m_BrushRed = Brushes.Red;
        Brush m_BrushBlue = Brushes.Blue;

        /// Declare Name properties
        public float Start
        {
            get { return m_fStartVal; }
        }
        public float End
        {
            get { return m_fEndVal; }
        }
        public float Max
        {
            get { return m_fMaxVal; }
        }
        public float Min
        {
            get { return m_fMinVal; }
        }
        public int Index
        {
            get { return m_iIndex; }
        }
        public long TmStart
        {
            get { return m_lStartTm; }
        }
        public long TmEnd
        {
            get { return m_lEndTm; }
        }
        public int Tick
        {
            get { return m_iTick; }
        }
        public float[] Avgs
        {
            get { return m_arrAvg; }
        }
        
        public float[] Bos
        {
            get { return m_arrBo; }
        }
        public RESULTSTATE Est_Type
        {
            get { return m_estType; }
        }
        public bool Est_Crossed
        {
            get { return m_estCrossed; }
        }
        public float Est_Cross
        {
            get { return m_estCross/ CtrlProperty._nValueRate; }
        }
        private float CanMax
        {
            get { return m_fStartVal >= m_fEndVal? m_fStartVal : m_fEndVal; }
        }
        private float CanMin
        {
            get { return m_fStartVal <= m_fEndVal ? m_fStartVal : m_fEndVal; }
        }
        private float CanHeight
        {
            get { return m_fMaxVal - m_fMinVal;/*m_fStartVal >= m_fEndVal ? m_fStartVal - m_fEndVal : m_fEndVal - m_fStartVal ;*/ }
        }
        public List<int> Orders
        {
            get; set;
        }
        public float Adx
        {
            get {
                if (m_iIndex < CtrlProperty._nAdxCnt - 1)
                    return 0;
                return m_fAdx; 
            } 
        }
        public float Cci
        {
            get
            {
                if (m_iIndex < CtrlProperty._nCciCnt - 1)
                    return 0;
                return m_fCci;
            }
        }
        public float Rsi
        {
            get
            {
                if (m_iIndex < CtrlProperty._nRsiCnt )
                    return 0;
                return m_fRsi;
            }
        }
        public int Conc
        {
            get { return m_nConc; }
        }
        public float Mean
        {
            get { return (m_fMaxVal + m_fMinVal + m_fEndVal) / 3; } //M(Mean price) = (H+L+C)/3
        }
        public float Au
        {
            get {
                if (m_iIndex < 1)
                    return 0;
                else if (m_fEndVal > CtrlProperty._RItemList[m_iIndex-1].m_fEndVal)
                    return m_fEndVal - CtrlProperty._RItemList[m_iIndex-1].m_fEndVal;
                else return 0;
            }
        }
        public float Ad
        {
            get
            {
                if (m_iIndex < 1)
                    return 0;
                else if (CtrlProperty._RItemList[m_iIndex-1].m_fEndVal > m_fEndVal)
                    return CtrlProperty._RItemList[m_iIndex-1].m_fEndVal - m_fEndVal;
                else return 0;
            }
        }
        public float GetMax()
        {
            float fMax = m_fMaxVal;
            if (Settings.Default.AvgLine)
            {
                float fAvgMax = m_arrAvg.Max();
                fMax = fAvgMax > fMax ? fAvgMax : fMax;
            }
            if (Settings.Default.BoLine)
            {
                float fBoMax = m_arrBo.Max();
                fMax = fBoMax > fMax ? fBoMax : fMax;
            }

            return fMax;
        }

        public float GetMin()
        {
            float fMin = m_fMinVal;
            if (Settings.Default.AvgLine)
            {
                float fAvgMin = 0;
                if (m_arrAvg.Max() > 0)
                    fAvgMin = (from fAvg in m_arrAvg where fAvg > 0 select fAvg).Min();
                fMin = fAvgMin > 0 && fAvgMin < fMin ? fAvgMin : fMin;

            }
            if (Settings.Default.BoLine)
            {
                float fBoMin = m_arrBo.Min();
                fMin = fBoMin > 0 && fBoMin < fMin ? fBoMin : fMin;
            }

            return fMin;
        }

        /// Declare Member Functions
        public void Draw(Graphics g)
        {
            Pen pen;
            Brush brush;
            if (m_fStartVal > m_fEndVal)
            {
                pen = m_PenBlue;
                brush = m_BrushBlue;
            }
            else
            {
                pen = m_PenRed;
                brush = m_BrushRed;
            }

            int itemX = m_iIndex * CtrlProperty._nItemWidth;

            try
            {
                if (itemX >= CtrlProperty._nTimeOrigin && itemX < CtrlProperty._nTimeMax)
                {
                    int nLineX = itemX + CtrlProperty._nItemWidth / 2;

                    int nRectW = CtrlProperty._nItemWidth - 20;

                    int nRectX = itemX + (CtrlProperty._nItemWidth - nRectW) / 2;
                    float fRectY = m_fEndVal > m_fStartVal ? m_fStartVal : m_fEndVal;
                    float fRectHeight = Math.Abs(m_fEndVal - m_fStartVal);

                    g.DrawLine(pen, nLineX, Min, nLineX, fRectY);
                    g.DrawLine(pen, nLineX, fRectY + fRectHeight, nLineX, Max);
                    if (fRectHeight <= 2)
                        fRectY -= 1;
                    //g.DrawRectangle(pen, nRectX, fRectY, nRectW, fRectHeight > 2 ? fRectHeight : 2);

                    g.FillRectangle(brush, new RectangleF(nRectX, fRectY, nRectW, fRectHeight > 10 ? fRectHeight : 10));


                    if (this.Orders.Count > 0)
                    {
                        int wTri = CtrlProperty._nItemWidth / 2 - 5;
                        if (wTri < 10) wTri = 10;
                        int hTri = (int)(20 * (CtrlProperty._fValueMax - CtrlProperty._fValueOrigin) / CtrlProperty._ClientH);
                        int y = (int)this.Max;
                        int hGap = hTri / 4;

                        for (int i = 0; i < Orders.Count; i++)
                        {
                            OrderVal orderInfo = CtrlProperty._OrderList[Orders[i]];
                            if (orderInfo == null)
                                continue;

                            if (orderInfo.ConcState == CONCSTATE.LIQUID)
                            {
                                pen = orderInfo.ResultState == RESULTSTATE.BUY ? m_PenRed : m_PenBlue;

                                if (orderInfo.ResultState == RESULTSTATE.BUY)
                                {
                                    y = (int)this.Min;
                                    Point[] points1 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points1);

                                    y -= (hTri - hGap);
                                    Point[] points2 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points2);
                                }
                                else
                                {
                                    y = (int)this.Max;
                                    Point[] points1 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points1);

                                    y += (hTri - hGap);
                                    Point[] points2 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points2);
                                }

                            }
                            else if (orderInfo.ConcState == CONCSTATE.CONCLUDE)
                            {
                                pen = orderInfo.ResultState == RESULTSTATE.BUY ? m_PenRed : m_PenBlue;

                                if (orderInfo.ResultState == RESULTSTATE.BUY)
                                {
                                    y = (int)this.Min;
                                    Point[] points1 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points1);

                                }
                                else
                                {
                                    y = (int)this.Max;
                                    Point[] points1 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points1);

                                }
                            }
                            else if (orderInfo.ConcState == CONCSTATE.RECONC)
                            {
                                if (orderInfo.ResultState == RESULTSTATE.BUY)
                                {
                                    pen = m_PenBlue;
                                    y = (int)this.Max;
                                    Point[] points1 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points1);

                                    y += (hTri - hGap);
                                    Point[] points2 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points2);

                                    pen = m_PenRed;
                                    y = (int)this.Min;
                                    Point[] points3 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points3);

                                }
                                else
                                {
                                    pen = m_PenRed;
                                    y = (int)this.Min;
                                    Point[] points1 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points1);

                                    y -= (hTri - hGap);
                                    Point[] points2 = { new Point(nLineX - wTri, y - hTri), new Point(nLineX + wTri, y - hTri), new Point(nLineX, y - hGap) };
                                    g.DrawPolygon(pen, points2);

                                    pen = m_PenBlue;
                                    y = (int)this.Max;
                                    Point[] points3 = { new Point(nLineX - wTri, y + hTri), new Point(nLineX + wTri, y + hTri), new Point(nLineX, y + hGap) };
                                    g.DrawPolygon(pen, points3);
                                }

                            }
                            //Trace.WriteLine(">>>c# Log>>> wTri=" + wTri.ToString() + " _fValueOrigin=" + CtrlProperty._fValueOrigin.ToString() + " hTri" + hTri.ToString() + " _fValueMax=" + CtrlProperty._fValueMax.ToString());
                            //g.DrawLine(pen, nRectX, OrderVal, nRectX+nRectW, OrderVal);
                        }

                    }

                }
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Function that Set End Value
        /// </summary>
        /// <param name="fVal"></param>
        public int SetEndValue(float fVal, int nTick, int nConc)
        {
            if (CtrlProperty._RTimeType == TIMETYPE.TIMETYPE_TICK && m_iTick == (int)CtrlProperty._RTimeUnitAmt)
                return nTick;

            m_fEndVal = fVal;
            if (fVal < m_fMinVal)
                m_fMinVal = fVal;
            if (fVal > m_fMaxVal)
                m_fMaxVal = fVal;
            m_nConc += nConc;
            SetAverageVal();
            SetBoLineVal(true);
            SetAdx();
            SetCci();
            SetRsi();
            if (CtrlProperty._RTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (m_iTick + nTick <= (int)CtrlProperty._RTimeUnitAmt)
                {
                    m_iTick = m_iTick + nTick;
                    return 0;
                } else
                {
                    nTick = m_iTick + nTick - (int)CtrlProperty._RTimeUnitAmt;
                    m_iTick = (int)CtrlProperty._RTimeUnitAmt;
                    return nTick;
                }
            }
            
            return 0;
        }
        /// <summary>
        /// Return true if Time is valid.
        /// </summary>
        /// <param name="ltmVal"></param>
        /// <returns></returns>
        public Boolean IsValidTm(float fTmVal)
        {
            
            if (fTmVal >= m_iIndex*CtrlProperty._nItemWidth && fTmVal < (m_iIndex+1) * CtrlProperty._nItemWidth)
                return true;
            
            return false;
        }
        /// <summary>
        /// Return true if time and value is valid.
        /// </summary>
        /// <param name="ltmVal"></param>
        /// <param name="fVal"></param>
        /// <returns></returns>
        public Boolean IsValid(float fTmVal, float fVal)
        {

            if (fTmVal < m_iIndex * CtrlProperty._nItemWidth || fTmVal > (m_iIndex+1) * CtrlProperty._nItemWidth)
                return false;
            else if (fVal < m_fMinVal-10 || fVal > m_fMaxVal+10)
                return false;
            
            //Trace.WriteLine(">>>c# Log>>> isValid() ltmVal: " + ftmVal.ToString() + " lTmOrigin:" + m_lOriginTm.ToString());
            //Trace.WriteLine(">>>c# Log>>> isValid() m_fStartTm: " + m_fStartTm.ToString() + " m_fEndTm:" + m_fEndTm.ToString());
            return true;
        }
        ///
        public int IsValidTime(long lTmVal, int nTick)
        {

            if(CtrlProperty._RTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (lTmVal >= m_lStartTm && m_iTick + nTick <= (int)CtrlProperty._RTimeUnitAmt)
                    return 1;
                else return 0;
            }
            else if (lTmVal >= m_lStartTm && lTmVal < m_lEndTm)
                return 1;

            return 0;
        }
        /// <summary>
        /// Set Move-Average Value
        /// </summary>
        public void SetAverageVal()
        {
            if (CtrlProperty._RItemList == null)
                return;

            List<RItem> listSubItems = null;
            float fSum = 0 ;
            int nAvgCnt = CtrlProperty._arrAvgItems.Length;
            for(int i=0; i<nAvgCnt; i++)
            {
                if (CtrlProperty._arrAvgItems[i] > 1 && m_iIndex >= CtrlProperty._arrAvgItems[i]-1 && CtrlProperty._RItemList.Count > m_iIndex-1)
                {
                    
                    listSubItems = CtrlProperty._RItemList.GetRange(m_iIndex - CtrlProperty._arrAvgItems[i] + 1, CtrlProperty._arrAvgItems[i] - 1);
                    if(listSubItems.Count > 0)
                    {
                        fSum = listSubItems.Sum(it => it.m_fEndVal);
                        fSum += m_fEndVal;
                        m_arrAvg[i] = fSum / CtrlProperty._arrAvgItems[i];
                    }
                }
            }
             
        }

        public int GetAvgValueCount()
        {
            int nResult = 0;
            int nAvgCnt = CtrlProperty._arrAvgItems.Length;
            for (int i = 0; i < nAvgCnt; i++)
            {
                if(m_arrAvg[i] > 0)
                {
                    nResult++;
                }
            }
            return nResult;
        }

        public void SetBoLineVal(bool bCompare)
        {
            if (CtrlProperty._RItemList == null)
                return;

            RItem prevItem = null;

            if (m_iIndex >= 1 && CtrlProperty._RItemList.Count > m_iIndex - 1)
            {
                prevItem = CtrlProperty._RItemList[m_iIndex - 1];

                if (m_fStartVal > m_fEndVal)     //매도
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL)
                    {
                        m_arrBo[0] = prevItem.m_arrBo[0];
                        m_arrBo[1] = m_fStartVal < prevItem.m_arrBo[1] ? m_fStartVal : prevItem.m_arrBo[1] - 1;
                        m_estType = RESULTSTATE.SELL;
                        m_estCrossed = false;
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)
                    {
                        // if (  m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustR * CtrlProperty._fOverTick * CtrlProperty._nValueRate) //Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100
                        if (m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100)
                        {
                            m_arrBo[0] = m_fEndVal > prevItem.m_arrBo[0] ? m_fEndVal : prevItem.m_arrBo[0] + 1;
                            m_arrBo[1] = prevItem.m_arrBo[1];
                            m_estType = RESULTSTATE.BUY;
                            m_estCrossed = false;
                        }
                        else
                        {
                            if (!m_estCrossed)
                            {
                                m_estCrossed = true;
                                m_estCross = m_fEndVal;
                            }

                            m_arrBo[0] = m_fStartVal;/*m_estCrossed ? m_estCross : m_fStartVal*/
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.SELL;

                        }
                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.SELL;
                        m_estCrossed = false;

                    }
                }
                else if (m_fStartVal < m_fEndVal)  //매수
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL) //매도
                    {
                        //if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustR * CtrlProperty._fOverTick * CtrlProperty._nValueRate) // Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100
                        if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100)
                        {
                            m_arrBo[0] = prevItem.m_arrBo[0];
                            m_arrBo[1] = m_fEndVal < prevItem.m_arrBo[1] ? m_fEndVal : prevItem.m_arrBo[1] - 1;
                            m_estType = RESULTSTATE.SELL;
                            m_estCrossed = false;

                        }
                        else
                        {
                            if (!m_estCrossed)
                            {
                                m_estCrossed = true;
                                m_estCross = m_fEndVal;
                            }
                            m_arrBo[0] = m_fStartVal;/*m_estCrossed ? m_estCross : m_fStartVal*/
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.BUY;

                        }
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)
                    {
                        m_arrBo[0] = m_fStartVal > prevItem.m_arrBo[0] ? m_fStartVal : prevItem.m_arrBo[0] + 1;
                        m_arrBo[1] = prevItem.m_arrBo[1]; ;
                        m_estType = RESULTSTATE.BUY;
                        m_estCrossed = false;

                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.BUY;
                        m_estCrossed = false;

                    }
                }
                else
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL)     //매도
                    {
                        //if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustR * CtrlProperty._fOverTick * CtrlProperty._nValueRate) //Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100
                        if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100)
                        {
                            m_arrBo[0] = prevItem.m_arrBo[0];
                            m_arrBo[1] = m_fEndVal < prevItem.m_arrBo[1] ? m_fEndVal : prevItem.m_arrBo[1] - 1;
                            m_estType = RESULTSTATE.SELL;
                            m_estCrossed = false;
                        }
                        else
                        {
                            if (!m_estCrossed)
                            {
                                m_estCrossed = true;
                                m_estCross = m_fEndVal;
                            }
                            m_arrBo[0] = m_fStartVal;/*m_estCrossed ? m_estCross : m_fStartVal*/
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.BUY;
                        }
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)     //매수
                    {
                        //if (m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustR * CtrlProperty._fOverTick * CtrlProperty._nValueRate) //Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100
                        if (m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustR * prevItem.CanHeight / 100)
                        {
                            m_arrBo[0] = m_fEndVal > prevItem.m_arrBo[0] ? m_fEndVal : prevItem.m_arrBo[0] + 1;
                            m_arrBo[1] = prevItem.m_arrBo[1]; ;
                            m_estType = RESULTSTATE.BUY;
                            m_estCrossed = false;
                        }
                        else
                        {
                            if (!m_estCrossed)
                            {
                                m_estCrossed = true;
                                m_estCross = m_fEndVal;
                            }
                            m_arrBo[0] = m_fStartVal;/*m_estCrossed ? m_estCross : m_fStartVal*/
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.SELL;
                        }
                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.IGNORE;
                        m_estCrossed = false;
                    }
                }
            }
            else
            {
                if(m_fStartVal > m_fEndVal)
                    m_estType = RESULTSTATE.SELL;
                else if (m_fStartVal < m_fEndVal)
                    m_estType = RESULTSTATE.BUY;
                else m_estType = RESULTSTATE.IGNORE;

                m_arrBo[0] = m_fStartVal;
                m_arrBo[1] = m_arrBo[0];                
            }

            //             if (m_iIndex == 0 || m_estCrossed)
            //                 return;

            //             if (m_estType2 != m_estType && bCompare)
            //             {
            //                 m_estCrossed = true;
            //                 m_estType2 = m_estType;
            //                 m_estCross = m_fEndVal;
            //             }

        }
        public void SetAdx()
        {
            if (CtrlProperty._RItemList == null)
                return;

            RItem prevItem = null;

            if (m_iIndex >= 1 && CtrlProperty._RItemList.Count > m_iIndex-1)
            {
                prevItem = CtrlProperty._RItemList[m_iIndex - 1];
                float fPdm = 0, fMdm = 0;
                if(m_fMaxVal - prevItem.m_fMaxVal > prevItem.m_fMinVal - m_fMinVal )
                {
                    fPdm = Math.Abs(m_fMaxVal - prevItem.m_fMaxVal);
                    fMdm = 0;
                } else
                {
                    fPdm = 0;
                    fMdm = Math.Abs(m_fMinVal - prevItem.m_fMinVal);
                }
//                 fPdm = Math.Abs(m_fMaxVal - prevItem.m_fMaxVal);
//                 fMdm = Math.Abs(m_fMinVal - prevItem.m_fMinVal);

                float[] arrVal = { Math.Abs(m_fMaxVal-m_fMinVal), Math.Abs(m_fMaxVal - prevItem.m_fEndVal), Math.Abs(m_fMinVal - prevItem.m_fEndVal) };
                float fTr = arrVal.Max();

                if (CtrlProperty._nAdxCnt < 2)
                    CtrlProperty._nAdxCnt = 2;
                float k = 2/ ((float)CtrlProperty._nAdxCnt + 1);
                // k = 1 - k;
                float fPdi = 0, fMdi = 0;
                if(m_iIndex == 1)
                {
                    m_fPdm = fPdm;
                    m_fMdm = fMdm;
                    m_fTr = fTr;
                    if(m_fTr != 0)
                    {
                        fPdi = m_fPdm / m_fTr;
                        fMdi = m_fMdm / m_fTr;
                        if (fPdi != 0 && fMdi != 0)
                            m_fDx = Math.Abs(fPdi - fMdi) * 100 / (fPdi + fMdi);
                    }
                    m_fAdx = m_fDx;
                } else if (m_iIndex >= 2)
                {
                    m_fPdm = fPdm* k + prevItem.m_fPdm * (1 - k);
                    m_fMdm = fMdm* k + prevItem.m_fMdm * (1 - k);
                    m_fTr = fTr * k + prevItem.m_fTr * (1 - k);

                    if (m_fTr != 0)
                    {
                        fPdi = m_fPdm / m_fTr;
                        fMdi = m_fMdm/ m_fTr;
                        if (fPdi != 0 && fMdi != 0)
                            m_fDx = Math.Abs(fPdi - fMdi) * 100 / (fPdi + fMdi);
                    }
                    m_fAdx = m_fDx * k + prevItem.m_fAdx*(1-k);
                }
//                 Trace.TraceInformation("<RItem> SetAdx() PDM:{0}, MDM:{1}, TR:{2}, PDI:{3}, MDI:{4}, DX:{5}, K:{6}, ADX:{7}, ", 
//                     m_fPdm, m_fMdm, m_fTr, fPdi, fMdi, m_fDx, k, m_fAdx);
            }

        }

        public void SetCci(bool bSum = false)
        {
            if (CtrlProperty._RItemList == null)
                return;


            if (m_iIndex >= 1 && CtrlProperty._RItemList.Count > CtrlProperty._nCciCnt - 1)
            {
                RItem prevItem = null;

                float fMean = 0;
                float fD = 0;
                float fSM = 0;

                if (bSum)
                {
                    m_fSMn = 0;
                    m_fDn = 0;
                    for(int i = CtrlProperty._nCciCnt - 1; i >= 1 ; i--)
                    {
                        prevItem = CtrlProperty._RItemList[m_iIndex - i];
                        fMean = prevItem.Mean;
                        m_fSMn += fMean ;
                        m_fDn += Math.Abs(fMean - m_fSMn/(CtrlProperty._nCciCnt - i));
                    }
                }
                fMean = this.Mean;
                fSM = (m_fSMn + fMean) / CtrlProperty._nCciCnt;
                fD = (m_fDn + Math.Abs(fMean - fSM)) / CtrlProperty._nCciCnt;
                m_fCci = (fMean - fSM) / (CtrlProperty._fCciConst * fD);

//                  Trace.TraceInformation("<RItem> SetCci() m_fSMn:{0:N2}, m_fDn:{1}, fMean:{2}, fSM:{3}, fD:{3}, m_fCci:{5} ",
//                      m_fSMn, m_fDn, fMean, fSM, fD, m_fCci);

            }

        }

        public void SetRsi()
        {
            if (CtrlProperty._RItemList == null)
                return;

            RItem prevItem = null;


            if (m_iIndex == CtrlProperty._nRsiCnt)
            {
                float fAuSum = 0;
                float fAdSum = 0;
                for (int i = 1; i < m_iIndex; i++)
                {
                    fAuSum += CtrlProperty._RItemList[i].Au;
                    fAdSum += CtrlProperty._RItemList[i].Ad;
                }
                fAuSum += Au;
                fAdSum += Ad;
                m_fAum = fAuSum / CtrlProperty._nRsiCnt;
                m_fAdm = fAdSum / CtrlProperty._nRsiCnt;
                m_fRsi = m_fAum * 100 / (m_fAum + m_fAdm);

//                 Trace.TraceInformation("<RItem> SetRsi() AUM:{0}, ADM:{1}, RSI:{2}",
//                     m_fAum, m_fAdm, m_fRsi);
            }
            else if (m_iIndex > CtrlProperty._nRsiCnt && CtrlProperty._RItemList.Count > m_iIndex - 1)
            {
                prevItem = CtrlProperty._RItemList[m_iIndex - 1];

                if (CtrlProperty._nRsiCnt < 2)
                    CtrlProperty._nRsiCnt = 2;
                float k = 2 / ((float)CtrlProperty._nRsiCnt + 1);
                if (m_iIndex >= 2)
                {
                    m_fAum = Au * k + prevItem.m_fAum * (1 - k);
                    m_fAdm = Ad * k + prevItem.m_fAdm * (1 - k);

                    m_fRsi = m_fAum * 100 / (m_fAum + m_fAdm);
                    // m_fAdx = m_fDx * k + prevItem.m_fAdx * (1 - k);
                }
//                 Trace.TraceInformation("<RItem> SetRsi() AUM:{0}, ADM:{1}, RSI:{2}", 
//                     m_fAum, m_fAdm, m_fRsi);
            }

        }

    }
}
