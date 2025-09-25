
namespace LuckyFuture.UI
{
    partial class FrmNotice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNotice));
            this.chkScroll = new System.Windows.Forms.CheckBox();
            this.txtNotice = new System.Windows.Forms.TextBox();
            this.chkDayNotice = new System.Windows.Forms.CheckBox();
            this.btnCancel = new ReaLTaiizor.Controls.DreamButton();
            this.SuspendLayout();
            // 
            // chkScroll
            // 
            this.chkScroll.BackColor = System.Drawing.Color.Azure;
            this.chkScroll.Checked = true;
            this.chkScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScroll.Font = new System.Drawing.Font("Gulim", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkScroll.Location = new System.Drawing.Point(563, 5);
            this.chkScroll.Margin = new System.Windows.Forms.Padding(4);
            this.chkScroll.Name = "chkScroll";
            this.chkScroll.Size = new System.Drawing.Size(129, 22);
            this.chkScroll.TabIndex = 10;
            this.chkScroll.Text = "자동스크롤";
            this.chkScroll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkScroll.UseVisualStyleBackColor = false;
            // 
            // txtNotice
            // 
            this.txtNotice.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtNotice.Font = new System.Drawing.Font("DotumChe", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtNotice.Location = new System.Drawing.Point(12, 12);
            this.txtNotice.Multiline = true;
            this.txtNotice.Name = "txtNotice";
            this.txtNotice.ReadOnly = true;
            this.txtNotice.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotice.Size = new System.Drawing.Size(485, 468);
            this.txtNotice.TabIndex = 11;
            // 
            // chkDayNotice
            // 
            this.chkDayNotice.AutoSize = true;
            this.chkDayNotice.Location = new System.Drawing.Point(12, 499);
            this.chkDayNotice.Name = "chkDayNotice";
            this.chkDayNotice.Size = new System.Drawing.Size(164, 19);
            this.chkDayNotice.TabIndex = 12;
            this.chkDayNotice.Text = "오늘 하루 보지 않기";
            this.chkDayNotice.UseVisualStyleBackColor = true;
            this.chkDayNotice.CheckedChanged += new System.EventHandler(this.chkDayNotice_CheckedChanged);
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
            this.btnCancel.Location = new System.Drawing.Point(387, 491);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 32);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "닫기";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(509, 533);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkDayNotice);
            this.Controls.Add(this.txtNotice);
            this.Controls.Add(this.chkScroll);
            this.Font = new System.Drawing.Font("Gulim", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNotice";
            this.Text = "공지사항";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkScroll;
        private System.Windows.Forms.TextBox txtNotice;
        private System.Windows.Forms.CheckBox chkDayNotice;
        private ReaLTaiizor.Controls.DreamButton btnCancel;
    }
}