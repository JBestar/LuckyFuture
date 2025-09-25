
namespace LuckyFuture.UI
{
    partial class FrmSync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSync));
            this.btnOk = new ReaLTaiizor.Controls.DreamButton();
            this.btnCancel = new ReaLTaiizor.Controls.DreamButton();
            this.dgvMemberInfo = new System.Windows.Forms.DataGridView();
            this.btnAdd = new ReaLTaiizor.Controls.DreamButton();
            this.lbUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.chkSync = new System.Windows.Forms.CheckBox();
            this.bsMemInfo = new System.Windows.Forms.BindingSource(this.components);
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.EndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemberInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMemInfo)).BeginInit();
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
            this.btnOk.Location = new System.Drawing.Point(35, 297);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(61, 26);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "동기화";
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
            this.btnCancel.Location = new System.Drawing.Point(121, 297);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 26);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "닫기";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvMemberInfo
            // 
            this.dgvMemberInfo.AllowUserToAddRows = false;
            this.dgvMemberInfo.AllowUserToDeleteRows = false;
            this.dgvMemberInfo.AllowUserToResizeColumns = false;
            this.dgvMemberInfo.AllowUserToResizeRows = false;
            this.dgvMemberInfo.AutoGenerateColumns = false;
            this.dgvMemberInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvMemberInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvMemberInfo.ColumnHeadersHeight = 30;
            this.dgvMemberInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMemberInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.deleteDataGridViewTextBoxColumn,
            this.EndColumn});
            this.dgvMemberInfo.DataSource = this.bsMemInfo;
            this.dgvMemberInfo.EnableHeadersVisualStyles = false;
            this.dgvMemberInfo.GridColor = System.Drawing.SystemColors.Window;
            this.dgvMemberInfo.Location = new System.Drawing.Point(12, 38);
            this.dgvMemberInfo.MultiSelect = false;
            this.dgvMemberInfo.Name = "dgvMemberInfo";
            this.dgvMemberInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvMemberInfo.RowHeadersVisible = false;
            this.dgvMemberInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMemberInfo.ShowCellErrors = false;
            this.dgvMemberInfo.ShowCellToolTips = false;
            this.dgvMemberInfo.ShowEditingIcon = false;
            this.dgvMemberInfo.ShowRowErrors = false;
            this.dgvMemberInfo.Size = new System.Drawing.Size(188, 209);
            this.dgvMemberInfo.TabIndex = 14;
            this.dgvMemberInfo.VirtualMode = true;
            this.dgvMemberInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRangeInfo_CellMouseClick);
            this.dgvMemberInfo.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvRangeInfo_EditingControlShowing);
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
            this.btnAdd.Location = new System.Drawing.Point(139, 254);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 26);
            this.btnAdd.TabIndex = 63;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Gulim", 9F);
            this.lbUser.Location = new System.Drawing.Point(11, 260);
            this.lbUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(29, 12);
            this.lbUser.TabIndex = 65;
            this.lbUser.Text = "유저";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(40, 256);
            this.txtUser.Margin = new System.Windows.Forms.Padding(2);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(80, 21);
            this.txtUser.TabIndex = 66;
            this.txtUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkSync
            // 
            this.chkSync.BackColor = System.Drawing.Color.Azure;
            this.chkSync.Checked = true;
            this.chkSync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSync.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkSync.Location = new System.Drawing.Point(13, 14);
            this.chkSync.Name = "chkSync";
            this.chkSync.Size = new System.Drawing.Size(95, 18);
            this.chkSync.TabIndex = 67;
            this.chkSync.Text = "동기화허용";
            this.chkSync.UseVisualStyleBackColor = false;
            this.chkSync.CheckedChanged += new System.EventHandler(this.chkSync_CheckedChanged);
            // 
            // bsMemInfo
            // 
            this.bsMemInfo.DataSource = typeof(LuckyFuture.Models.ValueObjects.MemberInfo);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "아이디";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.idDataGridViewTextBoxColumn.Width = 110;
            // 
            // deleteDataGridViewTextBoxColumn
            // 
            this.deleteDataGridViewTextBoxColumn.DataPropertyName = "Delete";
            this.deleteDataGridViewTextBoxColumn.HeaderText = "";
            this.deleteDataGridViewTextBoxColumn.Name = "deleteDataGridViewTextBoxColumn";
            this.deleteDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.deleteDataGridViewTextBoxColumn.Width = 60;
            // 
            // EndColumn
            // 
            this.EndColumn.HeaderText = "";
            this.EndColumn.Name = "EndColumn";
            this.EndColumn.ReadOnly = true;
            this.EndColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // FrmSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(220, 333);
            this.Controls.Add(this.chkSync);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvMemberInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Gulim", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSync";
            this.Text = "차트동기화설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemberInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMemInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ReaLTaiizor.Controls.DreamButton btnOk;
        private ReaLTaiizor.Controls.DreamButton btnCancel;
        private System.Windows.Forms.DataGridView dgvMemberInfo;
        private System.Windows.Forms.BindingSource bsMemInfo;
        private ReaLTaiizor.Controls.DreamButton btnAdd;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.CheckBox chkSync;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn deleteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndColumn;
    }
}