using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;

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
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCodes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit.");
                return;
            }

            // Get selected object
            // The DataSource is a DataTable, so BoundItem is DataRowView
            var row = dgvCodes.SelectedRows[0];
            var dataRow = (row.DataBoundItem as System.Data.DataRowView)?.Row;
            
            if (dataRow == null) return;

            // Map DataRow to CommonFeature
            var f = new CommonFeature
            {
                ID = Convert.ToInt32(dataRow["ID"]),
                Name = dataRow["FeatureName"].ToString(),
                Module = dataRow["Module"].ToString(),
                Address = dataRow["Address"].ToString(),
                Data1Mask = dataRow["Data1Mask"].ToString(),
                Data2Mask = dataRow["Data2Mask"].ToString(),
                Data3Mask = dataRow["Data3Mask"].ToString(),
                Notes = dataRow["Notes"].ToString()
            };

            using (var frm = new frmEditDefinition(f))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Update DB
                    try
                    {
                        CommonDatabase.UpdateEntry(frm.Result);
                        
                        // Update UI Grid directly (faster than reload)
                        dataRow["FeatureName"] = frm.Result.Name;
                        dataRow["Module"] = frm.Result.Module;
                        dataRow["Address"] = frm.Result.Address;
                        dataRow["Data1Mask"] = frm.Result.Data1Mask;
                        dataRow["Data2Mask"] = frm.Result.Data2Mask;
                        dataRow["Data3Mask"] = frm.Result.Data3Mask;
                        dataRow["Notes"] = frm.Result.Notes;
                        
                        // Also update memory cache if needed, but LoadData() pulls from DB usually.
                        // CommonDatabase.Features mismatch now? 
                        // CommonDatabase.ForceReload() might be safer but slower.
                        // Let's stick to Grid Update for responsiveness.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving: " + ex.Message);
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "AsBuilt_Export.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Export current Grid View or All?
                        // If User filtered via Search, maybe export only visible?
                        // For now, let's export ALL (CommonDatabase.Features).
                        // Or better: Export what is in the Grid (DataTable DefaultView)
                        
                        // Map DataTable to List<CommonFeature> for Export
                        var dt = dgvCodes.DataSource as System.Data.DataTable;
                        var list = new List<CommonFeature>();
                        
                        foreach (System.Data.DataRow dr in dt.DefaultView.ToTable().Rows)
                        {
                             list.Add(new CommonFeature
                             {
                                Name = dr["FeatureName"].ToString(),
                                Module = dr["Module"].ToString(),
                                Address = dr["Address"].ToString(),
                                Data1Mask = dr["Data1Mask"].ToString(),
                                Data2Mask = dr["Data2Mask"].ToString(),
                                Data3Mask = dr["Data3Mask"].ToString(),
                                Notes = dr["Notes"].ToString()
                             });
                        }
                        
                        CommonDatabase.ExportToCSV(sfd.FileName, list);
                        MessageBox.Show("Export Successful!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error exporting: " + ex.Message);
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CommonDatabase.ImportCSV(ofd.FileName);
                        LoadData(); // Reload UI
                        MessageBox.Show("Import Successful! Database reloaded.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error importing: " + ex.Message);
                    }
                }
            }
        }
    }
}
