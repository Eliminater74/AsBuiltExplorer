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
            LoadData();
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
