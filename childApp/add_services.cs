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
    public partial class add_services : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");

        public add_services()
        {
            InitializeComponent();
        }

        private void add_tarif_Load(object sender, EventArgs e)
        {
            string text_sql = @"SELECT 
                              public.tarif.id,
                              public.tarif.tarif_text,
                              public.tarif.tarif_taxa,
                              public.tarif.t_meas
                            FROM
                              public.tarif";
            NpgsqlCommand all_tarif = new NpgsqlCommand(text_sql, conn);
            conn.Open();
            NpgsqlDataReader reader_tarif = all_tarif.ExecuteReader();
            
            while (reader_tarif.Read())
            {
                comboBox1.Items.Add(reader_tarif[1].ToString() + " "+reader_tarif[2].ToString()+ "руб.");
            }

            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int  id_serv = 0;
            NpgsqlCommand max_id = new NpgsqlCommand("SELECT MAX(id) FROM public.service", conn);
            conn.Open();
            NpgsqlDataReader reader_id = max_id.ExecuteReader();
            
            while (reader_id.Read())
            {
                id_serv = Convert.ToInt32( reader_id[0].ToString());
            }

            conn.Close();



            MessageBox.Show(comboBox1.SelectedIndex.ToString());
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                id_serv++;
                int id = comboBox1.SelectedIndex + 1;
                string comment = textBox1.Text;
                int acc = Convert.ToInt32(all_user.acc_code);


                MessageBox.Show(id_serv + " " + id + " " + comment + " " + acc);
                //запись в базу новой услуги на лицевой счет
                NpgsqlCommand add_serv = new NpgsqlCommand("INSERT INTO public.service(id, acc_id, id_tarif, date_open, comment)  VALUES (" + id_serv + ", " + acc + ", " + id + ", '" + dateTimePicker1.Text + "', '" + textBox1.Text + "')", conn);
                conn.Open();
                add_serv.ExecuteNonQuery();
                conn.Close();
                this.Close();


                services.srv.refresh_table();

                this.Close();
            }
            else
            {
                MessageBox.Show("не правильно заполеныне поля"); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
