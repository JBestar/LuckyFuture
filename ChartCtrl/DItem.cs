using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ChartCtrl.Properties;

namespace ChartCtrl
{

    public class DItem
    {
        public DItem(float fStart, float fEnd, float fMin, float fMax, long tmStart, long tmEnd, int iIndex, int nTick, int nConc)
        {
            m_fStartVal = fStart;
            m_fEndVal = fEnd;
            m_fMinVal = fMin;
            m_fMaxVal = fMax;
            m_lStartTm = tmStart;
            m_lEndTm = tmEnd;
            m_iIndex = iIndex;
            Orders = new List<int>();
            if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK)
                m_iTick = nTick <= (int)CtrlProperty._DTimeUnitAmt ? nTick : (int)CtrlProperty._DTimeUnitAmt;

            m_bBet = false;
            SetAverageVal();
            SetBoLineVal();
            SetAdx();
            SetCci(true);
            SetRsi();
        }

        /// Declare Member Variables
        private float m_fStartVal;      //시가
        private float m_fEndVal;        //종가
        private float m_fMaxVal;        //고가
        private float m_fMinVal;        //저가
        private long m_lStartTm;        //시작시간
        private long m_lEndTm;          //마감시간
        private int m_iIndex;          //인뎃스
        private int m_iTick;           //틱개수
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

        private bool m_bBet;           //배팅상태
        private float[] m_arrAvg = new float[CtrlProperty._arrAvgItems.Length];         //이동평균값1~5
        private float[] m_arrBo = new float[2];         //하늘-주황색
        private RESULTSTATE m_estType = RESULTSTATE.IGNORE;


        /// Declare Name properties
        public float Start
        {
            get { return m_fStartVal / CtrlProperty._nValueRate; }
        }
        public float End
        {
            get { return m_fEndVal / CtrlProperty._nValueRate; }
        }
        public float Max
        {
            get { return m_fMaxVal / CtrlProperty._nValueRate; }
        }
        public float Min
        {
            get { return m_fMinVal / CtrlProperty._nValueRate; }
        }

        public long TmStart
        {
            get { return m_lStartTm; }
        }
        public long TmEnd
        {
            get { return m_lEndTm; }
        }
        public int Index
        {
            get { return m_iIndex; }
        }
        public int Tick
        {
            get { return m_iTick; }
        }
        public bool BetState
        {
            get { return m_bBet; }
            set { m_bBet = value; }
        }
        public float Adx
        {
            get
            {
                if (m_iIndex < 13)
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
                if (m_iIndex < CtrlProperty._nRsiCnt)
                    return 0;
                return m_fRsi;
            }
        }
        public RESULTSTATE Result
        {
            get
            {
                if (Start > End)        //sell state
                    return RESULTSTATE.SELL;
                else if (Start < End)   //buy state
                    return RESULTSTATE.BUY;
                else return RESULTSTATE.IGNORE;
            }

        }
        public float[] Bos
        {
            get { return m_arrBo; }
        }
        public RESULTSTATE Est_Type
        {
            get { return m_estType; }
        }

        private float CanMax
        {
            get { return m_fStartVal >= m_fEndVal ? m_fStartVal : m_fEndVal; }
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
        public float Mean
        {
            get { return (m_fMaxVal + m_fMinVal + m_fEndVal) / 3; } //M(Mean price) = (H+L+C)/3
        }
        public float Au
        {
            get
            {
                if (m_iIndex < 1)
                    return 0;
                else if (m_fEndVal > CtrlProperty._DItemList[m_iIndex - 1].m_fEndVal)
                    return m_fEndVal - CtrlProperty._DItemList[m_iIndex - 1].m_fEndVal;
                else return 0;
            }
        }
        public float Ad
        {
            get
            {
                if (m_iIndex < 1)
                    return 0;
                else if (CtrlProperty._DItemList[m_iIndex - 1].m_fEndVal > m_fEndVal)
                    return CtrlProperty._DItemList[m_iIndex - 1].m_fEndVal - m_fEndVal;
                else return 0;
            }
        }
        public int AvgPos
        {
            get
            {
                if (m_arrAvg[5] == 0)
                    return 0;
                else if (CanMax > m_arrAvg[5] && CanMin > m_arrAvg[5]) //Up => 1
                    return 1;
                else if (CanMax < m_arrAvg[5] && CanMin < m_arrAvg[5]) //Down => -1
                    return -1;
                else return 0;
            }

        }
        /// Declare Member Functions
        public bool IsCompleted()
        {
            if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (m_iTick == (int)CtrlProperty._DTimeUnitAmt)
                    return true;
            }
            return false;
        }

