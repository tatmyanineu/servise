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
    public partial class PrintTo : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public DataTable forLog = new DataTable();
        public PrintTo()
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
            DateTime commentDate = table.Rows[0].Field<DateTime>("date_cnt");
            textBox1.Text = commentDate.ToString("dd.MM.yyyy");
            textBox2.Text = table.Rows[0].Field<string>("name");

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


        private void PrintTo_Load(object sender, EventArgs e)
        {
            update_table_period();
            //textBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
            //textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            forLog.Columns.Add("id", typeof(int));
            forLog.Columns.Add("text", typeof(string));
            NpgsqlCommand all_group = new NpgsqlCommand("SELECT DISTINCT group_name FROM public.group ORDER BY group_name", conn);
            conn.Open();
            NpgsqlDataReader reader = all_group.ExecuteReader();
            AutoCompleteStringCollection group_collect = new AutoCompleteStringCollection();
            while (reader.Read())
            {
                group_collect.Add(reader[0].ToString());
            }
            textBox3.AutoCompleteCustomSource = group_collect;
            conn.Close();
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
                DateTime date = DateTime.Parse(row.Cells[4].Value.ToString());
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox1.Text = date.ToString("dd.MM.yyyy");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int l = textBox4.GetLineFromCharIndex(textBox4.SelectionStart);
            var lines = textBox4.Lines.ToList();
            lines.RemoveAt(l);
            textBox4.Lines = lines.ToArray();
            //MessageBox.Show(l.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                textBox4.AppendText(textBox3.Text + "\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable group_report = new DataTable();
            group_report.Columns.Add("Group", typeof(string));
            group_report.Columns.Add("Count", typeof(int));
            Dictionary<int, string> dic = new Dictionary<int, string>();
            int k = 0;
            NpgsqlCommand count = new NpgsqlCommand("SELECT DISTINCT  "
                                      + "public.group.group_name, "
                                     + " public.charge.acc_id "
                                    + "FROM "
                                      + "public.acc_info "
                                      + "INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id) "
                                      + "LEFT OUTER JOIN public.group ON (public.acc_info.plc_id = public.group.house_id) "
                                     + " INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                                    + "WHERE "
                                      + "public.charge.date_create = '" + textBox1.Text + "' AND "
                                        + "  public.charge.charge != '0' AND "
                                        + "public.group.group_name !=''"
                                      + "ORDER BY "
                                      + "public.group.group_name", conn);
            conn.Open();
            NpgsqlDataReader reader = count.ExecuteReader();

            while (reader.Read())
            {
                dic.Add(k, reader[0].ToString());
                k++;
            }
            conn.Close();


            int kol = 0;
            for (int i = 0; i < dic.Count; i++)
            {
                if (i + 1 != dic.Count)
                {
                    if (dic[i] != dic[i + 1])
                    {
                        kol++;
                        //MessageBox.Show(dic[i].ToString()+ " "+ kol);
                        DataRow dr = group_report.NewRow();
                        dr["Group"] = dic[i].ToString();
                        dr["Count"] = kol;
                        group_report.Rows.Add(dr);
                        kol = 0;
                    }
                    if (dic[i] == dic[i + 1])
                    {
                        kol++;
                    }
                }
                else
                {
                    kol++;
                    //MessageBox.Show(dic[i].ToString()+ " "+ kol);
                    DataRow dr = group_report.NewRow();
                    dr["Group"] = dic[i].ToString();
                    dr["Count"] = kol;
                    group_report.Rows.Add(dr);
                }
            }

            report1.Load("report_count.frx");
            report1.RegisterData(group_report, "groupCount");
            report1.SetParameterValue("name", textBox2.Text);
            report1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            forPrintGroup form5 = new forPrintGroup();
            form5.Owner = this;
            form5.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            report1.Load("reports.frx");
            conn.Close();
            //DateTime date = DateTime.Parse(textBox2)
            DataSet dataCharge = new DataSet();
            //String[] array = { "ПЕЧАТЬ: КА №1201", "ПЕЧАТЬ: КА №1202" };

            for (int i = 0; i < textBox4.Lines.Count(); i++)
            {
                NpgsqlDataAdapter all_info = new NpgsqlDataAdapter("SELECT DISTINCT "
                                     + "public.acc_info.owner, "
                                     + "places_cnt1.name, "
                                     + "places_cnt2.name, "
                                     + "public.acc_info.flat, "
                                     + "public.charge.date_create, "
                                     + "public.charge.saldo_out, "
                                     + "public.charge.saldo_in, "
                                     + "public.charge.pay, "
                                     + "public.charge.charge, "
                                     + "public.charge.maket, "
                                     + "public.acc_info.plc_id, "
                                     + "public.group.group_name, "
                                     + "public.acc_info.acc_id, "
                                     + "public.period.name "
                                   + "FROM "
                                     + "public.acc_info "
                                     + "INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id) "
                                     + "INNER JOIN public.places_cnt places_cnt2 ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                                     + "INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id) "
                                     + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                     + "LEFT OUTER JOIN public.group ON (public.acc_info.plc_id = public.group.house_id) "
                                     + "INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                                   + "WHERE "
                                     + "public.charge.date_create = '" + textBox1.Text + "'  AND "
                                     + "public.group.group_name = '" + textBox4.Lines[i] + "' AND "
                                    + " public.charge.charge != '0' ", conn);
                all_info.Fill(dataCharge, "dataCharge");

            }



            report1.RegisterData(dataCharge, "dataCharge");
            report1.SetParameterValue("group", textBox3.Text);
            conn.Close();

            report1.Show();
            textBox4.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            report1.Load("reports.frx");
            conn.Close();
            //DateTime date = DateTime.Parse(textBox2)
            DataSet dataCharge = new DataSet();
            //String[] array = { "ПЕЧАТЬ: КА №1201", "ПЕЧАТЬ: КА №1202" };

            for (int i = 0; i < textBox4.Lines.Count(); i++)
            {
                NpgsqlDataAdapter all_info = new NpgsqlDataAdapter("SELECT DISTINCT "
                                     + "public.acc_info.owner, "
                                     + "places_cnt1.name, "
                                     + "places_cnt2.name, "
                                     + "public.acc_info.flat, "
                                     + "public.charge.date_create, "
                                     + "public.charge.saldo_out, "
                                     + "public.charge.saldo_in, "
                                     + "public.charge.pay, "
                                     + "public.charge.charge, "
                                     + "public.charge.maket, "
                                     + "public.acc_info.plc_id, "
                                     + "public.group.group_name, "
                                     + "public.acc_info.acc_id, "
                                     + "public.period.name "
                                   + "FROM "
                                     + "public.acc_info "
                                     + "INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id) "
                                     + "INNER JOIN public.places_cnt places_cnt2 ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                                     + "INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id) "
                                     + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                     + "LEFT OUTER JOIN public.group ON (public.acc_info.plc_id = public.group.house_id) "
                                     + "INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt) "
                                   + "WHERE "
                                     + "public.charge.date_create = '" + textBox1.Text + "'  AND "
                                     + "public.group.group_name = '" + textBox4.Lines[i] + "' AND "
                                    + " public.charge.charge != '0' ", conn);
                all_info.Fill(dataCharge, "dataCharge");

            }



            report1.RegisterData(dataCharge, "dataCharge");
            report1.SetParameterValue("group", textBox3.Text);
            conn.Close();

            report1.Design();
            textBox4.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
