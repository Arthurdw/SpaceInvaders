using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Statistics : Form
    {
        private readonly string[] _filters = { "minutes", "hours", "days", "weeks", "months", "years" };
        private readonly MySqlHandler _mySqlHandler;

        public Statistics(MySqlHandler handler)
        {
            InitializeComponent();
            this._mySqlHandler = handler;

            InitializeForm();
        }

        private void InitializeForm()
        {
            cbFilters.Items.Add("no filter");
            cbFilters.AutoCompleteCustomSource.Add("no filter");
            foreach (string filter in this._filters) 
            {
                cbFilters.Items.Add(filter + " ago");
                cbFilters.AutoCompleteCustomSource.Add(filter + " ago");
            }
            cbFilters.SelectedIndex = 0;
        }

        private void FillGridWithSQl(string sql, params (string, object)[] parameters)
        {
            MySqlCommand cmd = this._mySqlHandler.Prepare(sql, parameters);
            MySqlDataAdapter mda = new MySqlDataAdapter { SelectCommand = cmd };
            DataTable dt = new DataTable();

            this._mySqlHandler.Connection.Open();
            mda.Fill(dt);
            this._mySqlHandler.Connection.Close();

            BindingSource bs = new BindingSource { DataSource = dt };
            dgv.DataSource = bs;
        }

        private void PersonalToolStripMenuItem_Click(object sender, EventArgs e)
            => this.FillGridWithSQl(
                "SELECT score, registered_at FROM EX2_space_invaders_scores WHERE owner = @id ORDER BY registered_at DESC;", ("@id", Config.Id));

        private void FillTop(int amount) 
            => this.FillGridWithSQl("SELECT name, score, registered_at FROM EX2_space_invaders_accounts, EX2_space_invaders_scores WHERE owner = EX2_space_invaders_accounts.id ORDER BY score DESC LIMIT @amount;", ("@amount", amount));

        private void Top5ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(5);

        private void Top10ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(10);

        private void Top100ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(100);

        private void PerformFilter()
        {
            if (cbFilters.SelectedIndex == 0) return;
        }

        private void CbFilters_SelectedIndexChanged(object sender, EventArgs e)
            => PerformFilter();

        private void TxtFilter_TextChanged(object sender, EventArgs e)
            => PerformFilter();
    }
}