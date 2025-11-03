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
    public partial class frmLoThuoc : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        Classes.Function ft = new Classes.Function();
        public frmLoThuoc()
        {
            InitializeComponent();
            Classes.UiTheme.Apply(this);
        }

        private void frmLoThuoc_Load(object sender, EventArgs e)
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tThuoc");
            ft.FillComBox(cboMaThuoc, dt, "MaThuoc", "MaThuoc");
            cboMaThuoc.SelectedIndex = -1;
            DataTable dtLoThuoc = dp.GetDataTable("SELECT * FROM tLoThuoc");
            dgvLoThuoc.DataSource = dtLoThuoc;
            Classes.UiTheme.ApplyGrid(dgvLoThuoc);

            dgvLoThuoc.Columns["MaLo"].HeaderText = "Mã Lô Thuốc";
            dgvLoThuoc.Columns["MaThuoc"].HeaderText = "Mã Thuốc";
            dgvLoThuoc.Columns["NgaySanXuat"].HeaderText = "Ngày Sản Xuất";
            dgvLoThuoc.Columns["HanSuDung"].HeaderText = "Hạn Sử Dụng";
            dgvLoThuoc.Columns["SoLuongTon"].HeaderText = "Số Lượng tồn";
        }
        void ResetValue()
        {
            txtMaLo.Clear();
            cboMaThuoc.SelectedIndex = -1;
            dtpNSX.Value = DateTime.Now;
            dtpHSD.Value = DateTime.Now;
            txtSLT.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            ResetValue();
            btnLuu.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;

            string sql = "SELECT TOP 1 MaLo FROM tLoThuoc ORDER BY MaLo DESC";
            DataTable dt = dp.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                string str = dt.Rows[0]["MaLo"].ToString();
                int so = Convert.ToInt32(str.Substring(1)) + 1;

                txtMaLo.Text = "L" + so.ToString("D3");
            }
            else
            {
                txtMaLo.Text = "L001";
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(txtMaLo.Text=="")
            {
                MessageBox.Show("Bạn phải nhập mã lô thuốc","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtMaLo.Focus();
                return;
            }
            if(cboMaThuoc.Text=="")
            {
                MessageBox.Show("Bạn phải chọn mã thuốc","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                cboMaThuoc.Focus();
                return;
            }
            if(dtpHSD.Value<=dtpNSX.Value)
            {
                MessageBox.Show("Hạn sử dụng phải lớn hơn ngày sản xuất","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                dtpHSD.Focus();
                return;
            }
            if(dtpNSX.Value>=DateTime.Now)
            {
                MessageBox.Show("Ngày sản xuất phải nhỏ hơn ngày hiện tại","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                dtpNSX.Focus();
                return;
            }
            if (txtSLT.Text=="")
            {
                MessageBox.Show("Bạn phải nhập số lượng tồn","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtSLT.Focus();
                return;
            }
            if (!int.TryParse(txtSLT.Text.Trim(), out int soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên không âm","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtSLT.Focus();
                return;
            }
            
            if (btnThem.Enabled == true)
            {
                DataTable dt = dp.GetDataTable("SELECT * FROM tLoThuoc WHERE MaLo = '" + txtMaLo.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Mã lô thuốc đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaLo.Focus();
                    return;
                }
                dp.ExecuteNonQuery("INSERT INTO tLoThuoc (MaLo, MaThuoc, NgaySanXuat, HanSuDung, SoLuongTon) " +
                    "VALUES ('" + txtMaLo.Text + "', '" + cboMaThuoc.Text + "', '" + dtpNSX.Value.ToString("yyyy-MM-dd") + "', '" +
                    dtpHSD.Value.ToString("yyyy-MM-dd") + "', " + soLuongTon + ")");
                MessageBox.Show("Thêm lô thuốc thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(btnSua.Enabled == true)
            {
                dp.ExecuteNonQuery("UPDATE tLoThuoc SET MaThuoc = '" + cboMaThuoc.Text + 
                    "', NgaySanXuat = '" + dtpNSX.Value.ToString("yyyy-MM-dd") + 
                    "', HanSuDung = '" + dtpHSD.Value.ToString("yyyy-MM-dd") + 
                    "', SoLuongTon = " + soLuongTon + 
                    " WHERE MaLo = '" + txtMaLo.Text + "'");
                MessageBox.Show("Cập nhật lô thuốc thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            DataTable dtLoThuoc = dp.GetDataTable("SELECT * FROM tLoThuoc");
            dgvLoThuoc.DataSource = dtLoThuoc;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnBoqua.Enabled = true;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnSua.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tLoThuoc WHERE MaLo = '" + txtMaLo.Text + "'");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Mã lô thuốc không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa lô thuốc này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dp.ExecuteNonQuery("DELETE FROM tLoThuoc WHERE MaLo = '" + txtMaLo.Text + "'");
                MessageBox.Show("Xóa lô thuốc thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Cập nhật lại DataGridView
                DataTable dtLoThuoc = dp.GetDataTable("SELECT * FROM tLoThuoc");
                dgvLoThuoc.DataSource = dtLoThuoc;
            }
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            txtMaLo.Clear();
            cboMaThuoc.SelectedIndex = -1;
            dtpNSX.Value = DateTime.Now;
            dtpHSD.Value = DateTime.Now;
            txtSLT.Clear();
            btnLuu.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvLoThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            txtMaLo.Text = dgvLoThuoc.CurrentRow.Cells["MaLo"].Value.ToString();
            cboMaThuoc.Text = dgvLoThuoc.CurrentRow.Cells["MaThuoc"].Value.ToString();
            dtpNSX.Value = Convert.ToDateTime(dgvLoThuoc.CurrentRow.Cells["NgaySanXuat"].Value);
            dtpHSD.Value = Convert.ToDateTime(dgvLoThuoc.CurrentRow.Cells["HanSuDung"].Value);
            txtSLT.Text = dgvLoThuoc.CurrentRow.Cells["SoLuongTon"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoqua.Enabled = true;

        }
    }
}
