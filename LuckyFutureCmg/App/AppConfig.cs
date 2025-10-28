using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using LuckyFuture.Properties;
using LuckyFuture.Models.ValueObjects;

namespace LuckyFuture
{
    public class AppConfig
    {
        public static readonly object _lockObj = new object();
        public static List<PayoffLossInfo> PayoffLossConfs = new List<PayoffLossInfo>();
        public static List<PayoffLossInfo> SmartLossConfs = new List<PayoffLossInfo>();
        public static List<PayoffLossInfo> CrossLossConfs = new List<PayoffLossInfo>();
        public static List<PayoffLossInfo> CciLossConfs = new List<PayoffLossInfo>();
        public static List<MemberInfo> SyncMemInfos = new List<MemberInfo>();
        public static string _PhysicalAddr = "";
        public static string _IpAddr = "";
        public static int _DtDelay = 0;
        public static bool ReadConfig(string filePath = "")
        {
            try
            {
                if (filePath.Length == 0)
                {
                    return false;
                }

                XmlDocument document = new XmlDocument();
                document.Load(filePath);

                XmlElement itemListElement = document["Settings"];

                string name = "", value = "";
                foreach (XmlElement itemElement in itemListElement.ChildNodes)
                {
                    try
                    {
                        name = itemElement.GetAttribute("Name");
                        value = itemElement.GetAttribute("Value");
                        switch (name)
                        {
                            case "LossStop": Settings.Default.LossStop = bool.Parse(value); break;
                            case "EarnStop": Settings.Default.EarnStop = bool.Parse(value); break;
                            case "LossPayoff": Settings.Default.LossPayoff = bool.Parse(value); break;
                            case "EarnPayoff": Settings.Default.EarnPayoff = bool.Parse(value); break;
                            case "LossPayoffMoney": Settings.Default.LossPayoffMoney = int.Parse(value); break;
                            case "EarnPayoffMoney": Settings.Default.EarnPayoffMoney = int.Parse(value); break;
                            case "LossStopMoney": Settings.Default.LossStopMoney = int.Parse(value); break;
                            case "EarnStopMoney": Settings.Default.EarnStopMoney = int.Parse(value); break;
                            case "BettingCandleCount": Settings.Default.BettingCandleCount = int.Parse(value); break;
                            case "ChartType": Settings.Default.ChartType = int.Parse(value); break;
                            case "AvgType": Settings.Default.AvgType = int.Parse(value); break;
                            case "CandlePayoff": Settings.Default.CandlePayoff = bool.Parse(value); break;
                            case "CandlePayoffCount": Settings.Default.CandlePayoffCount = int.Parse(value); break;
                            case "BettingType": Settings.Default.BettingType = int.Parse(value); break;
                            case "BettingTickCount": Settings.Default.BettingTickCount = int.Parse(value); break;
                            case "TickPayoffCount": Settings.Default.TickPayoffCount = int.Parse(value); break;
                            case "OrderStop": Settings.Default.OrderStop = bool.Parse(value); break;
                            case "OrderStopDelay": Settings.Default.OrderStopDelay = int.Parse(value); break;
                            case "BettingCandleComplete": Settings.Default.BettingCandleComplete = int.Parse(value); break;
                            case "OrderCount": Settings.Default.OrderCount = float.Parse(value); break;
                            case "OrderType": Settings.Default.OrderType = int.Parse(value); break;
                            // case "OrderMax": Settings.Default.OrderMax = int.Parse(value); break;
                            case "Reorder": Settings.Default.Reorder = bool.Parse(value); break;
                            case "ReturnOption": Settings.Default.ReturnOption = byte.Parse(value); break;
                            case "BoAdjustPerOn": Settings.Default.BoAdjustPerOn = bool.Parse(value); break;
                            case "BoLineAdjust": Settings.Default.BoLineAdjust = int.Parse(value); break;
                            case "BoAdjustSecOn": Settings.Default.BoAdjustSecOn = bool.Parse(value); break;
                            case "BoAdjustSec": Settings.Default.BoAdjustSec = int.Parse(value); break;
                            case "BettingEnter": Settings.Default.BettingEnter = bool.Parse(value); break;
                            case "LiquidStop": Settings.Default.LiquidStop = bool.Parse(value); break;
                            case "ForceEarnPayoff": Settings.Default.ForceEarnPayoff = bool.Parse(value); break;
                            case "SmartLossPayoff": Settings.Default.SmartLossPayoff = bool.Parse(value); break;
                            case "BandChart": Settings.Default.BandChart = bool.Parse(value); break;
                            case "BandVal1": Settings.Default.BandVal1 = int.Parse(value); break;
                            case "BandVal2": Settings.Default.BandVal2 = int.Parse(value); break;
                            case "BandVal3": Settings.Default.BandVal3 = int.Parse(value); break;
                            case "BandChartType1": Settings.Default.BandChartType1 = int.Parse(value); break;
                            case "BandChartType2": Settings.Default.BandChartType2 = int.Parse(value); break;
                            case "BandChartType3": Settings.Default.BandChartType3 = int.Parse(value); break;
                            case "BandChartType4": Settings.Default.BandChartType4 = int.Parse(value); break;
                            case "SmartEarnTick": Settings.Default.SmartEarnTick = int.Parse(value); break;
                            case "SmartLossTick": Settings.Default.SmartLossTick = int.Parse(value); break;
                            case "SmartLossUnit": Settings.Default.SmartLossUnit = int.Parse(value); break;
                            case "CrossLossPayoff": Settings.Default.CrossLossPayoff = bool.Parse(value); break;
                            case "CrossLossTick": Settings.Default.CrossLossTick = int.Parse(value); break;
                            case "CrossLossUnit": Settings.Default.CrossLossUnit = int.Parse(value); break;
                            case "AlarmStop": Settings.Default.AlarmStop = bool.Parse(value); break;
                            case "InformStop": Settings.Default.InformStop = bool.Parse(value); break;
                            case "EarnPayoffN": Settings.Default.EarnPayoffN = bool.Parse(value); break;
                            case "EarnPayoffMoneyN": Settings.Default.EarnPayoffMoneyN = int.Parse(value); break;
                            case "LossPayoffN": Settings.Default.LossPayoffN = bool.Parse(value); break;
                            case "LossPayoffMoneyN": Settings.Default.LossPayoffMoneyN = int.Parse(value); break;
                            case "OrderSelectOn": Settings.Default.OrderSelectOn = bool.Parse(value); break;
                            case "OrderSelectType": Settings.Default.OrderSelectType = int.Parse(value); break;
                            case "AutoReserveTime": Settings.Default.AutoReserveTime = DateTime.Parse(value); break;
                            case "AutoReserveOn": Settings.Default.AutoReserveOn = bool.Parse(value); break;
                            case "ProfitStop": Settings.Default.ProfitStop = bool.Parse(value); break;
                            case "ProfitStopRate": Settings.Default.ProfitStopRate = int.Parse(value); break;
                            case "Conc1On": Settings.Default.Conc1On = bool.Parse(value); break;
                            case "Conc1Min": Settings.Default.Conc1Min = int.Parse(value); break;
                            case "Conc1Cnt": Settings.Default.Conc1Cnt = int.Parse(value); break;
                            case "Conc2On": Settings.Default.Conc2On = bool.Parse(value); break;
                            case "Conc2Chart": Settings.Default.Conc2Chart = int.Parse(value); break;
                            case "Conc2Candle": Settings.Default.Conc2Candle = int.Parse(value); break;
                            case "Conc2Cnt": Settings.Default.Conc2Cnt = int.Parse(value); break;
                            case "AdxOn": Settings.Default.AdxOn = bool.Parse(value); break;
                            case "AdxCnt": Settings.Default.AdxCnt = int.Parse(value); break;
                            case "CciOn": Settings.Default.CciOn = bool.Parse(value); break;
                            case "CciRange1": Settings.Default.CciRange1 = int.Parse(value); break;
                            case "CciRange2": Settings.Default.CciRange2 = int.Parse(value); break;
                            case "CciRange11": Settings.Default.CciRange11 = int.Parse(value); break;
                            case "CciRange21": Settings.Default.CciRange21 = int.Parse(value); break;
                            case "CciSide1": Settings.Default.CciSide1 = int.Parse(value); break;
                            case "CciSide2": Settings.Default.CciSide2 = int.Parse(value); break;
                            case "SmartRangePayoff": Settings.Default.SmartRangePayoff = bool.Parse(value); break;
                            case "CrossRangePayoff": Settings.Default.CrossRangePayoff = bool.Parse(value); break;
                            case "RsiOn": Settings.Default.RsiOn = bool.Parse(value); break;
                            case "RsiRange1": Settings.Default.RsiRange1 = int.Parse(value); break;
                            case "RsiRange2": Settings.Default.RsiRange2 = int.Parse(value); break;
                            case "RsiSide1": Settings.Default.RsiSide1 = int.Parse(value); break;
                            case "RsiSide2": Settings.Default.RsiSide2 = int.Parse(value); break;
                            case "AvgsOn": Settings.Default.AvgsOn = bool.Parse(value); break;
                            case "AvgsCandle": Settings.Default.AvgsCandle = int.Parse(value); break;
                            case "AvgsSide1": Settings.Default.AvgsSide1 = int.Parse(value); break;
                            case "AvgsSide2": Settings.Default.AvgsSide2 = int.Parse(value); break;
                            case "LossRangePayoff": Settings.Default.LossRangePayoff = bool.Parse(value); break;
                            case "CciPayoff": Settings.Default.CciPayoff = bool.Parse(value); break;
                            case "CciPayoffValue1": Settings.Default.CciPayoffValue1 = int.Parse(value); break;
                            case "CciPayoffValue2": Settings.Default.CciPayoffValue2 = int.Parse(value); break;
                            case "CciPayoffValue3": Settings.Default.CciPayoffValue3 = int.Parse(value); break;
                            case "CciRangePayoff": Settings.Default.CciRangePayoff = bool.Parse(value); break;
                            case "BoOrdType": Settings.Default.BoOrdType = int.Parse(value); break;
                            case "PayoffLossRange": Settings.Default.PayoffLossRange = ParseSerialString(value); break;
                            case "SmartLossRange": Settings.Default.SmartLossRange = ParseSerialString(value); break;
                            case "CrossLossRange": Settings.Default.CrossLossRange = ParseSerialString(value); break;
                            case "CciLossRange": Settings.Default.CciLossRange = ParseSerialString(value); break;
                            case "OrdCnts": Settings.Default.OrdCnts = value; break;
                            case "EarnTicks": Settings.Default.EarnTicks = value; break;
                            case "LossTicks": Settings.Default.LossTicks = value; break;
                            case "SyncChart": Settings.Default.SyncChart = bool.Parse(value); break;
                            // case "SignalSiteOn": Settings.Default.SignalSiteOn = bool.Parse(value); break;
                            case "BollPayoff": Settings.Default.BollPayoff = bool.Parse(value); break;
                            case "BollPayoffDown": Settings.Default.BollPayoffDown = float.Parse(value); break;
                            case "BollPayoffUp": Settings.Default.BollPayoffUp = float.Parse(value); break;
                            case "MacdPayoff": Settings.Default.MacdPayoff = bool.Parse(value); break;
                            case "PayoffWithEarn": Settings.Default.PayoffWithEarn = bool.Parse(value); break;
                            case "BothOrder": Settings.Default.BothOrder = bool.Parse(value); break;
                            case "CrossAvgLine1": Settings.Default.CrossAvgLine1 = int.Parse(value); break;
                            case "CrossAvgLine2": Settings.Default.CrossAvgLine2 = int.Parse(value); break;
                            case "ReverseOrder": Settings.Default.ReverseOrder = bool.Parse(value); break;
                            case "ReverseOrdCnt1": Settings.Default.ReverseOrdCnt1 = int.Parse(value); break;
                            case "ReverseOrdSel1": Settings.Default.ReverseOrdSel1 = int.Parse(value); break;
                            case "ReverseOrdCnt2": Settings.Default.ReverseOrdCnt2 = int.Parse(value); break;
                            case "ReverseOrdSel2": Settings.Default.ReverseOrdSel2 = int.Parse(value); break;
                            case "GapLiquid": Settings.Default.GapLiquid = bool.Parse(value); break;
                            case "GapValue": Settings.Default.GapValue = int.Parse(value); break;

                            default: break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }

                ReadLossConfig();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool AddElement(XmlDocument document, XmlElement itemList, string name, string value)
        {
            XmlElement itemElement = document.CreateElement("setting");
            itemElement.SetAttribute("Name", name);
            itemElement.SetAttribute("Value", value);
            itemList.AppendChild(itemElement);
            return true;
        }

        public static bool SaveConfig(string filePath)
        {
            try
            {
                if (filePath.Length == 0)
                    return false;

                XmlDocument document = new XmlDocument();
                XmlElement itemListElement = document.CreateElement("Settings");
                document.AppendChild(itemListElement);

                //Except LoginId, IsSaveLoginId, SiteId, SitePassword
                AddElement(document, itemListElement, "LossStop", Settings.Default.LossStop.ToString());
                AddElement(document, itemListElement, "EarnStop", Settings.Default.EarnStop.ToString());
                AddElement(document, itemListElement, "LossPayoff", Settings.Default.LossPayoff.ToString());
                AddElement(document, itemListElement, "EarnPayoff", Settings.Default.EarnPayoff.ToString());
                AddElement(document, itemListElement, "LossPayoffMoney", Settings.Default.LossPayoffMoney.ToString());
                AddElement(document, itemListElement, "EarnPayoffMoney", Settings.Default.EarnPayoffMoney.ToString());
                AddElement(document, itemListElement, "LossStopMoney", Settings.Default.LossStopMoney.ToString());
                AddElement(document, itemListElement, "EarnStopMoney", Settings.Default.EarnStopMoney.ToString());
                AddElement(document, itemListElement, "BettingCandleCount", Settings.Default.BettingCandleCount.ToString());
                //Except IsAutoMode, SiteType
                AddElement(document, itemListElement, "ChartType", Settings.Default.ChartType.ToString());
                AddElement(document, itemListElement, "AvgType", Settings.Default.AvgType.ToString());
                AddElement(document, itemListElement, "CandlePayoff", Settings.Default.CandlePayoff.ToString());
                AddElement(document, itemListElement, "CandlePayoffCount", Settings.Default.CandlePayoffCount.ToString());
                AddElement(document, itemListElement, "BettingType", Settings.Default.BettingType.ToString());
                AddElement(document, itemListElement, "BettingTickCount", Settings.Default.BettingTickCount.ToString());
                AddElement(document, itemListElement, "TickPayoffCount", Settings.Default.TickPayoffCount.ToString());
                AddElement(document, itemListElement, "OrderStop", Settings.Default.OrderStop.ToString());
                AddElement(document, itemListElement, "OrderStopDelay", Settings.Default.OrderStopDelay.ToString());
                AddElement(document, itemListElement, "BettingCandleComplete", Settings.Default.BettingCandleComplete.ToString());
                AddElement(document, itemListElement, "OrderCount", Settings.Default.OrderCount.ToString());
                AddElement(document, itemListElement, "OrderType", Settings.Default.OrderType.ToString());
                // AddElement(document, itemListElement, "OrderMax", Settings.Default.OrderMax.ToString());
                AddElement(document, itemListElement, "Reorder", Settings.Default.Reorder.ToString());
                AddElement(document, itemListElement, "ReturnOption", Settings.Default.ReturnOption.ToString());
                //Except ServerTimeDelay
                AddElement(document, itemListElement, "BoAdjustPer", Settings.Default.BoAdjustPerOn.ToString());
                AddElement(document, itemListElement, "BoLineAdjust", Settings.Default.BoLineAdjust.ToString());
                AddElement(document, itemListElement, "BoAdjustSecOn", Settings.Default.BoAdjustSecOn.ToString());
                AddElement(document, itemListElement, "BoAdjustSec", Settings.Default.BoAdjustSec.ToString());
                AddElement(document, itemListElement, "BettingEnter", Settings.Default.BettingEnter.ToString());
                AddElement(document, itemListElement, "LiquidStop", Settings.Default.LiquidStop.ToString());
                AddElement(document, itemListElement, "ForceEarnPayoff", Settings.Default.ForceEarnPayoff.ToString());
                AddElement(document, itemListElement, "SmartLossPayoff", Settings.Default.SmartLossPayoff.ToString());
                AddElement(document, itemListElement, "BandChart", Settings.Default.BandChart.ToString());
                AddElement(document, itemListElement, "BandVal1", Settings.Default.BandVal1.ToString());
                AddElement(document, itemListElement, "BandVal2", Settings.Default.BandVal2.ToString());
                AddElement(document, itemListElement, "BandVal3", Settings.Default.BandVal3.ToString());
                AddElement(document, itemListElement, "BandChartType1", Settings.Default.BandChartType1.ToString());
                AddElement(document, itemListElement, "BandChartType2", Settings.Default.BandChartType2.ToString());
                AddElement(document, itemListElement, "BandChartType3", Settings.Default.BandChartType3.ToString());
                AddElement(document, itemListElement, "BandChartType4", Settings.Default.BandChartType4.ToString());
                AddElement(document, itemListElement, "SmartEarnTick", Settings.Default.SmartEarnTick.ToString());
                AddElement(document, itemListElement, "SmartLossTick", Settings.Default.SmartLossTick.ToString());
                AddElement(document, itemListElement, "SmartLossUnit", Settings.Default.SmartLossUnit.ToString());
                AddElement(document, itemListElement, "CrossLossPayoff", Settings.Default.CrossLossPayoff.ToString());
                AddElement(document, itemListElement, "CrossLossTick", Settings.Default.CrossLossTick.ToString());
                AddElement(document, itemListElement, "CrossLossUnit", Settings.Default.CrossLossUnit.ToString());
                AddElement(document, itemListElement, "AlarmStop", Settings.Default.AlarmStop.ToString());
                AddElement(document, itemListElement, "InformStop", Settings.Default.InformStop.ToString());
                //Except ChkNoticeDay, NoticeViewTime
                AddElement(document, itemListElement, "EarnPayoffN", Settings.Default.EarnPayoffN.ToString());
                AddElement(document, itemListElement, "EarnPayoffMoneyN", Settings.Default.EarnPayoffMoneyN.ToString());
                AddElement(document, itemListElement, "LossPayoffN", Settings.Default.LossPayoffN.ToString());
                AddElement(document, itemListElement, "LossPayoffMoneyN", Settings.Default.LossPayoffMoneyN.ToString());
                AddElement(document, itemListElement, "OrderSelectOn", Settings.Default.OrderSelectOn.ToString());
                AddElement(document, itemListElement, "OrderSelectType", Settings.Default.OrderSelectType.ToString());
                AddElement(document, itemListElement, "AutoReserveTime", Settings.Default.AutoReserveTime.ToString());
                AddElement(document, itemListElement, "AutoReserveOn", Settings.Default.AutoReserveOn.ToString());
                AddElement(document, itemListElement, "ProfitStop", Settings.Default.ProfitStop.ToString());
                AddElement(document, itemListElement, "ProfitStopRate", Settings.Default.ProfitStopRate.ToString());
                AddElement(document, itemListElement, "Conc1On", Settings.Default.Conc1On.ToString());
                AddElement(document, itemListElement, "Conc1Min", Settings.Default.Conc1Min.ToString());
                AddElement(document, itemListElement, "Conc1Cnt", Settings.Default.Conc1Cnt.ToString());
                AddElement(document, itemListElement, "Conc2On", Settings.Default.Conc2On.ToString());
                AddElement(document, itemListElement, "Conc2Chart", Settings.Default.Conc2Chart.ToString());
                AddElement(document, itemListElement, "Conc2Candle", Settings.Default.Conc2Candle.ToString());
                AddElement(document, itemListElement, "Conc2Cnt", Settings.Default.Conc2Cnt.ToString());
                AddElement(document, itemListElement, "AdxOn", Settings.Default.AdxOn.ToString());
                AddElement(document, itemListElement, "AdxCnt", Settings.Default.AdxCnt.ToString());
                AddElement(document, itemListElement, "CciOn", Settings.Default.CciOn.ToString());
                AddElement(document, itemListElement, "CciRange1", Settings.Default.CciRange1.ToString());
                AddElement(document, itemListElement, "CciRange11", Settings.Default.CciRange11.ToString());
                AddElement(document, itemListElement, "CciRange2", Settings.Default.CciRange2.ToString());
                AddElement(document, itemListElement, "CciRange21", Settings.Default.CciRange21.ToString());
                AddElement(document, itemListElement, "CciSide1", Settings.Default.CciSide1.ToString());
                AddElement(document, itemListElement, "CciSide2", Settings.Default.CciSide2.ToString());
                AddElement(document, itemListElement, "SmartRangePayoff", Settings.Default.SmartRangePayoff.ToString());
                AddElement(document, itemListElement, "CrossRangePayoff", Settings.Default.CrossRangePayoff.ToString());
                AddElement(document, itemListElement, "RsiOn", Settings.Default.RsiOn.ToString());
                AddElement(document, itemListElement, "RsiRange1", Settings.Default.RsiRange1.ToString());
                AddElement(document, itemListElement, "RsiRange2", Settings.Default.RsiRange2.ToString());
                AddElement(document, itemListElement, "RsiSide1", Settings.Default.RsiSide1.ToString());
                AddElement(document, itemListElement, "RsiSide2", Settings.Default.RsiSide2.ToString());
                AddElement(document, itemListElement, "AvgsOn", Settings.Default.AvgsOn.ToString());
                AddElement(document, itemListElement, "AvgsCandle", Settings.Default.AvgsCandle.ToString());
                AddElement(document, itemListElement, "AvgsSide1", Settings.Default.AvgsSide1.ToString());
                AddElement(document, itemListElement, "AvgsSide2", Settings.Default.AvgsSide2.ToString());
                AddElement(document, itemListElement, "LossRangePayoff", Settings.Default.LossRangePayoff.ToString());
                AddElement(document, itemListElement, "CciPayoff", Settings.Default.CciPayoff.ToString());
                AddElement(document, itemListElement, "CciPayoffValue1", Settings.Default.CciPayoffValue1.ToString());
                AddElement(document, itemListElement, "CciPayoffValue2", Settings.Default.CciPayoffValue2.ToString());
                AddElement(document, itemListElement, "CciPayoffValue3", Settings.Default.CciPayoffValue3.ToString());
                AddElement(document, itemListElement, "CciRangePayoff", Settings.Default.CciRangePayoff.ToString());
                AddElement(document, itemListElement, "BoOrdType", Settings.Default.BoOrdType.ToString());
                AddElement(document, itemListElement, "PayoffLossRange", ToSerialString(Settings.Default.PayoffLossRange));
                AddElement(document, itemListElement, "SmartLossRange", ToSerialString(Settings.Default.SmartLossRange));
                AddElement(document, itemListElement, "CrossLossRange", ToSerialString(Settings.Default.CrossLossRange));
                AddElement(document, itemListElement, "CciLossRange", ToSerialString(Settings.Default.CciLossRange));
                AddElement(document, itemListElement, "OrdCnts", Settings.Default.OrdCnts);
                AddElement(document, itemListElement, "EarnTicks", Settings.Default.EarnTicks);
                AddElement(document, itemListElement, "LossTicks", Settings.Default.LossTicks);
                AddElement(document, itemListElement, "SyncChart", Settings.Default.SyncChart.ToString());
                //AddElement(document, itemListElement, "SignalSiteOn", Settings.Default.SignalSiteOn.ToString());
                AddElement(document, itemListElement, "BollPayoff", Settings.Default.BollPayoff.ToString());
                AddElement(document, itemListElement, "BollPayoffDown", Settings.Default.BollPayoffDown.ToString());
                AddElement(document, itemListElement, "BollPayoffUp", Settings.Default.BollPayoffUp.ToString());
                AddElement(document, itemListElement, "MacdPayoff", Settings.Default.MacdPayoff.ToString());
                AddElement(document, itemListElement, "PayoffWithEarn", Settings.Default.PayoffWithEarn.ToString());
                AddElement(document, itemListElement, "BothOrder", Settings.Default.BothOrder.ToString());
                AddElement(document, itemListElement, "CrossAvgLine1", Settings.Default.CrossAvgLine1.ToString());
                AddElement(document, itemListElement, "CrossAvgLine2", Settings.Default.CrossAvgLine2.ToString());
                AddElement(document, itemListElement, "ReverseOrder", Settings.Default.ReverseOrder.ToString());
                AddElement(document, itemListElement, "ReverseOrdCnt1", Settings.Default.ReverseOrdCnt1.ToString());
                AddElement(document, itemListElement, "ReverseOrdSel1", Settings.Default.ReverseOrdSel1.ToString());
                AddElement(document, itemListElement, "ReverseOrdCnt2", Settings.Default.ReverseOrdCnt2.ToString());
                AddElement(document, itemListElement, "ReverseOrdSel2", Settings.Default.ReverseOrdSel2.ToString());
                AddElement(document, itemListElement, "GapLiquid", Settings.Default.GapLiquid.ToString());
                AddElement(document, itemListElement, "GapValue", Settings.Default.GapValue.ToString());

                document.Save(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string ToSerialString(StringCollection sCollection)
        {
            string result = "";
            foreach (string sValue in sCollection)
            {
                result += sValue + ";";
            }
            return result;
        }
        public static StringCollection ParseSerialString(string sValue)
        {
            string[] infos = sValue.Split(';');
            StringCollection sCollection = new StringCollection();
            foreach (string info in infos)
            {
                if (info.Length > 0)
                    sCollection.Add(info);
            }
            return sCollection;
        }

        public static bool ReadLossConfig()
        {
            try
            {
                lock (_lockObj)
                {
                    PayoffLossInfo lossInfo = null;
                    //PayoffLossConfs
                    PayoffLossConfs.Clear();
                    int iStage = 0;
                    bool bReset = false;
                    foreach (string lossRange in Settings.Default.PayoffLossRange)
                    {
                        string[] infos = lossRange.Split('#');
                        if (infos.Length < 3)
                            continue;
                        lossInfo = new PayoffLossInfo
                        {
                            Stage = ++iStage,
                            StageName = iStage.ToString() + "단계",
                            Amount = int.Parse(infos[1]),
                            AmountUnit = "USD",
                            Rate = int.Parse(infos[2]),
                            RateUnit = "틱",
                            Enabled = int.Parse(infos[0]),
                            Param = "0",
                            Param2 = "0",
                            ActionDelete = "삭제"
                        };
                        if (infos.Length > 3 && infos[3].Length > 0)
                            lossInfo.Param = infos[3];
                        if (infos.Length > 4 && infos[4].Length > 0)
                            lossInfo.Param2 = infos[4];

                        if (infos.Length < 5)
                            bReset = true;
                        PayoffLossConfs.Add(lossInfo);
                    }
                    if (bReset)
                    {
                        Settings.Default.PayoffLossRange.Clear();
                        foreach (PayoffLossInfo lossConf in PayoffLossConfs)
                        {
                            Settings.Default.PayoffLossRange.Add(lossConf.Enabled.ToString() + "#" + lossConf.Amount.ToString() + "#" + lossConf.Rate.ToString() + "#" + lossConf.Param + "#" + lossConf.Param2);
                        }
                    }

                    //SmartLossConfs
                    SmartLossConfs.Clear();
                    iStage = 0;
                    bReset = false;
                    foreach (string lossRange in Settings.Default.SmartLossRange)
                    {
                        string[] infos = lossRange.Split('#');
                        if (infos.Length < 3)
                            continue;
                        lossInfo = new PayoffLossInfo
                        {
                            Stage = ++iStage,
                            StageName = iStage.ToString() + "단계",
                            Amount = int.Parse(infos[1]),
                            AmountUnit = "USD",
                            Rate = int.Parse(infos[2]),
                            RateUnit = "%",
                            Enabled = int.Parse(infos[0]),
                            Param = "50",
                            Param2 = "30",
                            ActionDelete = "삭제"
                        };

                        if (infos.Length > 3 && infos[3].Length > 0)
                            lossInfo.Param = infos[3];
                        if (infos.Length > 4 && infos[4].Length > 0)
                            lossInfo.Param2 = infos[4];

                        if (infos.Length < 5)
                            bReset = true;
                        SmartLossConfs.Add(lossInfo);
                    }
                    if (bReset)
                    {
                        Settings.Default.SmartLossRange.Clear();
                        foreach(PayoffLossInfo lossConf in SmartLossConfs)
                        {
                            Settings.Default.SmartLossRange.Add(lossConf.Enabled.ToString() + "#" + lossConf.Amount.ToString() + "#" + lossConf.Rate.ToString() + "#" + lossConf.Param + "#" + lossConf.Param2);
                        }
                    }

                    //CrossLossConfs
                    CrossLossConfs.Clear();
                    iStage = 0;
                    bReset = false;
                    foreach (string lossRange in Settings.Default.CrossLossRange)
                    {
                        string[] infos = lossRange.Split('#');
                        if (infos.Length < 3)
                            continue;
                        lossInfo = new PayoffLossInfo
                        {
                            Stage = ++iStage,
                            StageName = iStage.ToString() + "단계",
                            Amount = int.Parse(infos[1]),
                            AmountUnit = "USD",
                            Rate = int.Parse(infos[2]),
                            RateUnit = "%",
                            Enabled = int.Parse(infos[0]),
                            Param = "50",
                            Param2 = "30",
                            ActionDelete = "삭제"
                        };
                        if (infos.Length > 3 && infos[3].Length > 0)
                            lossInfo.Param = infos[3];
                        if (infos.Length > 4 && infos[4].Length > 0)
                            lossInfo.Param2 = infos[4];

                        if (infos.Length < 5)
                            bReset = true;
                        CrossLossConfs.Add(lossInfo);
                    }
                    if (bReset)
                    {
                        Settings.Default.CrossLossRange.Clear();
                        foreach (PayoffLossInfo lossConf in CrossLossConfs)
                        {
                            Settings.Default.CrossLossRange.Add(lossConf.Enabled.ToString() + "#" + lossConf.Amount.ToString() + "#" + lossConf.Rate.ToString() + "#" + lossConf.Param + "#" + lossConf.Param2);
                        }
                    }

                    //CrossLossConfs
                    CciLossConfs.Clear();
                    iStage = 0;
                    bReset = false;
                    foreach (string lossRange in Settings.Default.CciLossRange)
                    {
                        string[] infos = lossRange.Split('#');
                        if (infos.Length < 3)
                            continue;
                        lossInfo = new PayoffLossInfo
                        {
                            Stage = ++iStage,
                            StageName = iStage.ToString() + "단계",
                            Amount = int.Parse(infos[1]),
                            AmountUnit = "이상",
                            Rate = int.Parse(infos[2]),
                            RateUnit = "%",
                            Enabled = int.Parse(infos[0]),
                            Param = "50",
                            Param2 = "30",
                            ActionDelete = "삭제"
                        };
                        if (infos.Length > 3 && infos[3].Length > 0)
                            lossInfo.Param = infos[3];
                        if (infos.Length > 4 && infos[4].Length > 0)
                            lossInfo.Param2 = infos[4];

                        if (infos.Length < 5)
                            bReset = true;
                        CciLossConfs.Add(lossInfo);
                    }
                    if (bReset)
                    {
                        Settings.Default.CciLossRange.Clear();
                        foreach (PayoffLossInfo lossConf in CciLossConfs)
                        {
                            Settings.Default.CciLossRange.Add(lossConf.Enabled.ToString() + "#" + lossConf.Amount.ToString() + "#" + lossConf.Rate.ToString() + "#" + lossConf.Param + "#" + lossConf.Param2);
                        }
                    }

                    //SyncMemInfos
                    if (Settings.Default.SyncMembers == null)
                    {
                        Settings.Default.SyncMembers = new StringCollection();
                    } else
                    {
                        SyncMemInfos.Clear();
                        MemberInfo memInfo = null;
                        foreach (string syncMember in Settings.Default.SyncMembers)
                        {
                            if (syncMember.Length < 1)
                                continue;
                            memInfo = new MemberInfo
                            {
                                Id = syncMember,
                                Delete = "삭제"
                            };
                            SyncMemInfos.Add(memInfo);
                        }
                    }
                    
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static void SetNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            // Console.WriteLine("Interface information for {0}.{1}     ", computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                // Console.WriteLine("  No network interfaces found.");
                return;
            }

            string physicalAddr = "";
            // Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();
                //Console.WriteLine();
                //Console.WriteLine(adapter.Description);
                //Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                //Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                //Console.Write("  Physical address ........................ : ");
                PhysicalAddress address = adapter.GetPhysicalAddress();
                byte[] bytes = address.GetAddressBytes();
                physicalAddr = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    physicalAddr += bytes[i].ToString("X2");
                    // Console.Write("{0}", bytes[i].ToString("X2"));
                    // Insert a hyphen after each byte, unless we're at the end of the address.
                    if (i != bytes.Length - 1)
                    {
                        physicalAddr += "-";
                        // Console.Write("-");
                    }
                }
                if(physicalAddr.Length > 0)
                {
                    _PhysicalAddr = physicalAddr;
                    break;
                }

                // Console.WriteLine();
            }
        }

    }


}
