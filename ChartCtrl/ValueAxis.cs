using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChartCtrl
{
    class ValueAxis
    {
        public ValueAxis()
        {
            //m_penRuler.DashPattern = m_dashValues;
            m_penBlue.DashPattern = m_blueValues;
            m_penGray.DashPattern = m_grayValues;
        }

        Font m_fontStr = new Font(new FontFamily("돋움"), 8, FontStyle.Bold);
        //Pen m_penRuler = new Pen(Color.FromArgb(250, 255, 255, 255), 1);
        Pen m_penBlue = new Pen(Color.DarkGreen, 1);
        Pen m_penGray = new Pen(Color.FromArgb(200, 50, 50, 50), 1);

        float[] m_dashValues = { 5, 2, 15, 4 };
        float[] m_blueValues = { 4, 3 };
        float[] m_grayValues = { 6, 6};
        public void Draw(Graphics g)
        {

            float fValueOrigin = CtrlProperty._fValueOrigin;
            float fValueMax = CtrlProperty._fValueMax;
            float nValueGap = CtrlProperty._nValueUnit ;

            int nClientW = CtrlProperty._ClientW;
            int nClientH = CtrlProperty._ClientH;

            float fValueHeigt = fValueMax - fValueOrigin;
            if (fValueHeigt <= 0) return;
            float fX = nClientW, fY;
            string strValue = "";

            g.FillRectangle(CtrlProperty._DrawPaneBrush, new RectangleF(fX, 0, CtrlProperty._nValueAxisBand, nClientH));
            if(nClientH > 5)
                g.DrawLine(Pens.Black, fX, 0, fX, nClientH-3);

            float fStartValue = (float)Math.Ceiling((fValueOrigin + nValueGap) / 100);
            fStartValue *= 100;

            if ((fValueMax - fStartValue) / nValueGap > 1000)
                return;

            for (float fVal = fStartValue ; fVal < fValueMax ; fVal += nValueGap)
            {
                fY = nClientH - (fVal- CtrlProperty._fValueOrigin) * nClientH / fValueHeigt;
                strValue = string.Format(CtrlProperty._tValueFormat, fVal / CtrlProperty._nValueRate);
                if (CtrlProperty._bGridRuler)
                    g.DrawLine(CtrlProperty._RulerPen, 0, fY, fX, fY);

                g.DrawLine(Pens.Black, fX, fY, fX + 5, fY);

                g.DrawString(strValue, m_fontStr, Brushes.Black, new PointF(fX + 5, fY - 5));
            }

        }

        public void DrawEndValue(Graphics g, float fEndVal)
        {
            int nClientW = CtrlProperty._ClientW;
            int nClientH = CtrlProperty._ClientH;

            float fValueHeigth = CtrlProperty._fValueMax - CtrlProperty._fValueOrigin;

            if (fValueHeigth <= 0)
                return;

            float fX = nClientW ;
            float fY = nClientH - (fEndVal - CtrlProperty._fValueOrigin) * nClientH / fValueHeigth;

            string strValue = string.Format(CtrlProperty._tValueFormat, fEndVal / CtrlProperty._nValueRate);

            g.FillRectangle(Brushes.Red, new RectangleF(fX, fY-10, CtrlProperty._nValueAxisBand, 20));
            //g.DrawLine(m_penBlue, 0, fY, fX, fY);
            g.DrawLine(Pens.White, fX, fY, fX + 5, fY);
            g.DrawString(strValue, m_fontStr, Brushes.White, new PointF(fX + 5, fY - 5));

        }

        public void DrawCursorValue(Graphics g, float fCursorY)
        {
            int nClientW = CtrlProperty._ClientW;
            int nClientH = CtrlProperty._ClientH;

            float fValueHeight = CtrlProperty._fValueMax - CtrlProperty._fValueOrigin;
            float fX = nClientW ;
            float fY = fCursorY;
            float fVal = CtrlProperty._fValueMax - fCursorY * fValueHeight / nClientH;
            string strValue = string.Format(CtrlProperty._tValueFormat, fVal / CtrlProperty._nValueRate);

            g.FillRectangle(Brushes.Black, new RectangleF(fX, fY - 10, CtrlProperty._nValueAxisBand, 20));
            g.DrawLine(m_penGray, 0, fY, fX, fY);
            g.DrawLine(Pens.White, fX, fY, fX + 5, fY);
            g.DrawString(strValue, m_fontStr, Brushes.White, new PointF(fX + 5, fY - 5));

        }


    }
}
