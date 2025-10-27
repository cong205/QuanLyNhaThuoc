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
    public partial class frmLoaiThuoc : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();

        public frmLoaiThuoc()
        {
            InitializeComponent();
        }

        private void frmLoaiThuoc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tLoaiThuoc");
            dgvLoaiThuoc.DataSource = dt;

            dgvLoaiThuoc.Columns["MaLoaiThuoc"].HeaderText = "Mã loại thuốc";
            dgvLoaiThuoc.Columns["TenLoaiThuoc"].HeaderText = "Tên loại thuốc";
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        void ResetValue()
        {
            txtMaLoai.Text = "";
            txtTenLoai.Text = "";
            txtMaLoai.Focus();
        }

        private void dgvLoaiThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnLuu.Enabled = false;

                txtMaLoai.Text = dgvLoaiThuoc.CurrentRow.Cells["MaLoaiThuoc"].Value.ToString();
                txtTenLoai.Text = dgvLoaiThuoc.CurrentRow.Cells["TenLoaiThuoc"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            ResetValue();

            // Sinh mã loại thuốc tự động
            string sql = "SELECT TOP 1 MaLoaiThuoc FROM tLoaiThuoc ORDER BY MaLoaiThuoc DESC";
            DataTable dt = dp.GetDataTable(sql);

            string newMa;
            if (dt.Rows.Count > 0)
            {
                string lastMa = dt.Rows[0]["MaLoaiThuoc"].ToString(); // Ví dụ "LT05"
                int num = int.Parse(lastMa.Substring(2)); // lấy phần số
                num++;
                newMa = "LT" + num.ToString("D2"); // LT06
            }
            else
            {
                newMa = "LT01";
            }

            txtMaLoai.Text = newMa;
            txtMaLoai.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTenLoai.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên loại thuốc!");
                txtTenLoai.Focus();
                return;
            }

            string sqlCheck = "SELECT * FROM tLoaiThuoc WHERE MaLoaiThuoc = '" + txtMaLoai.Text + "'";
            DataTable dtCheck = dp.GetDataTable(sqlCheck);

            if (dtCheck.Rows.Count > 0)
            {
                // Cập nhật
                string sqlUpdate = "UPDATE tLoaiThuoc SET TenLoaiThuoc = N'" + txtTenLoai.Text +
                                   "' WHERE MaLoaiThuoc = '" + txtMaLoai.Text + "'";
                dp.ExecuteNonQuery(sqlUpdate);
                MessageBox.Show("Đã cập nhật loại thuốc!");
            }
            else
            {
                // Thêm mới
                string sqlInsert = "INSERT INTO tLoaiThuoc VALUES('" + txtMaLoai.Text + "', N'" + txtTenLoai.Text + "')";
                dp.ExecuteNonQuery(sqlInsert);
                MessageBox.Show("Đã thêm loại thuốc mới!");
            }

            LoadData();
            ResetValue();
            btnThem.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa loại thuốc này không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sqlDel = "DELETE FROM tLoaiThuoc WHERE MaLoaiThuoc = '" + txtMaLoai.Text + "'";
                dp.ExecuteNonQuery(sqlDel);
                MessageBox.Show("Đã xóa loại thuốc!");
                LoadData();
                ResetValue();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ?", "Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void txtMaLoai_TextChanged(object sender, EventArgs e) { }
        private void txtTenLoai_TextChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvLoaiThuoc_CellClick(sender, e);
        }

        private void frmLoaiThuoc_Click(object sender, EventArgs e)
        {
            ResetValue();
            LoadData();
            btnThem.Enabled = true;
        }
    }
}
