using System;
using System.Windows.Forms;
using AsBuiltExplorer.Utilities;

namespace AsBuiltExplorer.Forms
{
    public partial class frmAddMod : Form
    {
        public ModEntry NewMod { get; private set; }

        public frmAddMod()
        {
            InitializeComponent();
        }

        public frmAddMod(string initialPlatform) : this()
        {
            if (!string.IsNullOrEmpty(initialPlatform))
            {
                txtPlatform.Text = initialPlatform;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPlatform.Text))
            {
                MessageBox.Show("Please enter a platform.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewMod = new ModEntry
            {
                Title = txtTitle.Text.Trim(),
                Platform = txtPlatform.Text.Trim(),
                Category = string.IsNullOrWhiteSpace(txtCategory.Text) ? "Custom" : txtCategory.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Instructions = txtInstructions.Text
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
