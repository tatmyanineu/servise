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
    public partial class charge : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public charge()
        {
            InitializeComponent();

        }

        private void charge_Load(object sender, EventArgs e)
        {
            NpgsqlCommand acc_info = new NpgsqlCommand("SELECT DISTINCT "
                               +" places_cnt1.name, "
                               +"places_cnt2.name, "
                               +"public.acc_info.flat "
                             +"FROM "
                               +"public.places_cnt places_cnt2 "
                               +"INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id) "
                               +"INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                               +"INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                             + "WHERE "
                              +" public.acc_info.acc_id = "+all_user.acc_code+"", conn);
            conn.Open();
            NpgsqlDataReader reader = acc_info.ExecuteReader();
            while (reader.Read())
            {
                this.Text = "Начисления по ЛС: " + all_user.acc_code + " Адрес:  " + reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString();
            }
            conn.Close();


            DataTable dt_charge = new DataTable();
            NpgsqlDataAdapter sql_all_charge = new NpgsqlDataAdapter("SELECT DISTINCT "
                          +" public.charge.date_period, "
                           + " public.period.name, "
                          +" public.charge.charge, "
                          +" public.charge.pay, "
                          +" public.charge.saldo_in, "
                          +" public.charge.saldo_out, "
                          +" public.charge.maket "
                         
                        +" FROM "
                          +" public.charge "
                          +" INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                        +" WHERE "
                          + " public.charge.acc_id = " + all_user.acc_code + " ORDER BY public.charge.date_period", conn);

            conn.Open();
            sql_all_charge.Fill(dt_charge);

            dataGridView1.DataSource = dt_charge;

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[0].HeaderText = "Дата периода";

            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[1].HeaderText = "Название периода";

            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].HeaderText = "Ежемесячное начисление";

            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].HeaderText = "Оплата в текущем месяце";

            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[4].HeaderText = "Долг на начало месяца";

            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[5].HeaderText = "долг на конец месяца";

            dataGridView1.Columns[6].Width = 100;
            dataGridView1.Columns[6].HeaderText = "Перерасчет.";



           //all_user.acc_code
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            pay newMDICheild = new pay();
            newMDICheild.MdiParent = this.MdiParent;
            // Display the new form.
            newMDICheild.Show();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
