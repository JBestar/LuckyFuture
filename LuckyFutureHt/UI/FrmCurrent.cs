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
using ChartCtrl;

namespace LuckyFuture.UI
{
	public partial class FrmCurrent : Form
	{
        public static readonly FrmCurrent Default = new FrmCurrent();
        // public event EventHandler<ChartEventArgs> ChartNoticeEvent;
        public FrmCurrent()
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
            this.dgvQuoteInfo.DoubleBuffered(true);
            this.dgvTotalQuoteInfo.DoubleBuffered(true);
            this.dgvItemPriceInfo.DoubleBuffered(true);

            this.QuoteInfo = new List<QuoteInfo>();
			this.CurrentInfo = new List<CurrentInfo>();
			this.ItemPriceInfo = new List<ItemPriceInfo>();
			this.TotalQuoteInfo = new List<TotalQuoteInfo>();


        }

		private readonly object _objLock = new object();
        private FutureSite CurrentSite { get => LogicAuto.Default.CurrentSite; }
        private FutureSite SignalSite { get => LogicAuto.Default.SignalSite; }

        // 호가고정
        public bool IsFixed => this.chkFixed.Checked;
		// 현재 오브젝트
		public List<CurrentInfo> CurrentInfo
		{
			get =>(List<CurrentInfo>)this.bsCurrentInfo.DataSource;
			set => this.bsCurrentInfo.DataSource = value;
		}

		// item price info
		public List<ItemPriceInfo> ItemPriceInfo
		{
			get => (List<ItemPriceInfo>)this.bsItemPriceInfo.DataSource;
			set => this.bsItemPriceInfo.DataSource = value;
		}

		// quote info
		public List<QuoteInfo> QuoteInfo
		{
			get => (List<QuoteInfo>)this.bsQuoteInfo.DataSource;
			set => this.bsQuoteInfo.DataSource = value;
		}

		// total quote info
		public List<TotalQuoteInfo> TotalQuoteInfo
		{
			get => (List<TotalQuoteInfo>)this.bsTotalQuoteInfo.DataSource;
			set => this.bsTotalQuoteInfo.DataSource = value;
		}

		// valuation info
// 		public List<ValuationInfo> ValuationInfo
// 		{
// 			get => (List<ValuationInfo>)this.bsValuationInfo.DataSource;
// 			set => this.bsValuationInfo.DataSource = value;
// 		}

// 		public List<OrderInfo> OrderInfo
// 		{
// 			get => (List<OrderInfo>)this.bsOrderInfo.DataSource;
// 			set => this.bsOrderInfo.DataSource = value;
// 		}

