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
        private string chucVu = "";
        private string maNhanVien = "";
        public void setChucVu(string cv,string maNV)
        {
            chucVu = cv;
            maNhanVien = maNV;
        }
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
            if(chucVu!="Quản lý")
            {
                MessageBox.Show("Chức năng này chỉ dành cho Quản lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
            hdn.setMaNV(maNhanVien);
            hdn.ShowDialog();

        }

        // =================== Tổng cục: Tìm kiếm, Báo cáo, Trợ giúp, Thoát ===================
        private void menuTimKiem_Click(object sender, EventArgs e)
        {
           
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

        
        

        private void báoCáoDoanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoDoanhThu bcdt = new frmBaoCaoDoanhThu();
            bcdt.ShowDialog();
        }

        private void báoCáoTồnKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoTonKho bctk = new frmBaoCaoTonKho();
            bctk.ShowDialog();
        }

        private void báoCáoNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoNhapHang bcnh = new frmBaoCaoNhapHang();
            bcnh.ShowDialog();
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lôThuốcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoThuoc frmLoThuoc = new frmLoThuoc();
            frmLoThuoc.ShowDialog();
        }
    }
}
