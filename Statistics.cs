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
            catch (Exception e) when (e is ArgumentNullException or OverflowException)
            {
                MessageBox.Show(@"Length value for filter is invalid. (no value was given or value was too big)");
            }

            DateTime GetDatetimeByFilter(int from)
            {
                //    1          2       3       4         5        6
                // "minutes", "hours", "days", "weeks", "months", "years"
                return cbFilters.SelectedIndex switch
                {
                    1 => DateTime.UtcNow.AddMinutes(-@from),
                    2 => DateTime.UtcNow.AddHours(-@from),
                    3 => DateTime.UtcNow.AddDays(-@from),
                    4 => DateTime.UtcNow.AddDays(-(@from * 7)),
                    5 => DateTime.UtcNow.AddMonths(-@from),
                    6 => DateTime.UtcNow.AddYears(-@from),
                    _ => throw new NotImplementedException($@"{cbFilters.SelectedIndex} is not a valid index!")
                };
            }

            this.FillWithPersonalScores(GetDatetimeByFilter(val));
        }

        private void CbFilters_SelectedIndexChanged(object sender, EventArgs e)
            => PerformFilter();

        private void TxtFilter_TextChanged(object sender, EventArgs e)
            => PerformFilter();
    }
}