﻿using Npgsql;
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

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
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
                    + " VALUES (" + max_id + ", " + plc_id + ", '" + textBox3.Text + "', '" + dateTimePicker1.Value.ToString("dd.MM.yyyy") + "', '" + dateTimePicker2.Value.ToString("dd.MM.yyyy") + "', '', '" + textBox1.Text + "', '"+textBox5.Text+"', " + key_tarif + ");", conn);
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
            long max_id = 0;
            string street_name = adres[key_street]["street"].ToString();

            NpgsqlCommand search_adres = new NpgsqlCommand("SELECT "
                                                           + "public.places_cnt.name, "
                                                           + "places_cnt1.name, "
                                                           + "places_cnt1.plc_id "
                                                         + "FROM "
                                                           + "public.places_cnt places_cnt1 "
                                                           + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                                         + "WHERE "
                                                           + "public.places_cnt.type_id = 1 AND  "
                                                           + "public.places_cnt.name =  '" + adres[key_street]["street"] + "' AND  "
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
                                              + " VALUES (" + max_id + ", '', '" + pn + "', '" + ps + "', '" + p + "', '" + textBox2.Text + "', " + adres[key_street]["id"] + ", 2)", conn);
                    conn.Open();
                    add_new_plc.ExecuteNonQuery();
                    conn.Close();
                    add_new_ls(max_id);

                }
                else
                {
                    MessageBox.Show("Нет такой улицы надо добавлять");
                }

            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
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
            //MessageBox.Show(comboBox2.SelectedIndex.ToString());
            key_tarif = comboBox2.SelectedIndex + 1;

            NpgsqlCommand sql_tarif = new NpgsqlCommand("SELECT * FROM  tarif WHERE id = " + key_tarif + "", conn);
            conn.Open();
            NpgsqlDataReader reader = sql_tarif.ExecuteReader();
            while (reader.Read())
            {
                tarif = Convert.ToInt16(reader[2]);
            }

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

                update_table_counter();
            }
            else
            {
                MessageBox.Show("Сначала необходимо добавить лицевой счет");
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                System.Windows.Forms.SendKeys.Send("{TAB}");
            }
        }
    }
}
