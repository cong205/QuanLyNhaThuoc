using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class frmNhanVien : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        public frmNhanVien()
        {
            InitializeComponent();
            Classes.UiTheme.Apply(this);
        }
        private void fillChucVu()
        {
            cboChucVu.Items.Add("Quản lý");
            cboChucVu.Items.Add("Nhân viên");
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            fillChucVu();
            DataTable dt = dp.GetDataTable("select * from tNhanVien");
            dgvNV.DataSource = dt;
            Classes.UiTheme.ApplyGrid(dgvNV);

            dgvNV.Columns[0].HeaderText = "Mã nhân viên";
            dgvNV.Columns[1].HeaderText = "Tên nhân viên";
            dgvNV.Columns[2].HeaderText = "Giới tính";
            dgvNV.Columns[3].HeaderText = "Ngày sinh";
            dgvNV.Columns[4].HeaderText = "Địa chỉ";
            dgvNV.Columns[5].HeaderText = "Số điện thoại";
            dgvNV.Columns[6].HeaderText = "Chức vụ";
            dgvNV.Columns[7].HeaderText = "Mật khẩu";

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Bạn có muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        void ResetValue()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            cboChucVu.SelectedIndex = -1;
            rdbNam.Checked = false;
            rdbNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoqua.Enabled = true;
            //ResetValue();
            ResetValue();
            string sql = "SELECT TOP 1 MaNV FROM tNhanVien ORDER BY MaNV DESC";
            DataTable dt = dp.GetDataTable(sql);

            string newMa;
            if (dt.Rows.Count > 0)
            {
                string lastMa = dt.Rows[0]["MaNV"].ToString(); // Ví dụ "NV05"
                int num = int.Parse(lastMa.Substring(2)); // lấy phần số
                num++;
                newMa = "NV" + num.ToString("D2"); // NV06
            }
            else
            {
                newMa = "NV01";
            }

            txtMaNV.Text = newMa;
            txtMaNV.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string tenNV = txtTenNV.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string chucVu = cboChucVu.Text.Trim();
            string matKhau = txtPW.Text.Trim();

            bool rdbNamChecked = rdbNam.Checked;
            bool rdbNuChecked = rdbNu.Checked;

            if (string.IsNullOrWhiteSpace(maNV))
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên");
                txtMaNV.Focus(); // Vẫn phải Focus vào control gốc
                return;
            }

            if (string.IsNullOrWhiteSpace(tenNV))
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên");
                txtTenNV.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Bạn phải nhập số điện thoại");
                txtSDT.Focus();
                return;
            }
            // Validate SDT: 0 và 10-11 số
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, "^0\\d{9,10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và 10-11 số)");
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(diaChi))
            {
                MessageBox.Show("Bạn phải nhập địa chỉ");
                txtDiaChi.Focus();
                return;
            }

            if (rdbNamChecked == false && rdbNuChecked == false)
            {
                MessageBox.Show("Bạn phải chọn giới tính");
                return;
            }

            if (dtpNgaySinh.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(chucVu))
            {
                MessageBox.Show("Bạn phải chọn chức vụ");
                cboChucVu.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Bạn phải nhập mật khẩu");
                txtPW.Focus();
                return;
            }
            else if (matKhau.Length < 4)
            {
                MessageBox.Show("Mật khẩu tối thiểu 4 ký tự");
                txtPW.Focus();
                return;
            }
            string gioiTinh = rdbNamChecked ? "Nam" : "Nữ";
            string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");



            // Neu dang them moi
            if (btnThem.Enabled == true)
            {
                DataTable dt = dp.GetDataTable("select * from tNhanVien where MaNV='" + maNV + "'");
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("ma nhan vien da ton tai");
                    txtMaNV.Focus();
                    return;
                }

                string sqlInsert = "INSERT INTO tNhanVien VALUES ('" + maNV + "', N'" + tenNV + "', N'" + gioiTinh +
                    "', '" + ngaySinh + "', N'" + diaChi + "', '" + sdt + "', N'" + chucVu + "','"+matKhau+ "')";
                dp.ExecuteNonQuery(sqlInsert);
            }

            // Neu dang sua
            if (btnSua.Enabled == true)
            {
                string sqlUpdate = "UPDATE tNhanVien SET TenNV = N'" + tenNV +
                    "', GioiTinh = N'" + gioiTinh +
                    "', NgaySinh = '" + ngaySinh +
                    "', DiaChi = N'" + diaChi +
                    "', SoDienThoai = '" + sdt +
                    "', ChucVu = N'" + chucVu +
                    "', Password = '" + matKhau +
                    "' WHERE MaNV = '" + maNV + "'";
                dp.ExecuteNonQuery(sqlUpdate);
            }

            DataTable dtnv = dp.GetDataTable("select * from tNhanVien");
            dgvNV.DataSource = dtnv;

            // Cam sua, xoa, luu, bo qua; chi duoc click them moi
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;

            // Xoa trang du lieu
            ResetValue();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoqua.Enabled = true;
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa nhân viên này không?",
                "Xóa nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maNV = txtMaNV.Text.Trim();
                dp.ExecuteNonQuery("DELETE FROM tNhanVien WHERE MaNV = '" + maNV + "'");
                dgvNV.DataSource = dp.GetDataTable("SELECT * FROM tNhanVien");
            }

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            ResetValue();
        }


        private void btnBoqua_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            ResetValue();
        }

        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnBoqua.Enabled = true;
            btnThem.Enabled = false;
            btnLuu.Enabled = false;

            txtMaNV.Text = dgvNV.CurrentRow.Cells["MaNV"].Value.ToString();
            txtTenNV.Text = dgvNV.CurrentRow.Cells["TenNV"].Value.ToString();
            string gioiTinh = dgvNV.CurrentRow.Cells["GioiTinh"].Value.ToString();
            rdbNam.Checked = gioiTinh == "Nam";
            rdbNu.Checked = gioiTinh == "Nữ";
            dtpNgaySinh.Value = Convert.ToDateTime(dgvNV.CurrentRow.Cells["NgaySinh"].Value);
            txtDiaChi.Text = dgvNV.CurrentRow.Cells["DiaChi"].Value.ToString();
            txtSDT.Text = dgvNV.CurrentRow.Cells["SoDienThoai"].Value.ToString();
            cboChucVu.Text = dgvNV.CurrentRow.Cells["ChucVu"].Value.ToString();
            txtPW.Text = dgvNV.CurrentRow.Cells["Password"].Value.ToString();
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (!char.IsControl(e.KeyChar) && txtSDT.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