        public float GetAvgVal(CH_AVGTYPE avgType)
        {
            int iLine = (int)avgType;
            if (iLine >= m_arrAvg.Length || iLine < 0)
                return 0;
            return m_arrAvg[iLine] / CtrlProperty._nValueRate;
        }

        public CH_TRENDTYPE GetTrendUp(CH_AVGTYPE avgType, int iFirstIdx = 0)
        {
            int iLine = (int)avgType;
            if (iLine >= m_arrAvg.Length || iLine < 0)
                return CH_TRENDTYPE.NONE;

            if (CtrlProperty._DItemList.Count <= m_iIndex || m_iIndex - 1 < 0)
                return CH_TRENDTYPE.NONE;

            DItem prevItem = null;
            if (iFirstIdx == 0 || iFirstIdx == m_iIndex)
                prevItem = CtrlProperty._DItemList[m_iIndex];
            else prevItem = CtrlProperty._DItemList[m_iIndex - 1];

            if (m_arrAvg[iLine] == 0 || prevItem.m_arrAvg[iLine] == 0)
                return CH_TRENDTYPE.NONE;

            if (m_arrAvg[iLine] >= prevItem.m_arrAvg[iLine] && m_fEndVal >= m_arrAvg[iLine])
                return CH_TRENDTYPE.UP;

            return CH_TRENDTYPE.NONE;
        }

        public CH_TRENDTYPE GetTrendDown(CH_AVGTYPE avgType, int iFirstIdx = 0)
        {
            int iLine = (int)avgType;
            if (iLine >= m_arrAvg.Length || iLine < 0)
                return CH_TRENDTYPE.NONE;

            if (CtrlProperty._DItemList.Count <= m_iIndex || m_iIndex - 1 < 0)
                return CH_TRENDTYPE.NONE;

            DItem prevItem = null;
            if (iFirstIdx == 0 || iFirstIdx == m_iIndex)
                prevItem = CtrlProperty._DItemList[m_iIndex];
            else prevItem = CtrlProperty._DItemList[m_iIndex - 1];
            if (prevItem == null)
                return CH_TRENDTYPE.NONE;

            if (m_arrAvg[iLine] == 0 || prevItem.m_arrAvg[iLine] == 0)
                return CH_TRENDTYPE.NONE;

            if (m_arrAvg[iLine] <= prevItem.m_arrAvg[iLine] && m_fEndVal <= m_arrAvg[iLine])
                return CH_TRENDTYPE.DOWN;

            return CH_TRENDTYPE.NONE;
        }

        public CH_TRENDTYPE GetCrossTrend(CH_AVGTYPE avgType1, CH_AVGTYPE avgType2)
        {
            if (m_bBet)
                return CH_TRENDTYPE.NONE;

            int iLine1 = (int)avgType1;
            int iLine2 = (int)avgType2;
            if (iLine1 >= m_arrAvg.Length || iLine1 < 0)
                return CH_TRENDTYPE.NONE;
            if (iLine2 >= m_arrAvg.Length || iLine2 < 0)
                return CH_TRENDTYPE.NONE;

            if (CtrlProperty._DItemList.Count <= m_iIndex || m_iIndex - 1 < 0)
                return CH_TRENDTYPE.NONE;

            DItem prevItem = CtrlProperty._DItemList[m_iIndex - 1];
            if (prevItem == null)
                return CH_TRENDTYPE.NONE;

            if (prevItem.m_arrAvg[iLine1] == 0 || prevItem.m_arrAvg[iLine2] == 0)
                return CH_TRENDTYPE.NONE;

            if (prevItem.m_arrAvg[iLine1] <= prevItem.m_arrAvg[iLine2] && m_arrAvg[iLine1] > m_arrAvg[iLine2])
            {
                m_bBet = true;
                return CH_TRENDTYPE.UP;
            }
            else if (prevItem.m_arrAvg[iLine1] >= prevItem.m_arrAvg[iLine2] && m_arrAvg[iLine1] < m_arrAvg[iLine2])
            {
                m_bBet = true;
                return CH_TRENDTYPE.DOWN;
            }


            return CH_TRENDTYPE.NONE;
        }
        /// <summary>
        /// Function that Set End Value
        /// </summary>
        /// <param name="fVal"></param>
        public int SetEndValue(float fVal, int nTick, int nConc)
        {
            if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK && m_iTick == (int)CtrlProperty._DTimeUnitAmt)
                return nTick;

