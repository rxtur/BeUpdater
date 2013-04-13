using System;
using System.ComponentModel;
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

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 1; i < 5; i++)
                {
                    Upgrade.Run(i);
                    backgroundWorker1.ReportProgress(i * 25);
                }
                MessageBox.Show("All done!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void ShowMsg(string msg)
        {
            MessageBox.Show(msg);
            backgroundWorker1.ReportProgress(1);
        }
    }
}
