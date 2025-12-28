using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AsBuiltExplorer.Utilities
{
    public static class ModernTheme
    {
        // --- Color Palette ---
        // Sidebar (Dark Gray)
        public static Color SidebarBack = Color.FromArgb(30, 30, 30); // VS Code style
        public static Color SidebarText = Color.FromArgb(200, 200, 200);
        public static Color SidebarSelectBack = Color.FromArgb(45, 45, 48); // Slightly lighter
        public static Color SidebarSelectFore = Color.White;
        public static Color Accent = Color.FromArgb(0, 122, 204); // VS Blue

        // Content (Light)
        public static Color ContentBack = Color.White;
        public static Color ContentText = Color.FromArgb(30, 30, 30);
        
        // Buttons
        public static Color ButtonBack = Color.FromArgb(240, 240, 240);
        public static Color ButtonFore = Color.Black;
        public static Color ButtonHover = Color.FromArgb(200, 200, 220);

        // --- Tab Drawing Logic ---
        public static void DrawSidebarTab(TabControl tabControl, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle bounds = tabControl.GetTabRect(e.Index);
            
            // Adjust bounds slightly to avoid gaps
            bounds.Inflate(1, 1);

            bool isSelected = (e.State == DrawItemState.Selected);

            // 1. Draw Background
            using (var brush = new SolidBrush(isSelected ? SidebarSelectBack : SidebarBack))
            {
                g.FillRectangle(brush, bounds);
            }

            // 2. Draw Accent Bar (Left side) if selected
            if (isSelected)
            {
                Rectangle accentRect = new Rectangle(bounds.Left, bounds.Top, 4, bounds.Height);
                using (var brush = new SolidBrush(Accent))
                {
                    g.FillRectangle(brush, accentRect);
                }
            }
            
            // 3. Draw Hover Effect (if purely using MouseMove events we could do more, 
            // but for now simple selection is good)

            // 4. Draw Text
            using (var brush = new SolidBrush(isSelected ? SidebarSelectFore : SidebarText))
            {
                // Align text to the left with padding
                // In Left-Aligned tabs, the Text is routed 90deg by default in some modes, 
                // but with ItemSize set correctly (Width,Height swapped), it stays horizontal if we use StringFormat correctly.
                // However, OwnerDrawFixed gives us the Bounds as they appear on screen.
                
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                // Font - Use Segoe UI if available, else Sans Serif
                Font tabFont = new Font("Segoe UI", 10f, isSelected ? FontStyle.Bold : FontStyle.Regular);
                
                g.DrawString(tabPage.Text, tabFont, brush, bounds, sf);
            }
        }

        // --- Control Styling Logic ---
        public static void Apply(Form form)
        {
            // Set Form Base
            form.BackColor = ContentBack; // Main area white
            form.ForeColor = ContentText; // Text dark

            foreach (Control c in form.Controls)
            {
                ApplyToControl(c, form);
            }
        }

        private static void ApplyToControl(Control c, Form parentForm)
        {
            // Skip Special Controls
            if (c.Name != null && (c.Name.StartsWith("lblComp") || c.Name.StartsWith("tbxComp"))) return;

            // 1. TabControl (Sidebar)
            if (c is TabControl tc)
            {
                // The sidebar container itself should be dark
                tc.BackColor = SidebarBack; // Note: TabControl BackColor property often does nothing in WinForms
                
                // We must set the 'Page' BackColor for the empty space behind tabs
                // This is tricky in pure WinForms. 
                // Instead, we ensure the Parent form behind it is White (done), 
                // and we rely on DrawItem to fill the tab strip.
                
                // Recursively style pages
                foreach(TabPage page in tc.TabPages)
                {
                    page.BackColor = ContentBack;
                    page.ForeColor = ContentText;
                    
                    foreach(Control child in page.Controls)
                    {
                        ApplyToControl(child, parentForm);
                    }
                }
            }
            // 2. Buttons
            else if (c is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.LightGray;
                btn.FlatAppearance.MouseOverBackColor = ButtonHover;
                btn.BackColor = ButtonBack;
                btn.ForeColor = ButtonFore;
                btn.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            }
            // 3. TextBoxes, Lists, Grids
            else if (c is TextBox || c is ListBox || c is ComboBox)
            {
                c.BackColor = Color.White; // Ensure clean white
                c.ForeColor = Color.Black;
            }
            // 4. GroupBoxes, Labels, Panels
            else if (c is GroupBox || c is Panel || c is Label)
            {
                 // Inherit transparent/parent colors usually
                 // For GroupBox we might want a specific text color
                 c.ForeColor = ContentText;
                 
                 // Recurse
                 if (c.HasChildren)
                 {
                     foreach(Control child in c.Controls)
                     {
                         ApplyToControl(child, parentForm);
                     }
                 }
            }
        }
    }
}
