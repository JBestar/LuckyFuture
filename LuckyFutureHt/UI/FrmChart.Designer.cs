
namespace LuckyFuture.UI
{
	partial class FrmChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            this.chartFuture = new ChartCtrl.ChartPanel();
            this.SuspendLayout();
            // 
            // chartFuture
            // 
            this.chartFuture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartFuture.ChartStyle = 0;
            this.chartFuture.GridRuler = true;
            this.chartFuture.Location = new System.Drawing.Point(8, -1);
            this.chartFuture.Margin = new System.Windows.Forms.Padding(2);
            this.chartFuture.Name = "chartFuture";
            this.chartFuture.SaveRtVal = false;
            this.chartFuture.Size = new System.Drawing.Size(1083, 487);
            this.chartFuture.TabIndex = 0;
            this.chartFuture.Text = "chartPanel1";
//             this.chartFuture.UnitTimeCount = ChartCtrl.TIMEUNIT.TIMEUNIT_60;
//             this.chartFuture.UnitTimeType = ChartCtrl.TIMETYPE.TIMETYPE_TICK;
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1099, 492);
            this.Controls.Add(this.chartFuture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "FrmChart";
            this.Text = "차트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmChat_FormClosing);
            this.Load += new System.EventHandler(this.FrmChat_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private ChartCtrl.ChartPanel chartFuture;
	}
}