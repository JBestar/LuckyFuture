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
    class RItemGroup
    {
        public RItemGroup()
        {
            //m_penRuler.DashPattern = m_dashValues;
            m_penGray.DashPattern = m_grayValues;

            listRItem = CtrlProperty._RItemList;

            SetAvgLinePen();
        }
        /// Declare Member Variables
        public List<RItem> listRItem = null;
        Pen m_PenRed = new Pen(Color.Red);
        Pen m_PenBlue = new Pen(Color.Blue);

        Font m_fontStr = new Font(new FontFamily("돋움"), 10, FontStyle.Bold);
        //Pen m_penRuler = new Pen(Color.FromArgb(250, 233, 233, 233), 1);
        Pen m_penGray = new Pen(Color.FromArgb(200, 50, 50, 50), 1);

        // float[] m_dashValues = { 5, 2, 15, 4 };
        float[] m_grayValues = { 6, 6 };
        Pen[] m_arrAvgPen = new Pen[5] ;
        Pen[] m_arrBoPen = new Pen[2];

        public int Count()
        {
            return listRItem.Count;
        }

        public void Clear()
        {
            listRItem.Clear();
        }
        public void SetAvgLinePen()
        {
            int nLineWidth = Settings.Default.AvgLineWidth > 0 ? Settings.Default.AvgLineWidth * 5 : 5;
            m_arrAvgPen = new Pen[] {new Pen(Settings.Default.AvgLineColor1, nLineWidth), new Pen(Settings.Default.AvgLineColor2, nLineWidth),
                new Pen(Settings.Default.AvgLineColor3, nLineWidth), new Pen(Settings.Default.AvgLineColor4, nLineWidth), 
                new Pen(Settings.Default.AvgLineColor5, nLineWidth), new Pen(Settings.Default.AvgLineColor6, nLineWidth)};
            m_arrBoPen = new Pen[] { new Pen(Settings.Default.BoLineColor1, nLineWidth), new Pen(Settings.Default.BoLineColor2, nLineWidth) };

        }
        public void Add(RItem item)
        {
            listRItem.Add(item);
        }

        public RItem Last()
        {
            if(listRItem.Count > 0)
                return listRItem.Last();
            return null;
        }
        public RItem GetValidTm(float ftmVal)
        {
            if (listRItem.Count < 1)
                return null;

            RItem itemSel = null;
            for (int i = 0; i < listRItem.Count; i++)
            {
                if (listRItem[i].IsValidTm(ftmVal))
                {
                    itemSel = listRItem[i];
                    break;
                }
            }
            return itemSel;
        }

        public RItem GetValidItem(float ftmVal, float fVal)
        {
            if (listRItem.Count < 1)
                return null;

            RItem itemSel = null;
            for(int i=0; i<listRItem.Count; i++)
            {
                if(listRItem[i].IsValid(ftmVal, fVal))
                {
                    itemSel = listRItem[i];
                    break;
                }
            }
            return itemSel;
        }

        /// Declare Member Functions
        public void Draw(Graphics g)
        {
            try { 
                if (Settings.Default.ChartStyle == 0)           //캔들차트
                    DrawChartCandle(g);
                else if (Settings.Default.ChartStyle == 1)      //라인차트
                    DrawChartLine(g);
                if (Settings.Default.AvgLine)
                    DrawAvgLine(g);
                if (Settings.Default.BoLine)
                    DrawBoLine(g);
            }
            catch (Exception) { }
        }

        public void DrawChartCandle(Graphics g)
        {
            // Trace.TraceInformation("<RItemGroup> DrawChartCandle = " + listRItem.Count);
            //캔들차트
            for (int i = 0; i < listRItem.Count; i++)
            {
                listRItem[i].Draw(g);
            }

        }

        public void DrawChartLine(Graphics g)
        {
            if(listRItem.Count > 1)
            {
                RItem itemPrev = listRItem.First();
                RItem itemNext = null;
                //캔들차트
                float fTmPrevX, fTmNextX;
                for (int i = 1; i < listRItem.Count; i++)
                {
                    itemNext = listRItem[i];

                    fTmPrevX = itemPrev.Index * CtrlProperty._nItemWidth + CtrlProperty._nItemWidth / 2;
                    fTmNextX = itemNext.Index * CtrlProperty._nItemWidth + CtrlProperty._nItemWidth / 2;

                    if (fTmPrevX >= CtrlProperty._nTimeOrigin - CtrlProperty._nItemWidth && fTmNextX <= CtrlProperty._nTimeMax )
                    {
                        g.DrawLine(m_PenRed, fTmPrevX, itemPrev.Start, fTmNextX, itemNext.Start);   //Start-Value Line
                        g.DrawLine(m_PenBlue, fTmPrevX, itemPrev.End, fTmNextX, itemNext.End);      //End-Value Line
                    }
                    
                    itemPrev = itemNext; 
                }
            }
            

        }

        public void DrawAvgLine(Graphics g)
        {
            if (listRItem.Count < 1)
                return;
            try { 
                RItem itemPrev = listRItem.First();
                RItem itemNext = null;
                //캔들차트
                float fTmPrevX, fTmNextX;
                int i, j;
                for (i = 1; i < listRItem.Count; i++)
                {
                    itemNext = listRItem[i];

                    fTmPrevX = itemPrev.Index * CtrlProperty._nItemWidth + CtrlProperty._nItemWidth / 2;
                    fTmNextX = fTmPrevX + CtrlProperty._nItemWidth;//itemNext.Index * CtrlProperty._nItemWidth + CtrlProperty._nItemWidth / 2;
                    if (fTmPrevX >= CtrlProperty._nTimeOrigin - CtrlProperty._nItemWidth && fTmNextX <= CtrlProperty._nTimeMax )
                    {
                        for(j=0; j<CtrlProperty._arrAvgItems.Length; j++)
                        {
                            if (j == 0 && !Settings.Default.Avg5On)
                                continue;
                            if (j == 1 && !Settings.Default.Avg10On)
                                continue;
                            if (j == 2 && !Settings.Default.Avg20On)
                                continue;
                            if (j == 3 && !Settings.Default.Avg60On)
                                continue;
                            if (j == 4 && !Settings.Default.Avg120On)
                                continue;
                            if (j == 5 && !Settings.Default.Avg200On)
                                continue;

                            if (itemPrev.Avgs[j] > 0 && itemNext.Avgs[j] > 0)
                            {
                                g.DrawLine(m_arrAvgPen[j], fTmPrevX, itemPrev.Avgs[j], fTmNextX, itemNext.Avgs[j]);
                            }                        
                        }                    
                    }

                    itemPrev = itemNext;

                }
            }
            catch (Exception) { }

        }

        public void DrawBoLine(Graphics g)
        {
            if (listRItem.Count < 1)
                return;

            RItem itemPrev = listRItem.First();
            RItem itemNext = null;
            //캔들차트
            float fTmPrevX, fTmNextX;
            List<PointF> pt1List = new List<PointF>();
            List<PointF> pt2List = new List<PointF>();
            int i;

            int wTri = CtrlProperty._nItemWidth / 2 - 5;
            if (wTri < 10) wTri = 10;
            int hTri = (int)(20 * (CtrlProperty._fValueMax - CtrlProperty._fValueOrigin) / CtrlProperty._ClientH);
            int hGap = hTri / 4;
            int y = 0;


            for (i = 1; i < listRItem.Count; i++)
            {
                itemNext = listRItem[i];

                fTmPrevX = itemPrev.Index * CtrlProperty._nItemWidth + CtrlProperty._nItemWidth / 2;
                fTmNextX = fTmPrevX + CtrlProperty._nItemWidth ;
                if (fTmPrevX >= CtrlProperty._nTimeOrigin - CtrlProperty._nItemWidth && fTmNextX <= CtrlProperty._nTimeMax)
                {

                    if(itemNext.Est_Type == itemPrev.Est_Type)
                    {
                        if (itemNext.Est_Type == RESULTSTATE.BUY)
                        {
                            if (pt1List.Count == 0)
                                pt1List.Add(new PointF(fTmPrevX, itemPrev.Bos[0]));
                            pt1List.Add(new PointF(fTmNextX, itemNext.Bos[0]));

                            g.DrawLine(m_arrBoPen[1], fTmPrevX, itemPrev.Bos[1], fTmNextX, itemNext.Bos[1]);

                        }
                        else if (itemNext.Est_Type == RESULTSTATE.SELL)
                        {
                            if (pt2List.Count == 0)
                                pt2List.Add(new PointF(fTmPrevX, itemPrev.Bos[1]));
                            pt2List.Add(new PointF(fTmNextX, itemNext.Bos[1]));
                            g.DrawLine(m_arrBoPen[0], fTmPrevX, itemPrev.Bos[0], fTmNextX, itemNext.Bos[0]);
                        }
                    } else
                    {
                        if (pt1List.Count > 1 && itemPrev.Est_Type == RESULTSTATE.BUY)
                        {
                            pt1List.Add(new PointF(fTmNextX, itemNext.Bos[0]));
                            g.DrawLine(m_arrBoPen[1], fTmPrevX, itemPrev.Bos[1], fTmNextX, itemNext.Bos[1]);

                        }
                        else if (pt2List.Count > 1 && itemPrev.Est_Type == RESULTSTATE.SELL)
                        {
                            pt2List.Add(new PointF(fTmNextX, itemNext.Bos[1]));
                            g.DrawLine(m_arrBoPen[0], fTmPrevX, itemPrev.Bos[0], fTmNextX, itemNext.Bos[0]);
                        } else
                        {
                            g.DrawLine(m_arrBoPen[0], fTmPrevX, itemPrev.Bos[0], fTmNextX, itemNext.Bos[0]);
                            g.DrawLine(m_arrBoPen[1], fTmPrevX, itemPrev.Bos[1], fTmNextX, itemNext.Bos[1]);
                        }

                        if (pt1List.Count > 0)
                        {
                            g.DrawCurve(m_arrBoPen[0], pt1List.ToArray());
                            pt1List.Clear();
                        }
                        if (pt2List.Count > 0)
                        {
                            g.DrawCurve(m_arrBoPen[1], pt2List.ToArray());
                            pt2List.Clear();
                        }
                    }
                        
                    if (itemPrev.Est_Type != RESULTSTATE.IGNORE && itemNext.Est_Type != RESULTSTATE.IGNORE && itemPrev.Est_Type != itemNext.Est_Type)
                    {
                        if (itemNext.Est_Type == RESULTSTATE.BUY)
                        {
                            y = (int)itemNext.Min;
                            
                            g.DrawLine(Pens.Red, fTmNextX, y - hGap * 2, fTmNextX, y - hTri);
                            g.DrawLine(Pens.Red, fTmNextX, y - hGap * 2, fTmNextX-wTri/2, y - hGap * 3);
                            g.DrawLine(Pens.Red, fTmNextX, y - hGap * 2, fTmNextX+wTri/2, y - hGap * 3);

                        }
                        else
                        {
                            y = (int)itemNext.Max;

                            g.DrawLine(Pens.Blue, fTmNextX, y + hGap * 2, fTmNextX, y + hTri);
                            g.DrawLine(Pens.Blue, fTmNextX, y + hGap * 2, fTmNextX - wTri/2, y + hGap * 3);
                            g.DrawLine(Pens.Blue, fTmNextX, y + hGap * 2, fTmNextX + wTri/2, y + hGap * 3);

                        }
                    }

                }

                itemPrev = itemNext;

            }
            if (pt1List.Count > 0)
            {
                g.DrawCurve(m_arrBoPen[0], pt1List.ToArray());
                pt1List.Clear();
            }
            if (pt2List.Count > 0)
            {
                g.DrawCurve(m_arrBoPen[1], pt2List.ToArray());
                pt2List.Clear();
            }


        }

        public void DrawAxis(Graphics g)
        {
            float fY = CtrlProperty._ClientH - CtrlProperty._nTimeAxisBand;
            g.FillRectangle(CtrlProperty._DrawPaneBrush, new RectangleF(0, fY, CtrlProperty._ClientW + CtrlProperty._nValueAxisBand, CtrlProperty._nTimeAxisBand));
            g.DrawLine(Pens.Black, 0, fY, CtrlProperty._ClientW + CtrlProperty._nValueAxisBand, fY);

            int nGap = CtrlProperty._nItemView / 5;
            if (nGap < 1) nGap = 1;

            for (int i = listRItem.Count-1; i >= 0; i -= nGap)
            {
                DrawItemAxis(g, listRItem[i]);
            }

        }

        public void DrawItemAxis(Graphics g, RItem item)
        {
            int itemX = item.Index * CtrlProperty._nItemWidth;

            if (itemX >= CtrlProperty._nTimeOrigin && itemX <= CtrlProperty._nTimeMax)
            {

                float fX = CtrlProperty.GetClientX(itemX);
                float fY = CtrlProperty._ClientH - CtrlProperty._nTimeAxisBand;
                DateTime dtStart = CtrlProperty.GetTime(item.TmStart);
                string strTime = string.Format(CtrlProperty._tTimePattern, dtStart);
                if (CtrlProperty._bGridRuler)
                    g.DrawLine(CtrlProperty._RulerPen, fX, 0, fX, fY);

                g.DrawLine(Pens.DarkGray, fX, fY, fX, fY + CtrlProperty._nTimeAxisBand);

                g.DrawString(strTime, m_fontStr, Brushes.Black, new PointF(fX, fY+2));

            }

        }



    }
}
