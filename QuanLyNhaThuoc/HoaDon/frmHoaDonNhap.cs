using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace QuanLyNhaThuoc.HoaDon
{
    public partial class frmHoaDonNhap : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        Classes.Function ft = new Classes.Function();
        bool isLoading = false;
        private string maNhanVien = "";
        private string tenNhanVien = "";
        public frmHoaDonNhap()
        {
            InitializeComponent();
        }
        public void setMaNV(string maNV)
        {
            maNhanVien = maNV;
            string sql = "SELECT * FROM tNhanVien WHERE MaNV = '" + maNV + "'";
            DataTable dt = dp.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                tenNhanVien = dt.Rows[0]["TenNV"].ToString();
            }
        }

        private void frmHoaDonNhap_Load(object sender, EventArgs e)
        {
            isLoading = true;
            LoadMaHoaDon();
            LoadMaLo();
            LoadMaNCC();
            //LoadMaNV(); //để tạm để test khi chưa đăng nhập
            cboMaNV.Text = maNhanVien;
            txtTenNV.Text = tenNhanVien;
            isLoading = false;
        }
        //private void LoadMaNV()
        //{
        //    DataTable dt = dp.GetDataTable("select * from tNhanVien");
        //    ft.FillComBox(cboMaNV, dt, "MaNV", "MaNV");
        //    cboMaNV.SelectedIndex = -1;
        //}
        private void LoadMaHoaDon()
        {
            DataTable dt = dp.GetDataTable("select * from tHoaDonNhap");
            ft.FillComBox(cboMaHoaDon, dt, "MaHDN", "MaHDN");
            cboMaHoaDon.SelectedIndex = -1;
        }
        private void LoadMaLo()
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tLoThuoc");
            ft.FillComBox(cboMaLo, dt, "MaLo", "MaLo");
            cboMaLo.SelectedIndex = -1;
        }
        private void LoadMaNCC()
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tNhaCungCap");
            ft.FillComBox(cboMaNCC, dt, "MaNCC", "MaNCC");
            cboMaNCC.SelectedIndex = -1;
        }
        private void cboMaNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            DataTable dt = dp.GetDataTable("SELECT * FROM tNhaCungCap WHERE MaNCC = '" + cboMaNCC.Text + "'");
            if (dt.Rows.Count > 0)
            {
                txtTenNCC.Text = dt.Rows[0]["TenNCC"].ToString();
                txtDiaChi.Text = dt.Rows[0]["DiaChi"].ToString();
                txtSDT.Text = dt.Rows[0]["SoDienThoai"].ToString();
            }
        }

        private void txtTimKiem_Click(object sender, EventArgs e)
        {
            string maHD = cboMaHoaDon.Text.Trim();
            if (maHD == "")
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn mã hóa đơn!");
                return;
            }
            // Lấy thông tin chung
            string sqlHD = "SELECT * FROM tHoaDonNhap hdn" +
                " join tNhaCungCap ncc on hdn.MaNCC=ncc.MaNCC " +
                " join tNhanVien nv on hdn.MaNV=nv.MaNV" +
                " WHERE MaHDN = '" + maHD + "'";
            DataTable dtHD = dp.GetDataTable(sqlHD);
            if (dtHD.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn!");
                return;
            }
            txtMaHDN.Text = dtHD.Rows[0]["MaHDN"].ToString();
            dtpNgayNhap.Text = dtHD.Rows[0]["NgayNhap"].ToString();
            cboMaNCC.Text = dtHD.Rows[0]["MaNCC"].ToString();
            txtTenNCC.Text= dtHD.Rows[0]["TenNCC"].ToString();
            txtDiaChi.Text= dtHD.Rows[0]["DiaChi"].ToString();
            txtSDT.Text= dtHD.Rows[0]["SoDienThoai"].ToString();
            cboMaNV.Text = dtHD.Rows[0]["MaNV"].ToString();
            txtTenNV.Text= dtHD.Rows[0]["TenNV"].ToString();
            txtTongTien.Text= dtHD.Rows[0]["TongTien"].ToString();

            // Lấy thông tin chi tiết hóa đơn
            loadCTHD(maHD);

        }
        private void loadCTHD(string maHD)
        {
            // Lấy thông tin chi tiết hóa đơn
            string sqlCT = @"SELECT c.MaLo,t.TenThuoc,SoLuongNhap,t.GiaNhap,ThanhTien
                 FROM tChiTietHDN c
                 JOIN tLoThuoc l ON c.MaLo = l.MaLo
                 JOIN tThuoc t ON l.MaThuoc = t.MaThuoc
                 WHERE c.MaHDN = '" + maHD + "'";

            DataTable dtCT = dp.GetDataTable(sqlCT);
            dgvCTHDN.DataSource = dtCT;
            dgvCTHDN.Columns[0].HeaderText = "Mã Lô";
            dgvCTHDN.Columns[1].HeaderText = "Tên Thuốc";
            dgvCTHDN.Columns[2].HeaderText = "Số Lượng Nhập";
            dgvCTHDN.Columns[3].HeaderText = "Đơn Giá Nhập";
            dgvCTHDN.Columns[4].HeaderText = "Thành Tiền";
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();
            string ttncc = "SELECT * FROM tNhaCungCap WHERE SoDienThoai = '" + sdt + "'";
            DataTable dt = dp.GetDataTable(ttncc);
            if (dt.Rows.Count > 0)
            {
                cboMaNCC.Text = dt.Rows[0]["MaNCC"].ToString();
                txtTenNCC.Text = dt.Rows[0]["TenNCC"].ToString();
                txtDiaChi.Text = dt.Rows[0]["DiaChi"].ToString();
            }
            else // THÊM MỚI NCC
            {
                if(txtTenNCC.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp!");
                    txtTenNCC.Focus();
                    return;
                }
                if(txtDiaChi.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ nhà cung cấp!");
                    txtDiaChi.Focus();
                    return;
                }
                string sql = "SELECT TOP 1 MaNCC FROM tNhaCungCap ORDER BY MaNCC DESC";
                DataTable dtb = dp.GetDataTable(sql);
                string lastMa = dtb.Rows[0]["MaNCC"].ToString(); // Ví dụ "NCC05"
                int num = int.Parse(lastMa.Substring(3)); // lấy phần số
                num++;
                string newMa = "NCC" + num.ToString("D2"); // NCC06
                //string newMa = "NCC" + DateTime.Now.Ticks.ToString().Substring(8);

                string insert = "INSERT INTO tNhaCungCap VALUES('" + newMa + "', N'" + txtTenNCC.Text +
                                "', N'" + txtDiaChi.Text + "', '" + sdt + "')";

                dp.ExecuteNonQuery(insert);
                LoadMaNCC();
                cboMaNCC.Text = newMa;
                MessageBox.Show("Đã thêm nhà cung cấp mới!");
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsDigit(e.KeyChar)&&!Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if(!Char.IsControl(e.KeyChar)&& txtSDT.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void cboMaLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (cboMaLo.SelectedIndex == -1)
            {
                return;
            }
                
            string sql = "SELECT * " +
                "FROM tLoThuoc l JOIN tThuoc t ON l.MaThuoc = t.MaThuoc " +
                "WHERE l.MaLo = '" + cboMaLo.Text + "'";
            DataTable dt = dp.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                txtTenThuoc.Text = dt.Rows[0]["TenThuoc"].ToString();
                txtDonGiaNhap.Text = dt.Rows[0]["GiaNhap"].ToString();
            }
        }
        void ResetForm()
        {
            cboMaHoaDon.SelectedIndex = -1;
            txtMaHDN.Clear();
            dtpNgayNhap.Value = DateTime.Now;
            cboMaNCC.SelectedIndex = -1;
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            cboMaNV.Text = maNhanVien;
            txtTenNV.Text = tenNhanVien;
            txtTongTien.Clear();
            cboMaLo.SelectedIndex = -1;
            txtTenThuoc.Clear();
            txtSoLuongNhap.Clear();
            txtDonGiaNhap.Clear();
            dgvCTHDN.DataSource = null;
        }

        private string TaoMaHoaDonMoi()
        {
            string maHD;
            Random rnd = new Random();
            bool trung;

            do
            {
                maHD = "HDN_" + DateTime.Now.ToString("ddMMyyyy") + "0" + rnd.Next(100, 999);
                string sqlCheck = $"SELECT COUNT(*) FROM tHoaDonNhap WHERE MaHDN = '{maHD}'";
                DataTable dt = dp.GetDataTable(sqlCheck);
                int count = Convert.ToInt32(dt.Rows[0][0]);
                trung = (count > 0);
            }
            while (trung); // nếu trùng thì sinh lại mã mới

            return maHD;
        }

        private void txtThem_Click(object sender, EventArgs e)
        {
            ResetForm();
            string maMoi = TaoMaHoaDonMoi();
            txtMaHDN.Text = maMoi;

            MessageBox.Show("Tạo mới hóa đơn: " + maMoi);
        }

        private void txtSoLuongNhap_TextChanged(object sender, EventArgs e)
        {
            if (txtSoLuongNhap.Text.Trim() == "" || txtDonGiaNhap.Text.Trim() == "")
            {
                txtThanhTien.Text = "";
                return;
            }

            int soluong = 0;
            double dongia = 0;

            // Chuyển đổi an toàn
            int.TryParse(txtSoLuongNhap.Text, out soluong);
            double.TryParse(txtDonGiaNhap.Text, out dongia);

            // Nếu người dùng nhập chữ hoặc 0 thì không tính
            if (soluong <= 0 || dongia <= 0)
            {
                txtThanhTien.Text = "";
                return;
            }

            double thanhtien = soluong * dongia;
            txtThanhTien.Text = thanhtien.ToString("0.##");
        }

        private void txtSoLuongNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) &&!char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLuu_Click(object sender, EventArgs e)
        {
            string maHDN = txtMaHDN.Text.Trim();
            string maNV = cboMaNV.Text.Trim();
            string maNCC = cboMaNCC.SelectedValue?.ToString() ?? "";
            DateTime ngayNhap = dtpNgayNhap.Value;

            if (string.IsNullOrEmpty(maHDN) || string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin hóa đơn chung (Mã HĐN, Nhà Cung Cấp)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpNgayNhap.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày nhập không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayNhap.Focus();
                return;
            }

            // 1. Kiểm tra Mã Lô (cboMaLo.SelectedValue phải khác null)
            if (cboMaLo.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Lô thuốc cần nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaLo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtThanhTien.Text))
            {
                MessageBox.Show("Không thể tính Thành Tiền. Vui lòng kiểm tra lại Số Lượng và Đơn Giá Nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtSoLuongNhap.Text) || string.IsNullOrEmpty(txtThanhTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin chi tiết hàng hóa!");
                return;
            }

            // Kiểm tra hóa đơn có chưa, nếu chưa thì thêm
            string sqlCheckHD = $"SELECT COUNT(*) FROM tHoaDonNhap WHERE MaHDN = '{maHDN}'";
            DataTable dtCheck = dp.GetDataTable(sqlCheckHD);
            if (Convert.ToInt32(dtCheck.Rows[0][0]) == 0)
            {
                // 1. Trường hợp 1: Hóa đơn CHƯA CÓ -> INSERT mới
                string sqlInsertHD = $"INSERT INTO tHoaDonNhap VALUES ('{maHDN}', '{maNCC}', '{maNhanVien}', '{ngayNhap:yyyy-MM-dd}', 0)";
                dp.ExecuteNonQuery(sqlInsertHD);
                cboMaNV.Text = maNhanVien;
                txtTenNV.Text = tenNhanVien;
            }
            else
            {
                // 2. Trường hợp 2: Hóa đơn ĐÃ CÓ -> UPDATE Mã NV sang người đang đăng nhập
                string sqlUpdateNV = $"UPDATE tHoaDonNhap SET MaNV = '{maNhanVien}' WHERE MaHDN = '{maHDN}'";
                dp.ExecuteNonQuery(sqlUpdateNV);
                cboMaNV.Text = maNhanVien;
                txtTenNV.Text = tenNhanVien;
            }
            
            string sqlInsertCT = $"INSERT INTO tChiTietHDN (MaHDN, MaLo, SoLuongNhap, ThanhTien) " +
                         $"VALUES ('{maHDN}', '{cboMaLo.SelectedValue}', {txtSoLuongNhap.Text}, {txtThanhTien.Text})";
            dp.ExecuteNonQuery(sqlInsertCT);
            //cap nhat so luong ton kho
            string sqlUpdateKho = $"UPDATE tLoThuoc SET SoLuongTon = SoLuongTon + {txtSoLuongNhap.Text} WHERE MaLo = '{cboMaLo.SelectedValue}'";
            dp.ExecuteNonQuery(sqlUpdateKho);
            // Cập nhật lại tổng tiền hóa đơn nhập
            string sqlUpdateTong = $"UPDATE tHoaDonNhap " +
                                   $"SET TongTien = (SELECT SUM(ThanhTien) FROM tChiTietHDN WHERE MaHDN = '{maHDN}') " +
                                   $"WHERE MaHDN = '{maHDN}'";
            dp.ExecuteNonQuery(sqlUpdateTong);
            txtTongTien.Text= dp.GetDataTable($"SELECT TongTien FROM tHoaDonNhap WHERE MaHDN = '{maHDN}'").Rows[0][0].ToString();
            MessageBox.Show("Lưu hóa đơn nhập thành công!");
            loadCTHD(maHDN);
        }

        private void dgvCTHDN_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa san pham nay?",
        "Xac nhan", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string maHDN = cboMaHoaDon.Text.Trim();
                string maLo = dgvCTHDN.CurrentRow.Cells["MaLo"].Value.ToString();
                int soLuong = Convert.ToInt32(dgvCTHDN.CurrentRow.Cells["SoLuongNhap"].Value);

                // 1. Xóa dòng chi tiết hóa đơn
                string sqlDelete = $"DELETE FROM tChiTietHDN WHERE MaHDN='{maHDN}' AND MaLo='{maLo}'";
                dp.ExecuteNonQuery(sqlDelete);

                // 2. Trừ lại số lượng trong kho
                string sqlUpdateKho = $"UPDATE tLoThuoc SET SoLuongTon = SoLuongTon - {soLuong} WHERE MaLo='{maLo}'";
                dp.ExecuteNonQuery(sqlUpdateKho);
                //3/ Cập nhật lại tổng tiền hóa đơn nhập
                string sqlUpdateTong = $"UPDATE tHoaDonNhap " +
                                       $"SET TongTien = (SELECT SUM(ThanhTien) FROM tChiTietHDN WHERE MaHDN = '{maHDN}') " +
                                       $"WHERE MaHDN = '{maHDN}'";
                dp.ExecuteNonQuery(sqlUpdateTong);
                txtTongTien.Text= dp.GetDataTable($"SELECT TongTien FROM tHoaDonNhap WHERE MaHDN = '{maHDN}'").Rows[0][0].ToString();
                // 4. Load lại DataGridView
                loadCTHD(maHDN);
                MessageBox.Show("Da xoa san pham khoi hoa don!");
                
            }
        }
        private void txtHuy_Click(object sender, EventArgs e)
        {
            string maHDN = cboMaHoaDon.Text.Trim();

            if (MessageBox.Show("Ban co chac muon huy hoa don nay?",
                "Xac nhan", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // 1. Trả lại số lượng vào kho
                string sqlSelect = $"SELECT MaLo, SoLuongNhap FROM tChiTietHDN WHERE MaHDN='{maHDN}'";
                DataTable dt = dp.GetDataTable(sqlSelect);
                foreach (DataRow row in dt.Rows)
                {
                    string maLo = row["MaLo"].ToString();
                    int soLuong = Convert.ToInt32(row["SoLuongNhap"]);
                    string sqlUpdate = $"UPDATE tLoThuoc SET SoLuongTon = SoLuongTon - {soLuong} WHERE MaLo='{maLo}'";
                    dp.ExecuteNonQuery(sqlUpdate);
                }

                // 2. Xóa chi tiết
                dp.ExecuteNonQuery($"DELETE FROM tChiTietHDN WHERE MaHDN='{maHDN}'");

                // 3. Xóa hóa đơn chính
                dp.ExecuteNonQuery($"DELETE FROM tHoaDonNhap WHERE MaHDN='{maHDN}'");

                MessageBox.Show("Da huy hoa don thanh cong!");
                loadCTHD(maHDN);
            }
        }

        private void txtIn_Click(object sender, EventArgs e)
        {
            string maHDN = txtMaHDN.Text.Trim();
            if (string.IsNullOrEmpty(maHDN))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần in!");
                return;
            }

            // Lấy thông tin hóa đơn nhập
            string sqlHD = @"SELECT hdn.MaHDN, hdn.NgayNhap, nv.TenNV, ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai, hdn.TongTien
                     FROM tHoaDonNhap hdn
                     JOIN tNhanVien nv ON hdn.MaNV = nv.MaNV
                     JOIN tNhaCungCap ncc ON hdn.MaNCC = ncc.MaNCC
                     WHERE hdn.MaHDN = '" + maHDN + "'";
            DataTable dtHD = dp.GetDataTable(sqlHD);

            if (dtHD.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn để in!");
                return;
            }

            // Lấy thông tin chi tiết
            string sqlCT = @"SELECT c.MaLo, t.TenThuoc, c.SoLuongNhap, t.GiaNhap, c.ThanhTien
                     FROM tChiTietHDN c
                     JOIN tLoThuoc l ON c.MaLo = l.MaLo
                     JOIN tThuoc t ON l.MaThuoc = t.MaThuoc
                     WHERE c.MaHDN = '" + maHDN + "'"; 
            DataTable dtCT = dp.GetDataTable(sqlCT);

            if (dtCT.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn này chưa có chi tiết để in!");
                return;
            }

            // Khởi tạo Excel
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
            exSheet.Name = "HoaDonNhap_" + maHDN;

            // ==== In tiêu đề ====
            exSheet.Range["A1:E1"].Merge();
            exSheet.Range["A1"].Value = "NHÀ THUỐC ABC";
            exSheet.Range["A1"].Font.Bold = true;
            exSheet.Range["A1"].Font.Size = 16;
            exSheet.Range["A1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            exSheet.Range["A3:E3"].Merge();
            exSheet.Range["A3"].Value = "HÓA ĐƠN NHẬP HÀNG";
            exSheet.Range["A3"].Font.Bold = true;
            exSheet.Range["A3"].Font.Size = 14;
            exSheet.Range["A3"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            // ==== Thông tin chung ====
            DataRow r = dtHD.Rows[0];
            exSheet.Range["A5"].Value = "Mã hóa đơn: " + r["MaHDN"].ToString();
            exSheet.Range["A6"].Value = "Ngày nhập: " + Convert.ToDateTime(r["NgayNhap"]).ToString("dd/MM/yyyy");
            exSheet.Range["A7"].Value = "Nhân viên nhập: " + r["TenNV"].ToString();
            exSheet.Range["A8"].Value = "Nhà cung cấp: " + r["TenNCC"].ToString();
            exSheet.Range["A9"].Value = "Địa chỉ: " + r["DiaChi"].ToString();
            exSheet.Range["A10"].Value = "SĐT: " + r["SoDienThoai"].ToString();

            // ==== Bảng chi tiết ====
            exSheet.Range["A12"].Value = "STT";
            exSheet.Range["B12"].Value = "Mã lô";
            exSheet.Range["C12"].Value = "Tên thuốc";
            exSheet.Range["D12"].Value = "Số lượng";
            exSheet.Range["E12"].Value = "Đơn giá";
            exSheet.Range["F12"].Value = "Thành tiền";

            exSheet.Range["A12:F12"].Font.Bold = true;
            exSheet.Range["A12:F12"].Borders.LineStyle = 1;
            exSheet.Range["A12:F12"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            int row = 13;
            int stt = 1;
            foreach (DataRow dr in dtCT.Rows)
            {
                exSheet.Cells[row, 1] = stt++;
                exSheet.Cells[row, 2] = dr["MaLo"].ToString();
                exSheet.Cells[row, 3] = dr["TenThuoc"].ToString();
                exSheet.Cells[row, 4] = dr["SoLuongNhap"].ToString();
                exSheet.Cells[row, 5] = dr["GiaNhap"].ToString();
                exSheet.Cells[row, 6] = dr["ThanhTien"].ToString();
                row++;
            }

            // Định dạng bảng
            exSheet.Range["A12:F" + (row - 1)].Borders.LineStyle = 1;
            exSheet.Columns["A:F"].AutoFit();

            // ==== Tổng tiền ====
            exSheet.Range["E" + row].Value = "Tổng tiền:";
            exSheet.Range["E" + row].Font.Bold = true;
            exSheet.Range["F" + row].Value = r["TongTien"].ToString();
            exSheet.Range["F" + row].Font.Bold = true;

            // ==== Chữ ký ====
            row += 2;
            exSheet.Range["E" + row + ":F" + row].Merge();
            exSheet.Range["E" + row].Value = "Người lập hóa đơn";
            exSheet.Range["E" + row].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["E" + (row + 1)].Value = r["TenNV"].ToString();
            exSheet.Range["E" + (row + 1)].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            exApp.Visible = true;
        }

        private void txtDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co chac muon dong form hoa don nhap?",
                "Xac nhan", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
