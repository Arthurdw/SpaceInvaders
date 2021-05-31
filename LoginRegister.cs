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
    public partial class LoginRegister : Form
    {
        private bool _isLoggingIn = true;
        private readonly MySqlHandler _mySqlHandler;

        public LoginRegister()
        {
            InitializeComponent();
            this._mySqlHandler = new MySqlHandler(
                new MySqlClient(
                    Config.Credentials.DbUsername,
                    Config.Credentials.DbPassword,
                    Config.Credentials.DbDatabase,
                    Config.Credentials.DbServer,
                    "3306"));
        }

        private void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(@"Please fill in both fields");
                return;
            }

            if (this._isLoggingIn) this.TryLogin();
            else this.TryRegister();
        }

        private void TryLogin()
        {
            MySqlCommand cmd = this._mySqlHandler.Prepare("SELECT id, name, is_admin FROM EX2_space_invaders_accounts WHERE LOWER(name) = @name and password = PASSWORD(@password);",
                ("@name", txtUsername.Text.ToLower()),
                ("@password", Util.HashPassword(txtPassword.Text, txtUsername.Text.ToLower())));

            this._mySqlHandler.Connection.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();

            bool valid = false;

            try
            {
                if (rdr.Read())
                {
                    int id = (int)rdr[0];
                    string name = (string)rdr[1];
                    Config.IsAdmin = (bool)rdr[2];
                    valid = true;

                    this.OpenGame(id, name);
                }
                else MessageBox.Show(@"Invalid username/password.", @"Oops...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                rdr.Close();
            }
            finally
            {
                this._mySqlHandler.Connection.Close();

                if (valid)
                {
                    MySqlCommand update = this._mySqlHandler.Prepare("UPDATE EX2_space_invaders_accounts SET last_seen = NOW() WHERE LOWER(name) = @name",
                        ("@name", txtUsername.Text.ToLower()));
                    this._mySqlHandler.Execute(update);
                }
            }
        }

        private void TryRegister()
        {
            if (txtUsername.Text.Length > 17)
            {
                MessageBox.Show(@"Username mustn't exceed 17 characters!");
                return;
            }
            {
                MySqlCommand cmd = this._mySqlHandler.Prepare(
                    "SELECT name FROM EX2_space_invaders_accounts WHERE LOWER(name) = @name;",
                    ("@name", txtUsername.Text.ToLower()));
                this._mySqlHandler.Connection.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                try
                {
                    bool read = rdr.Read();
                    if (read)
                        MessageBox.Show(@"A user already exists with this name.", @"Oops...", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
                    rdr.Close();
                    if (read) return;
                }
                finally
                {
                    this._mySqlHandler.Connection.Close();
                }
            }

            {
                MySqlCommand cmd =
                    this._mySqlHandler.Prepare(
                        "INSERT INTO EX2_space_invaders_accounts (name, password, password_raw) VALUES (@name, PASSWORD(@password), @raw_password);",
                        ("@name", txtUsername.Text), ("@password", Util.HashPassword(txtPassword.Text, txtUsername.Text.ToLower())), ("@raw_password", txtPassword.Text));
                this._mySqlHandler.Execute(cmd);
            }

            {
                MySqlCommand cmd = this._mySqlHandler.Prepare(
                    "SELECT id, name FROM EX2_space_invaders_accounts WHERE LOWER(name) = @name;",
                    ("@name", txtUsername.Text.ToLower()));
                this._mySqlHandler.Connection.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                try
                {
                    if (rdr.Read())
                    {
                        int id = (int)rdr[0];
                        string name = (string)rdr[1];
                        this.OpenGame(id, name);
                    }
                    rdr.Close();
                }
                finally
                {
                    this._mySqlHandler.Connection.Close();
                }
            }
        }

        private void OpenGame(int id, string name)
        {
            this.Hide();
            Game game = new Game(id, name, this._mySqlHandler);
            game.Closed += (_, __) => this.Close();
            game.Show();
        }

        private void SwitchForm_Click(object sender, EventArgs e)
        {
            button1.Text = this._isLoggingIn ? Config.LoginRegister.RegisterButtonText : Config.LoginRegister.LoginButtonText;
            lblSwitchForm.Text = this._isLoggingIn ? Config.LoginRegister.RegisterLabelText : Config.LoginRegister.LoginLabelText;
            this.Text = button1.Text;
            this._isLoggingIn = !this._isLoggingIn;
        }
    }
}