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
    public partial class forPrintPeriod : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        
        public forPrintPeriod()
        {
            InitializeComponent();
        }

        private void forPrintPeriod_Load(object sender, EventArgs e)
        {
            NpgsqlCommand period = new NpgsqlCommand("SELECT DISTINCT public.period.name, public.period.id FROM public.charge INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) WHERE public.charge.acc_id = " + all_user.acc_code + " ORDER BY public.period.id DESC", conn);
            conn.Open();
            NpgsqlDataReader reader = period.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            conn.Close();
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            report1.Load("reports_only.frx");
            DataSet dataCharge = new DataSet();
            NpgsqlDataAdapter data_ls = new NpgsqlDataAdapter("SELECT DISTINCT  "
                                         + " public.acc_info.owner, "
                                          + "places_cnt1.name,  "
                                         + " places_cnt2.name,  "
                                         + " public.acc_info.flat,  "
                                         + " public.charge.date_create,  "
                                         + " public.charge.saldo_out,  "
                                         + " public.charge.saldo_in,  "
                                         + " public.charge.pay,  "
                                         + " public.charge.charge,  "
                                         + " public.charge.maket,  "
                                         + " public.acc_info.plc_id,  "
                                         + "public.group.group_name,  "
                                         + " public.acc_info.acc_id, "
                                         + "public.period.name  "
                                       + " FROM  "
                                         + " public.acc_info  "
                                         + " INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id)  "
                                         + " INNER JOIN public.places_cnt places_cnt2 ON (places_cnt2.plc_id = public.acc_info.plc_id)  "
                                         + " INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id)  "
                                         + " INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id)  "
                                         + " LEFT OUTER JOIN public.group ON (public.acc_info.plc_id = public.group.house_id)  "
                                         + " INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt)  "
                                       + " WHERE  "
                                         + " public.charge.charge != '0' AND "
                                         + " public.acc_info.acc_id=" + all_user.acc_code + " AND"
                                         + " public.period.name = '"+comboBox1.Text+"'", conn);
            conn.Open();

            data_ls.Fill(dataCharge, "dataCharge");
            report1.RegisterData(dataCharge, "dataCharge");
            conn.Close();
            report1.Show();
        }
    }
}
