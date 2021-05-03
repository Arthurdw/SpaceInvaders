using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class LoginRegister : Form
    {
        private bool isLoggingIn = true;
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

            if (this.isLoggingIn) this.TryLogin();
            else this.TryRegister();
        }

        private void TryLogin()
        {
            MySqlCommand cmd = this._mySqlHandler.Prepare("SELECT id, name FROM EX2_space_invaders_accounts WHERE LOWER(name) = @name and password = PASSWORD(@password);",
                ("@name", txtUsername.Text.ToLower()),
                ("@password", this.HashPassword(txtPassword.Text, txtUsername.Text.ToLower())));

            this._mySqlHandler.Connection.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();

            bool valid = false;

            try
            {
                if (rdr.Read())
                {
                    int id = (int)rdr[0];
                    string name = (string)rdr[1];
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
                        ("@name", txtUsername.Text), ("@password", this.HashPassword(txtPassword.Text, txtUsername.Text.ToLower())), ("@raw_password", txtPassword.Text));
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

        private string HashPassword(string pwd, string username)
        {
            pwd = $"5p4c3 1nv4d3r5::{pwd}::{username}";

            byte[] dt = Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < dt.Length; i++)
                dt[i] = (byte)(~dt[i] | dt[i] >> 2 & dt[i] << 2);

            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(dt);

                StringBuilder sb = new StringBuilder();
                foreach (byte t in bytes)
                    sb.Append(t.ToString("x"));

                return sb.ToString();
            }
        }

        private void SwitchForm_Click(object sender, EventArgs e)
        {
            button1.Text = this.isLoggingIn ? Config.LoginRegister.RegisterButtonText : Config.LoginRegister.LoginButtonText;
            lblSwitchForm.Text = this.isLoggingIn ? Config.LoginRegister.RegisterLabelText : Config.LoginRegister.LoginLabelText;
            this.Text = button1.Text;
            this.isLoggingIn = !this.isLoggingIn;
        }
    }
}