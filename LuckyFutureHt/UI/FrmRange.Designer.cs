
namespace LuckyFuture.UI
{
    partial class FrmRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRange));
            this.btnOk = new ReaLTaiizor.Controls.DreamButton();
            this.btnCancel = new ReaLTaiizor.Controls.DreamButton();
            this.lbLossUnit = new System.Windows.Forms.Label();
            this.txtPercent = new System.Windows.Forms.TextBox();
            this.lbAmoutUnit = new System.Windows.Forms.Label();
            this.txtAmout = new System.Windows.Forms.TextBox();
            this.dgvRangeInfo = new System.Windows.Forms.DataGridView();
            this.enabledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RateUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.EndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsRangeInfo = new System.Windows.Forms.BindingSource(this.components);
            this.btnAdd = new ReaLTaiizor.Controls.DreamButton();
            this.lbAmount = new System.Windows.Forms.Label();
            this.lbStage = new System.Windows.Forms.Label();
            this.txtStage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRangeInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRangeInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnOk.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnOk.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnOk.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnOk.ColorE = System.Drawing.Color.White;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOk.Location = new System.Drawing.Point(60, 399);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(96, 26);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "저장";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCancel.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCancel.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnCancel.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnCancel.ColorE = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gulim", 9F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(213, 399);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 26);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "닫기";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbLossUnit
            // 
            this.lbLossUnit.AutoSize = true;
            this.lbLossUnit.Font = new System.Drawing.Font("Gulim", 9F);
            this.lbLossUnit.Location = new System.Drawing.Point(230, 344);
            this.lbLossUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLossUnit.Name = "lbLossUnit";
            this.lbLossUnit.Size = new System.Drawing.Size(67, 12);
            this.lbLossUnit.TabIndex = 62;
            this.lbLossUnit.Text = "% 하락청산";
            // 
            // txtPercent
            // 
            this.txtPercent.Location = new System.Drawing.Point(189, 340);
            this.txtPercent.Margin = new System.Windows.Forms.Padding(2);
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.Size = new System.Drawing.Size(37, 21);
            this.txtPercent.TabIndex = 61;
            this.txtPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbAmoutUnit
            // 
            this.lbAmoutUnit.AutoSize = true;
            this.lbAmoutUnit.Font = new System.Drawing.Font("Gulim", 9F);
            this.lbAmoutUnit.Location = new System.Drawing.Point(157, 345);
            this.lbAmoutUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbAmoutUnit.Name = "lbAmoutUnit";
            this.lbAmoutUnit.Size = new System.Drawing.Size(29, 12);
            this.lbAmoutUnit.TabIndex = 59;
            this.lbAmoutUnit.Text = "만원";
            // 
            // txtAmout
            // 
            this.txtAmout.Location = new System.Drawing.Point(110, 340);
            this.txtAmout.Margin = new System.Windows.Forms.Padding(2);
            this.txtAmout.Name = "txtAmout";
            this.txtAmout.Size = new System.Drawing.Size(40, 21);
            this.txtAmout.TabIndex = 58;
            this.txtAmout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvRangeInfo
            // 
            this.dgvRangeInfo.AllowUserToAddRows = false;
            this.dgvRangeInfo.AllowUserToDeleteRows = false;
            this.dgvRangeInfo.AllowUserToResizeColumns = false;
            this.dgvRangeInfo.AllowUserToResizeRows = false;
            this.dgvRangeInfo.AutoGenerateColumns = false;
            this.dgvRangeInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvRangeInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvRangeInfo.ColumnHeadersHeight = 30;
            this.dgvRangeInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRangeInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enabledDataGridViewCheckBoxColumn,
            this.StageName,
            this.amountDataGridViewTextBoxColumn,
            this.Spec,
            this.rateDataGridViewTextBoxColumn,
            this.RateUnit,
            this.ActionDelete,
            this.EndColumn});
            this.dgvRangeInfo.DataSource = this.bsRangeInfo;
            this.dgvRangeInfo.EnableHeadersVisualStyles = false;
            this.dgvRangeInfo.GridColor = System.Drawing.SystemColors.Window;
            this.dgvRangeInfo.Location = new System.Drawing.Point(12, 12);
            this.dgvRangeInfo.MultiSelect = false;
            this.dgvRangeInfo.Name = "dgvRangeInfo";
            this.dgvRangeInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRangeInfo.RowHeadersVisible = false;
            this.dgvRangeInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvRangeInfo.ShowCellErrors = false;
            this.dgvRangeInfo.ShowCellToolTips = false;
            this.dgvRangeInfo.ShowEditingIcon = false;
            this.dgvRangeInfo.ShowRowErrors = false;
            this.dgvRangeInfo.Size = new System.Drawing.Size(352, 318);
            this.dgvRangeInfo.TabIndex = 14;
            this.dgvRangeInfo.VirtualMode = true;
            this.dgvRangeInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRangeInfo_CellMouseClick);
            this.dgvRangeInfo.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvRangeInfo_EditingControlShowing);
            // 
            // enabledDataGridViewCheckBoxColumn
            // 
            this.enabledDataGridViewCheckBoxColumn.DataPropertyName = "Enabled";
            this.enabledDataGridViewCheckBoxColumn.FalseValue = "0";
            this.enabledDataGridViewCheckBoxColumn.FillWeight = 30F;
            this.enabledDataGridViewCheckBoxColumn.HeaderText = "";
            this.enabledDataGridViewCheckBoxColumn.Name = "enabledDataGridViewCheckBoxColumn";
            this.enabledDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.enabledDataGridViewCheckBoxColumn.TrueValue = "1";
            this.enabledDataGridViewCheckBoxColumn.Width = 20;
            // 
            // StageName
            // 
            this.StageName.DataPropertyName = "StageName";
            this.StageName.HeaderText = "단계";
            this.StageName.Name = "StageName";
            this.StageName.ReadOnly = true;
            this.StageName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StageName.Width = 60;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "금액";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.amountDataGridViewTextBoxColumn.Width = 60;
            // 
            // Spec
            // 
            this.Spec.DataPropertyName = "AmountUnit";
            this.Spec.HeaderText = "";
            this.Spec.Name = "Spec";
            this.Spec.ReadOnly = true;
            this.Spec.Width = 45;
            // 
            // rateDataGridViewTextBoxColumn
            // 
            this.rateDataGridViewTextBoxColumn.DataPropertyName = "Rate";
            this.rateDataGridViewTextBoxColumn.HeaderText = "손실";
            this.rateDataGridViewTextBoxColumn.Name = "rateDataGridViewTextBoxColumn";
            this.rateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.rateDataGridViewTextBoxColumn.Width = 60;
            // 
            // RateUnit
            // 
            this.RateUnit.DataPropertyName = "RateUnit";
            this.RateUnit.HeaderText = "";
            this.RateUnit.Name = "RateUnit";
            this.RateUnit.ReadOnly = true;
            this.RateUnit.Width = 45;
            // 
            // ActionDelete
            // 
            this.ActionDelete.DataPropertyName = "ActionDelete";
            this.ActionDelete.HeaderText = "";
            this.ActionDelete.Name = "ActionDelete";
            this.ActionDelete.ReadOnly = true;
            this.ActionDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ActionDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ActionDelete.Width = 50;
            // 
            // EndColumn
            // 
            this.EndColumn.HeaderText = "";
            this.EndColumn.Name = "EndColumn";
            this.EndColumn.ReadOnly = true;
            this.EndColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EndColumn.Width = 20;
            // 
            // bsRangeInfo
            // 
            this.bsRangeInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.PayoffLossInfo);
            // 
            // btnAdd
            // 
            this.btnAdd.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnAdd.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnAdd.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnAdd.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnAdd.ColorE = System.Drawing.Color.White;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Gulim", 9F);
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAdd.Location = new System.Drawing.Point(301, 336);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(63, 26);
            this.btnAdd.TabIndex = 63;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbAmount
            // 
            this.lbAmount.AutoSize = true;
            this.lbAmount.Font = new System.Drawing.Font("Gulim", 9F);
            this.lbAmount.Location = new System.Drawing.Point(77, 344);
            this.lbAmount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbAmount.Name = "lbAmount";
            this.lbAmount.Size = new System.Drawing.Size(29, 12);
            this.lbAmount.TabIndex = 64;
            this.lbAmount.Text = "금액";
            // 
            // lbStage
            // 
            this.lbStage.AutoSize = true;
            this.lbStage.Font = new System.Drawing.Font("Gulim", 9F);
            this.lbStage.Location = new System.Drawing.Point(11, 343);
            this.lbStage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbStage.Name = "lbStage";
            this.lbStage.Size = new System.Drawing.Size(29, 12);
            this.lbStage.TabIndex = 65;
            this.lbStage.Text = "단계";
            // 
            // txtStage
            // 
            this.txtStage.Location = new System.Drawing.Point(42, 339);
            this.txtStage.Margin = new System.Windows.Forms.Padding(2);
            this.txtStage.Name = "txtStage";
            this.txtStage.Size = new System.Drawing.Size(31, 21);
            this.txtStage.TabIndex = 66;
            this.txtStage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(380, 441);
            this.Controls.Add(this.txtStage);
            this.Controls.Add(this.lbStage);
            this.Controls.Add(this.lbAmount);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbAmoutUnit);
            this.Controls.Add(this.lbLossUnit);
            this.Controls.Add(this.dgvRangeInfo);
            this.Controls.Add(this.txtPercent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtAmout);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Gulim", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRange";
            this.Text = "청산영역설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRangeInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRangeInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ReaLTaiizor.Controls.DreamButton btnOk;
        private ReaLTaiizor.Controls.DreamButton btnCancel;
        private System.Windows.Forms.Label lbLossUnit;
        private System.Windows.Forms.TextBox txtPercent;
        private System.Windows.Forms.Label lbAmoutUnit;
        private System.Windows.Forms.TextBox txtAmout;
        private System.Windows.Forms.DataGridView dgvRangeInfo;
        private System.Windows.Forms.BindingSource bsRangeInfo;
        private ReaLTaiizor.Controls.DreamButton btnAdd;
        private System.Windows.Forms.Label lbAmount;
        private System.Windows.Forms.Label lbStage;
        private System.Windows.Forms.TextBox txtStage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RateUnit;
        private System.Windows.Forms.DataGridViewButtonColumn ActionDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndColumn;
    }
}