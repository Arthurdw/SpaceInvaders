
namespace SpaceInvaders
{
    partial class ConfigScreen
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
            this.btnResetPass = new System.Windows.Forms.Button();
            this.txtNewPass = new System.Windows.Forms.TextBox();
            this.btnDeleteAccount = new System.Windows.Forms.Button();
            this.btnTheme1 = new System.Windows.Forms.Button();
            this.btnTheme2 = new System.Windows.Forms.Button();
            this.btnTheme3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnResetPass
            // 
            this.btnResetPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.btnResetPass.Location = new System.Drawing.Point(202, 194);
            this.btnResetPass.Name = "btnResetPass";
            this.btnResetPass.Size = new System.Drawing.Size(184, 26);
            this.btnResetPass.TabIndex = 0;
            this.btnResetPass.Text = "Reset Password";
            this.btnResetPass.UseVisualStyleBackColor = true;
            this.btnResetPass.Click += new System.EventHandler(this.BtnResetPass_Click);
            // 
            // txtNewPass
            // 
            this.txtNewPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtNewPass.Location = new System.Drawing.Point(12, 194);
            this.txtNewPass.Name = "txtNewPass";
            this.txtNewPass.PasswordChar = '●';
            this.txtNewPass.Size = new System.Drawing.Size(184, 26);
            this.txtNewPass.TabIndex = 1;
            // 
            // btnDeleteAccount
            // 
            this.btnDeleteAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.btnDeleteAccount.Location = new System.Drawing.Point(12, 226);
            this.btnDeleteAccount.Name = "btnDeleteAccount";
            this.btnDeleteAccount.Size = new System.Drawing.Size(374, 34);
            this.btnDeleteAccount.TabIndex = 2;
            this.btnDeleteAccount.Text = "DELETE ACCOUNT";
            this.btnDeleteAccount.UseVisualStyleBackColor = true;
            this.btnDeleteAccount.Click += new System.EventHandler(this.BtnDeleteAccount_Click);
            // 
            // btnTheme1
            // 
            this.btnTheme1.Location = new System.Drawing.Point(12, 38);
            this.btnTheme1.Name = "btnTheme1";
            this.btnTheme1.Size = new System.Drawing.Size(120, 34);
            this.btnTheme1.TabIndex = 3;
            this.btnTheme1.UseVisualStyleBackColor = true;
            this.btnTheme1.Click += new System.EventHandler(this.BtnTheme1_Click);
            // 
            // btnTheme2
            // 
            this.btnTheme2.Location = new System.Drawing.Point(138, 38);
            this.btnTheme2.Name = "btnTheme2";
            this.btnTheme2.Size = new System.Drawing.Size(122, 34);
            this.btnTheme2.TabIndex = 4;
            this.btnTheme2.UseVisualStyleBackColor = true;
            this.btnTheme2.Click += new System.EventHandler(this.BtnTheme2_Click);
            // 
            // btnTheme3
            // 
            this.btnTheme3.Location = new System.Drawing.Point(266, 38);
            this.btnTheme3.Name = "btnTheme3";
            this.btnTheme3.Size = new System.Drawing.Size(120, 34);
            this.btnTheme3.TabIndex = 5;
            this.btnTheme3.UseVisualStyleBackColor = true;
            this.btnTheme3.Click += new System.EventHandler(this.BtnTheme3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Color Scheme:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label2.Location = new System.Drawing.Point(13, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Account:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label3.Location = new System.Drawing.Point(13, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Responsiveness:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button1.Location = new System.Drawing.Point(12, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(374, 34);
            this.button1.TabIndex = 9;
            this.button1.Text = "Disable Responsiveness";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ConfigScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 274);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTheme3);
            this.Controls.Add(this.btnTheme2);
            this.Controls.Add(this.btnTheme1);
            this.Controls.Add(this.btnDeleteAccount);
            this.Controls.Add(this.txtNewPass);
            this.Controls.Add(this.btnResetPass);
            this.Name = "ConfigScreen";
            this.Text = "Client Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnResetPass;
        private System.Windows.Forms.TextBox txtNewPass;
        private System.Windows.Forms.Button btnDeleteAccount;
        private System.Windows.Forms.Button btnTheme1;
        private System.Windows.Forms.Button btnTheme2;
        private System.Windows.Forms.Button btnTheme3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}