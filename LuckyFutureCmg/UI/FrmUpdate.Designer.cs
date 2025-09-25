
namespace LuckyFuture.UI
{
    partial class FrmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdate));
            this.lbLogFile = new System.Windows.Forms.Label();
            this.progFile = new System.Windows.Forms.ProgressBar();
            this.progTotal = new System.Windows.Forms.ProgressBar();
            this.lbLogTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbLogFile
            // 
            this.lbLogFile.AutoSize = true;
            this.lbLogFile.Font = new System.Drawing.Font("Gulim", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLogFile.Location = new System.Drawing.Point(16, 19);
            this.lbLogFile.Name = "lbLogFile";
            this.lbLogFile.Size = new System.Drawing.Size(53, 14);
            this.lbLogFile.TabIndex = 0;
            this.lbLogFile.Text = "Update";
            // 
            // progFile
            // 
            this.progFile.Location = new System.Drawing.Point(15, 44);
            this.progFile.Name = "progFile";
            this.progFile.Size = new System.Drawing.Size(363, 30);
            this.progFile.TabIndex = 1;
            this.progFile.Value = 70;
            // 
            // progTotal
            // 
            this.progTotal.Location = new System.Drawing.Point(14, 118);
            this.progTotal.Name = "progTotal";
            this.progTotal.Size = new System.Drawing.Size(363, 30);
            this.progTotal.TabIndex = 2;
            this.progTotal.Value = 70;
            // 
            // lbLogTotal
            // 
            this.lbLogTotal.AutoSize = true;
            this.lbLogTotal.Font = new System.Drawing.Font("Gulim", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLogTotal.Location = new System.Drawing.Point(16, 91);
            this.lbLogTotal.Name = "lbLogTotal";
            this.lbLogTotal.Size = new System.Drawing.Size(53, 14);
            this.lbLogTotal.TabIndex = 3;
            this.lbLogTotal.Text = "Update";
            // 
            // FrmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 168);
            this.Controls.Add(this.lbLogTotal);
            this.Controls.Add(this.progTotal);
            this.Controls.Add(this.progFile);
            this.Controls.Add(this.lbLogFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.Text = "업데이트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUpdate_FormClosing);
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbLogFile;
        private System.Windows.Forms.ProgressBar progFile;
        private System.Windows.Forms.ProgressBar progTotal;
        private System.Windows.Forms.Label lbLogTotal;
    }
}