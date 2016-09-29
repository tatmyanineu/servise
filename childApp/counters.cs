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
    public partial class counters : Form
    {
        public counters()
        {
            InitializeComponent();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            if (e.KeyCode == Keys.F5)
            {
                refresh_table();
            }
        }

        private void refresh_table()
        {
            throw new NotImplementedException();
        }

        private void counters_FormClosed(object sender, FormClosedEventArgs e)
        {
            all_user.acc_code = null;
        }
    }
}