            m_fEndVal = fVal;
            if (fVal < m_fMinVal)
                m_fMinVal = fVal;
            if (fVal > m_fMaxVal)
                m_fMaxVal = fVal;

            SetAverageVal();
            SetBoLineVal();
            SetAdx();
            SetCci();
            SetRsi();
            if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (m_iTick + nTick <= (int)CtrlProperty._DTimeUnitAmt)
                {
                    m_iTick = m_iTick + nTick;
                    return 0;
                }
                else
                {
                    nTick = m_iTick + nTick - (int)CtrlProperty._DTimeUnitAmt;
                    m_iTick = (int)CtrlProperty._DTimeUnitAmt;
                    return nTick;
                }
            }
            return 0;
        }

        ///
        public int IsValidTime(long lTmVal, int nTick)
        {

            if (CtrlProperty._DTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (lTmVal >= m_lStartTm && m_iTick + nTick <= (int)CtrlProperty._DTimeUnitAmt)
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
            if (CtrlProperty._DItemList == null)
                return;

            List<DItem> listSubItems = null;
            float fSum = 0;
            int nAvgCnt = CtrlProperty._arrAvgItems.Length;
            for (int i = 0; i < nAvgCnt; i++)
            {
                if (CtrlProperty._arrAvgItems[i] > 0 && m_iIndex >= CtrlProperty._arrAvgItems[i] - 1 && CtrlProperty._DItemList.Count > m_iIndex - 1)
                {

                    listSubItems = CtrlProperty._DItemList.GetRange(m_iIndex - CtrlProperty._arrAvgItems[i] + 1, CtrlProperty._arrAvgItems[i] - 1);
                    if (listSubItems.Count > 0)
                    {
                        fSum = listSubItems.Sum(it => it.m_fEndVal);
                        fSum += m_fEndVal;
                        m_arrAvg[i] = fSum / CtrlProperty._arrAvgItems[i];
                    }
                }
            }

        }

        public void SetBoLineVal()
        {
            if (CtrlProperty._DItemList == null)
                return;

            DItem prevItem = null;

            if (m_iIndex >= 1 && CtrlProperty._DItemList.Count > m_iIndex - 1)
            {
                prevItem = CtrlProperty._DItemList[m_iIndex - 1];

                if (m_fStartVal > m_fEndVal)     //매도
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL)
                    {
                        m_arrBo[0] = prevItem.m_arrBo[0];
                        m_arrBo[1] = m_fStartVal < prevItem.m_arrBo[1] ? m_fStartVal : prevItem.m_arrBo[1] - 1;
                        m_estType = RESULTSTATE.SELL;
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)
                    {
                        if (m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustD * CtrlProperty._fOverTick * CtrlProperty._nValueRate)/*Settings.Default.BoLineAdjustD * prevItem.CanHeight / 100*/
                        {
                            m_arrBo[0] = m_fEndVal > prevItem.m_arrBo[0] ? m_fEndVal : prevItem.m_arrBo[0] + 1;
                            m_arrBo[1] = prevItem.m_arrBo[1];
                            m_estType = RESULTSTATE.BUY;
                        }
                        else
                        {
                            m_arrBo[0] = m_fStartVal;
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.SELL;
                        }
                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.SELL;
                    }
                }
                else if (m_fStartVal < m_fEndVal)  //매수
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL) //매도
                    {
                        if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustD * CtrlProperty._fOverTick * CtrlProperty._nValueRate)
                        {
                            m_arrBo[0] = prevItem.m_arrBo[0];
                            m_arrBo[1] = m_fEndVal < prevItem.m_arrBo[1] ? m_fEndVal : prevItem.m_arrBo[1] - 1;
                            m_estType = RESULTSTATE.SELL;
                        }
                        else
                        {
                            m_arrBo[0] = m_fStartVal;
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.BUY;
                        }
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)
                    {
                        m_arrBo[0] = m_fStartVal > prevItem.m_arrBo[0] ? m_fStartVal : prevItem.m_arrBo[0] + 1;
                        m_arrBo[1] = prevItem.m_arrBo[1]; ;
                        m_estType = RESULTSTATE.BUY;
                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.BUY;
                    }
                }
                else
                {
                    if (prevItem.m_estType == RESULTSTATE.SELL)     //매도
                    {
                        if (m_fEndVal <= prevItem.CanMax + Settings.Default.BoLineAdjustD * CtrlProperty._fOverTick * CtrlProperty._nValueRate)
                        {
                            m_arrBo[0] = prevItem.m_arrBo[0];
                            m_arrBo[1] = m_fEndVal < prevItem.m_arrBo[1] ? m_fEndVal : prevItem.m_arrBo[1] - 1;
                            m_estType = RESULTSTATE.SELL;
                        }
                        else
                        {
                            m_arrBo[0] = m_fStartVal;
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.BUY;
                        }
                    }
                    else if (prevItem.m_estType == RESULTSTATE.BUY)     //매수
                    {
                        if (m_fEndVal >= prevItem.CanMin - Settings.Default.BoLineAdjustD * CtrlProperty._fOverTick * CtrlProperty._nValueRate)
                        {
                            m_arrBo[0] = m_fEndVal > prevItem.m_arrBo[0] ? m_fEndVal : prevItem.m_arrBo[0] + 1;
                            m_arrBo[1] = prevItem.m_arrBo[1]; ;
                            m_estType = RESULTSTATE.BUY;
                        }
                        else
                        {
                            m_arrBo[0] = m_fStartVal;
                            m_arrBo[1] = m_arrBo[0];
                            m_estType = RESULTSTATE.SELL;
                        }
                    }
                    else
                    {
                        m_arrBo[0] = m_fStartVal;
                        m_arrBo[1] = m_arrBo[0];
                        m_estType = RESULTSTATE.IGNORE;
                    }
                }
            }
            else
            {
                if (m_fStartVal > m_fEndVal)
                    m_estType = RESULTSTATE.SELL;
                else if (m_fStartVal < m_fEndVal)
                    m_estType = RESULTSTATE.BUY;
                else m_estType = RESULTSTATE.IGNORE;

                m_arrBo[0] = m_fStartVal;
                m_arrBo[1] = m_arrBo[0];
            }


        }
        public void SetAdx()
        {
            if (CtrlProperty._DItemList == null)
                return;

            DItem prevItem = null;

            if (m_iIndex >= 1 && CtrlProperty._DItemList.Count > m_iIndex - 1)
            {
                prevItem = CtrlProperty._DItemList[m_iIndex - 1];
                float fPdm = 0, fMdm = 0;
                //if (m_fMaxVal - prevItem.m_fMaxVal > prevItem.m_fMinVal - m_fMinVal)
                if (m_fMaxVal - prevItem.m_fMaxVal > prevItem.m_fMinVal - m_fMinVal)
                {
                    fPdm = Math.Abs(m_fMaxVal - prevItem.m_fMaxVal);
                    fMdm = 0;
                }
                else
                {
                    fPdm = 0;
                    fMdm = Math.Abs(m_fMinVal - prevItem.m_fMinVal);
                }
                //                 float fPdm = Math.Abs(m_fMaxVal - prevItem.m_fMaxVal);
                //                 float fMdm = Math.Abs(m_fMinVal - prevItem.m_fMinVal);

                float[] arrVal = { Math.Abs(m_fMaxVal - m_fMinVal), Math.Abs(m_fMaxVal - prevItem.m_fEndVal), Math.Abs(m_fMinVal - prevItem.m_fEndVal) };
                float fTr = arrVal.Max();

                if (CtrlProperty._nAdxCnt < 2)
                    CtrlProperty._nAdxCnt = 2;
                float k = 2 / ((float)CtrlProperty._nAdxCnt + 1);
                // k = 1 - k;
                float fPdi = 0, fMdi = 0;
                if (m_iIndex == 1)
                {
                    m_fPdm = fPdm;
                    m_fMdm = fMdm;
                    m_fTr = fTr;
                    if (m_fTr != 0)
                    {
                        fPdi = m_fPdm / m_fTr;
                        fMdi = m_fMdm / m_fTr;
                        if (fPdi != 0 && fMdi != 0)
                            m_fDx = Math.Abs(fPdi - fMdi) * 100 / (fPdi + fMdi);
                    }
                    m_fAdx = m_fDx;
                }
                else if (m_iIndex >= 2)
                {
                    m_fPdm = fPdm * k + prevItem.m_fPdm * (1 - k);
                    m_fMdm = fMdm * k + prevItem.m_fMdm * (1 - k);
                    m_fTr = fTr * k + prevItem.m_fTr * (1 - k);

                    if (m_fTr != 0)
                    {
                        fPdi = m_fPdm / m_fTr;
                        fMdi = m_fMdm / m_fTr;
                        if (fPdi != 0 && fMdi != 0)
                            m_fDx = Math.Abs(fPdi - fMdi) * 100 / (fPdi + fMdi);
                    }
                    m_fAdx = m_fDx * k + prevItem.m_fAdx * (1 - k);
                }
                //                 Trace.TraceInformation("<DItem> SetAdx() PDM:{0}, MDM:{1}, TR:{2}, PDI:{3}, MDI:{4}, DX:{5}, K:{6}, ADX:{7}, ",
                //                 m_fPdm, m_fMdm, m_fTr, fPdi, fMdi, m_fDx, k, m_fAdx);
            }
        }
        public void SetCci(bool bSum = false)
        {
            if (CtrlProperty._DItemList == null)
                return;


            if (m_iIndex >= 1 && CtrlProperty._DItemList.Count > CtrlProperty._nCciCnt - 1)
            {
                DItem prevItem = null;

                float fMean = 0;
                float fD = 0;
                float fSM = 0;

                if (bSum)
                {
                    m_fSMn = 0;
                    m_fDn = 0;
                    for (int i = CtrlProperty._nCciCnt - 1; i >= 1; i--)
                    {
                        prevItem = CtrlProperty._DItemList[m_iIndex - i];
                        fMean = prevItem.Mean;
                        m_fSMn += fMean;
                        m_fDn += Math.Abs(fMean - m_fSMn / (CtrlProperty._nCciCnt - i));
                    }
                }
                fMean = this.Mean;
                fSM = (m_fSMn + fMean) / CtrlProperty._nCciCnt;
                fD = (m_fDn + Math.Abs(fMean - fSM)) / CtrlProperty._nCciCnt;
                m_fCci = (fMean - fSM) / (CtrlProperty._fCciConst * fD);

                //                  Trace.TraceInformation("<DItem> SetCci() m_fSMn:{0:N2}, m_fDn:{1}, fMean:{2}, fSM:{3}, fD:{3}, m_fCci:{5} ",
                //                      m_fSMn, m_fDn, fMean, fSM, fD, m_fCci);


            }

        }

        public void SetRsi()
        {
            if (CtrlProperty._DItemList == null)
                return;

            DItem prevItem = null;


            if (m_iIndex == CtrlProperty._nRsiCnt)
            {
                float fAuSum = 0;
                float fAdSum = 0;
                for (int i = 1; i < m_iIndex; i++)
                {
                    fAuSum += CtrlProperty._DItemList[i].Au;
                    fAdSum += CtrlProperty._DItemList[i].Ad;
                }
                fAuSum += Au;
                fAdSum += Ad;
                m_fAum = fAuSum / CtrlProperty._nRsiCnt;
                m_fAdm = fAdSum / CtrlProperty._nRsiCnt;
                m_fRsi = m_fAum * 100 / (m_fAum + m_fAdm);

                //                 Trace.TraceInformation("<RItem> SetRsi() AUM:{0}, ADM:{1}, RSI:{2}",
                //                     m_fAum, m_fAdm, m_fRsi);
            }
            else if (m_iIndex > CtrlProperty._nRsiCnt && CtrlProperty._DItemList.Count > m_iIndex - 1)
            {
                prevItem = CtrlProperty._DItemList[m_iIndex - 1];

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
