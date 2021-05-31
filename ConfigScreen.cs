// MIT License

// Copyright (c) 2021 Arthurdw

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// ---------------------------------------------------------------------------- //
//                                    ABOUT                                     //
//                                                                              //
//  This project was created for my final exams in GO-AO informatics (6INFO).   //
//  The task was to create a project which is a playable game in the .NET       //
//  framework, which utilizes the MySQL.Data dll.                               //
//                                                                              //
//  The project was finished on the 31'st of may 2021.                          //
//                                                                              //
// ---------------------------------------------------------------------------- //

using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

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
            this.Height = Config.IsAdmin ? 357 : 310;
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

        private void BtnClearDatabase_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(@"This action is irreversible and will delete all user data! (so no admin data) Are you sure you want to continue?", @"WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

            if (res == DialogResult.OK)
            {
                MySqlCommand cmd =
                    this._mySqlHandler.Prepare("DELETE FROM EX2_space_invaders_accounts WHERE is_admin = 0;");
                this._mySqlHandler.Execute(cmd);
            }
        }
    }
}