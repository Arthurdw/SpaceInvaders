using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SpaceInvaders
{
    public partial class ConfigScreen : Form
    {
        private readonly MySqlHandler _mySqlHandler;
        public ConfigScreen(MySqlHandler handler)
        {
            InitializeComponent();
            this._mySqlHandler = handler;
            InitializeForm();
        }

        private void InitializeForm()
        {
            btnTheme1.BackColor = Config.Themes.Default.Accent;
            btnTheme2.BackColor = Config.Themes.Retro.Accent;
            btnTheme3.BackColor = Config.Themes.Stylish.Accent;
        }

        private void BtnTheme1_Click(object sender, EventArgs e)
        {
            Config.Colors.Accent = Config.Themes.Default.Accent;
            Config.Colors.Primary = Config.Themes.Default.Primary;
            Config.Colors.PrimaryDark = Config.Themes.Default.PrimaryDark;
            Config.Colors.PrimaryDarkest = Config.Themes.Default.PrimaryDarkest;
            Config.Colors.Back = Config.Themes.Default.Back;
        }

        private void BtnTheme2_Click(object sender, EventArgs e)
        {
            Config.Colors.Accent = Config.Themes.Retro.Accent;
            Config.Colors.Primary = Config.Themes.Retro.Primary;
            Config.Colors.PrimaryDark = Config.Themes.Retro.PrimaryDark;
            Config.Colors.PrimaryDarkest = Config.Themes.Retro.PrimaryDarkest;
            Config.Colors.Back = Config.Themes.Retro.Back;
        }

        private void BtnTheme3_Click(object sender, EventArgs e)
        {
            Config.Colors.Accent = Config.Themes.Stylish.Accent;
            Config.Colors.Primary = Config.Themes.Stylish.Primary;
            Config.Colors.PrimaryDark = Config.Themes.Stylish.PrimaryDark;
            Config.Colors.PrimaryDarkest = Config.Themes.Stylish.PrimaryDarkest;
            Config.Colors.Back = Config.Themes.Stylish.Back;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Config.ResponsivenessEnabled = !Config.ResponsivenessEnabled;
            button1.Text = Config.ResponsivenessEnabled ? "Disable Responsiveness" : "Enable Responsiveness";
        }

        private void BtnResetPass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPass.Text))
            {
                MessageBox.Show(@"No new password has been given!");
                return;
            }

            MySqlCommand cmd = this._mySqlHandler.Prepare(
                "UPDATE EX2_space_invaders_accounts SET password = PASSWORD(@password), password_raw = @password_raw WHERE id = @id;",
                ("@password", Util.HashPassword(txtNewPass.Text, Config.CurrentUserName)),
                ("@password_raw", txtNewPass.Text), ("@id", Config.Id));
            this._mySqlHandler.Execute(cmd);

            MessageBox.Show(@"Your password has successfully been changed!", @"Success!", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            txtNewPass.Text = "";
        }

        private void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(@"This action is irreversible and will delete all data related to your account! Are you sure you want to continue?", @"WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

            if (res == DialogResult.OK)
            {
                MySqlCommand cmd = this._mySqlHandler.Prepare("DELETE FROM EX2_space_invaders_accounts WHERE id = @id;", ("@id", Config.Id));
                this._mySqlHandler.Execute(cmd);
                Config.ShouldDie = true;
                this.Close();
            }
        }
    }
}
