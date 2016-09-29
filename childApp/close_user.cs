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
    public partial class close_user : Form
    {

        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public close_user()
        {
            InitializeComponent();
        }

        private void close_user_Load(object sender, EventArgs e)
        {
            this.Text += " : " + all_user.acc_code;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = 0;

            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            if (textBox1.Text != "")
            {

                NpgsqlCommand max_ls_close = new NpgsqlCommand("SELECT max(id) FROM public.acc_close", conn);
                conn.Open();
                NpgsqlDataReader reader = max_ls_close.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() != "")
                    {
                        id = Convert.ToInt32(reader[0]) + 1;
                    }
                }
                conn.Close();


                NpgsqlCommand add_ls_close = new NpgsqlCommand("INSERT INTO acc_close(id, acc_code, date_close, text_close) "
                                                                + "VALUES (" + id + ", " + all_user.acc_code + ", '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', '"+textBox1.Text+"')", conn);
                conn.Open();
                add_ls_close.ExecuteNonQuery();
                conn.Close();
                NpgsqlCommand close_ls = new NpgsqlCommand("UPDATE public.acc_info SET date_close='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', acc_close='" + dateTimePicker1.Value.ToString("dd.MM.yyyy") + "' WHERE acc_id = " + all_user.acc_code + "", conn);
                conn.Open();
                close_ls.ExecuteNonQuery();
                conn.Close();
               
                MessageBox.Show("Лицевой счет закрыт");
                all_user.acc_code = null;
                this.Close();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
