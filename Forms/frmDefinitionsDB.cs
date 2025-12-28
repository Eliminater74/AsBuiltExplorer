using System;
using System.Data;
using System.Windows.Forms;

namespace AsBuiltExplorer
{
    public partial class frmDefinitionsDB : Form
    {
        private DataTable _masterTable;

        public frmDefinitionsDB()
        {
            InitializeComponent();
        }

        private void frmDefinitionsDB_Load(object sender, EventArgs e)
        {
            dgvCodes.CellFormatting += DgvCodes_CellFormatting;
            LoadData();
        }

        private void DgvCodes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCodes.Columns[e.ColumnIndex].Name.Contains("Mask") && e.Value != null)
            {
                string val = e.Value.ToString();
                if (val.Contains("*"))
                {
                    e.CellStyle.Font = new System.Drawing.Font(dgvCodes.Font, System.Drawing.FontStyle.Bold);
                    e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    // Optional: Make text larger?
                    // e.CellStyle.Font = new System.Drawing.Font(dgvCodes.Font.FontFamily, 11f, System.Drawing.FontStyle.Bold);
                }
            }
        }

        private void LoadData()
        {
            try
            {
                _masterTable = DefinitionsDBHelper.GetAllCodes();
                dgvCodes.DataSource = _masterTable;
                
                // Format Columns
                if (dgvCodes.Columns["ID"] != null) dgvCodes.Columns["ID"].Visible = false;
                if (dgvCodes.Columns["FeatureName"] != null) dgvCodes.Columns["FeatureName"].Width = 200;
                if (dgvCodes.Columns["Notes"] != null) dgvCodes.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading definitions: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_masterTable == null) return;
            
            string filter = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(filter))
            {
                _masterTable.DefaultView.RowFilter = "";
            }
            else
            {
                _masterTable.DefaultView.RowFilter = string.Format("FeatureName LIKE '%{0}%' OR Module LIKE '%{0}%' OR Notes LIKE '%{0}%'", filter);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
