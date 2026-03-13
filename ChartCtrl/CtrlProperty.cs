using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ChartCtrl
{
    public enum RESULTSTATE
    {
        BUY,    //매수
        SELL,   //매도
        IGNORE
    }
    public enum CONCSTATE
    {
        CONCLUDE, //체결
        LIQUID,   //청산
        RECONC    //되돌림
    }
    public enum TIMETYPE
    {
        TIMETYPE_SEC,
        TIMETYPE_MIN,
        TIMETYPE_DAY,
        TIMETYPE_WEEK,
        TIMETYPE_MONTH,
        TIMETYPE_YEAR,
        TIMETYPE_TICK
    }

    public enum TIMEUNIT
    {
        TIMEUNIT_1 = 1,
        TIMEUNIT_3 = 3,
        TIMEUNIT_5 = 5,
        TIMEUNIT_15 = 15,
        TIMEUNIT_30 = 30,
        TIMEUNIT_60 = 60,
        TIMEUNIT_90 = 90,
        TIMEUNIT_120 = 120,
        TIMEUNIT_240 = 240,
        TIMEUNIT_350 = 350,
        TIMEUNIT_400 = 400,
        TIMEUNIT_600 = 600,
        TIMEUNIT_750 = 750,
        TIMEUNIT_990 = 990,
    }

    public enum CH_AVGTYPE
    {
        LINE_1 = 0,
        LINE_2 = 1,
        LINE_3 = 2,
        LINE_4 = 3,
        LINE_5 = 4,
    }

    public enum CH_TRENDTYPE
    {
        NONE = 0,
        UP = 1,
        DOWN = 2
    }
    public enum CHART_EVENTTYPE
    {
        DRAWING_CHANGED,
        BETTING_CHANGED,
        SETTING_CHANGED,
        ORDERCNT_CHANGED,
        EARNTICK_CHANGED,
        LOSSTICK_CHANGED,
        SYNCCHART_CHANGED,

    }
    public class ChartEventArgs : EventArgs
    {
        public ChartEventArgs(object data)
        {
            this.Data = data;
        }

        public object Data { get; set; }
    }

    public struct PointCh
    {

        public PointCh(long ltmX, float fvalY)
        {
            _lTmX = ltmX;
            _fValY = fvalY;
        }
        public long _lTmX { get; }
        public float _fValY { get; }
    }



    public struct RealVal
    {
        public RealVal(float fVal, DateTime dtReceive, int nTick, int nConc)
        {
            _fVal = fVal;
            _dtRec = dtReceive;
            _nTick = nTick;
            _nConc = nConc; 
        }
        public float _fVal { get; }
        public DateTime _dtRec { get; }
        public int _nTick { get; }
        public int _nConc{ get; }
    }

    public class OrderVal
    {

        public string OrderType { get; set; }
        public string Symbol { get; set; }
        public string AveragePrice { get; set; }
        public DateTime OrderTime { get; set; }
        public long OrderTm { get; set; }
        public double OrderQty { get; set; }
        public RESULTSTATE ResultState { get; set; }
        public CONCSTATE ConcState { get; set; }
        public TIMETYPE TimeType { get; set; }
        public TIMEUNIT TimeUnit { get; set; }
        public bool Layouted { get; set; }

    }

    public class CtrlProperty
    {
        public CtrlProperty()
        {

        }
        
        //static variables
        public static int _iChartStyle = 0;
        public static TIMETYPE _RTimeType = TIMETYPE.TIMETYPE_MIN;          ///Chart Time Unit Type
        public static TIMEUNIT _RTimeUnitAmt = TIMEUNIT.TIMEUNIT_1;         ///Chart Time Unit Amount

        public static TIMETYPE _DTimeType = TIMETYPE.TIMETYPE_MIN;              ///Chart Time Unit Type
        public static TIMEUNIT _DTimeUnitAmt = TIMEUNIT.TIMEUNIT_1;        ///Chart Time Unit Amount

        public static TIMETYPE _CTimeType = TIMETYPE.TIMETYPE_MIN;              ///Chart Time Unit Type
        public static TIMEUNIT _CTimeUnitAmt = TIMEUNIT.TIMEUNIT_1;        ///Chart Time Unit Amount

        public static int _nItemTotal = 30;             ///Total Item Count
        public static int _nItemView = 30;              ///Item Count For Display
        public static int _nItemWidth = 100;            ///Item Width 

        public static string _tTimePattern = "{0:HH:mm}";
        public static string _tDtPattern = "{0:MM/dd HH:mm:ss}";
        public static string _tDayPattern = "{0:yyyy/MM/dd}";
        public static string _tTmPattern = "{0:HH:mm:ss}";

        public static float _fValueOrigin = 0;     ///Chart Origin Value
        public static float _fValueMax = 0;        ///Chart Max Value
        public static float _nValueRate = 100;     ///Chart Value View Margin
        public static float _nValueUnit = 500;       ///Chart Value Unit
        public static float _nValueMargin = 300;     ///Chart Value View Margin
        public static string _tValueFormat = "{0:0.##}";
        public static float _fOverTick = 1;        ///Chart Value View Margin

        public static int _ClientW = 0;             ///Client Region Width
        public static int _ClientH = 0;             ///Client Region Height

        public static int _nHScrollbarHeight = 18;  ///Horizontal Scrollbar Height
        public static int _nValueAxisBand = 60;     ///Value-Axis Width
        public static int _nTimeAxisBand = 20;      ///Value-Axis Width

        public static bool _bGridRuler = true;
        public static bool _bSaveRtVal = false;

        public static int _nTimeOrigin = 0;        ///Chart Origin Time 
        public static int _nTimeMax = 0;           ///Chart Max Time 

        public static int[] _arrAvgItems = { 5, 10, 20, 60, 120, 200 };
        public static List<RItem> _RItemList = new List<RItem>();
        public static List<DItem> _DItemList = new List<DItem>();
        public static List<CItem> _CItemList = new List<CItem>();
        public static List<OrderVal> _OrderList = new List<OrderVal>();

        public static int _nBoLineAdjust = 0;
        public static int _nAdxCnt = 14;
        public static int _nCciCnt = 14;
        public static int _nRsiCnt = 14;
        public static float _fCciConst = 0.015f;

        public static SolidBrush _DrawPaneBrush = new SolidBrush(Color.FromArgb(255, 214, 214, 214));
        public static SolidBrush _RulerPaneBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        public static Pen _RulerPen = new Pen(Color.White, 1);

        public static void SetTimeType(TIMETYPE timeType)
        {
            
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_SEC: _RTimeType = TIMETYPE.TIMETYPE_SEC; _tTimePattern = "{0:HH:mm:ss}"; break;
                case TIMETYPE.TIMETYPE_MIN: _RTimeType = TIMETYPE.TIMETYPE_MIN; _tTimePattern = "{0:HH:mm}"; break;
                case TIMETYPE.TIMETYPE_DAY: _RTimeType = TIMETYPE.TIMETYPE_DAY; _tTimePattern = "{0:MM/dd}"; break;
                case TIMETYPE.TIMETYPE_WEEK: _RTimeType = TIMETYPE.TIMETYPE_WEEK; _tTimePattern = "{0:MM/dd}"; break;
                case TIMETYPE.TIMETYPE_MONTH: _RTimeType = TIMETYPE.TIMETYPE_MONTH; _tTimePattern = "{0:MM/dd}"; break;
                case TIMETYPE.TIMETYPE_YEAR: _RTimeType = TIMETYPE.TIMETYPE_YEAR; _tTimePattern = "{0:yyyy/MM/dd}"; break;
                case TIMETYPE.TIMETYPE_TICK: _RTimeType = TIMETYPE.TIMETYPE_TICK; _tTimePattern = "{0:HH:mm:ss}"; break;
                default: _RTimeType = TIMETYPE.TIMETYPE_MIN; _tTimePattern = "{0:HH:mm}"; break;
            }
            
        }
        public static void SetValueRate(int rate, string tFormat, float tick)
        {
            _nValueRate = rate;
            _nValueUnit = 5 * 100 / rate;
            _nValueMargin = 3 * 100 / rate;
            _tValueFormat = tFormat;
            _fOverTick = tick;
        }

        ///Get TimeStamp From DateTime
        public static long GetTimeStamp(DateTime dtValue)
        {
            //return ((DateTimeOffset)dtValue).ToUnixTimeSeconds();
            var timeSpan = dtValue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)timeSpan.TotalSeconds;

        }

        /// <summary>파일 전용 디버그 로그 (UI 노출 없음). 앱에서 SetFileLogWriter(WriteLog)로 연결.</summary>
        public static Action<string> FileLogWriter { get; set; }

        static void WriteFileLog(string msg)
        {
            try { FileLogWriter?.Invoke(msg); } catch { }
        }

        /// <summary>GetTime 정상 경로에서 FileLogWriter 동작 확인용, 1회만 기록</summary>
        static bool _getTimeNormalPathLogged = false;

        ///Get DateTime From TimeStamp
        /// <summary>초 단위 또는 밀리초 단위 Unix 타임스탬프를 DateTime으로 변환. 범위를 벗어나면 MinValue/MaxValue로 클램프하여 크래시 방지.</summary>
        public static DateTime GetTime(long lSecs)
        {
            long lSecsOrig = lSecs;
            // 밀리초 단위로 들어온 경우(1e12 초과) 초 단위로 변환
            if (lSecs > 1e12)
            {
                WriteFileLog(string.Format("[DateTime범위] GetTime lSecs={0} (밀리초로판단, /1000 적용)", lSecsOrig));
                lSecs = lSecs / 1000;
            }

            DateTime dtOrigin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            const long minSecs = -62135596800L;   // DateTime.MinValue 기준
            const long maxSecs = 253402300799L;   // DateTime.MaxValue 기준

            if (lSecs < minSecs)
            {
                WriteFileLog(string.Format("[DateTime범위] GetTime lSecs={0} → minSecs 미만, MinValue 반환", lSecsOrig));
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            if (lSecs > maxSecs)
            {
                WriteFileLog(string.Format("[DateTime범위] GetTime lSecs={0} → maxSecs 초과, MaxValue 반환", lSecsOrig));
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }

            try
            {
                DateTime result = dtOrigin.AddSeconds(lSecs);
                // 정상 경로에서 FileLogWriter 동작 여부 확인용 (1회만 로그)
                if (!_getTimeNormalPathLogged)
                {
                    _getTimeNormalPathLogged = true;
                    WriteFileLog(string.Format("[ChartCtrl] GetTime 정상경로 동작확인 lSecs={0} → FileLogWriter 연결됨", lSecsOrig));
                }
                return result;
            }
            catch (Exception ex)
            {
                WriteFileLog(string.Format("[DateTime범위] GetTime AddSeconds 예외 lSecs={0} ex={1}", lSecsOrig, ex.Message));
                if (lSecs < 0)
                    return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
        }

        public static DateTime GetStartTime(TIMETYPE timeType, TIMEUNIT timeUnit, DateTime dtVal)
        {
            DateTime dtStart = dtVal;
            int nTmPast;
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_SEC:
                    nTmPast = (dtVal.Minute * 60 + dtVal.Second) % (int)timeUnit;
                    dtStart = dtVal.AddSeconds(-nTmPast);
                    break;
                case TIMETYPE.TIMETYPE_MIN:
                    nTmPast = (dtVal.Hour * 60 + dtVal.Minute) % (int)timeUnit;
                    dtStart = dtVal.AddSeconds(-dtVal.Second);
                    dtStart = dtStart.AddMinutes(-nTmPast);
                    break;
                case TIMETYPE.TIMETYPE_DAY:
                    dtStart = new DateTime(dtVal.Year, dtVal.Month, dtVal.Day);
                    break;
                case TIMETYPE.TIMETYPE_WEEK:
                    dtStart = new DateTime(dtVal.Year, dtVal.Month, dtVal.Day);
                    dtStart = dtStart.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(dtVal.DayOfWeek));
                    break;
                case TIMETYPE.TIMETYPE_MONTH:
                    dtStart = new DateTime(dtVal.Year, dtVal.Month, 1);
                    break;
                case TIMETYPE.TIMETYPE_YEAR:
                    dtStart = new DateTime(dtVal.Year, 1, 1);
                    break;
                case TIMETYPE.TIMETYPE_TICK:
                    dtStart = dtVal;
                    break;
                default: break;
            }
            
            return dtStart;
        }
        public static DateTime GetEndTime(TIMETYPE timeType, TIMEUNIT timeUnit, DateTime dtStart, bool isNext)
        {
            DateTime dtEnd = dtStart;
            int nTimeUnit = (int)timeUnit;
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_SEC:
                    dtEnd = dtStart.AddSeconds(isNext ? nTimeUnit : -nTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_MIN:
                    dtEnd = dtStart.AddMinutes(isNext ? nTimeUnit : -nTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_DAY:
                    dtEnd = dtStart.AddDays(isNext ? 1 : -1);
                    break;
                case TIMETYPE.TIMETYPE_WEEK:
                    dtEnd = dtStart.AddDays(isNext ? 7 : -7);
                    break;
                case TIMETYPE.TIMETYPE_MONTH:
                    dtEnd = dtStart.AddMonths(isNext ? 1 : -1);
                    break;
                case TIMETYPE.TIMETYPE_YEAR:
                    dtEnd = dtStart.AddYears(isNext ? 1 : -1);
                    break;
                case TIMETYPE.TIMETYPE_TICK:
                    dtEnd = dtStart;
                    break;
                default: break;
            }

            return dtEnd;
        }

        ///Get World Point From Client Point
        public static PointF GetWorldPt(Point ptPos)
        {

            if (_ClientW == 0 || _ClientH == 0)
                return new PointF(-1, -1);

            int nTmWidth = _nTimeMax - _nTimeOrigin;
            float nValHeight = _fValueMax - _fValueOrigin;

            float fTmX = _nTimeOrigin + ptPos.X * nTmWidth / (float)_ClientW;
            float fValY = _fValueMax - ptPos.Y * nValHeight / (float)_ClientH;

            return new PointF(fTmX, fValY);
        }

        public static float GetClientX(int nTmX)
        {
            int nTmWidth = _nTimeMax - _nTimeOrigin;
            if (nTmWidth <= 0)
                return 0;
            float fX = (nTmX- _nTimeOrigin) * _ClientW / nTmWidth;
            return fX;
        }

       


    }

}
