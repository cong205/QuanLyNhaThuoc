using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.Classes;

namespace QuanLyNhaThuoc.DanhMuc
{
    public partial class frmNhaCungCap : Form
    {
        DataProcesser dp = new DataProcesser();
        string selectedMaNCC = "";
        public frmNhaCungCap()
        {
            InitializeComponent();
        }
        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvNCC.ClearSelection();
        }
        private void LoadData()
        {
            string sql = "SELECT MaNCC, TenNCC, DiaChi, SoDienThoai FROM tNhaCungCap";
            dgvNCC.DataSource = dp.GetDataTable(sql);
            
            // Thiết lập chiều rộng các cột để loại bỏ khoảng trắng
            if (dgvNCC.Columns.Count >= 4)
            {
                // Ẩn row headers để có thêm không gian
                dgvNCC.RowHeadersVisible = false;
                
                // Thiết lập FillWeight cho từng cột (tỷ lệ phân chia không gian)
                dgvNCC.Columns["MaNCC"].FillWeight = 15;      // 15% cho Mã
                dgvNCC.Columns["TenNCC"].FillWeight = 35;       // 35% cho Tên  
                dgvNCC.Columns["DiaChi"].FillWeight = 30;       // 30% cho Địa chỉ
                dgvNCC.Columns["SoDienThoai"].FillWeight = 20;  // 20% cho SĐT
                
                // Đặt tất cả các cột ở chế độ Fill để tự động chia đều không gian
                foreach (DataGridViewColumn column in dgvNCC.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if(txtMaNCC.Text == "" || txtTenNCC.Text == "")
            {
                MessageBox.Show("Mã và Tên nhà cung cấp không được bỏ trống!"); return;
            }
            // Validate số điện thoại (bắt đầu bằng 0 và 10-11 số)
            string phone = txtSoDT.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, "^0\\d{9,10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và 10-11 số)");
                txtSoDT.Focus();
                return;
            }
            string sql = $"INSERT INTO tNhaCungCap (MaNCC, TenNCC, DiaChi, SoDienThoai) VALUES (N'{txtMaNCC.Text}', N'{txtTenNCC.Text}', N'{txtDiaChi.Text}', N'{txtSoDT.Text}')";
            try { dp.ExecuteNonQuery(sql); LoadData(); btnLamMoi_Click(null, null); }
            catch(Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if(selectedMaNCC == "") { MessageBox.Show("Chọn nhà cung cấp để sửa!"); return; }
            // Validate số điện thoại
            string phone = txtSoDT.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, "^0\\d{9,10}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và 10-11 số)");
                txtSoDT.Focus();
                return;
            }
            string sql = $"UPDATE tNhaCungCap SET TenNCC=N'{txtTenNCC.Text}', DiaChi=N'{txtDiaChi.Text}', SoDienThoai=N'{txtSoDT.Text}' WHERE MaNCC=N'{selectedMaNCC}'";
            try { dp.ExecuteNonQuery(sql); LoadData(); btnLamMoi_Click(null, null); }
            catch(Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(selectedMaNCC=="") { MessageBox.Show("Chọn nhà cung cấp để xóa!"); return; }
            if(MessageBox.Show("Bạn chắc chắn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                string sql = $"DELETE FROM tNhaCungCap WHERE MaNCC=N'{selectedMaNCC}'";
                try { dp.ExecuteNonQuery(sql); LoadData(); btnLamMoi_Click(null, null); }
                catch(Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNCC.Text = txtTenNCC.Text = txtDiaChi.Text = txtSoDT.Text = "";
            selectedMaNCC = "";
            dgvNCC.ClearSelection();
            txtMaNCC.Enabled = true;
        }
        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                DataGridViewRow row = dgvNCC.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtSoDT.Text = row.Cells["SoDienThoai"].Value?.ToString();
                selectedMaNCC = txtMaNCC.Text;
                txtMaNCC.Enabled = false;
            }
        }
    }
}
