using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using LuckyFuture.Site;
using LuckyFuture.Models.ValueObjects;
using LuckyFutureLib.Include;
using LuckyFuture.Properties;
using LuckyFuture.Logic;
using System.Diagnostics;

namespace LuckyFuture.UI
{
	public partial class FrmCurrent2 : Form
	{
        public static readonly FrmCurrent2 Default = new FrmCurrent2();
        public FrmCurrent2()
		{

            InitializeComponent();
			CenterToScreen();
			InitializeComponentEx();
        }

		/// <summary>
		/// Initialize components additionally
		/// </summary>
		void InitializeComponentEx()
		{
			
			// double buffered
			this.dgvCurrentInfo.DoubleBuffered(true);
            
            this.CurrentInfo = new List<CurrentInfo>();
        }

        private FutureSite CurrentSite { get => LogicAuto.Default.CurrentSite; }

		public List<CurrentInfo> CurrentInfo
		{
			get =>(List<CurrentInfo>)this.bsCurrentInfo.DataSource;
			set => this.bsCurrentInfo.DataSource = value;
		}

		
		public void InitListView(bool bAll = true)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    InitListView(bAll);
                }));
            }
            else
            {
                CurrentInfo = null;
			}

        }
        private int _tickCurrent = 0;

        public void UpdateCurrentInfo()
		{
			if (!this.Visible)
				return;

			FutureSite currentSite = CurrentSite;

			if (currentSite != null && currentSite.CurrentList != null)
			{
                if(Math.Abs(Environment.TickCount - _tickCurrent) > 100)
                {
                    _tickCurrent = Environment.TickCount;
                    if (this.dgvCurrentInfo.RowCount >= 0)
                    {
                        int firstDisplayedScrollingRowIndex = this.dgvCurrentInfo.FirstDisplayedScrollingRowIndex;
                        if (currentSite.CurrentList.Count >= 0 && firstDisplayedScrollingRowIndex <= 0)
                        {

                            this.CurrentInfo = null;
                            this.CurrentInfo = currentSite.CurrentList;

                        }
                        if (firstDisplayedScrollingRowIndex < this.dgvCurrentInfo.RowCount && firstDisplayedScrollingRowIndex >= 0)
                        {
                            this.dgvCurrentInfo.FirstDisplayedScrollingRowIndex = ((firstDisplayedScrollingRowIndex >= 0) ? firstDisplayedScrollingRowIndex : 0);
                        }
                    }
                }
				
			}
        }
		
		
		// Event handlers
		private void FrmMain_Load(object sender, EventArgs e)
		{

		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
            Hide();
            e.Cancel = true;
		}

        private void dgvCurrentInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
                FutureSite currentSite = CurrentSite;
				
				if (currentSite == null || currentSite.CurrentList.Count < 1)
				{
					return;
				}
                if (this.CurrentInfo != null && this.CurrentInfo.Count > e.RowIndex)
				{
					if (e.ColumnIndex == 1)
					{
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.SELL)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.BUY)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
							return;
						}
					}
					else if (e.ColumnIndex == 2)
					{
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.SELL)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (this.CurrentInfo[e.RowIndex].TradeType == TRADETYPE.BUY)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						}
					}
				}
            }
            catch (Exception) { }
		}

		
		private void dgvCurrentInfo_SelectionChanged(object sender, EventArgs e)
		{
            try { 
				dgvCurrentInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		
		private void dgvCurrentInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
			// Trace.TraceInformation("<FrmCurrent> dgvCurrentInfo_DataError");
			e.Cancel = true;
			e.ThrowException = false;
		}

	}
}
