using ClosedXML.Excel;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BillingCodeMapperPro
{
    public partial class Form1 : Form
    {
        private DataTable reportData;
        private DataTable cptReference;
        private string loadedReportPath = string.Empty;

        public Form1()
        {
            InitializeComponent();
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            string iconPath = Path.Combine(appFolder, "CPTMapperPro.ico");

            if (File.Exists(iconPath))
            {
                this.Icon = new Icon(iconPath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLogo();
            LoadCptReference(); 
            LoadBackground();
        }

        private void LoadLogo()
        {
            try
            {
                string appFolder = AppDomain.CurrentDomain.BaseDirectory;
                string logoPath = Path.Combine(appFolder, "CPTMapperPro.jpg");

                if (File.Exists(logoPath))
                {
                    pictureBoxLogo.Image = Image.FromFile(logoPath);
                    pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    MessageBox.Show("Logo not found at: " + logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load logo: " + ex.Message);
            }
        }

        private void LoadBackground()
        {
            try
            {
                string appFolder = AppDomain.CurrentDomain.BaseDirectory;
                string backgroundPath = Path.Combine(appFolder, "background.jpg");

                if (File.Exists(backgroundPath))
                {
                    pictureBoxBackground.Image = Image.FromFile(backgroundPath);
                    pictureBoxBackground.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    MessageBox.Show("Background image not found at: " + backgroundPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load background image: " + ex.Message);
            }
        }

        private void btnLoadCptReference_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BillingCodeReference.xlsx");
            if (File.Exists(path))
            {
                Process.Start("explorer.exe", $"\"{path}\"");
            }
            else
            {
                MessageBox.Show("Reference file not found at expected location!");
            }
        }

        private void LoadCptReference()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BillingCodeReference.xlsx");

            if (!File.Exists(path))
            {
                MessageBox.Show("BillingCodeReference.xlsx not found. Check the file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheet(1);
            cptReference = new DataTable();
            cptReference.Columns.Add("ORD_PROCEDURE");
            cptReference.Columns.Add("CPT");

            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                var dr = cptReference.NewRow();
                dr["ORD_PROCEDURE"] = row.Cell(1).GetString();
                dr["CPT"] = row.Cell(2).GetString();
                cptReference.Rows.Add(dr);
            }
        }

        private void btnLoadReport_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                loadedReportPath = ofd.FileName;

                using var workbook = new XLWorkbook(ofd.FileName);
                var sheet = workbook.Worksheet(1);
                reportData = new DataTable();

                foreach (var cell in sheet.Row(1).Cells())
                    reportData.Columns.Add(cell.GetString());

                foreach (var row in sheet.RowsUsed().Skip(1))
                {
                    var dr = reportData.NewRow();
                    for (int i = 0; i < reportData.Columns.Count; i++)
                        dr[i] = row.Cell(i + 1).GetString();
                    reportData.Rows.Add(dr);
                }

                dataGridView1.DataSource = reportData;
                // MessageBox.Show("Report loaded!"); - for debug only
            }
        }

        private void btnMapCPT_Click(object sender, EventArgs e)
        {
            if (reportData == null || cptReference == null)
            {
                MessageBox.Show("CPT reference file not in expected location.");
                return;
            }

            if (!reportData.Columns.Contains("CPT"))
                reportData.Columns.Add("CPT");

            foreach (DataRow row in reportData.Rows)
            {
                if (string.IsNullOrWhiteSpace(row["CPT"]?.ToString()))
                {
                    string ordProc = row["ORD_PROCEDURE"]?.ToString();
                    var match = cptReference.AsEnumerable()
                        .FirstOrDefault(r => r.Field<string>("ORD_PROCEDURE") == ordProc);

                    if (match != null)
                        row["CPT"] = match["CPT"];
                }
            }

            dataGridView1.Refresh();
            // MessageBox.Show("CPT mapping complete."); - for debug only
        }

        private void btnDownloadReport_Click(object sender, EventArgs e)
        {
            if (reportData == null || string.IsNullOrWhiteSpace(loadedReportPath))
            {
                MessageBox.Show("No report data to export.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "Mapped_Report.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using var workbook = new XLWorkbook(loadedReportPath);
                var worksheet = workbook.Worksheet(1);

                // Get headers
                var headerRow = worksheet.Row(1);
                int colCount = headerRow.LastCellUsed().Address.ColumnNumber;
                int cptColIndex = -1;

                // Try to find CPT column
                for (int i = 1; i <= colCount; i++)
                {
                    if (headerRow.Cell(i).GetString().Trim().ToUpper() == "CPT")
                    {
                        cptColIndex = i;
                        break;
                    }
                }

                // If not found, add new CPT column
                if (cptColIndex == -1)
                {
                    cptColIndex = colCount + 1;
                    headerRow.Cell(cptColIndex).Value = "CPT";
                }

                // Get ORD_PROCEDURE column index
                int ordProcCol = GetColumnIndex(worksheet, "ORD_PROCEDURE");
                if (ordProcCol == -1)
                {
                    MessageBox.Show("ORD_PROCEDURE column not found in the loaded Excel file.");
                    return;
                }

                // Fill CPT values row-by-row
                for (int rowIndex = 2; rowIndex <= worksheet.LastRowUsed().RowNumber(); rowIndex++)
                {
                    string ordProc = worksheet.Cell(rowIndex, ordProcCol).GetString();
                    string cpt = FindCptForProcedure(ordProc);
                    worksheet.Cell(rowIndex, cptColIndex).Value = cpt;
                }

                workbook.SaveAs(sfd.FileName);
                MessageBox.Show("Formatted report exported with CPT codes.");
            }
        }

        private int GetColumnIndex(IXLWorksheet sheet, string columnName)
        {
            var headerRow = sheet.Row(1);
            foreach (var cell in headerRow.Cells())
            {
                if (cell.GetString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return cell.Address.ColumnNumber;
            }
            return -1;
        }

        private string FindCptForProcedure(string ordProcedure)
        {
            if (cptReference == null || string.IsNullOrWhiteSpace(ordProcedure)) return string.Empty;

            var match = cptReference.AsEnumerable()
                .FirstOrDefault(r => r.Field<string>("ORD_PROCEDURE").Equals(ordProcedure, StringComparison.OrdinalIgnoreCase));

            return match?["CPT"]?.ToString() ?? string.Empty;
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {

        }
    }
}
