
namespace XShortCoreIndex
{
    partial class BackgroundActivity
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
            this.timerBackgroundCheck = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerBackgroundCheck
            // 
            this.timerBackgroundCheck.Enabled = true;
            this.timerBackgroundCheck.Interval = 60000;
            this.timerBackgroundCheck.Tick += new System.EventHandler(this.timerBackgroundCheck_Tick);
            // 
            // BackgroundActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 6);
            this.Name = "BackgroundActivity";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "XShort Background Process";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BackgroundActivity_FormClosing);
            this.Load += new System.EventHandler(this.BackgroundActivity_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerBackgroundCheck;
    }
}

