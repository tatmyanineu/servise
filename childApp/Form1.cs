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
            newMDIChild.Width = Convert.ToInt16(w* 0.95);
            newMDIChild.Height = Convert.ToInt16(h * 0.90);


            // Display the new form.
            newMDIChild.Show();
        }
    }
}
