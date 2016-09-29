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
    public partial class Form1 : Form
    {
        //NpgsqlConnection conn = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=Servise;");
        public Form1()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_user newMDIChild = new add_user();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void добавитьЛСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_user newMDIChild = new add_user();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void открытьВсеЛСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            all_user newMDIChild = new all_user();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            double w = this.Width;
            int h = this.Height;
            newMDIChild.Width = Convert.ToInt16(w* 1);
            newMDIChild.Height = Convert.ToInt16(h * 1);


            // Display the new form.
            newMDIChild.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            add_user addUserMDI = new add_user();
            // Set the Parent Form of the Child window.
            addUserMDI.MdiParent = this;

            add_user closeUserMDI = new add_user();
            // Set the Parent Form of the Child window.
            closeUserMDI.MdiParent = this;

            period periodMDI = new period();
            periodMDI.MdiParent = this;
            // Display the new form.

            add_services add_tarifMDI = new add_services();
            add_tarifMDI.MdiParent = this;
            // Display the new form.

        }

        private void периодыToolStripMenuItem_Click(object sender, EventArgs e)
        {

            period frm = new period();
            frm.Owner = this;
            frm.Show();
        }

        private void загрузкаОплатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double w = this.Width;
            int h = this.Height;
            load_pay newMDICheild = new load_pay();
            newMDICheild.MdiParent = this;
            // Display the new form.
            newMDICheild.Show();
            newMDICheild.Width = Convert.ToInt16(w * 0.95);
            newMDICheild.Height = Convert.ToInt16(h * 0.90);
        }

        private void печатьКвитанцийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintTo newMDICheild = new PrintTo();
            newMDICheild.MdiParent = this;
            // Display the new form.
            newMDICheild.Show();
        }

        private void квитанцияДляЛСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            report1.Load("reports_only.frx");
            DataSet dataCharge = new DataSet();
            NpgsqlDataAdapter data_ls = new NpgsqlDataAdapter("SELECT DISTINCT  "
                                         + " public.acc_info.owner, "
                                          + "places_cnt1.name,  "
                                         + " places_cnt2.name,  "
                                         + " public.acc_info.flat,  "
                                         + " public.charge.date_create,  "
                                         + " public.charge.saldo_out,  "
                                         + " public.charge.saldo_in,  "
                                         + " public.charge.pay,  "
                                         + " public.charge.charge,  "
                                         + " public.charge.maket,  "
                                         + " public.acc_info.plc_id,  "
                                         + "public.group.group_name,  "
                                         + " public.acc_info.acc_id, "
                                         + "public.period.name  "
                                       + " FROM  "
                                         + " public.acc_info  "
                                         + " INNER JOIN public.charge ON (public.acc_info.acc_id = public.charge.acc_id)  "
                                         + " INNER JOIN public.places_cnt places_cnt2 ON (places_cnt2.plc_id = public.acc_info.plc_id)  "
                                         + " INNER JOIN public.places_cnt places_cnt1 ON (places_cnt2.parent_id = places_cnt1.plc_id)  "
                                         + " INNER JOIN public.places_cnt ON (places_cnt1.parent_id = public.places_cnt.plc_id)  "
                                         + " LEFT OUTER JOIN public.group ON (public.acc_info.plc_id = public.group.house_id)  "
                                         + " INNER JOIN public.period ON (public.charge.date_create = public.period.date_cnt)  "
                                       + " WHERE  "
                                         + " public.charge.charge != '0' AND "
                                         + " public.acc_info.acc_id= 100002 AND"
                                         + " public.period.name = '2016-06-01'", conn);
            conn.Open();

            data_ls.Fill(dataCharge, "dataCharge");
            report1.RegisterData(dataCharge, "dataCharge");
            conn.Close();
            report1.Design();
            

        }
    }
}
