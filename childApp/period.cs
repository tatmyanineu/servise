using Npgsql;
using System;
using System.Collections;
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
    public partial class period : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public string date_old = "";
        public period()
        {
            InitializeComponent();
        }

        public void update_table_period()
        {
            NpgsqlDataAdapter sql_table = new NpgsqlDataAdapter("SELECT * FROM period ORDER BY id DESC ", conn);
            DataTable table = new DataTable();
            conn.Open();
            sql_table.Fill(table);
            dataGridView1.DataSource = table;

            conn.Close();


            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[0].HeaderText = "id";

            dataGridView1.Columns[1].Width = 170;
            dataGridView1.Columns[1].HeaderText = "Название периода";

            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].HeaderText = "Начальная дата";

            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].HeaderText = "Конечная дата";

            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[4].HeaderText = "Дата периода";

            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[5].HeaderText = "Доп.";

        }

        private void period_Load(object sender, EventArgs e)
        {

            update_table_period();

            DateTime now = new DateTime();
            now = DateTime.Now;
            textBox1.Text = now.ToString("MMMM yyyy").ToUpper();
            //dateTimePicker1.Text = now.ToString("01.MM.yyyy");
            //dateTimePicker2.Text = DateTime.DaysInMonth(now.Year, now.Month)+" "+now.ToString(".MM.yyyy");
            dateTimePicker2.Text = now.ToString("dd.MM.yyyy");
            dateTimePicker5.Text = now.ToString("dd.MM.yyyy");
            NpgsqlCommand last_date = new NpgsqlCommand("SELECT * FROM period ORDER BY id DESC LIMIT 1", conn);
            conn.Open();
            NpgsqlDataReader reader = last_date.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    dateTimePicker1.Text = reader[3].ToString();
                    dateTimePicker4.Text = reader[3].ToString();
                    date_old = reader[3].ToString();
                }
            }

            dateTimePicker3.Text = now.ToString("01.MM.yyyy");

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int kol_month;
            string date_cnt = ""; //дата прошлого периода
            int id = 1;
            //считаем количество месяцев с прошлого периода
            DateTime old = DateTime.Parse(date_old);
            DateTime now = DateTime.Today;
            kol_month = ((now.Year - old.Year) * 12) + now.Month - old.Month; //колво месяцев для расчета
            kol_month++;
            MessageBox.Show(kol_month.ToString());

            // выбираем данные прогшлого периода по -1 день до сегодняшнего периода date_cnt

            NpgsqlCommand old_period = new NpgsqlCommand("SELECT * FROM public.period ORDER BY period.id DESC LIMIT 1", conn);
            conn.Open();

            NpgsqlDataReader read_period = old_period.ExecuteReader();

            while (read_period.Read())
            {
                date_cnt = read_period[4].ToString();
            }
            conn.Close();

            string sql_max_id = "SELECT MAX(public.charge.id) AS field_1 FROM public.charge";

            NpgsqlCommand max_id = new NpgsqlCommand(sql_max_id, conn);

            conn.Open(); //Открываем соединение.

            int i = 0;


            NpgsqlDataReader read_id = max_id.ExecuteReader();

            while (read_id.Read())
            {
                id = Convert.ToInt32(read_id[0]);
            }
            conn.Close();



            //тут должен быть алготирм начислений и запись в базу нового периода и начислений

            /*    NpgsqlCommand all_active_cherge = new NpgsqlCommand("SELECT "
                                                     + " public.acc_info.acc_id, "
                                                     + "  public.charge.date_period, "
                                                      + " public.tarif.tarif_taxa, "
                                                      + " public.charge.charge, "
                                                      + " public.charge.pay, "
                                                      + " public.charge.saldo_in, "
                                                      + " public.charge.saldo_out, "
                                                      + " public.charge.date_create, "
                                                      + " public.tarif.id "
                                                    + " FROM "
                                                      + " public.acc_info "
                                                      + " INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id) "
                                                      + " INNER JOIN public.tarif ON (public.charge.tarif_id = public.tarif.id) "
                                                    + " WHERE "
                                                      + " public.acc_info.acc_close IS NULL AND  "
                                                      + " public.charge.date_create = '" + date_cnt + "' AND "
                                                      + " public.charge.charge <> '0' OR "
                                                      + " public.acc_info.acc_close ='' AND  "
                                                      + " public.charge.date_create = '" + date_cnt + "' AND "
                                                      + " public.charge.charge <> '0'", conn);*/

            NpgsqlCommand all_active_cherge = new NpgsqlCommand("SELECT DISTINCT  "
                                                  + " public.acc_info.acc_id, "
                                                  + " public.charge.date_period, "
                                                  + " public.tarif.tarif_taxa, "
                                                  + " public.charge.charge, "
                                                  + " public.charge.pay, "
                                                  + " public.charge.saldo_in, "
                                                  + " public.charge.saldo_out, "
                                                  + " public.charge.date_create, "
                                                  + " public.tarif.id, "
                                                  + " public.charge.maket, "
                                                  + " public.charge.tarif_id, "
                                                  + " tarif1.tarif_taxa "
                                                + " FROM "
                                                  + " public.charge "
                                                  + " INNER JOIN public.acc_info ON (public.charge.acc_id = public.acc_info.acc_id) "
                                                  + " INNER JOIN public.tarif ON (public.acc_info.tarif_if = public.tarif.id) "
                                                  + " INNER JOIN public.tarif tarif1 ON (public.charge.tarif_id = tarif1.id) "
                                                + " WHERE "
                                                  + " public.acc_info.acc_close IS NULL AND  "
                                                  + " public.charge.charge != '0' AND  "
                                                  + " public.charge.date_create = '" + date_cnt + "' OR  "
                                                  + " public.acc_info.acc_close = '' AND  "
                                                  + " public.charge.charge != '0' AND  "
                                                  + " public.charge.date_create = '" + date_cnt + "' "
                                                + " ORDER BY  public.acc_info.acc_id", conn);


            conn.Open();
            NpgsqlDataReader reader_charge = all_active_cherge.ExecuteReader();
            Hashtable arr = new Hashtable();
            Hashtable arr1 = new Hashtable();
            Dictionary<int, Dictionary<string, string>> mainArray = new Dictionary<int, Dictionary<string, string>>();

            while (reader_charge.Read())
            {

                Dictionary<string, string> dic = new Dictionary<string, string>();
                Double charge = 0;
                Double saldo_in = 0;
                //var dateStr = Convert.ToDateTime(reader[6].ToString());
                DateTime dat = Convert.ToDateTime(reader_charge[7].ToString());
                id = id + 1;
                charge = Convert.ToDouble(reader_charge[2]);
                saldo_in = Convert.ToDouble(reader_charge[5]);



                //DateTime strd = new DateTime( Convert.ToDateTime(reader[6].ToString()));
                //textBox1.AppendText(i + " " + charge + " " + pay + " " + saldo_in + " " + saldo_out + " " + dat.AddMonths(-1).ToString("dd.MM.yyyy") + "\n");
                dic.Add("id", id.ToString());
                dic.Add("acc_id", reader_charge[0].ToString());
                dic.Add("tarif_id", reader_charge[8].ToString());
                dic.Add("charge", charge.ToString());
                dic.Add("saldo_in", saldo_in.ToString());
                dic.Add("date", dateTimePicker3.Value.ToString());
                dic.Add("pay", "0");
                dic.Add("saldo_out", reader_charge[6].ToString());
                dic.Add("tarif_taxa", reader_charge[2].ToString());
                mainArray.Add(i, dic);
                //dic.Clear();
                //NpgsqlCommand add_charge = new NpgsqlCommand("INSERT INTO charge (id, acc_id, date_period, tarif_id, charge, pay, saldo_in, saldo_out, date_create) VALUES (" + id + ", " + reader[0] + ", '" + dat.AddMonths(1).ToString("dd.MM.yyyy") + "',  " + reader[8] + ", " + charge + ", " + pay + ", " + saldo_in + ", " + saldo_out + " , '" + dat.AddMonths(1).ToString("yyyy-MM-dd") + "' )", conn);
                //add_charge.ExecuteNonQuery();

                i++;

                //Thread.Sleep(1000);

            }
            conn.Close();
            textBox2.AppendText("Обработанно: " + mainArray.Count + "  Лицевых счетов\n");
            Double g = 0;
            Double f = 0;

            for (int j = 0; j < mainArray.Count; j++)
            {

                //DateTime data = Convert.ToDateTime(mainArray[j]["date"]);
                //textBox2.AppendText(mainArray[j]["acc_id"] + " " + mainArray[j]["date"] + " " + mainArray[j]["id"] + " " + mainArray[j]["pay"] + "\n");

                NpgsqlCommand acc_pay = new NpgsqlCommand("SELECT * FROM public.pay WHERE acc_id = " + mainArray[j]["acc_id"] + " and date_pay >= '" + dateTimePicker4.Value + "' and date_pay<='" + dateTimePicker5.Value + "'", conn);
                conn.Open();
                Double pay = 0;
                NpgsqlDataReader reader_pay = acc_pay.ExecuteReader();
                while (reader_pay.Read())
                {
                    pay = pay + Convert.ToDouble(reader_pay[3]);

                }
                pay = Math.Round(pay, 2);
                conn.Close();

                mainArray[j]["pay"] = pay.ToString();
                mainArray[j]["saldo_in"] = mainArray[j]["saldo_out"];
                g = Convert.ToDouble(mainArray[j]["saldo_in"]) + (Convert.ToDouble(mainArray[j]["tarif_taxa"]) * (kol_month - 1)) - pay;
                //g = Math.Round(g, 2);
                //f = g + Convert.ToDouble(mainArray[j]["charge"]);
                //f = Math.Round(f, 2);
                
                mainArray[j]["saldo_out"] =g.ToString();
               

                //textBox2.AppendText(mainArray[j]["acc_id"] + " " + mainArray[j]["date"] + " " + mainArray[j]["charge"] + " " + mainArray[j]["saldo_in"] + " " + mainArray[j]["pay"] + " " + mainArray[j]["saldo_out"] + "\n");



            }


            for (int j = 0; j < mainArray.Count; j++)
            {
                //проверяем если ли начисление в создаваемом периоде
                NpgsqlCommand search_new_period = new NpgsqlCommand("SELECT DISTINCT public.charge.acc_id FROM public.charge WHERE acc_id = " + mainArray[j]["acc_id"] + " and date_create = '" + dateTimePicker3.Value + "' ", conn);
                conn.Open();
                NpgsqlDataReader read_ls = search_new_period.ExecuteReader();
                if (!read_ls.HasRows)
                {

                    conn.Close();
                    // конец проверки начисленией в создаваемом периоде

                    DateTime data = Convert.ToDateTime(mainArray[j]["date"]);
                    textBox2.AppendText(mainArray[j]["acc_id"] + " " + mainArray[j]["date"] + " " + mainArray[j]["saldo_out"] + " " + mainArray[j]["id"] + "\n");
                    conn.Open();
                    NpgsqlCommand add_charge = new NpgsqlCommand("INSERT INTO charge (id, acc_id, date_period, tarif_id, charge, pay, saldo_in, saldo_out, date_create, maket) VALUES (" + mainArray[j]["id"] + ", " + mainArray[j]["acc_id"] + ", '" + data.ToString("dd.MM.yyyy") + "',  " + mainArray[j]["tarif_id"] + ", '" + mainArray[j]["charge"] + "', '" + mainArray[j]["pay"] + "', '" + mainArray[j]["saldo_in"] + "', '" + mainArray[j]["saldo_out"] + "' , '" + data.ToString("yyyy-MM-dd") + "', '0' )", conn);
                    add_charge.ExecuteNonQuery();
                    //Thread.Sleep(1000);
                    conn.Close();
                }
                else
                {
                    conn.Close();
                }
            }


            NpgsqlCommand max_id_period = new NpgsqlCommand("SELECT max(id) FROM period", conn);

            conn.Open(); //Открываем соединение.

            int id_period = 0;


            NpgsqlDataReader read_id_period = max_id_period.ExecuteReader();

            while (read_id_period.Read())
            {
                id_period = Convert.ToInt32(read_id_period[0]);
            }
            conn.Close();

            id_period++;

            conn.Open();
            NpgsqlCommand add_period = new NpgsqlCommand("INSERT INTO period (id, name, date_open, date_close, date_cnt, text) VALUES (" + id_period + ", '" + textBox1.Text + "', '" + dateTimePicker1.Text + "',  '" + dateTimePicker2.Text + "', '" + dateTimePicker3.Text + "', 'учтены платежи с " + dateTimePicker4.Value + " по " + dateTimePicker5.Value + "')", conn);
            add_period.ExecuteNonQuery();
            //Thread.Sleep(1000);
            conn.Close();
            update_table_period();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in dataGridView1.SelectedCells)
            {
                cell = selectedCell;
                break;
            }

            if (cell != null)
            {

                DataGridViewRow row = cell.OwningRow;
                textBox1.Text = row.Cells[1].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                dateTimePicker2.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                dateTimePicker3.Value = DateTime.Parse(row.Cells[4].Value.ToString());
                dateTimePicker4.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                dateTimePicker5.Value = DateTime.Parse(row.Cells[3].Value.ToString());
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Double symm = 0;
            string summ = "";

            /* запрос на выборку для файла в Сисетму город
            SELECT 
      public.places_cnt.name,
      places_cnt1.name,
      places_cnt2.name,
      places_cnt2.plc_id,
      public.acc_info.plc_id,
      public.acc_info.acc_id,
      public.acc_info.flat,
      public.acc_info.owner,
      public.period.date_open,
      public.period.date_close,
      public."charge".saldo_out
    FROM
      public.places_cnt places_cnt1
      INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id)
      INNER JOIN public.places_cnt places_cnt2 ON (places_cnt1.plc_id = places_cnt2.parent_id)
      RIGHT OUTER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id)
      INNER JOIN public."charge" ON (public.acc_info.acc_id = public."charge".acc_id)
      INNER JOIN public.period ON (public."charge".date_create = public.period.date_cnt)
    WHERE
      public."charge".date_create = '"++"'

            */

            NpgsqlCommand summ_period = new NpgsqlCommand("SELECT saldo_out FROM charge WHERE date_create ='" + dateTimePicker3.Text + "'", conn);
            conn.Open();
            NpgsqlDataReader read_summ = summ_period.ExecuteReader();
            while (read_summ.Read())
            {
                symm = symm + Convert.ToDouble(read_summ[0].ToString());
            }

            summ = symm.ToString("#.##");
            conn.Close();
            System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"saldo.txt", false, Encoding.GetEncoding("cp866"));
            //System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"saldo.txt");
            textFile.WriteLine("#FILESUM    " + summ.Replace(',', '.'));
            textFile.WriteLine("#TYPE        7");
            textFile.WriteLine("#SERVICE     11501");

            NpgsqlCommand all_charge = new NpgsqlCommand("SELECT "
                           + "  public.acc_info.owner, "
                           + "  public.places_cnt.name, "
                          + "   places_cnt1.name, "
                          + "   places_cnt2.name, "
                           + "  public.acc_info.acc_id, "
                           + "  public.acc_info.flat, "
                           + "  public.charge.saldo_in, "
                           + "  public.period.date_open, "
                           + "  public.period.date_close, "
                          + "   public.charge.saldo_out, "
                            + "   places_cnt2.parent_name,"
                           + "    places_cnt2.parent_sor"
                       + "  FROM "
                         + "  public.places_cnt places_cnt1 "
                         + "  INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                         + "  INNER JOIN public.places_cnt places_cnt2 ON (places_cnt1.plc_id = places_cnt2.parent_id) "
                         + "  RIGHT OUTER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                         + "  INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id) "
                         + "  INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                        + " WHERE "
                          + " public.charge.date_create = '" + dateTimePicker3.Text + "'"
                            + "ORDER BY public.acc_info.acc_id", conn);

            conn.Open();
            NpgsqlDataReader reader = all_charge.ExecuteReader();
            int f = 0;
            while (reader.Read())
            {
                string flat = reader[5].ToString();
                string sor = reader[11].ToString();
                string s_in = reader[6].ToString();
                string s_out = reader[9].ToString();
                string d1 = reader[7].ToString();
                d1 = d1.Remove(11);

                string d2 = reader[8].ToString();
                d2 = d2.Remove(11);

                DateTime date1 = DateTime.Parse(d1);
                DateTime date2 = DateTime.Parse(d2);


                String s = reader[0].ToString();
                if (s != "")
                {
                    String[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Count() == 3)
                    {
                        if (words[1].Length > 1 & words[2].Length > 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1].Remove(1) + "." + words[2].Remove(1) + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }
                        if (words[1].Length > 1 & words[2].Length == 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1].Remove(1) + "." + words[2] + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }

                        if (words[1].Length == 1 & words[2].Length > 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1] + "." + words[2].Remove(1) + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }


                        if (words[1].Length == 1 & words[2].Length == 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1] + "." + words[2] + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }
                    }
                    if (words.Count() == 2)
                    {
                        if (words[1].Length > 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1].Remove(1) + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }
                        if (words[1].Length == 1)
                        {
                            textFile.WriteLine("" + words[0] + " " + words[1] + ".;" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                        }
                    }

                    if (words.Count() == 1)
                    {
                        textFile.WriteLine("" + words[0] + ";" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                    }


                }
                else
                {
                    textFile.WriteLine(";" + reader[1] + "," + reader[10] + " " + sor.Replace(".", "") + "," + reader[3] + "," + flat.Replace(" ", "") + ";" + reader[4] + ";" + s_in.Replace(",", ".") + ";;" + date1.ToString("dd") + "/" + date1.ToString("MM") + "/" + date1.ToString("yyyy") + ";" + date2.ToString("dd") + "/" + date2.ToString("MM") + "/" + date2.ToString("yyyy") + ";1:" + s_out.Replace(",", "."));
                }
                f++;
            }

            textBox2.AppendText("Количетсво записаных лицевых счетов: " + f+"\n");
            textBox2.AppendText("Файл saldo.txt созан и находится в папке с программой, необходимо перенести его в папку C:/invest/saldo/11501/ \n");
            conn.Close();
            textFile.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Dictionary<int, string> dic = new Dictionary<int, string>();
            Dictionary<int, string> saldo_in = new Dictionary<int, string>();
            int i = 0;
            double saldo = 0;
            NpgsqlCommand close_ls = new NpgsqlCommand("SELECT "
                                     +" public.acc_close.id, "
                                     +" public.acc_close.acc_code, "
                                     +" public.acc_close.date_close, "
                                     +" public.acc_close.text_close "
                                    +" FROM "
                                      +" public.acc_close "
                                    +" WHERE "
                                      +" public.acc_close.date_close >= '"+dateTimePicker1.Text+"' AND  "
                                      +" public.acc_close.date_close <= '"+dateTimePicker2.Text+"'", conn);
            conn.Open();
            NpgsqlDataReader reader = close_ls.ExecuteReader();
            while (reader.Read())
            {
                dic.Add(i, reader[1].ToString());
                i++;
            }
            conn.Close();

            for (int k = 0; k < dic.Count; k++)
            {
                NpgsqlCommand s_in = new NpgsqlCommand("SELECT public.charge.saldo_in, public.charge.id FROM public.charge WHERE public.charge.acc_id ="+dic[k].ToString()+" ORDER BY id desc limit 1 ",conn);
                conn.Open();
                NpgsqlDataReader reade_saldo = s_in.ExecuteReader();
                while (reade_saldo.Read())
                {
                    saldo += Convert.ToDouble(reade_saldo[0].ToString());
                    saldo_in.Add(k, reade_saldo[0].ToString());

                }

                conn.Close();
            }


            System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"close.txt", false, Encoding.GetEncoding("cp866"));
            //System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"saldo.txt");
            textFile.WriteLine("#FILESUM    "+ saldo.ToString());
            textFile.WriteLine("#TYPE        9");
            textFile.WriteLine("#SERVICE     11501");
            for (int l = 0; l < dic.Count(); l++)
            {
                textFile.WriteLine(";;"+dic[l].ToString()+";"+saldo_in[l].ToString()+";;;;");
            }
            textFile.Close();
            textBox2.AppendText("Количетсво закрытых лицевых счетов: " + dic.Count() + "\n");
            textBox2.AppendText("Файл close.txt созан и находится в папке с программой, необходимо перенести его в папку C:/invest/saldo/11501/ \n");
        }

        private void period_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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
