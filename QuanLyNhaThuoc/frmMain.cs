using QuanLyNhaThuoc.BaoCao;
using QuanLyNhaThuoc.Classes;
using QuanLyNhaThuoc.DanhMuc;
using QuanLyNhaThuoc.HoaDon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class frmMain : Form
    {
        private string chucVu = "";
        private string maNhanVien = "";
        Classes.DataProcesser dp = new Classes.DataProcesser();
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
            hdb.setMaNV(maNhanVien);
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
            bcdt.setMaNV(maNhanVien);
            bcdt.ShowDialog();
        }

        private void báoCáoTồnKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoTonKho bctk = new frmBaoCaoTonKho();
            bctk.setMaNV(maNhanVien);

            bctk.ShowDialog();
        }

        private void báoCáoNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoNhapHang bcnh = new frmBaoCaoNhapHang();
            bcnh.setMaNV(maNhanVien);

            bcnh.ShowDialog();
        }
        // Hàm hiển thị ComboBox tìm kiếm động
        private void ShowSearchCombo(string sql, string displayMember, string valueMember, string title, string infoQuery)
        {
            // Panel nền
            Panel pnlSearch = new Panel();
            pnlSearch.Size = new Size(320, 60);
            pnlSearch.Location = new Point(200, 120);
            pnlSearch.BackColor = Color.WhiteSmoke;
            pnlSearch.BorderStyle = BorderStyle.FixedSingle;

            // ComboBox
            ComboBox cbo = new ComboBox();
            cbo.DropDownStyle = ComboBoxStyle.DropDown;
            cbo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbo.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbo.Width = 280;
            cbo.Location = new Point(20, 15);

            // Nạp dữ liệu
            try
            {
                DataTable dt = dp.GetDataTable(sql);
                cbo.DataSource = dt;
                cbo.DisplayMember = displayMember;
                cbo.ValueMember = valueMember;
                cbo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                return;
            }

            // Khi chọn 1 mục
            cbo.SelectedIndexChanged += (s, ev) =>
            {
                if (cbo.SelectedIndex >= 0)
                {
                    string id = cbo.SelectedValue.ToString();

                    try
                    {
                        // Lấy thông tin chi tiết
                        DataTable info = dp.GetDataTable(string.Format(infoQuery, id));

                        // 🔹 Ẩn giờ trong cột DateTime
                        foreach (DataColumn col in info.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                foreach (DataRow row in info.Rows)
                                {
                                    if (row[col] != DBNull.Value)
                                        row[col] = Convert.ToDateTime(row[col]).ToString("dd/MM/yyyy");
                                }
                            }
                        }

                        if (info.Rows.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (DataColumn col in info.Columns)
                            {
                                sb.AppendLine($"{col.ColumnName}: {info.Rows[0][col]}");
                            }
                            MessageBox.Show(sb.ToString(), $"Chi tiết - {title}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi lấy thông tin chi tiết: " + ex.Message);
                    }

                    this.Controls.Remove(pnlSearch);
                }
            };

            cbo.Leave += (s, ev) => { this.Controls.Remove(pnlSearch); };

            pnlSearch.Controls.Add(cbo);
            this.Controls.Add(pnlSearch);
            pnlSearch.BringToFront();
            cbo.Focus();
        }
        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "SELECT MaNCC, TenNCC, (MaNCC + ' - ' + TenNCC) AS DisplayText FROM tNhaCungCap";
            string info = "SELECT MaNCC AS [Mã NCC], TenNCC AS [Tên NCC], DiaChi AS [Địa chỉ] FROM tNhaCungCap WHERE MaNCC = '{0}'";
            ShowSearchCombo(sql, "DisplayText", "MaNCC", "Nhà Cung Cấp", info);
        }
        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "SELECT MaKH, TenKH, (MaKH + ' - ' + TenKH) AS DisplayText FROM tKhachHang";
            string info = "SELECT MaKH AS [Mã KH], TenKH AS [Tên KH], GioiTinh AS [Giới tính], DiaChi AS [Địa chỉ], SoDienThoai AS [SĐT] FROM tKhachHang WHERE MaKH = '{0}'";
            ShowSearchCombo(sql, "DisplayText", "MaKH", "Khách Hàng", info);
        }
        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chucVu != "Quản lý")
            {
                MessageBox.Show("Chức năng này chỉ dành cho Quản lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "SELECT MaNV, TenNV, (MaNV + ' - ' + TenNV) AS DisplayText FROM tNhanVien";
            string info = "SELECT MaNV AS [Mã NV], TenNV AS [Tên NV], GioiTinh AS [Giới tính], DiaChi AS [Địa chỉ], SoDienThoai AS [SĐT], ChucVu AS [Chức vụ] FROM tNhanVien WHERE MaNV = '{0}'";
            ShowSearchCombo(sql, "DisplayText", "MaNV", "Nhân Viên", info);
        }


        private void lôThuốcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoThuoc frmLoThuoc = new frmLoThuoc();
            frmLoThuoc.ShowDialog();
        }
    }
}
