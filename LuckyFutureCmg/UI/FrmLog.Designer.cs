
namespace LuckyFuture.UI
{
    partial class FrmLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLog));
            this.listLog = new System.Windows.Forms.ListView();
            this.columnLog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listLog
            // 
            this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnLog});
            this.listLog.FullRowSelect = true;
            this.listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listLog.HideSelection = false;
            this.listLog.Location = new System.Drawing.Point(13, 35);
            this.listLog.Margin = new System.Windows.Forms.Padding(4);
            this.listLog.MultiSelect = false;
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(679, 613);
            this.listLog.TabIndex = 9;
            this.listLog.UseCompatibleStateImageBehavior = false;
            this.listLog.View = System.Windows.Forms.View.Details;
            // 
            // columnLog
            // 
            this.columnLog.Width = 520;
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
            this.chkScroll.CheckedChanged += new System.EventHandler(this.chkScroll_CheckedChanged);
            // 
            // FrmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(706, 661);
            this.Controls.Add(this.chkScroll);
            this.Controls.Add(this.listLog);
            this.Font = new System.Drawing.Font("Gulim", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLog";
            this.Text = "로그창";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listLog;
        private System.Windows.Forms.ColumnHeader columnLog;
        private System.Windows.Forms.CheckBox chkScroll;
    }
}