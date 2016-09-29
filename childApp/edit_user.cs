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
    public partial class edit_user : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");

        int key_street = 0;
        public int g = 0;
        Dictionary<int, Dictionary<string, string>> adres = new Dictionary<int, Dictionary<string, string>>();

        public edit_user()
        {
            InitializeComponent();
        }

        private void edit_user_Load(object sender, EventArgs e)
        {
            this.Text += " : " + all_user.acc_code;

            NpgsqlCommand acc_info = new NpgsqlCommand("SELECT DISTINCT  "
                                         + " public.acc_info.acc_id, "
                                         + " public.acc_info.owner, "
                                         + "  places_cnt2.parent_name, "
                                         + "  places_cnt2.parent_sor, "
                                         + "  places_cnt2.name, "
                                         + "  public.acc_info.flat, "
                                         + "  public.acc_info.date_open, "
                                         + "  public.acc_info.acc_close, "
                                         + "  public.acc_info.date_close, "
                                         + "  public.acc_info.tarif_if, "
                                         + "  public.acc_info.contakt "
                                       + "  FROM "
                                         + "  public.places_cnt places_cnt2 "
                                         + "  INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id) "
                                         + "  INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                         + "  INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                                       + "  WHERE  "
                                        + " public.acc_info.acc_id = " + all_user.acc_code + "", conn);

            conn.Open();
            NpgsqlDataReader acc_reader = acc_info.ExecuteReader();
            while (acc_reader.Read())
            {
                textBox1.Text = acc_reader[1].ToString();
                comboBox1.Text = acc_reader[2].ToString() + " " + acc_reader[3].ToString();
                textBox2.Text = acc_reader[4].ToString();
                textBox3.Text = acc_reader[5].ToString();
                textBox5.Text = acc_reader[10].ToString();
            }
            //tarif = g;
            conn.Close();


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
                    dic.Add("name", reader[2].ToString() + " " + reader[3].ToString());
                    adres.Add(i, dic);
                    i++;
                }
                catch
                {
                    MessageBox.Show("НЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕ");
                }

            }
            conn.Close(); //Закрываем соединение.

            DataTable dt_counter = new DataTable();
            NpgsqlDataAdapter counter = new NpgsqlDataAdapter("SELECT * FROM public.counter WHERE acc_id =" + all_user.acc_code + "", conn);
            conn.Open();
            counter.Fill(dt_counter);
            dataGridView1.DataSource = dt_counter;
            conn.Close();

        }


        public void add_new_ls(long plc_id)
        {

            button1.Enabled = false;
           // MessageBox.Show(id_tarif.ToString());
            if (textBox1.Text != "" && comboBox1.Text != "" && textBox2.Text != "" & textBox3.Text != "")
            {

                conn.Open();
                NpgsqlCommand edit_user = new NpgsqlCommand("UPDATE public.acc_info SET  plc_id=" + plc_id + ", flat='" + textBox3.Text + "', owner='" + textBox1.Text + "', contakt='" + textBox5.Text + "'  "
                                                            + " WHERE acc_id=" + all_user.acc_code + "", conn);
                edit_user.ExecuteNonQuery();
                conn.Close();
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            long max_id = 0;
            string street_name = "";
            string plc_id ="";
            for (int a = 0; a < adres.Count; a++)
            {
                if (comboBox1.Text == adres[a]["name"])
                {
                    street_name = adres[a]["street"];
                    plc_id = adres[a]["id"];
                }
            }


            NpgsqlCommand search_adres = new NpgsqlCommand("SELECT "
                                                           + "public.places_cnt.name, "
                                                           + "places_cnt1.name, "
                                                           + "places_cnt1.plc_id "
                                                         + "FROM "
                                                           + "public.places_cnt places_cnt1 "
                                                           + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                                         + "WHERE "
                                                           + "public.places_cnt.type_id = 1 AND  "
                                                           + "public.places_cnt.name =  '" + street_name + "' AND  "
                                                           + "places_cnt1.name = '" + textBox2.Text + "' ", conn);

            conn.Open();
            NpgsqlDataReader reader = search_adres.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    max_id = Convert.ToInt64(reader[2]);
                }
                conn.Close();
                add_new_ls(max_id);

            }
            else
            {

                conn.Close();

                NpgsqlCommand max_plc_id = new NpgsqlCommand("SELECT MAX(public.places_cnt.plc_id) AS field_1 FROM public.places_cnt", conn);
                conn.Open();
                NpgsqlDataReader read_ls = max_plc_id.ExecuteReader();

                while (read_ls.Read())
                {
                    max_id = Convert.ToInt64(read_ls[0]);
                }
                conn.Close();
                max_id++;

                NpgsqlCommand parent_data = new NpgsqlCommand("SELECT  "
                                                               + "public.places_cnt.plc_id, "
                                                               + "public.places_cnt.parent_name, "
                                                               + "public.places_cnt.parent_sor, "
                                                               + "public.places_cnt.name "
                                                             + "FROM "
                                                               + "public.places_cnt "
                                                             + "WHERE "
                                                               + "public.places_cnt.type_id = 1 AND  "
                                                               + "public.places_cnt.name = '" + street_name + "'", conn);
                conn.Open();
                string pn = "";
                string ps = "";
                string p = "";
                NpgsqlDataReader reader_data = parent_data.ExecuteReader();
                if (reader_data.HasRows)
                {
                    while (reader_data.Read())
                    {
                        pn = reader_data[1].ToString();
                        ps = reader_data[2].ToString();
                        p = reader_data[3].ToString();
                    }

                    conn.Close();
                    NpgsqlCommand add_new_plc = new NpgsqlCommand("INSERT INTO public.places_cnt(plc_id, district, parent_name, parent_sor, parent, name, parent_id, type_id) "
                                              + " VALUES (" + max_id + ", '', '" + pn + "', '" + ps + "', '" + p + "', '" + textBox2.Text + "', " + plc_id + ", 2)", conn);
                    conn.Open();
                    add_new_plc.ExecuteNonQuery();
                    conn.Close();
                    add_new_ls(max_id);

                }
                else
                {
                    MessageBox.Show("Нет такой улицы надо добавлять, звоните Жене!:)");
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Первый способ
            //MessageBox.Show(adres[comboBox1.SelectedIndex]["street"] + " " + adres[comboBox1.SelectedIndex]["district"]);
            // comboBox1.Text = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            key_street = comboBox1.SelectedIndex;
            NpgsqlCommand sql_house = new NpgsqlCommand("SELECT places_cnt2.plc_id, places_cnt2.name FROM public.places_cnt places_cnt2 WHERE places_cnt2.parent = '" + adres[key_street]["street"] + "'", conn);
            conn.Open();
            NpgsqlDataReader reader = sql_house.ExecuteReader();
            while (reader.Read())
            {
                textBox2.AutoCompleteCustomSource.Add(reader[1].ToString());
            }

            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ls = Convert.ToInt32(all_user.acc_code);
            if (ls != 0)
            {
                int id = 0;
                NpgsqlCommand max_id = new NpgsqlCommand("SELECT max(id) FROM counter", conn);
                conn.Open();
                NpgsqlDataReader read_id = max_id.ExecuteReader();
                if (read_id.FieldCount > 0)
                {
                    while (read_id.Read())
                    {
                        if (read_id[0].ToString() != "")
                        {
                            id = Convert.ToInt32(read_id[0]);
                        }
                    }
                }
                id++;
                conn.Close();
                NpgsqlCommand add_counter = new NpgsqlCommand("insert into counter (id, acc_id, count_numb, data_close, name) values(" + id + ", " + ls + ", '" + textBox4.Text + "', '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "', '" + textBox6.Text + "')", conn);
                conn.Open();
                add_counter.ExecuteReader();
                conn.Close();

                
            }
            else
            {
                MessageBox.Show("Сначала необходимо добавить лицевой счет");
            }
        }
    }
}
