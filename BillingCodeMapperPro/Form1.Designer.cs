namespace BillingCodeMapperPro
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.Button btnLoadReport;
        private System.Windows.Forms.Button btnLoadCptReference;
        private System.Windows.Forms.Button btnMapCPT;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Button btnDownloadReport;
        private System.Windows.Forms.PictureBox pictureBoxBackground;

        private void InitializeComponent()
        {
            btnLoadReport = new Button();
            btnLoadCptReference = new Button();
            btnMapCPT = new Button();
            dataGridView1 = new DataGridView();
            pictureBoxLogo = new PictureBox();
            btnDownloadReport = new Button();
            pictureBoxBackground = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBackground).BeginInit();
            SuspendLayout();
            // 
            // btnLoadReport
            // 
            btnLoadReport.Location = new Point(20, 20);
            btnLoadReport.Name = "btnLoadReport";
            btnLoadReport.Size = new Size(150, 30);
            btnLoadReport.TabIndex = 0;
            btnLoadReport.Text = "Load Report";
            btnLoadReport.Click += btnLoadReport_Click;
            // 
            // btnLoadCptReference
            // 
            btnLoadCptReference.Location = new Point(180, 20);
            btnLoadCptReference.Name = "btnLoadCptReference";
            btnLoadCptReference.Size = new Size(180, 30);
            btnLoadCptReference.TabIndex = 1;
            btnLoadCptReference.Text = "View CPT Reference";
            btnLoadCptReference.Click += btnLoadCptReference_Click;
            // 
            // btnMapCPT
            // 
            btnMapCPT.Location = new Point(370, 20);
            btnMapCPT.Name = "btnMapCPT";
            btnMapCPT.Size = new Size(150, 30);
            btnMapCPT.TabIndex = 2;
            btnMapCPT.Text = "Map CPT Codes";
            btnMapCPT.Click += btnMapCPT_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.Location = new Point(20, 70);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(760, 330);
            dataGridView1.TabIndex = 4;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxLogo.Location = new Point(656, -16);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(132, 97);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 5;
            pictureBoxLogo.TabStop = false;
            pictureBoxLogo.Click += pictureBoxLogo_Click;
            // 
            // btnDownloadReport
            // 
            btnDownloadReport.Location = new Point(630, 410);
            btnDownloadReport.Name = "btnDownloadReport";
            btnDownloadReport.Size = new Size(150, 30);
            btnDownloadReport.TabIndex = 3;
            btnDownloadReport.Text = "Download Report";
            btnDownloadReport.Click += btnDownloadReport_Click;
            // 
            // pictureBoxBackground
            // 
            pictureBoxBackground.Location = new Point(-133, 397);
            pictureBoxBackground.Name = "pictureBoxBackground";
            pictureBoxBackground.Size = new Size(480, 85);
            pictureBoxBackground.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBackground.TabIndex = 6;
            pictureBoxBackground.TabStop = false;
            // 
            // Form1
            // 
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLoadReport);
            Controls.Add(btnLoadCptReference);
            Controls.Add(btnMapCPT);
            Controls.Add(btnDownloadReport);
            Controls.Add(dataGridView1);
            Controls.Add(pictureBoxLogo);
            Controls.Add(pictureBoxBackground);
            Name = "Form1";
            Text = "CPT Mapper Pro";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBackground).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
