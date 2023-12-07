using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LuckyFutureLib.Include
{
    public enum BETTYPE
    {
        EQUIVALENT = 0,
        UPDOWN = 1,
        CROSS = 2,
        BOLINE = 3,
        HYBRID = 4,
        CCI = 5,
        BOT1 = 6,
    }

    public enum CHARTTYPE
    {
        MIN_1,
        TICK_60,
        TICK_90,
        TICK_120,
        TICK_240,
        TICK_350,
        TICK_400,
        TICK_600,
        TICK_750,
        TICK_990,
        MIN_5,
        MIN_15,
        MIN_30,
        NONE = 100,
    }
    public class Common
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "";
        }

        public static string GetPriceFormat(int precision)
        {
            string strFormat = "{0:0.00}";
            switch (precision)
            {
                case 0:strFormat = "{0:0}"; break;
                case 1: strFormat = "{0:0.0}"; break;
                case 2: strFormat = "{0:0.00}"; break;
                case 3: strFormat = "{0:0.000}"; break;
                case 4: strFormat = "{0:0.0000}"; break;
                case 5: strFormat = "{0:0.00000}"; break;
                case 6: strFormat = "{0:0.000000}"; break;
                default: break;
            }

            return strFormat;
            
        }

        public static string GetFormatStr(double value, int precision)
        {
            string strFormat = "{0:0,0}";
            switch (precision)
            {
                case 1: strFormat = "{0:0.0}"; break;
                case 2: strFormat = "{0:0.00}"; break;
                case 3: strFormat = "{0:0.000}"; break;
                case 4: strFormat = "{0:0.0000}"; break;
                case 5: strFormat = "{0:0.00000}"; break;
                case 6: strFormat = "{0:0.000000}"; break;
                default: break;
            }


            return String.Format(strFormat, value);
        }

        public static string GetChartTypeStr(CHARTTYPE chartType)
        {
            string strType = "";
            switch (chartType)
            {
                case CHARTTYPE.MIN_1: strType = "1분"; break;
                case CHARTTYPE.TICK_60: strType = "60틱"; break;
                case CHARTTYPE.TICK_90: strType = "90틱"; break;
                case CHARTTYPE.TICK_120: strType = "120틱"; break;
                case CHARTTYPE.TICK_240: strType = "240틱"; break;
                case CHARTTYPE.TICK_350: strType = "350틱"; break;
                case CHARTTYPE.TICK_400: strType = "400틱"; break;
                case CHARTTYPE.TICK_600: strType = "600틱"; break;
                case CHARTTYPE.TICK_750: strType = "750틱"; break;
                case CHARTTYPE.TICK_990: strType = "990틱"; break;
                case CHARTTYPE.MIN_5: strType = "5분"; break;
                case CHARTTYPE.MIN_15: strType = "15분"; break;
                case CHARTTYPE.MIN_30: strType = "30분"; break;
                default: break;
            }


            return strType;
        }
        public static int GetPrecisionRate(int precision)
        {
            int rate = 10;
            if (precision < 1 || precision > 6)
                return rate;

            return (int)Math.Pow(10, precision);

        }

        public static int GetPrecisionRate(string symbol, int precision = 0)
        {
            int rate = 2;

            if (symbol.StartsWith("NQ"))        //Nasdaq
            {
                rate = 2;
            }
            else if (symbol.StartsWith("ES")) //E-mini SP
            {
                rate = 3;
            }
            else if (symbol.StartsWith("HSI")) //Hang Seng
            {
                rate = 2;
            }
            else if (symbol.StartsWith("CL") || symbol.StartsWith("NG")) //Crude Oil
            {
                rate = 5;
            }
            else if (symbol.StartsWith("GC")) //Gold
            {
                rate = 3;
            }
            else if (symbol.StartsWith("SI")) //Silver
            {
                rate = 5;
            }
            else if (symbol.StartsWith("HG")) //Copper
            {
                rate = 6;
            }
            else if (symbol.StartsWith("URO") || symbol.StartsWith("6E") || symbol.StartsWith("6A") || symbol.StartsWith("6C")
                || symbol.StartsWith("BP") || symbol.StartsWith("6B") || symbol.StartsWith("6S") || symbol.StartsWith("6L")
                  || symbol.StartsWith("6N") || symbol.StartsWith("SR3")) //Euro FX
            {
                rate = 6;
            }
            else if (symbol.StartsWith("JY") || symbol.StartsWith("6J")) //Japanese Yen
            {
                rate = 3;
            }
            else if (symbol.StartsWith("101R")) //KOSPI 200 F
            {
                rate = 4;
            }
            else if (precision > 0)
            {
                if (precision < 6)
                    rate = precision + 1;
                else rate = 6;
            }

            return (int)Math.Pow(10, rate);

        }
        public static string GetValueFormat(int precision)
        {
            string strFormat = "{0:0.##}";
            switch (precision)
            {
                case 1: strFormat = "{0:0.##}"; break;
                case 2: strFormat = "{0:0.##}"; break;
                case 3: strFormat = "{0:0.###}"; break;
                case 4: strFormat = "{0:0.####}"; break;
                case 5: strFormat = "{0:0.#####}"; break;
                case 6: strFormat = "{0:0.######}"; break;
                default: break;
            }


            return strFormat;
        }

        public static long StrToLong(string sValue)
        {
            long lValue = 0;
            int iSignPos = sValue.IndexOf('-');
            if(iSignPos > 0)
            {
                sValue = sValue.Substring(iSignPos);
            }
            try
            {
                lValue = long.Parse(sValue);
            }
            catch (Exception) { }

            return lValue;

        }
        public static int ExtractString(out string dest, string soruce, string start, string end, int iStartIndex = 0)
        {
            dest = "";
            // start position
            iStartIndex = soruce.IndexOf(start, iStartIndex);
            if (iStartIndex < 0) return -1;
            iStartIndex += start.Length;
            // end position
            int iEnd = soruce.IndexOf(end, iStartIndex);
            if (iEnd < 0) return -1;
            // set value
            dest = soruce.Substring(iStartIndex, iEnd - iStartIndex);
            iStartIndex = iEnd + 1;
            return iStartIndex;
        }

        public static Process GetKFOpenLoginProc()
        {
            Process procKFLogin = Process.GetProcesses().FirstOrDefault<Process>(
                delegate (Process p)
                {
                    if (p.ProcessName.StartsWith("nfstarter"))
                        return true;
                    return false;
                }
            );
            return procKFLogin;
        }

        public static string GetBetTypeFullStr(BETTYPE betType)
        {
            string typeStr = "";
            switch (betType)
            {
                case BETTYPE.EQUIVALENT:
                    typeStr = "동일캔들"; break;
                case BETTYPE.UPDOWN:
                    typeStr = "이평 언오버"; break;
                case BETTYPE.CROSS:
                    typeStr = "이평 크로스"; break;
                case BETTYPE.BOLINE:
                    typeStr = "S-B선"; break;
                case BETTYPE.HYBRID:
                    typeStr = "이평-SB"; break;
                case BETTYPE.BOT1:
                    typeStr = "S-B T1"; break;
                case BETTYPE.CCI:
                    typeStr = "CCI"; break;
                default: break;

            }
            return typeStr;
        }

        public static string GetBetTypeStr(BETTYPE betType)
        {
            string typeStr = "";
            switch (betType)
            {
                case BETTYPE.EQUIVALENT:
                    typeStr = "동캔"; break;
                case BETTYPE.UPDOWN:
                    typeStr = "이상"; break;
                case BETTYPE.CROSS:
                    typeStr = "이교"; break;
                case BETTYPE.BOLINE:
                    typeStr = "주하"; break;
                case BETTYPE.HYBRID:
                    typeStr = "이주"; break;
                case BETTYPE.CCI:
                    typeStr = "CCI"; break;
                default: break;

            }
            return typeStr;
        }

        public static BETTYPE[] GetAllBetType()
        {
            BETTYPE[] betTypeList =
            {
                BETTYPE.EQUIVALENT, BETTYPE.UPDOWN, BETTYPE.CROSS,
                BETTYPE.BOLINE, BETTYPE.HYBRID, /*BETTYPE.BOT1 BETTYPE.CCI*/
            };
            return betTypeList;
        }
    }
}
