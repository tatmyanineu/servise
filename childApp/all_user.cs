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
    public partial class all_user : Form
    {
        public static string acc_code;

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public int ls = 0;
        public all_user()
        {
            InitializeComponent();
        }

        static class Data
        {
            public static string Value { get; set; }
        }

        public void update_main_table()
        {

            DataTable dt = new DataTable();
            string comm_text = @"SELECT DISTINCT 
                                  public.acc_info.acc_id,
                                  public.acc_info.owner,
                                  places_cnt2.district,
                                  places_cnt1.name,
                                  places_cnt2.name,
                                  public.acc_info.flat,
                                  public.acc_info.date_open,
                                  public.acc_info.acc_close,
                                  public.acc_info.contakt
                                FROM
                                  public.places_cnt places_cnt1
                                  INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id)
                                  INNER JOIN public.places_cnt places_cnt2 ON (places_cnt1.plc_id = places_cnt2.parent_id)
                                  INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id)
                                ORDER BY
                                  places_cnt1.name,
                                  places_cnt2.name,
                                  public.acc_info.flat";
            NpgsqlCommand comm = new NpgsqlCommand(comm_text, conn);
            conn.Open(); //Открываем соединение.
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(comm);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[0].HeaderText = "Лицевой счет";

            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[1].HeaderText = "ФИО собственика";

            dataGridView1.Columns[2].Width = 130;
            dataGridView1.Columns[2].HeaderText = "Район";

            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[3].HeaderText = "Улица";

            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[4].HeaderText = "Дом";

            dataGridView1.Columns[5].Width = 60;
            dataGridView1.Columns[5].HeaderText = "Квартира";

            dataGridView1.Columns[6].Width = 70;
            dataGridView1.Columns[6].HeaderText = "Открыт";

            dataGridView1.Columns[7].Width = 70;
            dataGridView1.Columns[7].HeaderText = "Закрыты";

            dataGridView1.Columns[8].Width = 200;
            dataGridView1.Columns[8].HeaderText = "Контактная информация";

        }


        public void search_table(string ls, string street, string house, string flat)
        {
            string sql = "SELECT "
                                                      + "public.acc_info.acc_id, "
                                                       + "public.acc_info.owner, "
                                                      + "places_cnt2.district, "
                                                      + "places_cnt1.name, "
                                                      + "places_cnt2.name, "
                                                      + "public.acc_info.flat, "
                                                      + "public.acc_info.date_open, "
                                                      + "public.acc_info.acc_close, "
                                                      + "public.acc_info.contakt "
                                                    + "FROM "
                                                      + "public.places_cnt places_cnt1 "
                                                      + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                                      + "INNER JOIN public.places_cnt places_cnt2 ON (places_cnt1.plc_id = places_cnt2.parent_id) "
                                                      + "INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) WHERE ";


            if (ls != "" & street == "" & house == "" & flat == "")
            {
                sql += " public.acc_info.acc_id = " + ls + " ";
            }

            if (ls == "" & street != "" & house != "" & flat != "")
            {
                sql += "  public.acc_info.flat LIKE '%" + flat + "%' AND "
                         + " places_cnt2.name LIKE '%" + house + "%' AND "
                         + " places_cnt1.name LIKE '%" + street + "%' ";
            }

            if (ls == "" & street != "" & house != "" & flat == "")
            {
                sql += " places_cnt2.name LIKE '%" + house + "%' AND "
                         + " places_cnt1.name LIKE '%" + street + "%' ";
            }

            if (ls == "" & street != "" & house == "" & flat == "")
            {
                sql += " places_cnt1.name LIKE '%" + street + "%' ";
            }

            DataTable dt = new DataTable();
            NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
            conn.Open(); //Открываем соединение.
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(comm);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

        }

        private void all_user_Load(object sender, EventArgs e)
        {
            update_main_table();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            searchForm f = new searchForm(this);
            f.Show();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            add_user frm = new add_user();
            frm.Owner = this;
            frm.Show();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                update_main_table();
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {

                charge newMDICheild = new charge();
                newMDICheild.MdiParent = this.MdiParent;
                // Display the new form.
                newMDICheild.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in dataGridView1.SelectedCells)
            {
                cell = selectedCell;
                break;
            }

            if (cell != null)
            {
                toolStripButton3.Enabled = true;
                toolStripButton4.Enabled = true;



                DataGridViewRow row = cell.OwningRow;
                ls = Convert.ToInt32(row.Cells[0].Value);

                acc_code = row.Cells[0].Value.ToString();
                this.Text = "Просмотр лицевых счетов. Выбран ЛС  - " + acc_code; 
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {

                pay newMDICheild = new pay();
                newMDICheild.MdiParent = this.MdiParent;
                // Display the new form.
                newMDICheild.WindowState = System.Windows.Forms.FormWindowState.Normal;
                newMDICheild.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {
                services newMDICheild = new services();
                newMDICheild.MdiParent = this.MdiParent;
                // Display the new form.
                newMDICheild.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {
                edit_user frm = new edit_user();
                frm.Owner = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {
                close_user frm = new close_user();
                frm.Owner = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {
                forPrintPeriod newMDICheild = new forPrintPeriod();
                newMDICheild.MdiParent = this.MdiParent;
                // Display the new form.
                newMDICheild.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (acc_code != null)
            {
                counters newMDICheild = new counters();
                newMDICheild.MdiParent = this.MdiParent;
                // Display the new form.
                newMDICheild.Show();
            }
            else
            {
                MessageBox.Show("Лицевой счет не выбран");
            }
        }

    }
}
