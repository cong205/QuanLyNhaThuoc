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
using QuanLyNhaThuoc.BaoCao;

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
            
        }

        private void munuTroGiup_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
        "📘 HƯỚNG DẪN SỬ DỤNG CHƯƠNG TRÌNH QUẢN LÝ NHÀ THUỐC\n\n" +
        "1️⃣ **Danh mục:**\n   - Quản lý danh sách thuốc, nhà cung cấp, nhân viên, khách hàng.\n" +
        "2️⃣ **Hóa đơn:**\n   - Lập hóa đơn bán, nhập hàng, và xem lịch sử giao dịch.\n" +
        "3️⃣ **Tìm kiếm:**\n   - Tìm thuốc hoặc nhà cung cấp theo Mã hoặc Tên.\n" +
        "4️⃣ **Báo cáo:**\n   - Xuất danh sách thuốc, hóa đơn, nhà cung cấp ra file Excel/CSV.\n" +
        "5️⃣ **Cấu hình:**\n   - Cài đặt thông tin cửa hàng, người dùng, và sao lưu dữ liệu.\n" +
        "6️⃣ **Thoát:**\n   - Đóng chương trình an toàn.\n\n" +
        "💡 Mẹo: Hãy thường xuyên sao lưu dữ liệu để tránh mất mát thông tin.",
        "Trợ giúp - Quản lý Nhà Thuốc",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
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

        private void báoCáoDoanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoDoanhThu bcdt = new frmBaoCaoDoanhThu();
            bcdt.ShowDialog();
        }
    }
}
