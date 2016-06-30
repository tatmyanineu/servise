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
        NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public searchForm()
        {
            InitializeComponent();
        }

        private void searchForm_Load(object sender, EventArgs e)
        {
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
            AutoCompleteStringCollection group_collect = new AutoCompleteStringCollection();
            reader = street.ExecuteReader();
            while (reader.Read())
            {
                    group_collect.Add(reader[2].ToString() + " " + reader[3].ToString());
            }
            textBox2.AutoCompleteCustomSource = group_collect;
            conn.Close(); //Закрываем соединение.


        }
    }
}
