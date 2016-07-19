using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace childApp
{
    public partial class load_pay : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public load_pay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string filename;
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                filename = openFileDialog1.FileName;
            textBox1.Text = openFileDialog1.FileName;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            char delimiter = ';';
            int i = 0;
            int kol_ls = 0;
            string numb = null;
            int max_id = 0;
            DateTime date_now = DateTime.Today;
            NpgsqlCommand search_id = new NpgsqlCommand("SELECT max(id) From public.pay", conn);
            conn.Open();
            NpgsqlDataReader reader_id = search_id.ExecuteReader();
            while (reader_id.Read())
            {
                max_id = Convert.ToInt32(reader_id[0]);
            }

            conn.Close();



            StreamReader reader = new StreamReader(textBox1.Text, Encoding.GetEncoding("cp866"));
            while (true)
            {


                string temp = reader.ReadLine();
                textBox2.AppendText(temp + "\n");
                if (temp == null) break;
                if (i == 0)
                {
                    var reestr = Regex.Match(temp, @"[0-9][0-9]+(?:\.[0-9]*)?");
                    numb = reestr.ToString();
                }

                if (temp.Substring(0, 1) != "#")
                {
                    String[] substrings = temp.Split(delimiter);
                    int j = 0;

                    string acc_code = null;
                    string summ = null;
                    string date = null;
                    string plat = null;
                    foreach (var substring in substrings)
                    {

                        switch (j)
                        {
                            case 2: acc_code = substring;
                                break;

                            case 3: summ = substring;
                                break;

                            case 8: date = substring;
                                break;

                            case 10: plat = substring;
                                break;
                        }

                        j++;
                    }
                    DateTime dt = DateTime.Parse(date);
                    //conn.Open();
                    kol_ls++;
                    int id = max_id + kol_ls;
                    //NpgsqlCommand add_pay = new NpgsqlCommand("INSERT INTO public.pay(id, acc_id, date_pay, summa, text, reestr, date_create) VALUES (" + id + " ," + acc_code + ", '" + dt.ToString() + "', " + summ.Replace(',', '.') + ", '" + plat + "', '" + numb + "', '" + date_now + "')", conn);
                    //add_pay.ExecuteNonQuery();
                    //conn.Close();

                    //textBox2.AppendText(temp + "\n");

                }
                i++;
            }
            textBox2.AppendText("------------------------------------------------------------------------------------- \n");
            textBox2.AppendText("РЕЕСТР № " + numb + " лицевых счетов обработанно " + kol_ls);
        }

        private void load_pay_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(@"C:\folder\11501", "*.txt", SearchOption.AllDirectories);
            //Console.WriteLine("The number of files starting with c is {0}.", dirs.Length);
            foreach (string dir in dirs)
            {
                textBox3.AppendText(Path.GetFileNameWithoutExtension(dir) + "\n");
            }
            textBox2.AppendText("Найдено файлов в папке: " + dirs.Length + "\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            char delimiter = ';';
            int i = 0;
            int all_ls = 0;
            double summa = 0;

            int max_id = 0;
            DateTime date_now = DateTime.Today;
            NpgsqlCommand search_id = new NpgsqlCommand("SELECT max(id) From public.pay", conn);
            conn.Open();
            NpgsqlDataReader reader_id = search_id.ExecuteReader();
            while (reader_id.Read())
            {
                max_id = Convert.ToInt32(reader_id[0]);
            }

            conn.Close();

            for (int k = 0; k < textBox3.Lines.Length - 1; k++)
            {
                textBox2.AppendText("шаг " + k + " Обрабатываем файл ->" + textBox3.Lines[k] + "\n");
                textBox2.AppendText("\n");
                string numb = null;
                int kol_ls = 0;
                StreamReader reader = new StreamReader(@"C:\folder\11501\" + textBox3.Lines[k] + ".txt", Encoding.GetEncoding("cp866"));
                while (true)
                {


                    string temp = reader.ReadLine();
                    textBox2.AppendText(temp + "\n");
                    if (temp == null) break;
                    if (i == 0)
                    {
                        var reestr = Regex.Match(temp, @"[0-9][0-9]+(?:\.[0-9]*)?");
                        numb = reestr.ToString();
                    }

                    if (temp.Substring(0, 1) != "#")
                    {
                        String[] substrings = temp.Split(delimiter);
                        int j = 0;

                        string acc_code = null;
                        string summ = null;
                        string date = null;
                        string plat = null;
                        foreach (var substring in substrings)
                        {

                            switch (j)
                            {
                                case 2: acc_code = substring;
                                    break;

                                case 3: summ = substring.Replace(".", ",");
                                    break;

                                case 8: date = substring;
                                    break;

                                case 10: plat = substring;
                                    break;
                            }

                            j++;
                        }
                        DateTime dt = DateTime.Parse(date);
                        kol_ls++;
                        summa += Convert.ToDouble(summ);
                        /*conn.Open();
                        
                        int id = max_id + kol_ls;
                        NpgsqlCommand add_pay = new NpgsqlCommand("INSERT INTO public.pay(id, acc_id, date_pay, summa, text, reestr, date_create) VALUES (" + id + " ," + acc_code + ", '" + dt.ToString() + "', " + summ.Replace(',', '.') + ", '" + plat + "', '" + numb + "', '"+date_now+"')", conn);
                        add_pay.ExecuteNonQuery();
                        conn.Close();*/

                        //textBox2.AppendText(temp + "\n");

                    }
                    i++;
                }
                all_ls += kol_ls;
                textBox2.AppendText("РЕЕСТР № " + textBox3.Lines[k] + " лицевых счетов обработанно " + kol_ls + "------------------------------------------------------------\n");
                textBox2.AppendText("\n");
            }
            textBox2.AppendText("Записанно в базу оплат "+all_ls+" записи, на сумму "+summa+" рублей");
            textBox2.AppendText("\n");
        }
    }
}
