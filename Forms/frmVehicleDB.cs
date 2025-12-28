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

            // Auto-suggest name from VIN
            if (!string.IsNullOrEmpty(currentVIN))
            {
                var temp = new VehicleEntry { VIN = currentVIN };
                VehicleDatabase.UpdateVehicleDataFromVIN(temp);

                if (!string.IsNullOrEmpty(temp.Model))
                {
                    // "2008 Ford Expedition EL XLT (1FMFK...)"
                    txtName.Text = $"{temp.Year} {temp.Make} {temp.Model} ({currentVIN})".Trim();
                }
            }
        }

        void frmVehicleDB_Load(object sender, EventArgs e)
        {
            VehicleDatabase.Load();
            RefreshList();
        }

        void RefreshList()
        {
            lstVehicles.Items.Clear();

            foreach (var v in VehicleDatabase.Entries)
                lstVehicles.Items.Add(v);
            

            // Sort?
        }

        void btnAdd_Click(object sender, EventArgs e)
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

        void btnDelete_Click(object sender, EventArgs e)
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

        void btnUse_Click(object sender, EventArgs e)
        {
            if (lstVehicles.SelectedItem is VehicleEntry entry)
            {
                // Logic: If original path exists, use it.
                // If not, but we have Content, write to Cache and use that.
                var finalPath = entry.FilePath;

                if (!System.IO.File.Exists(finalPath))
                {
                    if (!string.IsNullOrEmpty(entry.FileContent))
                    {
                        // Create cache dir
                        var cacheDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache");
                        System.IO.Directory.CreateDirectory(cacheDir);

                        // Sanitize filename from VIN or Name
                        var safeName = entry.VIN;
                        if (string.IsNullOrEmpty(safeName)) safeName = "Unknown_" + Guid.NewGuid().ToString().Substring(0, 8);

                        var cacheFile = System.IO.Path.Combine(cacheDir, safeName + ".ab");
                        System.IO.File.WriteAllText(cacheFile, entry.FileContent);
                        finalPath = cacheFile;
                    }
                }

                SelectedFilePath = finalPath;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please select a vehicle from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnClose_Click(object sender, EventArgs e) => Close();

        void lstVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Display details in groupbox if we wanted edit mode
        }
    }
}