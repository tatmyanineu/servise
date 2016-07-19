using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace childApp
{
    public partial class period : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public string date_old = "";
        public period()
        {
            InitializeComponent();
        }

        public void update_table_period()
        {
            NpgsqlDataAdapter sql_table = new NpgsqlDataAdapter("SELECT * FROM period ORDER BY id DESC ", conn);
            DataTable table = new DataTable();
            conn.Open();
            sql_table.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();


            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[0].HeaderText = "id";

            dataGridView1.Columns[1].Width = 170;
            dataGridView1.Columns[1].HeaderText = "Название периода";

            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].HeaderText = "Начальная дата";

            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].HeaderText = "Конечная дата";

            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[4].HeaderText = "Дата периода";

            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[5].HeaderText = "Доп.";

        }

        private void period_Load(object sender, EventArgs e)
        {

            update_table_period();

            DateTime now = new DateTime();
            now = DateTime.Now;
            textBox1.Text = now.ToString("MMMM yyyy");
            //dateTimePicker1.Text = now.ToString("01.MM.yyyy");
            //dateTimePicker2.Text = DateTime.DaysInMonth(now.Year, now.Month)+" "+now.ToString(".MM.yyyy");
            dateTimePicker2.Text = now.ToString("dd.MM.yyyy");
            NpgsqlCommand last_date = new NpgsqlCommand("SELECT * FROM period ORDER BY id DESC LIMIT 1", conn);
            conn.Open();
            NpgsqlDataReader reader = last_date.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    dateTimePicker1.Text = reader[3].ToString();
                    date_old = reader[3].ToString();
                }
            }

            dateTimePicker3.Text = now.ToString("01.MM.yyyy");

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
