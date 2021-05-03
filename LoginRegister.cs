using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SpaceInvaders
{
    public partial class LoginRegister : Form
    {
        private MySqlHandler _mySqlHandler;
        public LoginRegister()
        {
            InitializeComponent();
            this._mySqlHandler = new MySqlHandler(
                new MySqlClient(
                    Config.Credentials.DB_Username,
                    Config.Credentials.DB_Password,
                    Config.Credentials.DB_Database,
                    Config.Credentials.DB_Server,
                    "3306"));
        }

        private void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(@"Please fill in both fields");
                return;
            }
            
            MySqlCommand cmd = this._mySqlHandler.Prepare("SELECT id, name FROM space_invaders_accounts WHERE name=")
        }

        private string HashString(string str)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

                StringBuilder sb = new StringBuilder();
                foreach (byte t in bytes)
                    sb.Append(t.ToString("x"));

                return sb.ToString();
            }
        }
    }
}
