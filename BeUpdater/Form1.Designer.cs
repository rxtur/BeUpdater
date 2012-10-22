namespace BeUpdater
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialogOld = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialogNew = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOld = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(614, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = "This application will copy all your custom files from current (older) BlogEngine." +
    "NET install directory and merge them INTO NEW BlogEngine installation directory." +
    "";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOld
            // 
            this.btnOld.Location = new System.Drawing.Point(488, 103);
            this.btnOld.Name = "btnOld";
            this.btnOld.Size = new System.Drawing.Size(158, 23);
            this.btnOld.TabIndex = 1;
            this.btnOld.Text = "Old BE Directory";
            this.btnOld.UseVisualStyleBackColor = true;
            this.btnOld.Click += new System.EventHandler(this.btnOld_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(488, 132);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(158, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New BE Directory";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpgrade.Location = new System.Drawing.Point(488, 175);
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Size = new System.Drawing.Size(158, 28);
            this.btnUpgrade.TabIndex = 3;
            this.btnUpgrade.Text = "Upgrade!";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(29, 106);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(291, 15);
            this.lblFrom.TabIndex = 4;
            this.lblFrom.Text = "Select your current blog directory to upgrade from  ->";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(29, 135);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(283, 15);
            this.lblTo.TabIndex = 5;
            this.lblTo.Text = "Select directory with latest BlogEngine.NET files  ->";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 215);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.btnUpgrade);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnOld);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "BlogEngine.NET Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOld;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogNew;
        private System.Windows.Forms.Button btnOld;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnUpgrade;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
    }
}

