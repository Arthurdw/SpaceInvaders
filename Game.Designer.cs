
namespace SpaceInvaders
{
    partial class Game
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
            this.pnl = new System.Windows.Forms.Panel();
            this.FrameHandler = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pnl
            // 
            this.pnl.Location = new System.Drawing.Point(0, 0);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(750, 750);
            this.pnl.TabIndex = 0;
            this.pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPanel);
            // 
            // FrameHandler
            // 
            this.FrameHandler.Tick += new System.EventHandler(this.FrameHandler_Tick);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 751);
            this.Controls.Add(this.pnl);
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Space Invaders";
            this.SizeChanged += new System.EventHandler(this.HandleWindowContentLocation);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleFormKeydown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.Timer FrameHandler;
    }
}

