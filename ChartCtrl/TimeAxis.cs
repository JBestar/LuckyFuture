using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ChartCtrl
{
    class TimeAxis
    {
        public TimeAxis()
        {
            //m_penRuler.DashPattern = m_dashValues;
            m_penGray.DashPattern = m_grayValues;
            
        }
        
        Font m_fontStr = new Font(new FontFamily("돋움"), 10, FontStyle.Bold);
        //Pen m_penRuler = new Pen(Color.FromArgb(250, 233, 233, 233), 1);
        Pen m_penGray = new Pen(Color.FromArgb(200, 50, 50, 50), 1);
        
        float[] m_dashValues = { 5, 2, 15, 4 };
        float[] m_grayValues = { 6, 6 };
        
        public void Draw(Graphics g)
        {
            float fY = CtrlProperty._ClientH - CtrlProperty._nTimeAxisBand;

            g.FillRectangle(CtrlProperty._RulerPaneBrush, new RectangleF(0, fY, CtrlProperty._ClientW, CtrlProperty._nTimeAxisBand));

 
        }

        public void DrawCursorValue(Graphics g, float fCursorX, long lTmStart)
        {

            
            float fX = fCursorX;
            float fY = CtrlProperty._ClientH - CtrlProperty._nTimeAxisBand;


            string strTime = string.Format(CtrlProperty._tDtPattern, CtrlProperty.GetTime(lTmStart));
            

            g.FillRectangle(Brushes.Black, new RectangleF(fX-45, fY, 100, CtrlProperty._nTimeAxisBand));
            g.DrawLine(m_penGray, fX, 0, fX, fY);
            //g.DrawLine(Pens.White, fX, fY, fX, fY + 5);
            g.DrawString(strTime, m_fontStr, Brushes.White, new PointF(fX - 45, fY+2));


        }

    }
}
