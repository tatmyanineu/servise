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
    public partial class add_user : Form
    {

        NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        int key_street = 0;
        int key_tarif = 0;
        Dictionary<int, Dictionary<string, string>> adres = new Dictionary<int, Dictionary<string, string>>();
        int tarif = 0;
        int ls = 0;
        public add_user()
        {
            InitializeComponent();
        }

        public void update_table_counter()
        {
            conn.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter table = new NpgsqlDataAdapter("SELECT * FROM counter WHERE acc_id = " + ls + "", conn);
            table.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();


            dataGridView1.Columns[0].HeaderText = "№";
            dataGridView1.Columns[1].HeaderText = "Лицевйо счет";
            dataGridView1.Columns[2].HeaderText = "Номер счетчика";
            dataGridView1.Columns[3].HeaderText = "Дата поверки";

            textBox4.Text = "";
            textBox6.Text = "";

        }

        public void add_new_ls(long plc_id)
        {

            button1.Enabled = false;
            int max_id = 0;
            int max_id_charge = 0;
            if (textBox1.Text != "" && comboBox1.Text != "" && textBox2.Text != "" & textBox3.Text != "" && comboBox2.Text != "")
            {
                NpgsqlCommand max_ls = new NpgsqlCommand("SELECT max(acc_id) FROM public.acc_info", conn);
                conn.Open();
                NpgsqlDataReader reader_ls = max_ls.ExecuteReader();
                while (reader_ls.Read())
                {
                    max_id = Convert.ToInt32(reader_ls[0]);
                }
                conn.Close();

                NpgsqlCommand max_charge = new NpgsqlCommand("SELECT max(id) FROM public.charge", conn);
                conn.Open();
                NpgsqlDataReader reader_charge = max_charge.ExecuteReader();
                while (reader_charge.Read())
                {
                    max_id_charge = Convert.ToInt32(reader_charge[0]);
                }
                conn.Close();


                if (max_id != 0)
                {

                    int saldo_out = 0;
                    int maket = 0;
                    DateTime now = DateTime.Today;
                    saldo_out = ((now.Year - dateTimePicker1.Value.Year) * 12) + now.Month - dateTimePicker1.Value.Month; //колво месяцев для расчета

                    saldo_out++;
                    maket = (saldo_out - 1) * tarif;
                    saldo_out = saldo_out * tarif;


                    now = new DateTime(now.Year, now.Month, 01);


                    //MessageBox.Show(now.ToString());

                    max_id++;
                    max_id_charge++;
                    ls = max_id;
                    NpgsqlCommand add_charge = new NpgsqlCommand("insert into charge (id, acc_id, date_period, tarif_id, charge, pay, saldo_in, saldo_out, date_create, maket) values (" + max_id_charge + ", " + max_id + ", '" + now.ToString("dd.MM.yyyy") + "', " + key_tarif + ", '" + tarif + "', '0', '0','" + saldo_out + "', '" + now.ToString("yyyy-MM-dd") + "', '" + maket + "')", conn);
                    conn.Open();
                    add_charge.ExecuteNonQuery();
                    conn.Close();


                    NpgsqlCommand add_ls = new NpgsqlCommand("INSERT INTO public.acc_info(acc_id, plc_id, flat, date_open, date_close, acc_close, owner, contakt, tarif_if) "
                    + " VALUES (" + max_id + ", " + plc_id + ", '" + textBox3.Text + "', '" + dateTimePicker1.Value.ToString("dd.MM.yyyy") + "', '" + dateTimePicker2.Value.ToString("dd.MM.yyyy") + "', '', '" + textBox1.Text + "', '', " + key_tarif + ");", conn);
                    conn.Open();
                    add_ls.ExecuteNonQuery();
                    conn.Close();
                    if (add_ls.Statements != null && add_charge.Statements != null)
                    {
                        MessageBox.Show("Лицевйо счет добавлен");
                    }
                }

            }

            textBox1.Text = "";
            comboBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox2.Text = "";
        }

        private void add_user_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = dateTimePicker2.Value.AddYears(6);
            dateTimePicker3.Value = dateTimePicker3.Value.AddYears(6);
            NpgsqlCommand street = new NpgsqlCommand("SELECT  "
                                                      + "public.places_cnt.name,  "
                                                      + "public.places_cnt.plc_id,  "
                                                      + "public.places_cnt.parent_name,  "
                                                      + "public.places_cnt.parent_sor,  "
                                                      + "public.places_cnt.parent  "
                                                    + "FROM  "
                                                      + "public.places_cnt  "
                                                    + "WHERE  "
                                                      + "public.places_cnt.type_id = 1", conn);
            conn.Open(); //Открываем соединение.
            NpgsqlDataReader reader;
            reader = street.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                try
                {
                    comboBox1.Items.Add(reader[2].ToString() + " " + reader[3].ToString());
                    dic.Add("street", reader[0].ToString());
                    dic.Add("id", reader[1].ToString());
                    adres.Add(i, dic);
                    i++;
                }
                catch
                {
                    MessageBox.Show("НЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕ");
                }

            }
            conn.Close(); //Закрываем соединение.



            NpgsqlCommand tarif = new NpgsqlCommand("SELECT * FROM  public.tarif", conn);
            conn.Open(); //Открываем соединение.
            NpgsqlDataReader reader2;
            reader2 = tarif.ExecuteReader();
            while (reader2.Read())
            {
                try
                {
                    comboBox2.Items.Add(reader2[4].ToString() + " " + reader2[2].ToString() + "руб.");
                }
                catch
                {
                    MessageBox.Show("ошбика");
                }

            }
            conn.Close(); //Закрываем соединение.

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
