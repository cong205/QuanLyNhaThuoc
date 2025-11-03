using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.Classes
{
    internal static class UiTheme
    {
        private static readonly Font DefaultFont = new Font("Segoe UI", 9.75F);
        private static readonly Color FormBackColor = Color.White;
        private static readonly Color Primary = Color.FromArgb(0, 158, 247);
        private static readonly Color Muted = Color.FromArgb(108, 117, 125);

        public static void Apply(Form form)
        {
            if (form == null) return;
            form.Font = DefaultFont;
            form.BackColor = FormBackColor;
            form.MaximizeBox = false;

            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is GroupBox gb)
                {
                    gb.Font = new Font("Segoe UI", 13F, FontStyle.Regular);
                    gb.ForeColor = Color.FromArgb(33, 37, 41);
                }
                else if (c is Label)
                {
                    // Keep default label style for clarity
                }
                else if (c is Button btn)
                {
                    // Primary style for action buttons based on name heuristics
                    var isPrimary = new[] { "btnLogin", "btnLuu", "btnThem", "btnXacNhan" }
                        .Any(n => string.Equals(btn.Name, n, StringComparison.OrdinalIgnoreCase));

                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = isPrimary ? Primary : Muted;
                    btn.ForeColor = Color.White;
                }
                else if (c is DataGridView dgv)
                {
                    ApplyGrid(dgv);
                }

                if (c.HasChildren)
                {
                    ApplyToControls(c.Controls);
                }
            }
        }

        public static void ApplyGrid(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}


