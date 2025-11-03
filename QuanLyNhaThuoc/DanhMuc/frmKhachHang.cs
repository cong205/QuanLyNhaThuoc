using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.DanhMuc
{
    public partial class frmKhachHang : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();

        public frmKhachHang()
        {
            InitializeComponent();
            Classes.UiTheme.Apply(this);
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            // Apply theme to grid if present
            if (dgvKhachHang != null)
                Classes.UiTheme.ApplyGrid(dgvKhachHang);
        }

        void LoadData()
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tKhachHang");
            dgvKhachHang.DataSource = dt;

            dgvKhachHang.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
            dgvKhachHang.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvKhachHang.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
        }

        void ResetValue()
        {
            txtMaKhach.Text = "";
            txtTenKhach.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            rdoNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            txtMaKhach.Focus();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnLuu.Enabled = false;

                txtMaKhach.Text = dgvKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
                txtTenKhach.Text = dgvKhachHang.CurrentRow.Cells["TenKH"].Value.ToString();
                txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
                txtDienThoai.Text = dgvKhachHang.CurrentRow.Cells["SoDienThoai"].Value.ToString();

                string gt = dgvKhachHang.CurrentRow.Cells["GioiTinh"].Value.ToString();
                if (gt == "Nam") rdoNam.Checked = true;
                else rdoNu.Checked = true;

                if (dgvKhachHang.CurrentRow.Cells["NgaySinh"].Value != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(dgvKhachHang.CurrentRow.Cells["NgaySinh"].Value);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            ResetValue();
            // Sinh mã khách hàng tự động
            string sql = "SELECT TOP 1 MaKH FROM tKhachHang ORDER BY MaKH DESC";
            DataTable dt = dp.GetDataTable(sql);

            string newMaKH;
            if (dt.Rows.Count > 0)
            {
                string lastMa = dt.Rows[0]["MaKH"].ToString(); // Ví dụ "KH05"
                int num = int.Parse(lastMa.Substring(2)); // Lấy phần số -> 5
                num++;
                newMaKH = "KH" + num.ToString("D2"); // => "KH06"
            }
            else
            {
                newMaKH = "KH01"; // Nếu chưa có khách hàng nào
            }

            txtMaKhach.Text = newMaKH;
            txtMaKhach.Enabled = false; // Không cho sửa mã
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (txtMaKhach.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng!");
                txtMaKhach.Focus();
                return;
            }
            // Định dạng mã KH: KH + 2-4 chữ số
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtMaKhach.Text.Trim(), "^KH\\d{2,4}$"))
            {
                MessageBox.Show("Mã khách hàng không hợp lệ (định dạng KHxx)");
                txtMaKhach.Focus();
                return;
            }
            if (txtTenKhach.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                txtTenKhach.Focus();
                return;
            }
            // Kiểm tra ngày sinh không vượt quá hiện tại
            if (dtpNgaySinh.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!");
                dtpNgaySinh.Focus();
                return;
            }
            // Kiểm tra số điện thoại VN cơ bản (0 và 10-11 số)
            if (!string.IsNullOrWhiteSpace(txtDienThoai.Text) && !System.Text.RegularExpressions.Regex.IsMatch(txtDienThoai.Text.Trim(), "^0\\d{9,10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và 10-11 số)");
                txtDienThoai.Focus();
                return;
            }

            string gt = rdoNam.Checked ? "Nam" : "Nữ";
            string sqlCheck = "SELECT * FROM tKhachHang WHERE MaKH = '" + txtMaKhach.Text + "'";
            DataTable dtCheck = dp.GetDataTable(sqlCheck);

            if (dtCheck.Rows.Count > 0)
            {
                // Cập nhật
                string sqlUpdate = "UPDATE tKhachHang SET " +
                                   "TenKH = N'" + txtTenKhach.Text + "', " +
                                   "GioiTinh = N'" + gt + "', " +
                                   "NgaySinh = '" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "', " +
                                   "DiaChi = N'" + txtDiaChi.Text + "', " +
                                   "SoDienThoai = '" + txtDienThoai.Text + "' " +
                                   "WHERE MaKH = '" + txtMaKhach.Text + "'";
                dp.ExecuteNonQuery(sqlUpdate);
                MessageBox.Show("Đã cập nhật thông tin khách hàng!");
            }
            else
            {
                // Thêm mới
                string sqlInsert = "INSERT INTO tKhachHang VALUES('" + txtMaKhach.Text + "', N'" + txtTenKhach.Text +
                                   "', N'" + gt + "', '" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "', N'" +
                                   txtDiaChi.Text + "', '" + txtDienThoai.Text + "')";
                dp.ExecuteNonQuery(sqlInsert);
                MessageBox.Show("Đã thêm khách hàng mới!");
            }

            LoadData();
            ResetValue();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa khách hàng này không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sqlDel = "DELETE FROM tKhachHang WHERE MaKH = '" + txtMaKhach.Text + "'";
                dp.ExecuteNonQuery(sqlDel);
                MessageBox.Show("Đã xóa khách hàng!");
                LoadData();
                ResetValue();
            }
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ?", "Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
        private void txtMaKhach_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenKhach_TextChanged(object sender, EventArgs e)
        {
        }

        private void rdoNam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoNu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDienThoai_TextChanged(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void frmKhachHang_Click(object sender, EventArgs e)
        {
            ResetValue();
            LoadData();
        }
    }
}
