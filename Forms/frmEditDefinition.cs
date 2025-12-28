using System;
using System.Windows.Forms;

namespace AsBuiltExplorer
{
    public partial class frmEditDefinition : Form
    {
        public CommonFeature Result { get; private set; }

        TextBox txtName, txtModule, txtAddress, txtD1, txtD2, txtD3, txtNotes;
        Button btnSave, btnCancel;

        public frmEditDefinition(CommonFeature feature)
        {
            InitializeComponent();
            LoadFeature(feature);
        }

        void LoadFeature(CommonFeature f)
        {
            txtName.Text = f.Name;
            txtModule.Text = f.Module;
            txtAddress.Text = f.Address;
            txtD1.Text = f.Data1Mask;
            txtD2.Text = f.Data2Mask;
            txtD3.Text = f.Data3Mask;
            txtNotes.Text = f.Notes;

            // Clone to avoid direct mutation until save
            Result = new CommonFeature
            {
                ID = f.ID,
                Name = f.Name,
                Module = f.Module,
                Address = f.Address,
                Data1Mask = f.Data1Mask,
                Data2Mask = f.Data2Mask,
                Data3Mask = f.Data3Mask,
                Notes = f.Notes
            };
        }

        void InitializeComponent()
        {
            Text = "Edit Feature Definition";
            Size = new System.Drawing.Size(400, 350);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var lblName = new Label { Text = "Feature Name:", Top = 10, Left = 10, Width = 100 };
            txtName = new TextBox { Top = 10, Left = 120, Width = 250 };

            var lblModule = new Label { Text = "Module:", Top = 40, Left = 10, Width = 100 };
            txtModule = new TextBox { Top = 40, Left = 120, Width = 250 };

            var lblAddress = new Label { Text = "Address:", Top = 70, Left = 10, Width = 100 };
            txtAddress = new TextBox { Top = 70, Left = 120, Width = 250 };

            var lblD1 = new Label { Text = "Data 1 Mask:", Top = 100, Left = 10, Width = 100 };
            txtD1 = new TextBox { Top = 100, Left = 120, Width = 250 };

            var lblD2 = new Label { Text = "Data 2 Mask:", Top = 130, Left = 10, Width = 100 };
            txtD2 = new TextBox { Top = 130, Left = 120, Width = 250 };

            var lblD3 = new Label { Text = "Data 3 Mask:", Top = 160, Left = 10, Width = 100 };
            txtD3 = new TextBox { Top = 160, Left = 120, Width = 250 };

            var lblNotes = new Label { Text = "Notes:", Top = 190, Left = 10, Width = 100 };
            txtNotes = new TextBox { Top = 190, Left = 120, Width = 250, Height = 60, Multiline = true };

            btnSave = new Button { Text = "Save", Top = 270, Left = 210, DialogResult = DialogResult.OK };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Cancel", Top = 270, Left = 295, DialogResult = DialogResult.Cancel };

            Controls.AddRange(new Control[] { lblName, txtName, lblModule, txtModule, lblAddress, txtAddress,
                                                 lblD1, txtD1, lblD2, txtD2, lblD3, txtD3, lblNotes, txtNotes,
                                                 btnSave, btnCancel });

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        void BtnSave_Click(object sender, EventArgs e)
        {
            Result.Name = txtName.Text;
            Result.Module = txtModule.Text;
            Result.Address = txtAddress.Text;
            Result.Data1Mask = txtD1.Text;
            Result.Data2Mask = txtD2.Text;
            Result.Data3Mask = txtD3.Text;
            Result.Notes = txtNotes.Text;
            Close();
        }
    }
}