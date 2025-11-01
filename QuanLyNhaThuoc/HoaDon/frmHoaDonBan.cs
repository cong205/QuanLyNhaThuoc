using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyNhaThuoc.HoaDon
{
    public partial class frmHoaDonBan : Form
    {
        Classes.DataProcesser dtp = new Classes.DataProcesser();

        bool isLoading = false;

        public frmHoaDonBan()
        {
            InitializeComponent();
        }

        private void frmHoaDonBan_Load(object sender, EventArgs e)
        {
            isLoading = true; // Bắt đầu load dữ liệu
            // load dữ liệu lên combobox Mã hóa đơn
            string strSQL = "SELECT MaHDB FROM tHoaDonBan";
            DataTable dt = dtp.GetDataTable(strSQL);
            cboTKMaHD.DisplayMember = "MaHDB";
            cboTKMaHD.DataSource = dt;
            cboTKMaHD.SelectedIndex = -1;
            cboTKMaHD.IntegralHeight = false;
            cboTKMaHD.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTKMaHD.MaxDropDownItems = 5;

            // load dữ liệu lên Ma hàng
            strSQL = "SELECT MaThuoc FROM tThuoc";
            DataTable dtMT = dtp.GetDataTable(strSQL);
            cboMaT.DataSource = dtMT;
            cboMaT.DisplayMember = "MaThuoc";
            cboMaT.SelectedIndex = -1;
            cboMaT.IntegralHeight = false;
            cboMaT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaT.MaxDropDownItems = 5;
            isLoading = false; // Kết thúc load dữ liệu

            dgvCTHDB.ReadOnly = true;

            // load dữ liệu lên combobox Mã nhân viên
            strSQL = "SELECT MaNV FROM tNhanVien";
            DataTable dtNV = dtp.GetDataTable(strSQL);
            cboMaNV.DisplayMember = "MaNV";
            cboMaNV.DataSource = dtNV;
            cboMaNV.SelectedIndex = -1;
            cboMaNV.IntegralHeight = false;
            cboMaNV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaNV.MaxDropDownItems = 5;

            btnHuyHD.Enabled = false;
            btnLuu.Enabled = false;

            txtTenNV.ReadOnly = true;
            txtTenT.ReadOnly= true;
            txtDonGia.ReadOnly= true;

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboTKMaHD.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn mã hóa đơn cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maHDB = cboTKMaHD.Text;

            // ====== Lấy thông tin chung ======
            string sqlThongTin = $@"
                SELECT hdb.MaHDB, hdb.NgayBan, nv.MaNV, nv.TenNV, kh.MaKH, kh.TenKH, kh.DiaChi, kh.SoDienThoai, hdb.TongTien
                FROM tHoaDonBan hdb
                JOIN tNhanVien nv ON hdb.MaNV = nv.MaNV
                JOIN tKhachHang kh ON hdb.MaKH = kh.MaKH
                WHERE hdb.MaHDB = '{maHDB}'";

            DataTable dtThongTin = dtp.GetDataTable(sqlThongTin);
            if (dtThongTin.Rows.Count > 0)
            {
                DataRow r = dtThongTin.Rows[0];
                txtMaHD.Text = r["MaHDB"].ToString();
                dtpNgayBan.Value = Convert.ToDateTime(r["NgayBan"]);
                cboMaNV.Text = r["MaNV"].ToString();
                txtTenNV.Text = r["TenNV"].ToString();
                cboMaKH.Text = r["MaKH"].ToString();
                txtTenKH.Text = r["TenKH"].ToString();
                txtDiaChi.Text = r["DiaChi"].ToString();
                txtDienThoai.Text = r["SoDienThoai"].ToString();
                txtTongTien.Text = r["TongTien"].ToString();
            }

            // ====== Lấy chi tiết hóa đơn ======
            string sqlChiTiet = $@"
                SELECT t.MaThuoc, t.TenThuoc, cthdb.SoLuongBan, t.GiaBan, cthdb.GiamGia, cthdb.ThanhTien
                FROM tChiTietHDB cthdb
                JOIN tLoThuoc lt ON cthdb.MaLo = lt.MaLo
                JOIN tThuoc t ON lt.MaThuoc = t.MaThuoc
                WHERE cthdb.MaHDB = '{maHDB}'";

            DataTable dtChiTiet = dtp.GetDataTable(sqlChiTiet);
            dgvCTHDB.DataSource = dtChiTiet;

            // Tùy chọn: chỉnh lại tiêu đề cột
            dgvCTHDB.Columns["MaThuoc"].HeaderText = "Mã thuốc";
            dgvCTHDB.Columns["TenThuoc"].HeaderText = "Tên thuốc";
            dgvCTHDB.Columns["SoLuongBan"].HeaderText = "Số lượng";
            dgvCTHDB.Columns["GiaBan"].HeaderText = "Đơn giá";
            dgvCTHDB.Columns["GiamGia"].HeaderText = "Giảm giá";
            dgvCTHDB.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }

        private void cboMaT_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu chưa chọn hoặc combo rỗng thì bỏ qua
            if (isLoading || cboMaT.SelectedIndex == -1)
                return; // Nếu đang load dữ liệu hoặc chưa chọn thì bỏ qua

            string maThuoc = cboMaT.Text; // hoặc cboMaThuoc.SelectedValue nếu bạn dùng DataSource

            // Câu SQL lấy thông tin thuốc
            string sql = $@"
                    SELECT TenThuoc, GiaBan 
                    FROM tThuoc 
                    WHERE MaThuoc = '{maThuoc}'";

            DataTable dt = dtp.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                // Gán giá trị cho các textbox
                txtTenT.Text = dt.Rows[0]["TenThuoc"].ToString();
                txtDonGia.Text = dt.Rows[0]["GiaBan"].ToString();

                // Gán mặc định
                txtSoLuong.Text = "1";
                txtGiamGia.Text = "0";

                // Tính thành tiền = Số lượng * Đơn giá - Giảm giá
                TinhThanhTien();
            }


        }
        private void TinhThanhTien()
        {
            double soLuong = 0, donGia = 0, giamGia = 0;
            double.TryParse(txtSoLuong.Text, out soLuong);
            double.TryParse(txtDonGia.Text, out donGia);
            double.TryParse(txtGiamGia.Text, out giamGia);

            double thanhTien = (1 - giamGia) * (soLuong * donGia);
            txtThanhTien.Text = thanhTien.ToString("0.##");
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
                return;

            if (int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                if (soLuong <= 0 || soLuong >= 1001)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0 và nhỏ hơn 1000!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Text = "1";
                }
            }
            else
            {
                txtSoLuong.Text = "1";
            }

            TinhThanhTien();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGiamGia.Text))
                return;

            if (double.TryParse(txtGiamGia.Text, out double giamGia))
            {
                if (giamGia < 0 || giamGia > 1)
                {
                    MessageBox.Show("Giảm giá phải nằm trong khoảng 0 đến 1!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiamGia.Text = "0";
                }
            }
            else
            {
                txtGiamGia.Text = "0";
            }

            TinhThanhTien();
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho nhập số và phím điều khiển (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtGiamGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho nhập số, dấu chấm (.), và phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Chỉ cho phép một dấu chấm
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        public void ResetForm()
        {
            cboTKMaHD.SelectedIndex = -1;
            txtMaHD.Clear();
            dtpNgayBan.Value = DateTime.Now;
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Clear();
            cboMaKH.SelectedIndex = -1;
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            txtTongTien.Clear();
            dgvCTHDB.DataSource = null;
            cboMaT.SelectedIndex = -1;
            txtTenT.Clear();
            txtDonGia.Clear();
            txtSoLuong.Clear();
            txtGiamGia.Clear();
            txtThanhTien.Clear();

        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            btnHuyHD.Enabled = true;
            // 1️⃣ Reset lại form
            ResetForm();

            // 2️⃣ Tự sinh mã hóa đơn: HĐB_ddMMyyyy0xxx
            string ngay = DateTime.Now.ToString("ddMMyyyy");
            string maHD;

            do
            {
                Random rand = new Random();
                int soRandom = rand.Next(1, 10000); // 1 -> 9999
                maHD = $"HĐB{ngay}{soRandom:0000}";

                // 3️⃣ Kiểm tra trùng mã hóa đơn trong DB
                string sql = $"SELECT COUNT(*) AS SoLuong FROM tHoaDonBan WHERE MaHDB = '{maHD}'";
                DataTable dt = dtp.GetDataTable(sql);
                int count = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
                if (count == 0)
                {
                    // Không trùng
                    break;
                }
            }
            while (true);

            // 4️⃣ Gán mã vào textbox
            txtMaHD.Text = maHD;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text) ||
                cboMaKH.Text.Length == 0 ||
                cboMaNV.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hóa đơn và mặt hàng!", "Thông báo");
                return;
            }

            if (cboMaT.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn mã thuốc!", "Thông báo");
                cboMaT.Focus();
                return;
            }

            string maHDB = txtMaHD.Text;
            string ngayBan = dtpNgayBan.Value.ToString("yyyy-MM-dd");
            string maKH = cboMaKH.Text;
            string maNV = cboMaNV.Text;
            decimal tongTien =0;

            string maT = cboMaT.Text;
            string TenT = txtTenT.Text;
            int soLuong = Convert.ToInt32(txtSoLuong.Text);
            decimal donGia = Convert.ToDecimal(txtDonGia.Text);
            decimal giamGia = Convert.ToDecimal(txtGiamGia.Text);
            decimal thanhTien = Convert.ToDecimal(txtThanhTien.Text);

            // Kiểm tra tồn kho
            string sqlCheck = $"SELECT MaLo, SoLuongTon FROM tLoThuoc WHERE MaThuoc = '{maT}'";
            DataTable dtCheck = dtp.GetDataTable(sqlCheck);
            int soLuongTon = Convert.ToInt32(dtCheck.Rows[0]["SoLuongTon"]);
            if (soLuong > soLuongTon)
            {
                MessageBox.Show("Số lượng bán vượt quá tồn kho!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            // kiem tra ma hoa don da ton tai chua
            string sqlCheckHD = $"SELECT COUNT(*) AS SoLuong FROM tHoaDonBan WHERE MaHDB = '{maHDB}'";
            DataTable dtCheckHD = dtp.GetDataTable(sqlCheckHD);
            int countHD = Convert.ToInt32(dtCheckHD.Rows[0]["SoLuong"]);
            if (countHD == 0)
            {
                string sqlHD = $@"
                    INSERT INTO tHoaDonBan(MaHDB, NgayBan, MaKH, MaNV, TongTien)
                    VALUES('{maHDB}', '{ngayBan}', '{maKH}', '{maNV}', {tongTien})";
                dtp.ExecuteNonQuery(sqlHD);
            }

            string maLo = dtCheck.Rows[0]["MaLo"].ToString();
            // Lưu chi tiết
            string sqlCT = $@"
                   INSERT INTO tChiTietHDB(MaHDB, MaLo, SoLuongBan, GiamGia, ThanhTien)
                   VALUES('{maHDB}', '{maLo}', {soLuong}, {giamGia}, {thanhTien})";
            dtp.ExecuteNonQuery(sqlCT);

            // Cập nhật số lượng kho
            string sqlUpdate = $@"
                        UPDATE tLoThuoc
                        SET SoLuongTon = SoLuongTon - {soLuong}
                        WHERE MaThuoc = '{maT}'";
            dtp.ExecuteNonQuery(sqlUpdate);

                // ====== Lấy chi tiết hóa đơn ======
                string sqlChiTiet = $@"
                SELECT t.MaThuoc, t.TenThuoc, cthdb.SoLuongBan, t.GiaBan, cthdb.GiamGia, cthdb.ThanhTien
                FROM tChiTietHDB cthdb
                JOIN tLoThuoc lt ON cthdb.MaLo = lt.MaLo
                JOIN tThuoc t ON lt.MaThuoc = t.MaThuoc
                WHERE cthdb.MaHDB = '{maHDB}'";

                DataTable dtChiTiet = dtp.GetDataTable(sqlChiTiet);
                dgvCTHDB.DataSource = dtChiTiet;

                // Tùy chọn: chỉnh lại tiêu đề cột
                dgvCTHDB.Columns["MaThuoc"].HeaderText = "Mã thuốc";
                dgvCTHDB.Columns["TenThuoc"].HeaderText = "Tên thuốc";
                dgvCTHDB.Columns["SoLuongBan"].HeaderText = "Số lượng";
                dgvCTHDB.Columns["GiaBan"].HeaderText = "Đơn giá";
                dgvCTHDB.Columns["GiamGia"].HeaderText = "Giảm giá";
                dgvCTHDB.Columns["ThanhTien"].HeaderText = "Thành tiền";

                MessageBox.Show("Lưu hóa đơn và trừ kho thành công!", "Thông báo");
            foreach (DataRow row in dtChiTiet.Rows)
            {
                tongTien += Convert.ToDecimal(row["ThanhTien"]);
            }
            txtTongTien.Text = tongTien.ToString("N0");

            // Cập nhật tổng tiền vào hóa đơn
            string sqlUpdateTongTien = $@"
                        UPDATE tHoaDonBan
                        SET TongTien = {tongTien}
                        WHERE MaHDB = '{maHDB}'";
            dtp.ExecuteNonQuery(sqlUpdateTongTien);


        }

        private void dgvCTHDB_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng double click vào header hoặc ngoài vùng dữ liệu
            if (e.RowIndex < 0 || e.RowIndex >= dgvCTHDB.Rows.Count)
                return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này khỏi hóa đơn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return; // Người dùng chọn No, không xóa
            }


            // Lấy mã thuốc từ dòng được chọn
            string maThuoc = dgvCTHDB.Rows[e.RowIndex].Cells["MaThuoc"].Value.ToString();
            string maHDB = txtMaHD.Text;

            // Xóa chi tiết hóa đơn khỏi cơ sở dữ liệu
            string sqlDeleteCT = $@"
                DELETE cthdb
                FROM tChiTietHDB cthdb
                JOIN tLoThuoc lt ON cthdb.MaLo = lt.MaLo
                WHERE cthdb.MaHDB = '{maHDB}' AND lt.MaThuoc = '{maThuoc}'";
            dtp.ExecuteNonQuery(sqlDeleteCT);

            // Cập nhật lại số lượng tồn kho (giả sử bạn đã lưu số lượng bán trước đó)
            int soLuongBan = Convert.ToInt32(dgvCTHDB.Rows[e.RowIndex].Cells["SoLuongBan"].Value);
            string sqlUpdateKho = $@"
                UPDATE tLoThuoc
                SET SoLuongTon = SoLuongTon + {soLuongBan}
                WHERE MaThuoc = '{maThuoc}'";
            dtp.ExecuteNonQuery(sqlUpdateKho);

            // Cập nhật lại DataGridView
            dgvCTHDB.Rows.RemoveAt(e.RowIndex);
            MessageBox.Show("Đã xóa mặt hàng khỏi hóa đơn và cập nhật lại kho!", "Thông báo");

            // Cập nhật lại tổng tiền
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvCTHDB.Rows)
            {
                tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
            }
            txtTongTien.Text = tongTien.ToString("N0");
            // Cập nhật tổng tiền vào hóa đơn
            string sqlUpdateTongTien = $@"
                        UPDATE tHoaDonBan
                        SET TongTien = {tongTien}
                        WHERE MaHDB = '{maHDB}'";
            dtp.ExecuteNonQuery(sqlUpdateTongTien);

        }

        private void btnHuyHD_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn hủy hóa đơn này?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return; // Người dùng chọn No, không hủy
            }

            string maHDB = txtMaHD.Text;

            // Lấy chi tiết hóa đơn để cập nhật lại kho
            string sqlChiTiet = $@"
                SELECT t.MaThuoc, cthdb.SoLuongBan
                FROM tChiTietHDB cthdb
                JOIN tLoThuoc lt ON cthdb.MaLo = lt.MaLo
                JOIN tThuoc t ON lt.MaThuoc = t.MaThuoc
                WHERE cthdb.MaHDB = '{maHDB}'";
            DataTable dtChiTiet = dtp.GetDataTable(sqlChiTiet);

            // Cập nhật lại kho
            foreach (DataRow row in dtChiTiet.Rows)
            {
                string maThuoc = row["MaThuoc"].ToString();
                int soLuongBan = Convert.ToInt32(row["SoLuongBan"]);
                string sqlUpdateKho = $@"
                    UPDATE tLoThuoc
                    SET SoLuongTon = SoLuongTon + {soLuongBan}
                    WHERE MaThuoc = '{maThuoc}'";
                dtp.ExecuteNonQuery(sqlUpdateKho);
            }

            // Xóa chi tiết hóa đơn
            string sqlDeleteCT = $@"
                DELETE FROM tChiTietHDB
                WHERE MaHDB = '{maHDB}'";
            dtp.ExecuteNonQuery(sqlDeleteCT);

            // Xóa hóa đơn
            string sqlDeleteHD = $@"
                DELETE FROM tHoaDonBan
                WHERE MaHDB = '{maHDB}'";
            dtp.ExecuteNonQuery(sqlDeleteHD);

            // Reset form
            ResetForm();
            MessageBox.Show("Đã hủy hóa đơn và cập nhật lại kho!", "Thông báo");

            btnLuu.Enabled = false;
            btnHuyHD.Enabled = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDienThoai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtDienThoai.Text.Length < 10 || txtDienThoai.Text.Length > 11)
                {
                    MessageBox.Show("Số điện thoại phải từ 10 đến 11 chữ số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDienThoai.Focus();
                    return;
                }
                DataTable dt = dtp.GetDataTable($"SELECT * FROM tKhachHang WHERE SoDienThoai = '{txtDienThoai.Text}'");
                if (dt.Rows.Count == 0)
                {
                    if (cboMaKH.Text.Length == 0)
                    {
                        MessageBox.Show("Vui lòng nhập mã khách hàng!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cboMaKH.Focus();
                        return;
                    }
                    if (txtTenKH.Text.Length == 0)
                    {
                        MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTenKH.Focus();
                        return;
                    }
                    if (txtDiaChi.Text.Length == 0)
                    {
                        MessageBox.Show("Vui lòng nhập địa chỉ khách hàng!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtDiaChi.Focus();
                        return;
                    }
                    DataTable dtCheck = dtp.GetDataTable("SELECT MaKH FROM tKhachHang WHERE MaKH = '" + cboMaKH.Text + "'");
                    if (dtCheck.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã khách hàng đã tồn tại, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cboMaKH.Focus();
                        return;
                    }
                    string sqlInsert = "INSERT INTO tKhachHang (MaKH, TenKH, GioiTinh, NgaySinh, DiaChi, SoDienThoai) " +
                        "VALUES ('" + cboMaKH.Text + "', N'" + txtTenKH.Text + "', null, null, N'" + txtDiaChi.Text + "', '" + txtDienThoai.Text + "')";
                    dtp.ExecuteNonQuery(sqlInsert);
                    MessageBox.Show("Đã thêm khách hàng mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cboMaKH.Text = dt.Rows[0]["MaKH"].ToString();
                    txtTenKH.Text = dt.Rows[0]["TenKH"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["DiaChi"].ToString();
                    txtDienThoai.Text = dt.Rows[0]["SoDienThoai"].ToString();
                }

            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đóng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(cboMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(cboMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập điện thoại khách hàng để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook hdbBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet hdbsheet = (Excel.Worksheet)hdbBook.Worksheets[1];
            Excel.Range hdbRange = (Excel.Range)hdbsheet.Cells[1, 1];

            // Tiêu đề
            hdbRange.Range["A1", "F1"].Merge();
            hdbRange.Range["A1", "F1"].Font.Size = 16;
            hdbRange.Range["A1", "F1"].Font.Bold = true;
            hdbRange.Range["A1", "F1"].Font.Color = System.Drawing.Color.Red;
            hdbRange.Range["A1", "F1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            hdbRange.Range["A1", "F1"].Value = "HÓA ĐƠN BÁN THUỐC";

            // Thông tin chung
            hdbRange.Range["A3"].Value = "Mã hóa đơn: "+ txtMaHD.Text;
            hdbRange.Range["A4"].Value = "Ngày bán: " + dtpNgayBan.Value.ToString("dd/MM/yyyy");
            hdbRange.Range["A5"].Value = "Nhân viên: " + cboMaNV.Text + " - " + txtTenNV.Text;
            hdbRange.Range["A6"].Value = "Khách hàng: " + cboMaKH.Text + " - " + txtTenKH.Text;
            hdbRange.Range["A7"].Value = "Địa chỉ: " + txtDiaChi.Text; ;
            hdbRange.Range["A8"].Value = "Điện thoại: " + txtDienThoai.Text;

            // In dong tieu de
            hdbsheet.Range["A10:G10"].Font.Size = 12;
            hdbsheet.Range["A10:G10"].Font.Bold = true;
            hdbsheet.Range["A10:G10"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            hdbsheet.Range["A10"].ColumnWidth = 5;   // STT
            hdbsheet.Range["A10"].Value = "STT";
            hdbsheet.Range["B10"].ColumnWidth = 15;  // Mã thuốc
            hdbsheet.Range["B10"].Value = "Mã thuốc";
            hdbsheet.Range["C10"].ColumnWidth = 30;  // Tên thuốc
            hdbsheet.Range["C10"].Value = "Tên thuốc";
            hdbsheet.Range["D10"].ColumnWidth = 10;  // Số lượng
            hdbsheet.Range["D10"].Value = "Số lượng";
            hdbsheet.Range["E10"].ColumnWidth = 15;  // Đơn giá
            hdbsheet.Range["E10"].Value = "Đơn giá";
            hdbsheet.Range["F10"].ColumnWidth = 10;  // Giảm giá
            hdbsheet.Range["F10"].Value = "Giảm giá";
            hdbsheet.Range["G10"].ColumnWidth = 15;  // Thành tiền
            hdbsheet.Range["G10"].Value = "Thành tiền";

            // In chi tiết
            int currentRow = 11;
            for (int i = 0; i < dgvCTHDB.Rows.Count - 1; i++)
            {
                hdbsheet.Range["A" + currentRow].Value = (i + 1).ToString();
                hdbsheet.Range["B" + currentRow].Value = dgvCTHDB.Rows[i].Cells["MaThuoc"].Value.ToString();
                hdbsheet.Range["C" + currentRow].Value = dgvCTHDB.Rows[i].Cells["TenThuoc"].Value.ToString();
                hdbsheet.Range["D" + currentRow].Value = dgvCTHDB.Rows[i].Cells["SoLuongBan"].Value.ToString();
                hdbsheet.Range["E" + currentRow].Value = Convert.ToDecimal(dgvCTHDB.Rows[i].Cells["GiaBan"].Value).ToString("N0");
                hdbsheet.Range["F" + currentRow].Value = Convert.ToDecimal(dgvCTHDB.Rows[i].Cells["GiamGia"].Value).ToString("P0");
                hdbsheet.Range["G" + currentRow].Value = Convert.ToDecimal(dgvCTHDB.Rows[i].Cells["ThanhTien"].Value).ToString("N0");
                currentRow++;
            }

            // Tổng tiền
            hdbsheet.Range["G" + (currentRow + 1)].Font.Size = 12;
            hdbsheet.Range["G" + (currentRow + 1)].Font.Bold = true;
            hdbsheet.Range["G" + (currentRow + 1)].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            hdbsheet.Range["G" + (currentRow + 1)].ColumnWidth = 15;
            hdbsheet.Range["G" + (currentRow + 1)].Font.Color = System.Drawing.Color.Red;
            hdbsheet.Range["G" + (currentRow + 1)].Value = "Tổng tiền: " + Convert.ToDecimal(txtTongTien.Text).ToString("N0");

            hdbsheet.Name = txtMaHD.Text;
            hdbBook.Activate();

            // luu file
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Workbook|*.xlsx|All Files|*.*";

            if (save.ShowDialog() == DialogResult.OK)
            {
                hdbBook.SaveAs(save.FileName.ToString());
                MessageBox.Show("In hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            excelApp.Quit();


        }
    }
    
}
