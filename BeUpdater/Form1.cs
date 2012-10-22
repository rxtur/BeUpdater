using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOld_Click(object sender, EventArgs e)
        {
            folderBrowserDialogOld.ShowDialog();
            lblFrom.Text = folderBrowserDialogOld.SelectedPath;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            folderBrowserDialogNew.ShowDialog();
            lblTo.Text = folderBrowserDialogNew.SelectedPath;
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            Upgrade.Old = folderBrowserDialogOld.SelectedPath;
            Upgrade.New = folderBrowserDialogNew.SelectedPath;

            MessageBox.Show(Upgrade.Run());
        }
    }
}
