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
    public partial class all_user : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public int ls = 0;
        public all_user()
        {
            InitializeComponent();
        }

        public void update_main_table()
        {

            DataTable dt = new DataTable();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT "
                                                      + "public.acc_info.acc_id, "
                                                       + "public.acc_info.owner, "
                                                      + "places_cnt2.district, "
                                                      + "places_cnt1.name, "
                                                      + "places_cnt2.name, "
                                                      + "public.acc_info.flat, "
                                                      + "public.acc_info.date_open, "
                                                      + "public.acc_info.acc_close, "
                                                      + "public.acc_info.contakt, "
                                                      + "public.tarif.tarif_taxa, "
                                                      + "public.tarif.tarif_text "
                                                    + "FROM "
                                                      + "public.places_cnt places_cnt1 "
                                                      + "INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id) "
                                                      + "INNER JOIN public.places_cnt places_cnt2 ON (places_cnt1.plc_id = places_cnt2.parent_id) "
                                                      + "INNER JOIN public.acc_info ON (places_cnt2.plc_id = public.acc_info.plc_id) "
                                                      + "INNER JOIN public.tarif ON (public.acc_info.tarif_if = public.tarif.id)", conn);
            conn.Open(); //Открываем соединение.
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(comm);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[0].HeaderText = "Лицевой счет";

            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[1].HeaderText = "ФИО собственика";

            dataGridView1.Columns[2].Width = 130;
            dataGridView1.Columns[2].HeaderText = "Район";

            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[3].HeaderText = "Улица";

            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[4].HeaderText = "Дом";

            dataGridView1.Columns[5].Width = 60;
            dataGridView1.Columns[5].HeaderText = "Квартира";

            dataGridView1.Columns[6].Width = 70;
            dataGridView1.Columns[6].HeaderText = "Открыт";

            dataGridView1.Columns[7].Width = 70;
            dataGridView1.Columns[7].HeaderText = "Закрыты";

            dataGridView1.Columns[8].Width = 200;
            dataGridView1.Columns[8].HeaderText = "Контактная информация";

            dataGridView1.Columns[9].Width = 80;
            dataGridView1.Columns[9].HeaderText = "Тариф(ставка)";

            dataGridView1.Columns[10].Width = 190;
            dataGridView1.Columns[10].HeaderText = "Тариф";

        }


        private void all_user_Load(object sender, EventArgs e)
        {
            update_main_table();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            searchForm f = new searchForm();
            f.Show();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            add_user frm = new add_user();
            frm.Owner = this;
            frm.Show();
        }
    }
}