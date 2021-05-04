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
    public partial class Statistics : Form
    {
        private readonly MySqlHandler _mySqlHandler;
        public Statistics(MySqlHandler handler)
        {
            InitializeComponent();
            this._mySqlHandler = handler;
        }

        private void PersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = this._mySqlHandler.Prepare("SELECT name, score, registered_at FROM EX2_space_invaders_accounts, EX2_space_invaders_scores WHERE owner = EX2_space_invaders_accounts.id;");

        }
    }
}
