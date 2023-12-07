
namespace LuckyFuture.UI
{
    partial class FrmOrdCnt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOrdCnt));
            this.btnOk = new ReaLTaiizor.Controls.DreamButton();
            this.btnCancel = new ReaLTaiizor.Controls.DreamButton();
            this.groupAvgLine = new System.Windows.Forms.GroupBox();
            this.spin4 = new System.Windows.Forms.NumericUpDown();
            this.spin3 = new System.Windows.Forms.NumericUpDown();
            this.spin2 = new System.Windows.Forms.NumericUpDown();
            this.spin1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupAvgLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin1)).BeginInit();
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
            this.btnOk.Location = new System.Drawing.Point(11, 86);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(96, 26);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "저 장";
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
            this.btnCancel.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(200, 86);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취 소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupAvgLine
            // 
            this.groupAvgLine.Controls.Add(this.spin4);
            this.groupAvgLine.Controls.Add(this.spin3);
            this.groupAvgLine.Controls.Add(this.spin2);
            this.groupAvgLine.Controls.Add(this.spin1);
            this.groupAvgLine.Controls.Add(this.label1);
            this.groupAvgLine.Font = new System.Drawing.Font("Gulim", 9F);
            this.groupAvgLine.Location = new System.Drawing.Point(11, 11);
            this.groupAvgLine.Margin = new System.Windows.Forms.Padding(2);
            this.groupAvgLine.Name = "groupAvgLine";
            this.groupAvgLine.Padding = new System.Windows.Forms.Padding(2);
            this.groupAvgLine.Size = new System.Drawing.Size(285, 59);
            this.groupAvgLine.TabIndex = 4;
            this.groupAvgLine.TabStop = false;
            this.groupAvgLine.Text = "주문수량 버튼설정";
            // 
            // spin4
            // 
            this.spin4.Location = new System.Drawing.Point(232, 21);
            this.spin4.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spin4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spin4.Name = "spin4";
            this.spin4.Size = new System.Drawing.Size(44, 21);
            this.spin4.TabIndex = 5;
            this.spin4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spin3
            // 
            this.spin3.Location = new System.Drawing.Point(179, 21);
            this.spin3.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spin3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spin3.Name = "spin3";
            this.spin3.Size = new System.Drawing.Size(44, 21);
            this.spin3.TabIndex = 4;
            this.spin3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spin2
            // 
            this.spin2.Location = new System.Drawing.Point(127, 21);
            this.spin2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spin2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spin2.Name = "spin2";
            this.spin2.Size = new System.Drawing.Size(44, 21);
            this.spin2.TabIndex = 3;
            this.spin2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spin1
            // 
            this.spin1.Location = new System.Drawing.Point(76, 21);
            this.spin1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spin1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spin1.Name = "spin1";
            this.spin1.Size = new System.Drawing.Size(44, 21);
            this.spin1.TabIndex = 2;
            this.spin1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 9F);
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "파생수량";
            // 
            // FrmOrdCnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(307, 125);
            this.Controls.Add(this.groupAvgLine);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Gulim", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOrdCnt";
            this.Text = "주문수량설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            this.groupAvgLine.ResumeLayout(false);
            this.groupAvgLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ReaLTaiizor.Controls.DreamButton btnOk;
        private ReaLTaiizor.Controls.DreamButton btnCancel;
        private System.Windows.Forms.GroupBox groupAvgLine;
        private System.Windows.Forms.NumericUpDown spin4;
        private System.Windows.Forms.NumericUpDown spin3;
        private System.Windows.Forms.NumericUpDown spin2;
        private System.Windows.Forms.NumericUpDown spin1;
        private System.Windows.Forms.Label label1;
    }
}