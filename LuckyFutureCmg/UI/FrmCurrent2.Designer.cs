
namespace LuckyFuture.UI
{
	partial class FrmCurrent2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCurrent2));
            this.bsQuoteInfo = new System.Windows.Forms.BindingSource(this.components);
            this.dgvCurrentInfo = new System.Windows.Forms.DataGridView();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentPriceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.conclusionQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsCurrentInfo = new System.Windows.Forms.BindingSource(this.components);
            this.bsTotalQuoteInfo = new System.Windows.Forms.BindingSource(this.components);
            this.bsItemPriceInfo = new System.Windows.Forms.BindingSource(this.components);
            this.hopeForm1 = new ReaLTaiizor.Forms.HopeForm();
            this.bsValuationInfo = new System.Windows.Forms.BindingSource(this.components);
            this.bsOrderInfo = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrentInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotalQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemPriceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValuationInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrderInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // bsQuoteInfo
            // 
            this.bsQuoteInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.QuoteInfo);
            // 
            // dgvCurrentInfo
            // 
            this.dgvCurrentInfo.AllowUserToAddRows = false;
            this.dgvCurrentInfo.AllowUserToDeleteRows = false;
            this.dgvCurrentInfo.AllowUserToResizeColumns = false;
            this.dgvCurrentInfo.AllowUserToResizeRows = false;
            this.dgvCurrentInfo.AutoGenerateColumns = false;
            this.dgvCurrentInfo.BackgroundColor = System.Drawing.Color.White;
            this.dgvCurrentInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvCurrentInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvCurrentInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCurrentInfo.ColumnHeadersHeight = 25;
            this.dgvCurrentInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCurrentInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timeDataGridViewTextBoxColumn,
            this.currentPriceDataGridViewTextBoxColumn1,
            this.conclusionQtyDataGridViewTextBoxColumn,
            this.tradeTypeDataGridViewTextBoxColumn});
            this.dgvCurrentInfo.DataSource = this.bsCurrentInfo;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCurrentInfo.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCurrentInfo.Enabled = false;
            this.dgvCurrentInfo.EnableHeadersVisualStyles = false;
            this.dgvCurrentInfo.Location = new System.Drawing.Point(20, 60);
            this.dgvCurrentInfo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgvCurrentInfo.MultiSelect = false;
            this.dgvCurrentInfo.Name = "dgvCurrentInfo";
            this.dgvCurrentInfo.ReadOnly = true;
            this.dgvCurrentInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvCurrentInfo.RowHeadersVisible = false;
            this.dgvCurrentInfo.RowHeadersWidth = 20;
            this.dgvCurrentInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCurrentInfo.RowTemplate.Height = 18;
            this.dgvCurrentInfo.RowTemplate.ReadOnly = true;
            this.dgvCurrentInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCurrentInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCurrentInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCurrentInfo.ShowCellErrors = false;
            this.dgvCurrentInfo.ShowCellToolTips = false;
            this.dgvCurrentInfo.ShowEditingIcon = false;
            this.dgvCurrentInfo.ShowRowErrors = false;
            this.dgvCurrentInfo.Size = new System.Drawing.Size(310, 525);
            this.dgvCurrentInfo.TabIndex = 0;
            this.dgvCurrentInfo.VirtualMode = true;
            this.dgvCurrentInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrentInfo_CellFormatting);
            this.dgvCurrentInfo.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCurrentInfo_DataError);
            this.dgvCurrentInfo.SelectionChanged += new System.EventHandler(this.dgvCurrentInfo_SelectionChanged);
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "HH:mm:ss";
            this.timeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.timeDataGridViewTextBoxColumn.HeaderText = "시간";
            this.timeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.timeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.timeDataGridViewTextBoxColumn.Width = 76;
            // 
            // currentPriceDataGridViewTextBoxColumn1
            // 
            this.currentPriceDataGridViewTextBoxColumn1.DataPropertyName = "CurrentPriceStr";
            dataGridViewCellStyle3.NullValue = null;
            this.currentPriceDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.currentPriceDataGridViewTextBoxColumn1.HeaderText = "Bid";
            this.currentPriceDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.currentPriceDataGridViewTextBoxColumn1.Name = "currentPriceDataGridViewTextBoxColumn1";
            this.currentPriceDataGridViewTextBoxColumn1.ReadOnly = true;
            this.currentPriceDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.currentPriceDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.currentPriceDataGridViewTextBoxColumn1.Width = 76;
            // 
            // conclusionQtyDataGridViewTextBoxColumn
            // 
            this.conclusionQtyDataGridViewTextBoxColumn.DataPropertyName = "CurrentPrice2Str";
            this.conclusionQtyDataGridViewTextBoxColumn.HeaderText = "Ask";
            this.conclusionQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.conclusionQtyDataGridViewTextBoxColumn.Name = "conclusionQtyDataGridViewTextBoxColumn";
            this.conclusionQtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.conclusionQtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.conclusionQtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.conclusionQtyDataGridViewTextBoxColumn.Width = 76;
            // 
            // tradeTypeDataGridViewTextBoxColumn
            // 
            this.tradeTypeDataGridViewTextBoxColumn.DataPropertyName = "TradeType";
            this.tradeTypeDataGridViewTextBoxColumn.HeaderText = "구분";
            this.tradeTypeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tradeTypeDataGridViewTextBoxColumn.Name = "tradeTypeDataGridViewTextBoxColumn";
            this.tradeTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.tradeTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tradeTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tradeTypeDataGridViewTextBoxColumn.Visible = false;
            this.tradeTypeDataGridViewTextBoxColumn.Width = 125;
            // 
            // bsCurrentInfo
            // 
            this.bsCurrentInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.CurrentInfo);
            // 
            // bsTotalQuoteInfo
            // 
            this.bsTotalQuoteInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.TotalQuoteInfo);
            // 
            // bsItemPriceInfo
            // 
            this.bsItemPriceInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.ItemPriceInfo);
            // 
            // hopeForm1
            // 
            this.hopeForm1.BackColor = System.Drawing.SystemColors.Control;
            this.hopeForm1.ControlBoxColorH = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.hopeForm1.ControlBoxColorHC = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.hopeForm1.ControlBoxColorN = System.Drawing.Color.White;
            this.hopeForm1.Cursor = System.Windows.Forms.Cursors.Default;
            this.hopeForm1.Dock = System.Windows.Forms.DockStyle.Top;
            this.hopeForm1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.hopeForm1.ForeColor = System.Drawing.SystemColors.Info;
            this.hopeForm1.Image = global::LuckyFuture.Properties.Resources.main_icon;
            this.hopeForm1.Location = new System.Drawing.Point(0, 0);
            this.hopeForm1.Margin = new System.Windows.Forms.Padding(4);
            this.hopeForm1.MaximizeBox = false;
            this.hopeForm1.Name = "hopeForm1";
            this.hopeForm1.Size = new System.Drawing.Size(350, 40);
            this.hopeForm1.TabIndex = 7;
            this.hopeForm1.Text = "호가창";
            this.hopeForm1.ThemeColor = System.Drawing.Color.DarkCyan;
            // 
            // bsValuationInfo
            // 
            this.bsValuationInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.ValuationInfo);
            // 
            // bsOrderInfo
            // 
            this.bsOrderInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.OrderInfo);
            // 
            // FrmCurrent2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.BackgroundImage = global::LuckyFuture.Properties.Resources.form_border_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(350, 610);
            this.Controls.Add(this.hopeForm1);
            this.Controls.Add(this.dgvCurrentInfo);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2400, 1288);
            this.MinimumSize = new System.Drawing.Size(190, 40);
            this.Name = "FrmCurrent2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prime";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrentInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotalQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemPriceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValuationInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrderInfo)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView dgvCurrentInfo;
		private ReaLTaiizor.Forms.HopeForm hopeForm1;
		private System.Windows.Forms.BindingSource bsCurrentInfo;
		private System.Windows.Forms.BindingSource bsQuoteInfo;
		private System.Windows.Forms.BindingSource bsTotalQuoteInfo;
		private System.Windows.Forms.BindingSource bsItemPriceInfo;
		private System.Windows.Forms.BindingSource bsValuationInfo;
		private System.Windows.Forms.BindingSource bsOrderInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentPriceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn conclusionQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeTypeDataGridViewTextBoxColumn;
    }
}