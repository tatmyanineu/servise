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
    public partial class forPrintGroup : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");

        public forPrintGroup()
        {
            InitializeComponent();
        }

        private void forPrintGroup_Load(object sender, EventArgs e)
        {
            PrintTo main = this.Owner as PrintTo;
            conn.Open();
            NpgsqlDataAdapter search_group = new NpgsqlDataAdapter("SELECT DISTINCT group_name FROM public.group WHERE group_name LIKE '%" + main.textBox3.Text + "%' ORDER BY group_name", conn);
            DataTable dt = new DataTable();
            search_group.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 350;

            conn.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            PrintTo frm = this.Owner as PrintTo;
            if (e.KeyCode == Keys.Enter)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1[j, i].Selected)
                            frm.textBox4.AppendText(dataGridView1[j, i].Value.ToString() + "\n");
                        //MessageBox.Show(dataGridView1[j, i].Value.ToString());
                    }
                }
                this.Close();
            }
        }
    }
}
