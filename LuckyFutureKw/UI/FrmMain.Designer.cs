
using System;

namespace LuckyFuture.UI
{
	partial class FrmMain
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
            try
            {

			    if (disposing && (components != null))
			    {
				    components.Dispose();
			    }
			    base.Dispose(disposing);
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvQuoteInfo = new System.Windows.Forms.DataGridView();
            this.sellOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceSymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsQuoteInfo = new System.Windows.Forms.BindingSource(this.components);
            this.chkFixed = new System.Windows.Forms.CheckBox();
            this.dgvValuationInfo = new System.Windows.Forms.DataGridView();
            this.balanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.averageUnitPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valuationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalValuationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalProfitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lossCutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsValuationInfo = new System.Windows.Forms.BindingSource(this.components);
            this.txtId = new System.Windows.Forms.TextBox();
            this.cmbSiteList = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvOrderInfo = new System.Windows.Forms.DataGridView();
            this.orderTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.averagePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentPriceDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valuationDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bsOrderInfo = new System.Windows.Forms.BindingSource(this.components);
            this.btnSetting = new System.Windows.Forms.Button();
            this.dgvCurrentInfo = new System.Windows.Forms.DataGridView();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentPriceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.conclusionQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsCurrentInfo = new System.Windows.Forms.BindingSource(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.hopeForm1 = new ReaLTaiizor.Forms.HopeForm();
            this.btnLogin = new System.Windows.Forms.Button();
            this.listLog = new System.Windows.Forms.ListView();
            this.columnLog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnChat = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvTotalQuoteInfo = new System.Windows.Forms.DataGridView();
            this.totalSellOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalSellCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalSellQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.differenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalBuyQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalBuyCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalBuyOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsTotalQuoteInfo = new System.Windows.Forms.BindingSource(this.components);
            this.cmbUserAccounts = new System.Windows.Forms.ComboBox();
            this.dgvItemPriceInfo = new System.Windows.Forms.DataGridView();
            this.title1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contrastDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contrastPerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.highPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsItemPriceInfo = new System.Windows.Forms.BindingSource(this.components);
            this.chkAutoMode = new System.Windows.Forms.CheckBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.axKFOpenAPI = new AxKFOpenAPILib.AxKFOpenAPI();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSave = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValuationInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValuationInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrderInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrentInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTotalQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotalQuoteInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemPriceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemPriceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKFOpenAPI)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQuoteInfo
            // 
            this.dgvQuoteInfo.AllowUserToAddRows = false;
            this.dgvQuoteInfo.AllowUserToDeleteRows = false;
            this.dgvQuoteInfo.AllowUserToResizeColumns = false;
            this.dgvQuoteInfo.AllowUserToResizeRows = false;
            this.dgvQuoteInfo.AutoGenerateColumns = false;
            this.dgvQuoteInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvQuoteInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvQuoteInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvQuoteInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQuoteInfo.ColumnHeadersHeight = 20;
            this.dgvQuoteInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvQuoteInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sellOrderDataGridViewTextBoxColumn,
            this.sellCountDataGridViewTextBoxColumn,
            this.sellQtyDataGridViewTextBoxColumn,
            this.priceSymbolDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.buyQtyDataGridViewTextBoxColumn,
            this.buyCountDataGridViewTextBoxColumn,
            this.buyOrderDataGridViewTextBoxColumn});
            this.dgvQuoteInfo.DataSource = this.bsQuoteInfo;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQuoteInfo.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvQuoteInfo.EnableHeadersVisualStyles = false;
            this.dgvQuoteInfo.Location = new System.Drawing.Point(9, 174);
            this.dgvQuoteInfo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgvQuoteInfo.MultiSelect = false;
            this.dgvQuoteInfo.Name = "dgvQuoteInfo";
            this.dgvQuoteInfo.ReadOnly = true;
            this.dgvQuoteInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvQuoteInfo.RowHeadersVisible = false;
            this.dgvQuoteInfo.RowHeadersWidth = 20;
            this.dgvQuoteInfo.RowTemplate.Height = 18;
            this.dgvQuoteInfo.RowTemplate.ReadOnly = true;
            this.dgvQuoteInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQuoteInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvQuoteInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvQuoteInfo.ShowCellErrors = false;
            this.dgvQuoteInfo.ShowCellToolTips = false;
            this.dgvQuoteInfo.ShowEditingIcon = false;
            this.dgvQuoteInfo.ShowRowErrors = false;
            this.dgvQuoteInfo.Size = new System.Drawing.Size(679, 388);
            this.dgvQuoteInfo.TabIndex = 0;
            this.dgvQuoteInfo.VirtualMode = true;
            this.dgvQuoteInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvQuoteInfo_CellFormatting);
            this.dgvQuoteInfo.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvQuoteInfo_CellMouseUp);
            this.dgvQuoteInfo.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvQuoteInfo_CellPainting);
            this.dgvQuoteInfo.SelectionChanged += new System.EventHandler(this.dgvQuoteInfo_SelectionChanged);
            // 
            // sellOrderDataGridViewTextBoxColumn
            // 
            this.sellOrderDataGridViewTextBoxColumn.DataPropertyName = "SellOrder";
            this.sellOrderDataGridViewTextBoxColumn.FillWeight = 143.0603F;
            this.sellOrderDataGridViewTextBoxColumn.HeaderText = "매도";
            this.sellOrderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sellOrderDataGridViewTextBoxColumn.Name = "sellOrderDataGridViewTextBoxColumn";
            this.sellOrderDataGridViewTextBoxColumn.ReadOnly = true;
            this.sellOrderDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sellOrderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sellOrderDataGridViewTextBoxColumn.Width = 70;
            // 
            // sellCountDataGridViewTextBoxColumn
            // 
            this.sellCountDataGridViewTextBoxColumn.DataPropertyName = "AskCount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sellCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.sellCountDataGridViewTextBoxColumn.FillWeight = 23.19896F;
            this.sellCountDataGridViewTextBoxColumn.HeaderText = "건수";
            this.sellCountDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sellCountDataGridViewTextBoxColumn.Name = "sellCountDataGridViewTextBoxColumn";
            this.sellCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.sellCountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sellCountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sellCountDataGridViewTextBoxColumn.Width = 70;
            // 
            // sellQtyDataGridViewTextBoxColumn
            // 
            this.sellQtyDataGridViewTextBoxColumn.DataPropertyName = "AskQty";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sellQtyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.sellQtyDataGridViewTextBoxColumn.FillWeight = 23.19896F;
            this.sellQtyDataGridViewTextBoxColumn.HeaderText = "잔량";
            this.sellQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sellQtyDataGridViewTextBoxColumn.Name = "sellQtyDataGridViewTextBoxColumn";
            this.sellQtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.sellQtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sellQtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sellQtyDataGridViewTextBoxColumn.Width = 70;
            // 
            // priceSymbolDataGridViewTextBoxColumn
            // 
            this.priceSymbolDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.priceSymbolDataGridViewTextBoxColumn.DataPropertyName = "PriceSymbol";
            this.priceSymbolDataGridViewTextBoxColumn.HeaderText = "";
            this.priceSymbolDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.priceSymbolDataGridViewTextBoxColumn.Name = "priceSymbolDataGridViewTextBoxColumn";
            this.priceSymbolDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceSymbolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.priceSymbolDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.priceSymbolDataGridViewTextBoxColumn.Width = 28;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.priceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.priceDataGridViewTextBoxColumn.FillWeight = 440.9449F;
            this.priceDataGridViewTextBoxColumn.HeaderText = "";
            this.priceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.priceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.priceDataGridViewTextBoxColumn.Width = 75;
            // 
            // buyQtyDataGridViewTextBoxColumn
            // 
            this.buyQtyDataGridViewTextBoxColumn.DataPropertyName = "BidQty";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.buyQtyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.buyQtyDataGridViewTextBoxColumn.FillWeight = 23.19896F;
            this.buyQtyDataGridViewTextBoxColumn.HeaderText = "잔량";
            this.buyQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.buyQtyDataGridViewTextBoxColumn.Name = "buyQtyDataGridViewTextBoxColumn";
            this.buyQtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.buyQtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.buyQtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.buyQtyDataGridViewTextBoxColumn.Width = 70;
            // 
            // buyCountDataGridViewTextBoxColumn
            // 
            this.buyCountDataGridViewTextBoxColumn.DataPropertyName = "BidCount";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.buyCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.buyCountDataGridViewTextBoxColumn.FillWeight = 23.19896F;
            this.buyCountDataGridViewTextBoxColumn.HeaderText = "건수";
            this.buyCountDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.buyCountDataGridViewTextBoxColumn.Name = "buyCountDataGridViewTextBoxColumn";
            this.buyCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.buyCountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.buyCountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.buyCountDataGridViewTextBoxColumn.Width = 70;
            // 
            // buyOrderDataGridViewTextBoxColumn
            // 
            this.buyOrderDataGridViewTextBoxColumn.DataPropertyName = "BuyOrder";
            this.buyOrderDataGridViewTextBoxColumn.FillWeight = 23.19896F;
            this.buyOrderDataGridViewTextBoxColumn.HeaderText = "매수";
            this.buyOrderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.buyOrderDataGridViewTextBoxColumn.Name = "buyOrderDataGridViewTextBoxColumn";
            this.buyOrderDataGridViewTextBoxColumn.ReadOnly = true;
            this.buyOrderDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.buyOrderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.buyOrderDataGridViewTextBoxColumn.Width = 70;
            // 
            // bsQuoteInfo
            // 
            this.bsQuoteInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.QuoteInfo);
            // 
            // chkFixed
            // 
            this.chkFixed.BackColor = System.Drawing.SystemColors.Control;
            this.chkFixed.Checked = true;
            this.chkFixed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixed.Location = new System.Drawing.Point(282, 176);
            this.chkFixed.Margin = new System.Windows.Forms.Padding(4);
            this.chkFixed.Name = "chkFixed";
            this.chkFixed.Size = new System.Drawing.Size(111, 22);
            this.chkFixed.TabIndex = 1;
            this.chkFixed.Text = "호가고정";
            this.chkFixed.UseVisualStyleBackColor = false;
            this.chkFixed.CheckedChanged += new System.EventHandler(this.chkFixed_CheckedChanged);
            // 
            // dgvValuationInfo
            // 
            this.dgvValuationInfo.AllowUserToAddRows = false;
            this.dgvValuationInfo.AllowUserToDeleteRows = false;
            this.dgvValuationInfo.AllowUserToResizeColumns = false;
            this.dgvValuationInfo.AllowUserToResizeRows = false;
            this.dgvValuationInfo.AutoGenerateColumns = false;
            this.dgvValuationInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvValuationInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvValuationInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvValuationInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvValuationInfo.ColumnHeadersHeight = 18;
            this.dgvValuationInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvValuationInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.balanceDataGridViewTextBoxColumn,
            this.averageUnitPriceDataGridViewTextBoxColumn,
            this.valuationDataGridViewTextBoxColumn,
            this.totalValuationDataGridViewTextBoxColumn,
            this.totalProfitDataGridViewTextBoxColumn,
            this.lossCutDataGridViewTextBoxColumn});
            this.dgvValuationInfo.DataSource = this.bsValuationInfo;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvValuationInfo.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvValuationInfo.EnableHeadersVisualStyles = false;
            this.dgvValuationInfo.Location = new System.Drawing.Point(9, 95);
            this.dgvValuationInfo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgvValuationInfo.MultiSelect = false;
            this.dgvValuationInfo.Name = "dgvValuationInfo";
            this.dgvValuationInfo.ReadOnly = true;
            this.dgvValuationInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvValuationInfo.RowHeadersVisible = false;
            this.dgvValuationInfo.RowHeadersWidth = 20;
            this.dgvValuationInfo.RowTemplate.Height = 18;
            this.dgvValuationInfo.RowTemplate.ReadOnly = true;
            this.dgvValuationInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvValuationInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvValuationInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvValuationInfo.ShowCellErrors = false;
            this.dgvValuationInfo.ShowCellToolTips = false;
            this.dgvValuationInfo.ShowEditingIcon = false;
            this.dgvValuationInfo.ShowRowErrors = false;
            this.dgvValuationInfo.Size = new System.Drawing.Size(679, 49);
            this.dgvValuationInfo.TabIndex = 0;
            this.dgvValuationInfo.VirtualMode = true;
            this.dgvValuationInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvValuationInfo_CellFormatting);
            this.dgvValuationInfo.SelectionChanged += new System.EventHandler(this.dgvValuationInfo_SelectionChanged);
            // 
            // balanceDataGridViewTextBoxColumn
            // 
            this.balanceDataGridViewTextBoxColumn.DataPropertyName = "Balance";
            this.balanceDataGridViewTextBoxColumn.HeaderText = "잔고";
            this.balanceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.balanceDataGridViewTextBoxColumn.Name = "balanceDataGridViewTextBoxColumn";
            this.balanceDataGridViewTextBoxColumn.ReadOnly = true;
            this.balanceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.balanceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.balanceDataGridViewTextBoxColumn.Width = 90;
            // 
            // averageUnitPriceDataGridViewTextBoxColumn
            // 
            this.averageUnitPriceDataGridViewTextBoxColumn.DataPropertyName = "AverageUnitPrice";
            dataGridViewCellStyle9.Format = "N2";
            this.averageUnitPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.averageUnitPriceDataGridViewTextBoxColumn.HeaderText = "평균단가";
            this.averageUnitPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.averageUnitPriceDataGridViewTextBoxColumn.Name = "averageUnitPriceDataGridViewTextBoxColumn";
            this.averageUnitPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.averageUnitPriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.averageUnitPriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.averageUnitPriceDataGridViewTextBoxColumn.Width = 90;
            // 
            // valuationDataGridViewTextBoxColumn
            // 
            this.valuationDataGridViewTextBoxColumn.DataPropertyName = "Valuation";
            dataGridViewCellStyle10.Format = "N0";
            this.valuationDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.valuationDataGridViewTextBoxColumn.HeaderText = "평가손익";
            this.valuationDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.valuationDataGridViewTextBoxColumn.Name = "valuationDataGridViewTextBoxColumn";
            this.valuationDataGridViewTextBoxColumn.ReadOnly = true;
            this.valuationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.valuationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.valuationDataGridViewTextBoxColumn.Width = 90;
            // 
            // totalValuationDataGridViewTextBoxColumn
            // 
            this.totalValuationDataGridViewTextBoxColumn.DataPropertyName = "TotalValuation";
            dataGridViewCellStyle11.Format = "N0";
            this.totalValuationDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
            this.totalValuationDataGridViewTextBoxColumn.HeaderText = "평가손익합";
            this.totalValuationDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalValuationDataGridViewTextBoxColumn.Name = "totalValuationDataGridViewTextBoxColumn";
            this.totalValuationDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalValuationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalValuationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalValuationDataGridViewTextBoxColumn.Width = 90;
            // 
            // totalProfitDataGridViewTextBoxColumn
            // 
            this.totalProfitDataGridViewTextBoxColumn.DataPropertyName = "TotalProfit";
            dataGridViewCellStyle12.Format = "N0";
            this.totalProfitDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
            this.totalProfitDataGridViewTextBoxColumn.HeaderText = "실현손익";
            this.totalProfitDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalProfitDataGridViewTextBoxColumn.Name = "totalProfitDataGridViewTextBoxColumn";
            this.totalProfitDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalProfitDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalProfitDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalProfitDataGridViewTextBoxColumn.Width = 90;
            // 
            // lossCutDataGridViewTextBoxColumn
            // 
            this.lossCutDataGridViewTextBoxColumn.DataPropertyName = "LossCut";
            dataGridViewCellStyle13.Format = "N0";
            this.lossCutDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle13;
            this.lossCutDataGridViewTextBoxColumn.HeaderText = "로스컷";
            this.lossCutDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.lossCutDataGridViewTextBoxColumn.Name = "lossCutDataGridViewTextBoxColumn";
            this.lossCutDataGridViewTextBoxColumn.ReadOnly = true;
            this.lossCutDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lossCutDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lossCutDataGridViewTextBoxColumn.Width = 90;
            // 
            // bsValuationInfo
            // 
            this.bsValuationInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.ValuationInfo);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(210, 61);
            this.txtId.Margin = new System.Windows.Forms.Padding(4);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(65, 25);
            this.txtId.TabIndex = 1;
            this.txtId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtId.Visible = false;
            // 
            // cmbSiteList
            // 
            this.cmbSiteList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSiteList.FormattingEnabled = true;
            this.cmbSiteList.Location = new System.Drawing.Point(33, 41);
            this.cmbSiteList.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSiteList.Name = "cmbSiteList";
            this.cmbSiteList.Size = new System.Drawing.Size(110, 23);
            this.cmbSiteList.TabIndex = 0;
            this.cmbSiteList.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(210, 61);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(95, 25);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "이 름";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvOrderInfo
            // 
            this.dgvOrderInfo.AllowUserToAddRows = false;
            this.dgvOrderInfo.AllowUserToDeleteRows = false;
            this.dgvOrderInfo.AllowUserToResizeColumns = false;
            this.dgvOrderInfo.AllowUserToResizeRows = false;
            this.dgvOrderInfo.AutoGenerateColumns = false;
            this.dgvOrderInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvOrderInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvOrderInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvOrderInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvOrderInfo.ColumnHeadersHeight = 20;
            this.dgvOrderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOrderInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderTypeDataGridViewTextBoxColumn,
            this.symbolDataGridViewTextBoxColumn,
            this.qtyDataGridViewTextBoxColumn,
            this.averagePriceDataGridViewTextBoxColumn,
            this.currentPriceDataGridViewTextBoxColumn2,
            this.valuationDataGridViewTextBoxColumn1,
            this.actionDataGridViewTextBoxColumn});
            this.dgvOrderInfo.DataSource = this.bsOrderInfo;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrderInfo.DefaultCellStyle = dataGridViewCellStyle17;
            this.dgvOrderInfo.EnableHeadersVisualStyles = false;
            this.dgvOrderInfo.Location = new System.Drawing.Point(9, 589);
            this.dgvOrderInfo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgvOrderInfo.MultiSelect = false;
            this.dgvOrderInfo.Name = "dgvOrderInfo";
            this.dgvOrderInfo.ReadOnly = true;
            this.dgvOrderInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOrderInfo.RowHeadersVisible = false;
            this.dgvOrderInfo.RowHeadersWidth = 20;
            this.dgvOrderInfo.RowTemplate.Height = 20;
            this.dgvOrderInfo.RowTemplate.ReadOnly = true;
            this.dgvOrderInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrderInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvOrderInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvOrderInfo.ShowCellErrors = false;
            this.dgvOrderInfo.ShowCellToolTips = false;
            this.dgvOrderInfo.ShowEditingIcon = false;
            this.dgvOrderInfo.ShowRowErrors = false;
            this.dgvOrderInfo.Size = new System.Drawing.Size(679, 145);
            this.dgvOrderInfo.TabIndex = 0;
            this.dgvOrderInfo.VirtualMode = true;
            this.dgvOrderInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOrderInfo_CellFormatting);
            this.dgvOrderInfo.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOrderInfo_CellMouseUp);
            this.dgvOrderInfo.SelectionChanged += new System.EventHandler(this.dgvOrderInfo_SelectionChanged);
            // 
            // orderTypeDataGridViewTextBoxColumn
            // 
            this.orderTypeDataGridViewTextBoxColumn.DataPropertyName = "OrderType";
            this.orderTypeDataGridViewTextBoxColumn.HeaderText = "구분";
            this.orderTypeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderTypeDataGridViewTextBoxColumn.Name = "orderTypeDataGridViewTextBoxColumn";
            this.orderTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.orderTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.orderTypeDataGridViewTextBoxColumn.Width = 70;
            // 
            // symbolDataGridViewTextBoxColumn
            // 
            this.symbolDataGridViewTextBoxColumn.DataPropertyName = "Symbol";
            this.symbolDataGridViewTextBoxColumn.HeaderText = "종목코드";
            this.symbolDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.symbolDataGridViewTextBoxColumn.Name = "symbolDataGridViewTextBoxColumn";
            this.symbolDataGridViewTextBoxColumn.ReadOnly = true;
            this.symbolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.symbolDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.symbolDataGridViewTextBoxColumn.Width = 70;
            // 
            // qtyDataGridViewTextBoxColumn
            // 
            this.qtyDataGridViewTextBoxColumn.DataPropertyName = "Qty";
            this.qtyDataGridViewTextBoxColumn.HeaderText = "수량";
            this.qtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.qtyDataGridViewTextBoxColumn.Name = "qtyDataGridViewTextBoxColumn";
            this.qtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.qtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.qtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qtyDataGridViewTextBoxColumn.Width = 70;
            // 
            // averagePriceDataGridViewTextBoxColumn
            // 
            this.averagePriceDataGridViewTextBoxColumn.DataPropertyName = "AveragePrice";
            this.averagePriceDataGridViewTextBoxColumn.HeaderText = "주문가";
            this.averagePriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.averagePriceDataGridViewTextBoxColumn.Name = "averagePriceDataGridViewTextBoxColumn";
            this.averagePriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.averagePriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.averagePriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.averagePriceDataGridViewTextBoxColumn.Width = 70;
            // 
            // currentPriceDataGridViewTextBoxColumn2
            // 
            this.currentPriceDataGridViewTextBoxColumn2.DataPropertyName = "CurrentPrice";
            this.currentPriceDataGridViewTextBoxColumn2.HeaderText = "현재가";
            this.currentPriceDataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.currentPriceDataGridViewTextBoxColumn2.Name = "currentPriceDataGridViewTextBoxColumn2";
            this.currentPriceDataGridViewTextBoxColumn2.ReadOnly = true;
            this.currentPriceDataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.currentPriceDataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.currentPriceDataGridViewTextBoxColumn2.Width = 70;
            // 
            // valuationDataGridViewTextBoxColumn1
            // 
            this.valuationDataGridViewTextBoxColumn1.DataPropertyName = "Valuation";
            dataGridViewCellStyle16.Format = "N0";
            this.valuationDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle16;
            this.valuationDataGridViewTextBoxColumn1.HeaderText = "평가손익";
            this.valuationDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.valuationDataGridViewTextBoxColumn1.Name = "valuationDataGridViewTextBoxColumn1";
            this.valuationDataGridViewTextBoxColumn1.ReadOnly = true;
            this.valuationDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.valuationDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.valuationDataGridViewTextBoxColumn1.Width = 125;
            // 
            // actionDataGridViewTextBoxColumn
            // 
            this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
            this.actionDataGridViewTextBoxColumn.HeaderText = "주문";
            this.actionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
            this.actionDataGridViewTextBoxColumn.ReadOnly = true;
            this.actionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.actionDataGridViewTextBoxColumn.Width = 65;
            // 
            // bsOrderInfo
            // 
            this.bsOrderInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.OrderInfo);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(556, 59);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(62, 32);
            this.btnSetting.TabIndex = 5;
            this.btnSetting.Text = "설 정";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // dgvCurrentInfo
            // 
            this.dgvCurrentInfo.AllowUserToAddRows = false;
            this.dgvCurrentInfo.AllowUserToDeleteRows = false;
            this.dgvCurrentInfo.AllowUserToResizeColumns = false;
            this.dgvCurrentInfo.AllowUserToResizeRows = false;
            this.dgvCurrentInfo.AutoGenerateColumns = false;
            this.dgvCurrentInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvCurrentInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvCurrentInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvCurrentInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvCurrentInfo.ColumnHeadersHeight = 25;
            this.dgvCurrentInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCurrentInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timeDataGridViewTextBoxColumn,
            this.currentPriceDataGridViewTextBoxColumn1,
            this.conclusionQtyDataGridViewTextBoxColumn,
            this.tradeTypeDataGridViewTextBoxColumn});
            this.dgvCurrentInfo.DataSource = this.bsCurrentInfo;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCurrentInfo.DefaultCellStyle = dataGridViewCellStyle21;
            this.dgvCurrentInfo.EnableHeadersVisualStyles = false;
            this.dgvCurrentInfo.Location = new System.Drawing.Point(695, 118);
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
            this.dgvCurrentInfo.Size = new System.Drawing.Size(315, 345);
            this.dgvCurrentInfo.TabIndex = 0;
            this.dgvCurrentInfo.VirtualMode = true;
            this.dgvCurrentInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrentInfo_CellFormatting);
            this.dgvCurrentInfo.SelectionChanged += new System.EventHandler(this.dgvCurrentInfo_SelectionChanged);
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.Format = "HH:mm:ss";
            this.timeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle19;
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
            this.currentPriceDataGridViewTextBoxColumn1.DataPropertyName = "CurrentPrice";
            dataGridViewCellStyle20.Format = "N2";
            dataGridViewCellStyle20.NullValue = null;
            this.currentPriceDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle20;
            this.currentPriceDataGridViewTextBoxColumn1.HeaderText = "체결가";
            this.currentPriceDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.currentPriceDataGridViewTextBoxColumn1.Name = "currentPriceDataGridViewTextBoxColumn1";
            this.currentPriceDataGridViewTextBoxColumn1.ReadOnly = true;
            this.currentPriceDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.currentPriceDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.currentPriceDataGridViewTextBoxColumn1.Width = 76;
            // 
            // conclusionQtyDataGridViewTextBoxColumn
            // 
            this.conclusionQtyDataGridViewTextBoxColumn.DataPropertyName = "ConclusionQty";
            this.conclusionQtyDataGridViewTextBoxColumn.HeaderText = "체결량";
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(161, 68);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "계  좌";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hopeForm1
            // 
            this.hopeForm1.ControlBoxColorH = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.hopeForm1.ControlBoxColorHC = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.hopeForm1.ControlBoxColorN = System.Drawing.Color.White;
            this.hopeForm1.Cursor = System.Windows.Forms.Cursors.Default;
            this.hopeForm1.Dock = System.Windows.Forms.DockStyle.Top;
            this.hopeForm1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.hopeForm1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.hopeForm1.Image = ((System.Drawing.Image)(resources.GetObject("hopeForm1.Image")));
            this.hopeForm1.Location = new System.Drawing.Point(0, 0);
            this.hopeForm1.Margin = new System.Windows.Forms.Padding(4);
            this.hopeForm1.MaximizeBox = false;
            this.hopeForm1.Name = "hopeForm1";
            this.hopeForm1.Size = new System.Drawing.Size(1015, 40);
            this.hopeForm1.TabIndex = 7;
            this.hopeForm1.Text = "LuckyFuture";
            this.hopeForm1.ThemeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogin.Location = new System.Drawing.Point(352, 59);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(62, 32);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "접 속";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // listLog
            // 
            this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnLog});
            this.listLog.FullRowSelect = true;
            this.listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listLog.HideSelection = false;
            this.listLog.Location = new System.Drawing.Point(694, 469);
            this.listLog.Margin = new System.Windows.Forms.Padding(4);
            this.listLog.MultiSelect = false;
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(314, 264);
            this.listLog.TabIndex = 8;
            this.listLog.UseCompatibleStateImageBehavior = false;
            this.listLog.View = System.Windows.Forms.View.Details;
            // 
            // columnLog
            // 
            this.columnLog.Width = 220;
            // 
            // btnChat
            // 
            this.btnChat.Location = new System.Drawing.Point(624, 59);
            this.btnChat.Margin = new System.Windows.Forms.Padding(4);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(62, 32);
            this.btnChat.TabIndex = 6;
            this.btnChat.Text = "차 트";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(71, 62);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(82, 25);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBalance
            // 
            this.txtBalance.BackColor = System.Drawing.Color.MistyRose;
            this.txtBalance.Location = new System.Drawing.Point(785, 87);
            this.txtBalance.Margin = new System.Windows.Forms.Padding(4);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(220, 25);
            this.txtBalance.TabIndex = 2;
            this.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(709, 93);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "담보금";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvTotalQuoteInfo
            // 
            this.dgvTotalQuoteInfo.AllowUserToAddRows = false;
            this.dgvTotalQuoteInfo.AllowUserToDeleteRows = false;
            this.dgvTotalQuoteInfo.AllowUserToResizeColumns = false;
            this.dgvTotalQuoteInfo.AllowUserToResizeRows = false;
            this.dgvTotalQuoteInfo.AutoGenerateColumns = false;
            this.dgvTotalQuoteInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTotalQuoteInfo.ColumnHeadersHeight = 18;
            this.dgvTotalQuoteInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTotalQuoteInfo.ColumnHeadersVisible = false;
            this.dgvTotalQuoteInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.totalSellOrderDataGridViewTextBoxColumn,
            this.totalSellCountDataGridViewTextBoxColumn,
            this.totalSellQtyDataGridViewTextBoxColumn,
            this.differenceDataGridViewTextBoxColumn,
            this.totalBuyQtyDataGridViewTextBoxColumn,
            this.totalBuyCountDataGridViewTextBoxColumn,
            this.totalBuyOrderDataGridViewTextBoxColumn});
            this.dgvTotalQuoteInfo.DataSource = this.bsTotalQuoteInfo;
            this.dgvTotalQuoteInfo.Location = new System.Drawing.Point(9, 560);
            this.dgvTotalQuoteInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTotalQuoteInfo.MultiSelect = false;
            this.dgvTotalQuoteInfo.Name = "dgvTotalQuoteInfo";
            this.dgvTotalQuoteInfo.ReadOnly = true;
            this.dgvTotalQuoteInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTotalQuoteInfo.RowHeadersVisible = false;
            this.dgvTotalQuoteInfo.RowHeadersWidth = 16;
            this.dgvTotalQuoteInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTotalQuoteInfo.RowTemplate.Height = 16;
            this.dgvTotalQuoteInfo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTotalQuoteInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTotalQuoteInfo.ShowCellErrors = false;
            this.dgvTotalQuoteInfo.ShowCellToolTips = false;
            this.dgvTotalQuoteInfo.ShowEditingIcon = false;
            this.dgvTotalQuoteInfo.ShowRowErrors = false;
            this.dgvTotalQuoteInfo.Size = new System.Drawing.Size(679, 22);
            this.dgvTotalQuoteInfo.TabIndex = 9;
            this.dgvTotalQuoteInfo.VirtualMode = true;
            this.dgvTotalQuoteInfo.SelectionChanged += new System.EventHandler(this.dgvTotalQuoteInfo_SelectionChanged);
            // 
            // totalSellOrderDataGridViewTextBoxColumn
            // 
            this.totalSellOrderDataGridViewTextBoxColumn.DataPropertyName = "TotalSellOrder";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.totalSellOrderDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle22;
            this.totalSellOrderDataGridViewTextBoxColumn.FillWeight = 81.27339F;
            this.totalSellOrderDataGridViewTextBoxColumn.HeaderText = "TotalSellOrder";
            this.totalSellOrderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalSellOrderDataGridViewTextBoxColumn.Name = "totalSellOrderDataGridViewTextBoxColumn";
            this.totalSellOrderDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalSellOrderDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalSellOrderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalSellOrderDataGridViewTextBoxColumn.Width = 70;
            // 
            // totalSellCountDataGridViewTextBoxColumn
            // 
            this.totalSellCountDataGridViewTextBoxColumn.DataPropertyName = "TotalAskCount";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalSellCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle23;
            this.totalSellCountDataGridViewTextBoxColumn.FillWeight = 116.3529F;
            this.totalSellCountDataGridViewTextBoxColumn.HeaderText = "TotalAskCount";
            this.totalSellCountDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalSellCountDataGridViewTextBoxColumn.Name = "totalSellCountDataGridViewTextBoxColumn";
            this.totalSellCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalSellCountDataGridViewTextBoxColumn.Width = 70;
            // 
            // totalSellQtyDataGridViewTextBoxColumn
            // 
            this.totalSellQtyDataGridViewTextBoxColumn.DataPropertyName = "TotalAskQty";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalSellQtyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle24;
            this.totalSellQtyDataGridViewTextBoxColumn.FillWeight = 172.4202F;
            this.totalSellQtyDataGridViewTextBoxColumn.HeaderText = "TotalAskQty";
            this.totalSellQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalSellQtyDataGridViewTextBoxColumn.Name = "totalSellQtyDataGridViewTextBoxColumn";
            this.totalSellQtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalSellQtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalSellQtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalSellQtyDataGridViewTextBoxColumn.Width = 70;
            // 
            // differenceDataGridViewTextBoxColumn
            // 
            this.differenceDataGridViewTextBoxColumn.DataPropertyName = "Difference";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.differenceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle25;
            this.differenceDataGridViewTextBoxColumn.FillWeight = 262.0321F;
            this.differenceDataGridViewTextBoxColumn.HeaderText = "Difference";
            this.differenceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.differenceDataGridViewTextBoxColumn.Name = "differenceDataGridViewTextBoxColumn";
            this.differenceDataGridViewTextBoxColumn.ReadOnly = true;
            this.differenceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.differenceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.differenceDataGridViewTextBoxColumn.Width = 103;
            // 
            // totalBuyQtyDataGridViewTextBoxColumn
            // 
            this.totalBuyQtyDataGridViewTextBoxColumn.DataPropertyName = "TotalBidQty";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalBuyQtyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle26;
            this.totalBuyQtyDataGridViewTextBoxColumn.FillWeight = 22.64045F;
            this.totalBuyQtyDataGridViewTextBoxColumn.HeaderText = "TotalBidQty";
            this.totalBuyQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalBuyQtyDataGridViewTextBoxColumn.Name = "totalBuyQtyDataGridViewTextBoxColumn";
            this.totalBuyQtyDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalBuyQtyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalBuyQtyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalBuyQtyDataGridViewTextBoxColumn.Width = 70;
            // 
            // totalBuyCountDataGridViewTextBoxColumn
            // 
            this.totalBuyCountDataGridViewTextBoxColumn.DataPropertyName = "TotalBidCount";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalBuyCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle27;
            this.totalBuyCountDataGridViewTextBoxColumn.FillWeight = 22.64045F;
            this.totalBuyCountDataGridViewTextBoxColumn.HeaderText = "TotalBidCount";
            this.totalBuyCountDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalBuyCountDataGridViewTextBoxColumn.Name = "totalBuyCountDataGridViewTextBoxColumn";
            this.totalBuyCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalBuyCountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalBuyCountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalBuyCountDataGridViewTextBoxColumn.Width = 70;
            // 
            // totalBuyOrderDataGridViewTextBoxColumn
            // 
            this.totalBuyOrderDataGridViewTextBoxColumn.DataPropertyName = "TotalBuyOrder";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.totalBuyOrderDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle28;
            this.totalBuyOrderDataGridViewTextBoxColumn.FillWeight = 22.64045F;
            this.totalBuyOrderDataGridViewTextBoxColumn.HeaderText = "TotalBuyOrder";
            this.totalBuyOrderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalBuyOrderDataGridViewTextBoxColumn.Name = "totalBuyOrderDataGridViewTextBoxColumn";
            this.totalBuyOrderDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalBuyOrderDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.totalBuyOrderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.totalBuyOrderDataGridViewTextBoxColumn.Width = 70;
            // 
            // bsTotalQuoteInfo
            // 
            this.bsTotalQuoteInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.TotalQuoteInfo);
            // 
            // cmbUserAccounts
            // 
            this.cmbUserAccounts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(254)))), ((int)(((byte)(255)))));
            this.cmbUserAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserAccounts.FormattingEnabled = true;
            this.cmbUserAccounts.Location = new System.Drawing.Point(216, 62);
            this.cmbUserAccounts.Margin = new System.Windows.Forms.Padding(4);
            this.cmbUserAccounts.Name = "cmbUserAccounts";
            this.cmbUserAccounts.Size = new System.Drawing.Size(117, 23);
            this.cmbUserAccounts.TabIndex = 10;
            this.cmbUserAccounts.SelectedIndexChanged += new System.EventHandler(this.cmbUserAccounts_SelectedIndexChanged);
            // 
            // dgvItemPriceInfo
            // 
            this.dgvItemPriceInfo.AllowUserToAddRows = false;
            this.dgvItemPriceInfo.AllowUserToDeleteRows = false;
            this.dgvItemPriceInfo.AllowUserToResizeColumns = false;
            this.dgvItemPriceInfo.AllowUserToResizeRows = false;
            this.dgvItemPriceInfo.AutoGenerateColumns = false;
            this.dgvItemPriceInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvItemPriceInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvItemPriceInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvItemPriceInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.dgvItemPriceInfo.ColumnHeadersHeight = 18;
            this.dgvItemPriceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItemPriceInfo.ColumnHeadersVisible = false;
            this.dgvItemPriceInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.title1DataGridViewTextBoxColumn,
            this.currentPriceDataGridViewTextBoxColumn,
            this.contrastDataGridViewTextBoxColumn,
            this.contrastPerDataGridViewTextBoxColumn,
            this.title2DataGridViewTextBoxColumn,
            this.startPriceDataGridViewTextBoxColumn,
            this.highPriceDataGridViewTextBoxColumn,
            this.lowPriceDataGridViewTextBoxColumn});
            this.dgvItemPriceInfo.DataSource = this.bsItemPriceInfo;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle38.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemPriceInfo.DefaultCellStyle = dataGridViewCellStyle38;
            this.dgvItemPriceInfo.EnableHeadersVisualStyles = false;
            this.dgvItemPriceInfo.Location = new System.Drawing.Point(9, 142);
            this.dgvItemPriceInfo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dgvItemPriceInfo.MultiSelect = false;
            this.dgvItemPriceInfo.Name = "dgvItemPriceInfo";
            this.dgvItemPriceInfo.ReadOnly = true;
            this.dgvItemPriceInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvItemPriceInfo.RowHeadersVisible = false;
            this.dgvItemPriceInfo.RowHeadersWidth = 20;
            this.dgvItemPriceInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvItemPriceInfo.RowTemplate.Height = 18;
            this.dgvItemPriceInfo.RowTemplate.ReadOnly = true;
            this.dgvItemPriceInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemPriceInfo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvItemPriceInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvItemPriceInfo.ShowCellErrors = false;
            this.dgvItemPriceInfo.ShowCellToolTips = false;
            this.dgvItemPriceInfo.ShowEditingIcon = false;
            this.dgvItemPriceInfo.ShowRowErrors = false;
            this.dgvItemPriceInfo.Size = new System.Drawing.Size(679, 25);
            this.dgvItemPriceInfo.TabIndex = 0;
            this.dgvItemPriceInfo.VirtualMode = true;
            this.dgvItemPriceInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvItemPriceInfo_CellFormatting);
            this.dgvItemPriceInfo.SelectionChanged += new System.EventHandler(this.dgvItemPriceInfo_SelectionChanged);
            // 
            // title1DataGridViewTextBoxColumn
            // 
            this.title1DataGridViewTextBoxColumn.DataPropertyName = "Title1";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.title1DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle30;
            this.title1DataGridViewTextBoxColumn.HeaderText = "Title1";
            this.title1DataGridViewTextBoxColumn.MinimumWidth = 6;
            this.title1DataGridViewTextBoxColumn.Name = "title1DataGridViewTextBoxColumn";
            this.title1DataGridViewTextBoxColumn.ReadOnly = true;
            this.title1DataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.title1DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.title1DataGridViewTextBoxColumn.Width = 65;
            // 
            // currentPriceDataGridViewTextBoxColumn
            // 
            this.currentPriceDataGridViewTextBoxColumn.DataPropertyName = "CurrentPrice";
            dataGridViewCellStyle31.Format = "N2";
            this.currentPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle31;
            this.currentPriceDataGridViewTextBoxColumn.HeaderText = "CurrentPrice";
            this.currentPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.currentPriceDataGridViewTextBoxColumn.Name = "currentPriceDataGridViewTextBoxColumn";
            this.currentPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.currentPriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.currentPriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.currentPriceDataGridViewTextBoxColumn.Width = 75;
            // 
            // contrastDataGridViewTextBoxColumn
            // 
            this.contrastDataGridViewTextBoxColumn.DataPropertyName = "Contrast";
            dataGridViewCellStyle32.Format = "N2";
            this.contrastDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle32;
            this.contrastDataGridViewTextBoxColumn.HeaderText = "Contrast";
            this.contrastDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.contrastDataGridViewTextBoxColumn.Name = "contrastDataGridViewTextBoxColumn";
            this.contrastDataGridViewTextBoxColumn.ReadOnly = true;
            this.contrastDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.contrastDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.contrastDataGridViewTextBoxColumn.Width = 75;
            // 
            // contrastPerDataGridViewTextBoxColumn
            // 
            this.contrastPerDataGridViewTextBoxColumn.DataPropertyName = "ContrastPer";
            dataGridViewCellStyle33.Format = "N2";
            dataGridViewCellStyle33.NullValue = null;
            this.contrastPerDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle33;
            this.contrastPerDataGridViewTextBoxColumn.HeaderText = "ContrastPer";
            this.contrastPerDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.contrastPerDataGridViewTextBoxColumn.Name = "contrastPerDataGridViewTextBoxColumn";
            this.contrastPerDataGridViewTextBoxColumn.ReadOnly = true;
            this.contrastPerDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.contrastPerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.contrastPerDataGridViewTextBoxColumn.Width = 55;
            // 
            // title2DataGridViewTextBoxColumn
            // 
            this.title2DataGridViewTextBoxColumn.DataPropertyName = "Title2";
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.title2DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle34;
            this.title2DataGridViewTextBoxColumn.HeaderText = "Title2";
            this.title2DataGridViewTextBoxColumn.MinimumWidth = 6;
            this.title2DataGridViewTextBoxColumn.Name = "title2DataGridViewTextBoxColumn";
            this.title2DataGridViewTextBoxColumn.ReadOnly = true;
            this.title2DataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.title2DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.title2DataGridViewTextBoxColumn.Width = 65;
            // 
            // startPriceDataGridViewTextBoxColumn
            // 
            this.startPriceDataGridViewTextBoxColumn.DataPropertyName = "StartPrice";
            dataGridViewCellStyle35.Format = "N2";
            this.startPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle35;
            this.startPriceDataGridViewTextBoxColumn.HeaderText = "StartPrice";
            this.startPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.startPriceDataGridViewTextBoxColumn.Name = "startPriceDataGridViewTextBoxColumn";
            this.startPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.startPriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.startPriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.startPriceDataGridViewTextBoxColumn.Width = 69;
            // 
            // highPriceDataGridViewTextBoxColumn
            // 
            this.highPriceDataGridViewTextBoxColumn.DataPropertyName = "HighPrice";
            dataGridViewCellStyle36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle36.Format = "N2";
            this.highPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle36;
            this.highPriceDataGridViewTextBoxColumn.HeaderText = "HighPrice";
            this.highPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.highPriceDataGridViewTextBoxColumn.Name = "highPriceDataGridViewTextBoxColumn";
            this.highPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.highPriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.highPriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.highPriceDataGridViewTextBoxColumn.Width = 68;
            // 
            // lowPriceDataGridViewTextBoxColumn
            // 
            this.lowPriceDataGridViewTextBoxColumn.DataPropertyName = "LowPrice";
            dataGridViewCellStyle37.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle37.Format = "N2";
            this.lowPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle37;
            this.lowPriceDataGridViewTextBoxColumn.HeaderText = "LowPrice";
            this.lowPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.lowPriceDataGridViewTextBoxColumn.Name = "lowPriceDataGridViewTextBoxColumn";
            this.lowPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.lowPriceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lowPriceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lowPriceDataGridViewTextBoxColumn.Width = 68;
            // 
            // bsItemPriceInfo
            // 
            this.bsItemPriceInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.ItemPriceInfo);
            // 
            // chkAutoMode
            // 
            this.chkAutoMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkAutoMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(94)))), ((int)(((byte)(86)))));
            this.chkAutoMode.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.chkAutoMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAutoMode.ForeColor = System.Drawing.Color.Black;
            this.chkAutoMode.Location = new System.Drawing.Point(488, 59);
            this.chkAutoMode.Margin = new System.Windows.Forms.Padding(0);
            this.chkAutoMode.Name = "chkAutoMode";
            this.chkAutoMode.Size = new System.Drawing.Size(62, 32);
            this.chkAutoMode.TabIndex = 11;
            this.chkAutoMode.Text = "수 동";
            this.chkAutoMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAutoMode.UseVisualStyleBackColor = false;
            this.chkAutoMode.CheckedChanged += new System.EventHandler(this.chkAutoMode_CheckedChanged);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogout.Location = new System.Drawing.Point(420, 59);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(62, 32);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "해 제";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // axKFOpenAPI
            // 
            this.axKFOpenAPI.Enabled = true;
            this.axKFOpenAPI.Location = new System.Drawing.Point(-1, -100);
            this.axKFOpenAPI.Name = "axKFOpenAPI";
            this.axKFOpenAPI.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKFOpenAPI.OcxState")));
            this.axKFOpenAPI.Size = new System.Drawing.Size(51, 10);
            this.axKFOpenAPI.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(709, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 19);
            this.label1.TabIndex = 14;
            this.label1.Text = "보유금";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSave
            // 
            this.txtSave.BackColor = System.Drawing.Color.LightBlue;
            this.txtSave.Location = new System.Drawing.Point(785, 58);
            this.txtSave.Margin = new System.Windows.Forms.Padding(4);
            this.txtSave.Name = "txtSave";
            this.txtSave.ReadOnly = true;
            this.txtSave.Size = new System.Drawing.Size(220, 25);
            this.txtSave.TabIndex = 13;
            this.txtSave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1015, 740);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.axKFOpenAPI);
            this.Controls.Add(this.chkAutoMode);
            this.Controls.Add(this.cmbUserAccounts);
            this.Controls.Add(this.dgvTotalQuoteInfo);
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.hopeForm1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnChat);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSiteList);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.chkFixed);
            this.Controls.Add(this.dgvItemPriceInfo);
            this.Controls.Add(this.dgvCurrentInfo);
            this.Controls.Add(this.dgvValuationInfo);
            this.Controls.Add(this.dgvOrderInfo);
            this.Controls.Add(this.dgvQuoteInfo);
            this.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2400, 1288);
            this.MinimumSize = new System.Drawing.Size(190, 40);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LuckyFuture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValuationInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValuationInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOrderInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrentInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTotalQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotalQuoteInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemPriceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemPriceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKFOpenAPI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        private AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI;
        private System.Windows.Forms.DataGridView dgvTotalQuoteInfo;
		private System.Windows.Forms.DataGridView dgvQuoteInfo;
		private System.Windows.Forms.CheckBox chkFixed;
		private System.Windows.Forms.DataGridView dgvValuationInfo;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.ComboBox cmbSiteList;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridView dgvOrderInfo;
		private System.Windows.Forms.Button btnSetting;
		private System.Windows.Forms.DataGridView dgvCurrentInfo;
		private System.Windows.Forms.Label label7;
		private ReaLTaiizor.Forms.HopeForm hopeForm1;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.ListView listLog;
		private System.Windows.Forms.Button btnChat;
		private System.Windows.Forms.ColumnHeader columnLog;
		private System.Windows.Forms.BindingSource bsCurrentInfo;
		private System.Windows.Forms.BindingSource bsQuoteInfo;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.TextBox txtBalance;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.BindingSource bsTotalQuoteInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalSellOrderDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalSellCountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalSellQtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn differenceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalBuyQtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalBuyCountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalBuyOrderDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sellOrderDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sellCountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sellQtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn priceSymbolDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn buyQtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn buyCountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn buyOrderDataGridViewTextBoxColumn;
		private System.Windows.Forms.ComboBox cmbUserAccounts;
		private System.Windows.Forms.DataGridView dgvItemPriceInfo;
		private System.Windows.Forms.BindingSource bsItemPriceInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn title1DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn currentPriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn contrastDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn contrastPerDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn title2DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn startPriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn highPriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn lowPriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource bsValuationInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn balanceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn averageUnitPriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valuationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalValuationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn totalProfitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn lossCutDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource bsOrderInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn orderTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn symbolDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn qtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn averagePriceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn currentPriceDataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn valuationDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewButtonColumn actionDataGridViewTextBoxColumn;
		private System.Windows.Forms.CheckBox chkAutoMode;
		private System.Windows.Forms.Button btnLogout;
		private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn currentPriceDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn conclusionQtyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn tradeTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSave;
    }
}