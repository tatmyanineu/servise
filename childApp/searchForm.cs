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
    public partial class searchForm : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public all_user frm;
        string key_street = null;
        Dictionary<int, Dictionary<string, string>> adres = new Dictionary<int, Dictionary<string, string>>();
        public searchForm(all_user f1)
        {
            InitializeComponent();
            frm = f1;
        }

        private void searchForm_Load(object sender, EventArgs e)
        {
            int i = 0;
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
            //AutoCompleteStringCollection group_collect = new AutoCompleteStringCollection();
            reader = street.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();

                comboBox1.Items.Add(reader[2].ToString() + " " + reader[3].ToString());
                dic.Add("street", reader[0].ToString());
                dic.Add("id", reader[1].ToString());
                adres.Add(i, dic);
                i++;
            }
            //textBox2.AutoCompleteCustomSource = group_collect;
            conn.Close(); //Закрываем соединение.


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string street = "";

            if (key_street == null)
            {
                street = "";
            }
            else
            {
                int k = Convert.ToInt32(key_street);
                street = adres[k]["street"];
            }

            string ls = textBox1.Text;

            string house = textBox3.Text;
            string flat = textBox4.Text;

            frm.search_table(ls, street, house, flat);
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            key_street = comboBox1.SelectedIndex.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
