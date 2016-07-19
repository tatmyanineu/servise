using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace childApp
{
    public partial class all_charge : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public all_charge()
        {
            InitializeComponent();
        }

        public void view_charge(string period)
        {
            MessageBox.Show(period.ToString());
            NpgsqlCommand charge = new NpgsqlCommand("SELECT DISTINCT "
                                         +" public.charge.charge, "
                                         +" public.charge.pay, "
                                          +" public.charge.saldo_in, "
                                          +" public.charge.saldo_out, "
                                          +" public.charge.maket, "
                                          +" public.charge.acc_id "
                                        +" FROM "
                                          +" public.charge "
                                          +" INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                                        +" WHERE "
                                          +" public.period.name = '"+period+"' AND  "
                                          +" public.charge.acc_id = "+all_user.acc_code+"", conn);

            conn.Open();
            NpgsqlDataReader reader = charge.ExecuteReader();

            while (reader.Read())
            {
                textBox1.Text = reader[2].ToString();
                textBox2.Text = reader[0].ToString();
                textBox3.Text = reader[4].ToString();
                textBox4.Text = reader[1].ToString();
                textBox5.Text = reader[3].ToString();
            }
            conn.Close();
        }

        private void all_charge_Load(object sender, EventArgs e)
        {
           NpgsqlCommand period = new NpgsqlCommand("SELECT DISTINCT public.period.name, public.period.id FROM public.period ORDER BY public.period.id DESC" , conn);
           conn.Open();
           NpgsqlDataReader r_period = period.ExecuteReader();
           while (r_period.Read())
           {
               comboBox1.Items.Add(r_period[0].ToString());
           }
           
           conn.Close();
           comboBox1.SelectedIndex = 0;
           view_charge(comboBox1.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pay newMDICheild = new pay();
            newMDICheild.MdiParent = this.MdiParent;
            // Display the new form.
            newMDICheild.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            view_charge(comboBox1.Text);
        }
    }
}