		// selected user account
		
		
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
                // ValuationInfo = null;
                ItemPriceInfo = null;
                QuoteInfo = null;
                TotalQuoteInfo = null;
                // OrderInfo = null;
                CurrentInfo = null;
                
			}

        }
        private int _tickCurrent = 0;

        public void UpdateCurrentInfo()
		{
			if (!this.Visible)
				return;

			FutureSite currentSite = CurrentSite;
// 			if (Settings.Default.SignalSiteOn)
// 				currentSite = SignalSite;


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

		public void UpdateItemPriceInfo()
		{
            if (!this.Visible)
                return;

            FutureSite currentSite = CurrentSite;
//             if (Settings.Default.SignalSiteOn)
//                 currentSite = SignalSite;

            if (currentSite != null)
			{
				if (this.dgvItemPriceInfo.RowCount >= 0)
				{
					ItemPriceInfo = null;
					//this.dgvItemPriceInfo.DataSource = null;
					this.ItemPriceInfo = currentSite.ItemPriceList;
					//dgvItemPriceInfo.DataSource = this.ItemPriceInfo;
				}
			}
		}

		public void UpdateQuoteInfo()
		{
            if (!this.Visible)
                return;

            FutureSite currentSite = CurrentSite;
//             if (Settings.Default.SignalSiteOn)
//                 currentSite = SignalSite;

            if (currentSite != null)
            {
				if (this.dgvQuoteInfo.RowCount >= 0)
				{
					//this.dgvQuoteInfo.DataSource = null;
					this.QuoteInfo = currentSite.QuoteList;
					//dgvQuoteInfo.DataSource = this.QuoteInfo;
					dgvQuoteInfo.Invalidate();
				}
			}
				
			
		}

		public void UpdateTotalQuoteInfo()
		{

            if (!this.Visible)
                return;

            FutureSite currentSite = CurrentSite;
//             if (Settings.Default.SignalSiteOn)
//                 currentSite = SignalSite;

            if (currentSite != null)
            {
				if (this.dgvTotalQuoteInfo.RowCount >= 0)
				{
					//this.dgvTotalQuoteInfo.DataSource = null;
					this.TotalQuoteInfo = currentSite.TotalQuoteList;
					//this.dgvTotalQuoteInfo.DataSource = this.TotalQuoteInfo;
					dgvTotalQuoteInfo.Invalidate();
				}
			}
            

        }
		
		public void SetQuoteInfoTopRow(int topRow)
		{
			if (!this.Visible)
				return;
			if (this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex >= 0)
			{
				int num = this.dgvQuoteInfo.DisplayedRowCount(true);
				int num2 = (num > 1) ? (topRow - (num / 2 - 1)) : 0;
				this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex = ((num2 >= 0) ? num2 : 0);
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
				//                 if (Settings.Default.SignalSiteOn)
				//                     currentSite = SignalSite;

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

		private void dgvQuoteInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{

                FutureSite currentSite = CurrentSite;
//                 if (Settings.Default.SignalSiteOn)
//                     currentSite = SignalSite;

                if (currentSite != null)
				{
					switch (e.ColumnIndex)
					{
						case 0:
							e.CellStyle.BackColor = Color.FromArgb(218, 232, 254);
							break;
						case 1:
						case 2:
							if (e.RowIndex < currentSite.Bid1Row.QuoteInfoId || !(currentSite.Bid1Row.BidQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(240, 240, 253) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 3:
							if (e.Value != null)
							{
								string a = e.Value.ToString();
								e.CellStyle.ForeColor = a == "시" ? Color.Green :
									(a == "고" ? Color.Red :
									(a == "저") ? Color.Blue : Color.Black);
							}
							if (currentSite.PositionRow != null && e.RowIndex == currentSite.PositionRow.QuoteInfoId && currentSite.PositionTradeType == TRADETYPE.SELL)
							{
								e.CellStyle.BackColor = Color.FromArgb(0, 0, 192);
							}
							else if (currentSite.PositionRow != null && e.RowIndex == currentSite.PositionRow.QuoteInfoId && currentSite.PositionTradeType == TRADETYPE.BUY)
							{
								e.CellStyle.BackColor = Color.FromArgb(192, 0, 0);
							}
							break;
						case 4:
							e.CellStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
							e.CellStyle.ForeColor = Color.Red;
							if (e.RowIndex == currentSite.CurrentPriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(128, 255, 128);
								
								return;
							}
							if (e.RowIndex >= currentSite.HighPriceRow.QuoteInfoId && e.RowIndex <= currentSite.LowPriceRow.QuoteInfoId && e.RowIndex < currentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(255, 227, 227);
								return;
							}
							if (e.RowIndex <= currentSite.LowPriceRow.QuoteInfoId && e.RowIndex >= currentSite.HighPriceRow.QuoteInfoId && e.RowIndex > currentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.FromArgb(220, 241, 252);
								return;
							}
							if (e.RowIndex == currentSite.BeforeClosePriceRow.QuoteInfoId)
							{
								e.CellStyle.BackColor = Color.LightGray;
								return;
							}
							break;
						case 5:
							if (e.RowIndex > currentSite.Ask1Row.QuoteInfoId || !(currentSite.Ask1Row.AskQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(255, 227, 227) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 6:
							if (e.RowIndex > currentSite.Ask1Row.QuoteInfoId || !(currentSite.Ask1Row.AskQty > 0))
							{
								e.CellStyle.BackColor = ((e.Value != null) ? Color.FromArgb(255, 227, 227) : Color.White);
								return;
							}
							if (e.Value != null)
							{
								if (Convert.ToInt32(e.Value) > 0)
								{
									e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
									return;
								}
								if (Convert.ToInt32(e.Value) < 0)
								{
									e.CellStyle.ForeColor = Color.Blue;
									return;
								}
								e.CellStyle.ForeColor = Color.Black;
								return;
							}
							break;
						case 7:
							e.CellStyle.BackColor = Color.FromArgb(252, 213, 181);
							break;
					}
				}
			}
			catch (Exception) { }

		}

		private void dgvQuoteInfo_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
            try { 
				int num = this.dgvQuoteInfo.FirstDisplayedScrollingRowIndex + this.dgvQuoteInfo.DisplayedRowCount(true) / 2;
				if (e.RowIndex == num && this.IsFixed)
				{
					using (Pen pen = new Pen(Color.Black, 1f))
					{
						e.Graphics.DrawLine(pen, e.CellBounds.X, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Top - 1);
					}
				}
            }
            catch (Exception) { }

		}

		private void chkFixed_CheckedChanged(object sender, EventArgs e)
		{
			try
			{

                FutureSite currentSite = CurrentSite;
//                 if (Settings.Default.SignalSiteOn)
//                     currentSite = SignalSite;

                if (currentSite != null && this.QuoteInfo != null && this.QuoteInfo.Any<QuoteInfo>() && this.IsFixed)
				{
					if (currentSite.CurrentPriceRow != null)
					{
						int num = (from q in this.QuoteInfo
								   where q.AskQty != null
								   orderby q.QuoteInfoId descending
								   select q.QuoteInfoId).FirstOrDefault<int>();
						this.SetQuoteInfoTopRow(
							(num != 0) ? num : (currentSite != null ? currentSite.CurrentPriceRow.QuoteInfoId : 0)
						);
					}
				}
				dgvQuoteInfo.Invalidate();
			}
			catch (Exception) { }
		}

		private void dgvQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvQuoteInfo.ClearSelection();
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

		private void dgvTotalQuoteInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvTotalQuoteInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvItemPriceInfo_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				dgvItemPriceInfo.ClearSelection();
			}
			catch (Exception) { }
		}

		private void dgvItemPriceInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{

				if (e.ColumnIndex == 0)
				{
					e.CellStyle.BackColor = SystemColors.Control;
					return;
				}
				if (e.ColumnIndex == 4)
				{
					e.CellStyle.BackColor = SystemColors.Control;
					return;
				}
				if (e.ColumnIndex == 2)
				{
					if (e.Value != null)
					{
						if (Convert.ToDouble(e.Value) < 0.0)
						{
							e.CellStyle.ForeColor = Color.Blue;
							return;
						}
						if (Convert.ToDouble(e.Value) > 0.0)
						{
							e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
							return;
						}
						e.CellStyle.ForeColor = Color.Black;
						return;
					}
				}
				else if (e.ColumnIndex == 3 && e.Value != null)
				{
					if (Convert.ToDouble(e.Value) < 0.0)
					{
						e.CellStyle.ForeColor = Color.Blue;
						return;
					}
					if (Convert.ToDouble(e.Value) > 0.0)
					{
						e.CellStyle.ForeColor = Color.FromArgb(192, 0, 0);
						return;
					}
					e.CellStyle.ForeColor = Color.Black;
				}
			}
			catch (Exception) { }
		}


		private void dgvQuoteInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.RowIndex < 0)
					return;

				if (Settings.Default.IsAutoMode || CurrentSite == null)
					return;

				QuoteInfo quoteInfo = this.QuoteInfo[e.RowIndex];

				switch (e.ColumnIndex)
				{
					case 0:
						CurrentSite.DoSellOrder(quoteInfo, 
							Settings.Default.OrderCount <= Settings.Default.OrderMax ? Settings.Default.OrderCount : Settings.Default.OrderMax, 
							false);
						break;
					case 7:
						CurrentSite.DoBuyOrder(quoteInfo, 
							Settings.Default.OrderCount <= Settings.Default.OrderMax ? Settings.Default.OrderCount : Settings.Default.OrderMax, 
							false);
						break;
				}
            }
            catch (Exception) { }
		}


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
