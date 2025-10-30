using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.DanhMuc;
using QuanLyNhaThuoc.HoaDon;
using QuanLyNhaThuoc.Classes;

namespace QuanLyNhaThuoc
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmKhachHang kh = new frmKhachHang();
            kh.ShowDialog();
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nv = new frmNhanVien();
            nv.ShowDialog();
        }


        private void mnuThuoc_Click(object sender, EventArgs e)
        {
            frmThuoc t = new frmThuoc();
            t.ShowDialog();
        }

        private void mnuLoaiThuoc_Click(object sender, EventArgs e)
        {
            frmLoaiThuoc lt = new frmLoaiThuoc();
            lt.ShowDialog();
        }

        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            frmNhaCungCap ncc = new frmNhaCungCap();
            ncc.ShowDialog();
        }

        private void mnuDonViThuoc_Click(object sender, EventArgs e)
        {
            frmDonVi dv = new frmDonVi();
            dv.ShowDialog();
        }

        private void mnuHoaDonBan_Click(object sender, EventArgs e)
        {
            frmHoaDonBan hdb = new frmHoaDonBan();
            hdb.ShowDialog();

        }

        private void mnuHoaDonNhap_Click(object sender, EventArgs e)
        {
            frmHoaDonNhap hdn = new frmHoaDonNhap();
            hdn.ShowDialog();

        }

        // =================== Tổng cục: Tìm kiếm, Báo cáo, Trợ giúp, Thoát ===================
        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = PromptKeyword("Nhập từ khóa (Mã/Tên NCC):");
            if (string.IsNullOrWhiteSpace(keyword)) return;
            DataProcesser dp = new DataProcesser();
            string sql = "SELECT MaNCC, TenNCC, DiaChi, SoDienThoai FROM tNhaCungCap " +
                         $"WHERE MaNCC LIKE N'%{keyword.Replace("'", "''")}%' OR TenNCC LIKE N'%{keyword.Replace("'", "''")}%'";
            DataTable dt = dp.GetDataTable(sql);
            ShowGridDialog(dt, "Kết quả tìm kiếm Nhà cung cấp");
        }

        private void menuBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                DataProcesser dp = new DataProcesser();
                DataTable dt = dp.GetDataTable("SELECT MaNCC, TenNCC, DiaChi, SoDienThoai FROM tNhaCungCap");
                SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV Files (*.csv)|*.csv", FileName = "BaoCaoNhaCungCap.csv" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new System.IO.StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < dt.Columns.Count; i++) sw.Write((i > 0 ? "," : "") + dt.Columns[i].ColumnName);
                        sw.WriteLine();
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++) sw.Write((i > 0 ? "," : "") + row[i]?.ToString()?.Replace(",", " "));
                            sw.WriteLine();
                        }
                    }
                    MessageBox.Show("Xuất báo cáo thành công!\n" + sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất báo cáo: " + ex.Message);
            }
        }

        private void munuTroGiup_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Tổng cục:\n- Tìm kiếm NCC theo Mã/Tên từ menu Tìm kiếm\n- Báo cáo: xuất CSV danh sách nhà cung cấp\n- Danh mục/Hóa đơn: mở các màn hình quản lý tương ứng\n- Thoát: đóng ứng dụng",
                "Trợ giúp");
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Helpers
        private string PromptKeyword(string title)
        {
            Form f = new Form();
            f.Text = title;
            f.StartPosition = FormStartPosition.CenterParent;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MinimizeBox = false;
            f.MaximizeBox = false;
            f.ClientSize = new Size(380, 120);
            Label lbl = new Label { Left = 12, Top = 15, Width = 80, Text = "Từ khóa:" };
            TextBox tb = new TextBox { Left = 95, Top = 12, Width = 270 };
            Button ok = new Button { Text = "Tìm", Left = 210, Width = 70, Top = 55, DialogResult = DialogResult.OK };
            Button cancel = new Button { Text = "Hủy", Left = 295, Width = 70, Top = 55, DialogResult = DialogResult.Cancel };
            f.Controls.Add(lbl); f.Controls.Add(tb); f.Controls.Add(ok); f.Controls.Add(cancel);
            f.AcceptButton = ok; f.CancelButton = cancel;
            return f.ShowDialog(this) == DialogResult.OK ? tb.Text : string.Empty;
        }

        private void ShowGridDialog(DataTable data, string title)
        {
            Form f = new Form();
            f.Text = title;
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new Size(700, 400);
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.DataSource = data;
            f.Controls.Add(grid);
            f.ShowDialog(this);
        }
    }
}
