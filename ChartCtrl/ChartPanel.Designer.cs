
using System.Windows.Forms;

namespace ChartCtrl
{
    partial class ChartPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStyleCandle = new System.Windows.Forms.Button();
            this.btnStyleLine = new System.Windows.Forms.Button();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.cmbAvgLineWidth = new System.Windows.Forms.ComboBox();
            this.panelTimeUnit1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTmUnit1 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit2 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit3 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit4 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit5 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit6 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTmUnit7 = new ReaLTaiizor.Controls.DreamButton();
            this.panelTimeType = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTimeDay = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeWeek = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeMonth = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeYear = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeMin = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeTick = new ReaLTaiizor.Controls.DreamButton();
            this.btnTimeSec = new ReaLTaiizor.Controls.DreamButton();
            this.panelTool = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAvg = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.panelTimeUnit2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTkUnit01 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit02 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit03 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit1 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit2 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit3 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit4 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit5 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit6 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit7 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit8 = new ReaLTaiizor.Controls.DreamButton();
            this.btnTkUnit9 = new ReaLTaiizor.Controls.DreamButton();
            this.panelGraphType = new System.Windows.Forms.FlowLayoutPanel();
            this.rChartCtrl = new ChartCtrl.RChartCtrl();
            this.txtSpec = new System.Windows.Forms.RichTextBox();
            this.panelTimeUnit1.SuspendLayout();
            this.panelTimeType.SuspendLayout();
            this.panelTool.SuspendLayout();
            this.panelTimeUnit2.SuspendLayout();
            this.panelGraphType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStyleCandle
            // 
            this.btnStyleCandle.Location = new System.Drawing.Point(0, 0);
            this.btnStyleCandle.Margin = new System.Windows.Forms.Padding(0);
            this.btnStyleCandle.Name = "btnStyleCandle";
            this.btnStyleCandle.Size = new System.Drawing.Size(50, 36);
            this.btnStyleCandle.TabIndex = 3;
            this.btnStyleCandle.Text = "봉";
            this.btnStyleCandle.UseVisualStyleBackColor = true;
            this.btnStyleCandle.Visible = false;
            this.btnStyleCandle.Click += new System.EventHandler(this.btnStyleCandle_Click);
            // 
            // btnStyleLine
            // 
            this.btnStyleLine.Location = new System.Drawing.Point(50, 0);
            this.btnStyleLine.Margin = new System.Windows.Forms.Padding(0);
            this.btnStyleLine.Name = "btnStyleLine";
            this.btnStyleLine.Size = new System.Drawing.Size(50, 36);
            this.btnStyleLine.TabIndex = 4;
            this.btnStyleLine.Text = "라인";
            this.btnStyleLine.UseVisualStyleBackColor = true;
            this.btnStyleLine.Visible = false;
            this.btnStyleLine.Click += new System.EventHandler(this.btnStyleLine_Click);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, -20);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(0, 20);
            this.hScrollBar.TabIndex = 19;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // cmbAvgLineWidth
            // 
            this.cmbAvgLineWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbAvgLineWidth.BackColor = System.Drawing.Color.White;
            this.cmbAvgLineWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAvgLineWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAvgLineWidth.FormattingEnabled = true;
            this.cmbAvgLineWidth.Location = new System.Drawing.Point(1, 37);
            this.cmbAvgLineWidth.Margin = new System.Windows.Forms.Padding(1);
            this.cmbAvgLineWidth.Name = "cmbAvgLineWidth";
            this.cmbAvgLineWidth.Size = new System.Drawing.Size(45, 28);
            this.cmbAvgLineWidth.TabIndex = 21;
            this.cmbAvgLineWidth.Visible = false;
            this.cmbAvgLineWidth.SelectedIndexChanged += new System.EventHandler(this.cmbAvgLineWidth_SelectedIndexChanged);
            // 
            // panelTimeUnit1
            // 
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit1);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit2);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit3);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit4);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit5);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit6);
            this.panelTimeUnit1.Controls.Add(this.btnTmUnit7);
            this.panelTimeUnit1.Location = new System.Drawing.Point(381, 12);
            this.panelTimeUnit1.Margin = new System.Windows.Forms.Padding(0);
            this.panelTimeUnit1.Name = "panelTimeUnit1";
            this.panelTimeUnit1.Size = new System.Drawing.Size(303, 36);
            this.panelTimeUnit1.TabIndex = 26;
            // 
            // btnTmUnit1
            // 
            this.btnTmUnit1.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit1.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit1.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit1.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit1.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit1.ColorE = System.Drawing.Color.White;
            this.btnTmUnit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit1.FlatAppearance.BorderSize = 0;
            this.btnTmUnit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit1.Location = new System.Drawing.Point(0, 0);
            this.btnTmUnit1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit1.Name = "btnTmUnit1";
            this.btnTmUnit1.Size = new System.Drawing.Size(38, 36);
            this.btnTmUnit1.TabIndex = 12;
            this.btnTmUnit1.Text = "1";
            this.btnTmUnit1.UseVisualStyleBackColor = true;
            this.btnTmUnit1.Click += new System.EventHandler(this.btnTmUnit1_Click);
            // 
            // btnTmUnit2
            // 
            this.btnTmUnit2.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit2.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit2.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit2.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit2.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit2.ColorE = System.Drawing.Color.White;
            this.btnTmUnit2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit2.FlatAppearance.BorderSize = 0;
            this.btnTmUnit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit2.Location = new System.Drawing.Point(39, 0);
            this.btnTmUnit2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit2.Name = "btnTmUnit2";
            this.btnTmUnit2.Size = new System.Drawing.Size(38, 36);
            this.btnTmUnit2.TabIndex = 16;
            this.btnTmUnit2.Text = "3";
            this.btnTmUnit2.UseVisualStyleBackColor = true;
            this.btnTmUnit2.Click += new System.EventHandler(this.btnTmUnit2_Click);
            // 
            // btnTmUnit3
            // 
            this.btnTmUnit3.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit3.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit3.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit3.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit3.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit3.ColorE = System.Drawing.Color.White;
            this.btnTmUnit3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit3.FlatAppearance.BorderSize = 0;
            this.btnTmUnit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit3.Location = new System.Drawing.Point(78, 0);
            this.btnTmUnit3.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit3.Name = "btnTmUnit3";
            this.btnTmUnit3.Size = new System.Drawing.Size(38, 36);
            this.btnTmUnit3.TabIndex = 17;
            this.btnTmUnit3.Text = "5";
            this.btnTmUnit3.UseVisualStyleBackColor = true;
            this.btnTmUnit3.Click += new System.EventHandler(this.btnTmUnit3_Click);
            // 
            // btnTmUnit4
            // 
            this.btnTmUnit4.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit4.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit4.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit4.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit4.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit4.ColorE = System.Drawing.Color.White;
            this.btnTmUnit4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit4.FlatAppearance.BorderSize = 0;
            this.btnTmUnit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTmUnit4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit4.Location = new System.Drawing.Point(117, 0);
            this.btnTmUnit4.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit4.Name = "btnTmUnit4";
            this.btnTmUnit4.Size = new System.Drawing.Size(44, 36);
            this.btnTmUnit4.TabIndex = 15;
            this.btnTmUnit4.Text = "15";
            this.btnTmUnit4.UseVisualStyleBackColor = true;
            this.btnTmUnit4.Click += new System.EventHandler(this.btnTmUnit4_Click);
            // 
            // btnTmUnit5
            // 
            this.btnTmUnit5.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit5.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit5.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit5.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit5.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit5.ColorE = System.Drawing.Color.White;
            this.btnTmUnit5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit5.FlatAppearance.BorderSize = 0;
            this.btnTmUnit5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit5.Location = new System.Drawing.Point(162, 0);
            this.btnTmUnit5.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit5.Name = "btnTmUnit5";
            this.btnTmUnit5.Size = new System.Drawing.Size(44, 36);
            this.btnTmUnit5.TabIndex = 18;
            this.btnTmUnit5.Text = "30";
            this.btnTmUnit5.UseVisualStyleBackColor = true;
            this.btnTmUnit5.Click += new System.EventHandler(this.btnTmUnit5_Click);
            // 
            // btnTmUnit6
            // 
            this.btnTmUnit6.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit6.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit6.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit6.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit6.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit6.ColorE = System.Drawing.Color.White;
            this.btnTmUnit6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit6.FlatAppearance.BorderSize = 0;
            this.btnTmUnit6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit6.Location = new System.Drawing.Point(207, 0);
            this.btnTmUnit6.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit6.Name = "btnTmUnit6";
            this.btnTmUnit6.Size = new System.Drawing.Size(44, 36);
            this.btnTmUnit6.TabIndex = 14;
            this.btnTmUnit6.Text = "60";
            this.btnTmUnit6.UseVisualStyleBackColor = true;
            this.btnTmUnit6.Click += new System.EventHandler(this.btnTmUnit6_Click);
            // 
            // btnTmUnit7
            // 
            this.btnTmUnit7.BackColor = System.Drawing.Color.Transparent;
            this.btnTmUnit7.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit7.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTmUnit7.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit7.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTmUnit7.ColorE = System.Drawing.Color.White;
            this.btnTmUnit7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmUnit7.FlatAppearance.BorderSize = 0;
            this.btnTmUnit7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTmUnit7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTmUnit7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTmUnit7.Location = new System.Drawing.Point(252, 0);
            this.btnTmUnit7.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTmUnit7.Name = "btnTmUnit7";
            this.btnTmUnit7.Size = new System.Drawing.Size(44, 36);
            this.btnTmUnit7.TabIndex = 13;
            this.btnTmUnit7.Text = "240";
            this.btnTmUnit7.UseVisualStyleBackColor = true;
            this.btnTmUnit7.Click += new System.EventHandler(this.btnTmUnit7_Click);
            // 
            // panelTimeType
            // 
            this.panelTimeType.Controls.Add(this.btnTimeDay);
            this.panelTimeType.Controls.Add(this.btnTimeWeek);
            this.panelTimeType.Controls.Add(this.btnTimeMonth);
            this.panelTimeType.Controls.Add(this.btnTimeYear);
            this.panelTimeType.Controls.Add(this.btnTimeMin);
            this.panelTimeType.Controls.Add(this.btnTimeTick);
            this.panelTimeType.Controls.Add(this.btnTimeSec);
            this.panelTimeType.Location = new System.Drawing.Point(144, 12);
            this.panelTimeType.Margin = new System.Windows.Forms.Padding(0);
            this.panelTimeType.Name = "panelTimeType";
            this.panelTimeType.Size = new System.Drawing.Size(237, 36);
            this.panelTimeType.TabIndex = 27;
            // 
            // btnTimeDay
            // 
            this.btnTimeDay.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeDay.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeDay.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeDay.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeDay.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeDay.ColorE = System.Drawing.Color.White;
            this.btnTimeDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeDay.FlatAppearance.BorderSize = 0;
            this.btnTimeDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeDay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeDay.Location = new System.Drawing.Point(0, 0);
            this.btnTimeDay.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeDay.Name = "btnTimeDay";
            this.btnTimeDay.Size = new System.Drawing.Size(35, 36);
            this.btnTimeDay.TabIndex = 8;
            this.btnTimeDay.Text = "일";
            this.btnTimeDay.UseVisualStyleBackColor = false;
            this.btnTimeDay.Click += new System.EventHandler(this.btnTimeDay_Click);
            // 
            // btnTimeWeek
            // 
            this.btnTimeWeek.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeWeek.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeWeek.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeWeek.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeWeek.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeWeek.ColorE = System.Drawing.Color.White;
            this.btnTimeWeek.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeWeek.FlatAppearance.BorderSize = 0;
            this.btnTimeWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeWeek.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeWeek.Location = new System.Drawing.Point(36, 0);
            this.btnTimeWeek.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeWeek.Name = "btnTimeWeek";
            this.btnTimeWeek.Size = new System.Drawing.Size(35, 36);
            this.btnTimeWeek.TabIndex = 9;
            this.btnTimeWeek.Text = "주";
            this.btnTimeWeek.UseVisualStyleBackColor = true;
            this.btnTimeWeek.Click += new System.EventHandler(this.btnTimeWeek_Click);
            // 
            // btnTimeMonth
            // 
            this.btnTimeMonth.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeMonth.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeMonth.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeMonth.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeMonth.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeMonth.ColorE = System.Drawing.Color.White;
            this.btnTimeMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeMonth.FlatAppearance.BorderSize = 0;
            this.btnTimeMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeMonth.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeMonth.Location = new System.Drawing.Point(72, 0);
            this.btnTimeMonth.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeMonth.Name = "btnTimeMonth";
            this.btnTimeMonth.Size = new System.Drawing.Size(35, 36);
            this.btnTimeMonth.TabIndex = 7;
            this.btnTimeMonth.Text = "월";
            this.btnTimeMonth.UseVisualStyleBackColor = true;
            this.btnTimeMonth.Click += new System.EventHandler(this.btnTimeMonth_Click);
            // 
            // btnTimeYear
            // 
            this.btnTimeYear.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeYear.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeYear.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeYear.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeYear.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeYear.ColorE = System.Drawing.Color.White;
            this.btnTimeYear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeYear.FlatAppearance.BorderSize = 0;
            this.btnTimeYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeYear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeYear.Location = new System.Drawing.Point(108, 0);
            this.btnTimeYear.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeYear.Name = "btnTimeYear";
            this.btnTimeYear.Size = new System.Drawing.Size(35, 36);
            this.btnTimeYear.TabIndex = 10;
            this.btnTimeYear.Text = "년";
            this.btnTimeYear.UseVisualStyleBackColor = true;
            this.btnTimeYear.Click += new System.EventHandler(this.btnTimeYear_Click);
            // 
            // btnTimeMin
            // 
            this.btnTimeMin.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeMin.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeMin.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeMin.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeMin.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeMin.ColorE = System.Drawing.Color.White;
            this.btnTimeMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeMin.FlatAppearance.BorderSize = 0;
            this.btnTimeMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeMin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeMin.Location = new System.Drawing.Point(144, 0);
            this.btnTimeMin.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeMin.Name = "btnTimeMin";
            this.btnTimeMin.Size = new System.Drawing.Size(35, 36);
            this.btnTimeMin.TabIndex = 6;
            this.btnTimeMin.Text = "분";
            this.btnTimeMin.UseVisualStyleBackColor = true;
            this.btnTimeMin.Click += new System.EventHandler(this.btnTimeMin_Click);
            // 
            // btnTimeTick
            // 
            this.btnTimeTick.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeTick.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeTick.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeTick.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeTick.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeTick.ColorE = System.Drawing.Color.White;
            this.btnTimeTick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeTick.FlatAppearance.BorderSize = 0;
            this.btnTimeTick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeTick.Location = new System.Drawing.Point(180, 0);
            this.btnTimeTick.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeTick.Name = "btnTimeTick";
            this.btnTimeTick.Size = new System.Drawing.Size(35, 36);
            this.btnTimeTick.TabIndex = 11;
            this.btnTimeTick.Text = "틱";
            this.btnTimeTick.UseVisualStyleBackColor = false;
            this.btnTimeTick.Click += new System.EventHandler(this.btnTimeTick_Click);
            // 
            // btnTimeSec
            // 
            this.btnTimeSec.BackColor = System.Drawing.Color.Transparent;
            this.btnTimeSec.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeSec.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTimeSec.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeSec.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTimeSec.ColorE = System.Drawing.Color.White;
            this.btnTimeSec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeSec.FlatAppearance.BorderSize = 0;
            this.btnTimeSec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimeSec.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTimeSec.Location = new System.Drawing.Point(0, 36);
            this.btnTimeSec.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTimeSec.Name = "btnTimeSec";
            this.btnTimeSec.Size = new System.Drawing.Size(35, 36);
            this.btnTimeSec.TabIndex = 5;
            this.btnTimeSec.Text = "초";
            this.btnTimeSec.UseVisualStyleBackColor = true;
            this.btnTimeSec.Visible = false;
            this.btnTimeSec.Click += new System.EventHandler(this.btnTimeSec_Click);
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.btnAvg);
            this.panelTool.Controls.Add(this.btnZoomIn);
            this.panelTool.Controls.Add(this.btnZoomOut);
            this.panelTool.Controls.Add(this.cmbAvgLineWidth);
            this.panelTool.Location = new System.Drawing.Point(5, 12);
            this.panelTool.Margin = new System.Windows.Forms.Padding(0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(134, 36);
            this.panelTool.TabIndex = 28;
            // 
            // btnAvg
            // 
            this.btnAvg.BackgroundImage = global::ChartCtrl.Properties.Resources.main;
            this.btnAvg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAvg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAvg.Location = new System.Drawing.Point(1, 0);
            this.btnAvg.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnAvg.Name = "btnAvg";
            this.btnAvg.Size = new System.Drawing.Size(36, 36);
            this.btnAvg.TabIndex = 20;
            this.btnAvg.UseVisualStyleBackColor = true;
            this.btnAvg.Click += new System.EventHandler(this.btnAvg_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.White;
            this.btnZoomIn.BackgroundImage = global::ChartCtrl.Properties.Resources.zoomIn_6;
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnZoomIn.Location = new System.Drawing.Point(39, 0);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(36, 36);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.White;
            this.btnZoomOut.BackgroundImage = global::ChartCtrl.Properties.Resources.zoomOut_6;
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnZoomOut.Location = new System.Drawing.Point(77, 0);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(36, 36);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // panelTimeUnit2
            // 
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit01);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit02);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit03);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit1);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit2);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit3);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit4);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit5);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit6);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit7);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit8);
            this.panelTimeUnit2.Controls.Add(this.btnTkUnit9);
            this.panelTimeUnit2.Location = new System.Drawing.Point(381, 12);
            this.panelTimeUnit2.Margin = new System.Windows.Forms.Padding(0);
            this.panelTimeUnit2.Name = "panelTimeUnit2";
            this.panelTimeUnit2.Size = new System.Drawing.Size(528, 36);
            this.panelTimeUnit2.TabIndex = 29;
            // 
            // btnTkUnit01
            // 
            this.btnTkUnit01.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit01.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit01.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit01.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit01.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit01.ColorE = System.Drawing.Color.White;
            this.btnTkUnit01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit01.FlatAppearance.BorderSize = 0;
            this.btnTkUnit01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit01.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit01.Location = new System.Drawing.Point(0, 0);
            this.btnTkUnit01.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit01.Name = "btnTkUnit01";
            this.btnTkUnit01.Size = new System.Drawing.Size(38, 36);
            this.btnTkUnit01.TabIndex = 24;
            this.btnTkUnit01.Text = "1";
            this.btnTkUnit01.UseVisualStyleBackColor = true;
            this.btnTkUnit01.Click += new System.EventHandler(this.btnTkUnit01_Click);
            // 
            // btnTkUnit02
            // 
            this.btnTkUnit02.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit02.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit02.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit02.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit02.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit02.ColorE = System.Drawing.Color.White;
            this.btnTkUnit02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit02.FlatAppearance.BorderSize = 0;
            this.btnTkUnit02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit02.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit02.Location = new System.Drawing.Point(39, 0);
            this.btnTkUnit02.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit02.Name = "btnTkUnit02";
            this.btnTkUnit02.Size = new System.Drawing.Size(38, 36);
            this.btnTkUnit02.TabIndex = 25;
            this.btnTkUnit02.Text = "15";
            this.btnTkUnit02.UseVisualStyleBackColor = true;
            this.btnTkUnit02.Click += new System.EventHandler(this.btnTkUnit02_Click);
            // 
            // btnTkUnit03
            // 
            this.btnTkUnit03.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit03.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit03.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit03.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit03.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit03.ColorE = System.Drawing.Color.White;
            this.btnTkUnit03.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit03.FlatAppearance.BorderSize = 0;
            this.btnTkUnit03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit03.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit03.Location = new System.Drawing.Point(78, 0);
            this.btnTkUnit03.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit03.Name = "btnTkUnit03";
            this.btnTkUnit03.Size = new System.Drawing.Size(38, 36);
            this.btnTkUnit03.TabIndex = 26;
            this.btnTkUnit03.Text = "30";
            this.btnTkUnit03.UseVisualStyleBackColor = true;
            this.btnTkUnit03.Click += new System.EventHandler(this.btnTkUnit03_Click);
            // 
            // btnTkUnit1
            // 
            this.btnTkUnit1.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit1.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit1.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit1.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit1.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit1.ColorE = System.Drawing.Color.White;
            this.btnTkUnit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit1.FlatAppearance.BorderSize = 0;
            this.btnTkUnit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit1.Location = new System.Drawing.Point(117, 0);
            this.btnTkUnit1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit1.Name = "btnTkUnit1";
            this.btnTkUnit1.Size = new System.Drawing.Size(38, 36);
            this.btnTkUnit1.TabIndex = 12;
            this.btnTkUnit1.Text = "60";
            this.btnTkUnit1.UseVisualStyleBackColor = true;
            this.btnTkUnit1.Click += new System.EventHandler(this.btnTkUnit1_Click);
            // 
            // btnTkUnit2
            // 
            this.btnTkUnit2.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit2.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit2.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit2.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit2.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit2.ColorE = System.Drawing.Color.White;
            this.btnTkUnit2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit2.FlatAppearance.BorderSize = 0;
            this.btnTkUnit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit2.Location = new System.Drawing.Point(156, 0);
            this.btnTkUnit2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit2.Name = "btnTkUnit2";
            this.btnTkUnit2.Size = new System.Drawing.Size(38, 36);
            this.btnTkUnit2.TabIndex = 16;
            this.btnTkUnit2.Text = "90";
            this.btnTkUnit2.UseVisualStyleBackColor = true;
            this.btnTkUnit2.Click += new System.EventHandler(this.btnTkUnit2_Click);
            // 
            // btnTkUnit3
            // 
            this.btnTkUnit3.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit3.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit3.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit3.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit3.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit3.ColorE = System.Drawing.Color.White;
            this.btnTkUnit3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit3.FlatAppearance.BorderSize = 0;
            this.btnTkUnit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit3.Location = new System.Drawing.Point(195, 0);
            this.btnTkUnit3.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit3.Name = "btnTkUnit3";
            this.btnTkUnit3.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit3.TabIndex = 17;
            this.btnTkUnit3.Text = "120";
            this.btnTkUnit3.UseVisualStyleBackColor = true;
            this.btnTkUnit3.Click += new System.EventHandler(this.btnTkUnit3_Click);
            // 
            // btnTkUnit4
            // 
            this.btnTkUnit4.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit4.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit4.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit4.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit4.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit4.ColorE = System.Drawing.Color.White;
            this.btnTkUnit4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit4.FlatAppearance.BorderSize = 0;
            this.btnTkUnit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTkUnit4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit4.Location = new System.Drawing.Point(240, 0);
            this.btnTkUnit4.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit4.Name = "btnTkUnit4";
            this.btnTkUnit4.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit4.TabIndex = 15;
            this.btnTkUnit4.Text = "240";
            this.btnTkUnit4.UseVisualStyleBackColor = true;
            this.btnTkUnit4.Click += new System.EventHandler(this.btnTkUnit4_Click);
            // 
            // btnTkUnit5
            // 
            this.btnTkUnit5.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit5.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit5.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit5.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit5.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit5.ColorE = System.Drawing.Color.White;
            this.btnTkUnit5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit5.FlatAppearance.BorderSize = 0;
            this.btnTkUnit5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit5.Location = new System.Drawing.Point(285, 0);
            this.btnTkUnit5.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit5.Name = "btnTkUnit5";
            this.btnTkUnit5.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit5.TabIndex = 18;
            this.btnTkUnit5.Text = "350";
            this.btnTkUnit5.UseVisualStyleBackColor = true;
            this.btnTkUnit5.Click += new System.EventHandler(this.btnTkUnit5_Click);
            // 
            // btnTkUnit6
            // 
            this.btnTkUnit6.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit6.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit6.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit6.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit6.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit6.ColorE = System.Drawing.Color.White;
            this.btnTkUnit6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit6.FlatAppearance.BorderSize = 0;
            this.btnTkUnit6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit6.Location = new System.Drawing.Point(330, 0);
            this.btnTkUnit6.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit6.Name = "btnTkUnit6";
            this.btnTkUnit6.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit6.TabIndex = 14;
            this.btnTkUnit6.Text = "400";
            this.btnTkUnit6.UseVisualStyleBackColor = true;
            this.btnTkUnit6.Click += new System.EventHandler(this.btnTkUnit6_Click);
            // 
            // btnTkUnit7
            // 
            this.btnTkUnit7.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit7.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit7.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit7.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit7.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit7.ColorE = System.Drawing.Color.White;
            this.btnTkUnit7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit7.FlatAppearance.BorderSize = 0;
            this.btnTkUnit7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTkUnit7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit7.Location = new System.Drawing.Point(375, 0);
            this.btnTkUnit7.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit7.Name = "btnTkUnit7";
            this.btnTkUnit7.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit7.TabIndex = 13;
            this.btnTkUnit7.Text = "600";
            this.btnTkUnit7.UseVisualStyleBackColor = true;
            this.btnTkUnit7.Click += new System.EventHandler(this.btnTkUnit7_Click);
            // 
            // btnTkUnit8
            // 
            this.btnTkUnit8.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit8.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit8.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit8.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit8.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit8.ColorE = System.Drawing.Color.White;
            this.btnTkUnit8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit8.FlatAppearance.BorderSize = 0;
            this.btnTkUnit8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTkUnit8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit8.Location = new System.Drawing.Point(420, 0);
            this.btnTkUnit8.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit8.Name = "btnTkUnit8";
            this.btnTkUnit8.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit8.TabIndex = 22;
            this.btnTkUnit8.Text = "750";
            this.btnTkUnit8.UseVisualStyleBackColor = true;
            this.btnTkUnit8.Click += new System.EventHandler(this.btnTkUnit8_Click);
            // 
            // btnTkUnit9
            // 
            this.btnTkUnit9.BackColor = System.Drawing.Color.Transparent;
            this.btnTkUnit9.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit9.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnTkUnit9.ColorC = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit9.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnTkUnit9.ColorE = System.Drawing.Color.White;
            this.btnTkUnit9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTkUnit9.FlatAppearance.BorderSize = 0;
            this.btnTkUnit9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkUnit9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTkUnit9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTkUnit9.Location = new System.Drawing.Point(465, 0);
            this.btnTkUnit9.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnTkUnit9.Name = "btnTkUnit9";
            this.btnTkUnit9.Size = new System.Drawing.Size(44, 36);
            this.btnTkUnit9.TabIndex = 23;
            this.btnTkUnit9.Text = "990";
            this.btnTkUnit9.UseVisualStyleBackColor = true;
            this.btnTkUnit9.Click += new System.EventHandler(this.btnTkUnit9_Click);
            // 
            // panelGraphType
            // 
            this.panelGraphType.Controls.Add(this.btnStyleCandle);
            this.panelGraphType.Controls.Add(this.btnStyleLine);
            this.panelGraphType.Location = new System.Drawing.Point(955, 12);
            this.panelGraphType.Margin = new System.Windows.Forms.Padding(0);
            this.panelGraphType.Name = "panelGraphType";
            this.panelGraphType.Size = new System.Drawing.Size(126, 36);
            this.panelGraphType.TabIndex = 30;
            // 
            // rChartCtrl
            // 
            this.rChartCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rChartCtrl.ChartStyle = 0;
            this.rChartCtrl.GridRuler = true;
            this.rChartCtrl.Location = new System.Drawing.Point(0, 74);
            this.rChartCtrl.Name = "rChartCtrl";
            this.rChartCtrl.SaveRtVal = false;
            this.rChartCtrl.Size = new System.Drawing.Size(0, 0);
            this.rChartCtrl.TabIndex = 0;
            this.rChartCtrl.Text = "rChartCtrl1";
            this.rChartCtrl.UnitTimeCount = ChartCtrl.TIMEUNIT.TIMEUNIT_1;
            this.rChartCtrl.UnitTimeType = ChartCtrl.TIMETYPE.TIMETYPE_MIN;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtSpec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpec.Location = new System.Drawing.Point(5, 50);
            this.txtSpec.Multiline = false;
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(491, 20);
            this.txtSpec.TabIndex = 31;
            this.txtSpec.Text = "";
            // 
            // ChartPanel
            // 
            this.Controls.Add(this.txtSpec);
            this.Controls.Add(this.panelTimeUnit1);
            this.Controls.Add(this.panelTimeUnit2);
            this.Controls.Add(this.panelGraphType);
            this.Controls.Add(this.panelTool);
            this.Controls.Add(this.panelTimeType);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.rChartCtrl);
            this.panelTimeUnit1.ResumeLayout(false);
            this.panelTimeType.ResumeLayout(false);
            this.panelTool.ResumeLayout(false);
            this.panelTimeUnit2.ResumeLayout(false);
            this.panelGraphType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RChartCtrl rChartCtrl;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnStyleCandle;
        private System.Windows.Forms.Button btnStyleLine;
        private ReaLTaiizor.Controls.DreamButton btnTimeSec;
        private ReaLTaiizor.Controls.DreamButton btnTimeMin;
        private ReaLTaiizor.Controls.DreamButton btnTimeMonth;
        private ReaLTaiizor.Controls.DreamButton btnTimeDay;
        private ReaLTaiizor.Controls.DreamButton btnTimeWeek;
        private ReaLTaiizor.Controls.DreamButton btnTimeYear;
        private ReaLTaiizor.Controls.DreamButton btnTimeTick;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit01;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit02;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit03;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit1;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit5;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit3;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit2;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit4;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit6;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit7;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit8;
        private ReaLTaiizor.Controls.DreamButton btnTkUnit9;
        private HScrollBar hScrollBar;
        private Button btnAvg;
        private ComboBox cmbAvgLineWidth;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit1;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit7;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit6;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit2;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit5;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit3;
        private ReaLTaiizor.Controls.DreamButton btnTmUnit4;
        private FlowLayoutPanel panelTimeUnit1;
        private FlowLayoutPanel panelTimeType;
        private FlowLayoutPanel panelTool;
        private FlowLayoutPanel panelTimeUnit2;
        private FlowLayoutPanel panelGraphType;
        private RichTextBox txtSpec;
    }
}
