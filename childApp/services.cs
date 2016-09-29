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
    public partial class services : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");



        public void refresh_table()
        {
            string text_sql = @"SELECT 
                          public.service.id,
                          public.acc_info.acc_id,
                          public.tarif.tarif_text,
                          public.tarif.tarif_taxa,
                          public.service.date_open,
                          public.service.date_close,
                          public.service.comment
                        FROM
                          public.service
                          INNER JOIN public.tarif ON (public.service.id_tarif = public.tarif.id)
                          INNER JOIN public.acc_info ON (public.service.acc_id = public.acc_info.acc_id)
                        WHERE public.acc_info.acc_id =" + all_user.acc_code + "";

            DataTable dt_serv = new DataTable();
            NpgsqlDataAdapter all_serv = new NpgsqlDataAdapter(text_sql, conn);
            conn.Open();
            all_serv.Fill(dt_serv);

            conn.Close();
            dataGridView1.DataSource = dt_serv;


            dataGridView1.Columns[0].HeaderText = "Номер";
            dataGridView1.Columns[1].HeaderText = "Лицевйо счет";
            dataGridView1.Columns[2].HeaderText = "Тариф(описание)";
            dataGridView1.Columns[3].HeaderText = "Такса";
            dataGridView1.Columns[4].HeaderText = "Дата открытия услуги";
            dataGridView1.Columns[5].HeaderText = "Дата закрытия услуги";
            dataGridView1.Columns[6].HeaderText = "Комментарий";
        }


        public services()
        {
            InitializeComponent();
            srv = this;
        }

        public static services srv
        {
            get;
            set;
        }

        private void services_FormClosed(object sender, FormClosedEventArgs e)
        {
            all_user.acc_code = null;
        }

        private void services_Load(object sender, EventArgs e)
        {
            refresh_table();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            add_services frm = new add_services();
            frm.Owner = this;
            frm.Show();

        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            if (e.KeyCode == Keys.F5)
            {
                refresh_table();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
