using System;
using System.Windows.Forms;

namespace AsBuiltExplorer
{
    public partial class frmVehicleDB : Form
    {
        public string SelectedFilePath { get; private set; }

        public frmVehicleDB(string currentPath, string currentVIN)
        {
            InitializeComponent();
            txtPath.Text = currentPath;
            txtVIN.Text = currentVIN;
            
            // Auto-suggest name from VIN (just simplistic for now)
            if (!string.IsNullOrEmpty(currentVIN))
            {
                // Try to see if this VIN already exists? 
                // For now, let user type name
            }
        }

        private void frmVehicleDB_Load(object sender, EventArgs e)
        {
            VehicleDatabase.Load();
            RefreshList();
        }

        private void RefreshList()
        {
            lstVehicles.Items.Clear();
            foreach (var v in VehicleDatabase.Entries)
            {
                lstVehicles.Items.Add(v);
            }
            // Sort?
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter a name for this vehicle.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show("No file path to save.", "Missing Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VehicleDatabase.AddEntry(txtName.Text, txtVIN.Text, txtPath.Text);
            txtName.Text = ""; // Clear name after add
            RefreshList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstVehicles.SelectedItem is VehicleEntry entry)
            {
                if (MessageBox.Show($"Are you sure you want to delete '{entry.FriendlyName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    VehicleDatabase.DeleteEntry(entry);
                    RefreshList();
                }
            }
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            if (lstVehicles.SelectedItem is VehicleEntry entry)
            {
                SelectedFilePath = entry.FilePath;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a vehicle from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
             // Optional: Display details in groupbox if we wanted edit mode
        }
    }
}
