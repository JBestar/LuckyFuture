using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ChartCtrl.Properties;

namespace ChartCtrl
{

    public class CItem
    {
        public CItem(long tmStart, long tmEnd, int nTick, int nConc)
        {
            m_lStartTm = tmStart;
            m_lEndTm = tmEnd;
            if (CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK)
                m_iTick = nTick <= (int)CtrlProperty._CTimeUnitAmt ? nTick : (int)CtrlProperty._CTimeUnitAmt;
            m_nConc = nConc;
        }

        /// Declare Member Variables
        private long m_lStartTm;        //시작시간
        private long m_lEndTm;          //마감시간
        private int m_iTick;           //틱수
        private int m_nConc;           //거래량


        /// Declare Name properties
        
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
        public int Conc
        {
            get { return m_nConc; }
        }
        
        /// Declare Member Functions
        public bool IsCompleted()
        {
            if(CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (m_iTick == (int)CtrlProperty._CTimeUnitAmt)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Function that Set End Value
        /// </summary>
        /// <param name="fVal"></param>
        public int SetEndValue(float fVal, int nTick, int nConc)
        {
            if (CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK && m_iTick == (int)CtrlProperty._CTimeUnitAmt)
                return nTick;

            m_nConc += nConc ;

            if (CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (m_iTick + nTick <= (int)CtrlProperty._CTimeUnitAmt)
                {
                    m_iTick = m_iTick + nTick;
                    return 0;
                }
                else
                {
                    nTick = m_iTick + nTick - (int)CtrlProperty._CTimeUnitAmt;
                    m_iTick = (int)CtrlProperty._CTimeUnitAmt;
                    return nTick;
                }
            }
            return 0;
        }
        
        ///
        public int IsValidTime(long lTmVal, int nTick)
        {

            if (CtrlProperty._CTimeType == TIMETYPE.TIMETYPE_TICK)
            {
                if (lTmVal >= m_lStartTm && m_iTick + nTick <= (int)CtrlProperty._CTimeUnitAmt)
                    return 1;
                else return 0;
            }
            else if (lTmVal >= m_lStartTm && lTmVal < m_lEndTm)
                return 1;

            return 0;
        }

        

    }
}
