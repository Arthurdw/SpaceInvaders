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

        private void FillGridWithSQl(bool filter, string sql, params (string, object)[] parameters)
        {
            txtFilter.Enabled = filter;
            cbFilters.Enabled = filter;

            MySqlCommand cmd = this._mySqlHandler.Prepare(sql, parameters);
            MySqlDataAdapter mda = new MySqlDataAdapter { SelectCommand = cmd };
            DataTable dt = new DataTable();

            this._mySqlHandler.Connection.Open();
            mda.Fill(dt);
            this._mySqlHandler.Connection.Close();

            BindingSource bs = new BindingSource { DataSource = dt };
            dgv.DataSource = bs;
        }

        private void FillWithPersonalScores(DateTime laterThan)
        => this.FillGridWithSQl(true, "SELECT score AS 'Score', registered_at AS 'Registered At' FROM EX2_space_invaders_scores WHERE owner = @id AND registered_at >= @date ORDER BY registered_at DESC;", ("@id", Config.Id), ("@date", laterThan));

        private void PersonalToolStripMenuItem_Click(object sender, EventArgs e)
            => FillWithPersonalScores(DateTime.MinValue);

        private void FillTop(int amount)
            => this.FillGridWithSQl(false, "SELECT name AS 'Name', score AS 'Score', registered_at AS 'Registered At' FROM EX2_space_invaders_accounts, EX2_space_invaders_scores WHERE owner = EX2_space_invaders_accounts.id ORDER BY score DESC LIMIT @amount;", ("@amount", amount));

        private void Top5ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(5);

        private void Top10ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(10);

        private void Top100ToolStripMenuItem_Click(object sender, EventArgs e)
            => FillTop(100);

        private void PerformFilter()
        {
            if (string.IsNullOrEmpty(txtFilter.Text) || cbFilters.SelectedIndex == 0)
            {
                this.FillWithPersonalScores(DateTime.MinValue);
                return;
            }

            int val = 0;

            try
            {
                val = (int)uint.Parse(txtFilter.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Length value for filter is invalid. (not a valid number)");
            }
            catch (Exception e) when (e is ArgumentNullException || e is OverflowException)
            {
                MessageBox.Show(@"Length value for filter is invalid. (no value was given or value was too big)");
            }

            DateTime GetDatetimeByFilter(int from)
            {
                //    1          2       3       4         5        6
                // "minutes", "hours", "days", "weeks", "months", "years"
                switch (cbFilters.SelectedIndex)
                {
                    case 1:
                        return DateTime.UtcNow.AddMinutes(-from);

                    case 2:
                        return DateTime.UtcNow.AddHours(-from);

                    case 3:
                        return DateTime.UtcNow.AddDays(-from);

                    case 4:
                        return DateTime.UtcNow.AddDays(-(from * 7));

                    case 5:
                        return DateTime.UtcNow.AddMonths(-from);

                    case 6:
                        return DateTime.UtcNow.AddYears(-from);

                    default:
                        throw new NotImplementedException($@"{cbFilters.SelectedIndex} is not a valid index!");
                }
            }

            this.FillWithPersonalScores(GetDatetimeByFilter(val));
        }

        private void CbFilters_SelectedIndexChanged(object sender, EventArgs e)
            => PerformFilter();

        private void TxtFilter_TextChanged(object sender, EventArgs e)
            => PerformFilter();
    }
}