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
    public partial class pay : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public pay()
        {
            InitializeComponent();
        }

        private void pay_Load(object sender, EventArgs e)
        {
            NpgsqlCommand acc_info = new NpgsqlCommand("SELECT DISTINCT "
                               + " places_cnt1.name, "
                               + "places_cnt2.name, "
                               + "public.acc_info.flat "
                             + "FROM "
                               + "public.places_cnt places_cnt2 "
                               + "INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id) "
                               + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                               + "INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                             + "WHERE "
                              + " public.acc_info.acc_id = " + all_user.acc_code + "", conn);
            conn.Open();
            NpgsqlDataReader reader = acc_info.ExecuteReader();
            while (reader.Read())
            {
                this.Text = "Оплаты по ЛС: " + all_user.acc_code + " Адрес:  " + reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString();
            }
            conn.Close();




            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM public.pay WHERE acc_id = "+all_user.acc_code+"", conn);

            conn.Open();
            da.Fill(dt);
            conn.Close();

            dataGridView1.DataSource = dt;
        }
    }
}
