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
    public partial class frmDonVi : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        public frmDonVi()
        {
            InitializeComponent();
        }

        private void frmDonVi_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        void LoadData()
        {
            DataTable dt = dp.GetDataTable("SELECT * FROM tDonVi");
            dgvDonVi.DataSource = dt;
            dgvDonVi.Columns["MaDonVi"].HeaderText = "Mã đơn vị";
            dgvDonVi.Columns["TenDonVi"].HeaderText = "Tên đơn vị";
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        void ResetValue()
        {
            txtMaDonVi.Text = "";
            txtTenDonVi.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled=true;
            ResetValue();
            string sql = "SELECT TOP 1 MaDonVi FROM tDonVi ORDER BY MaDonVi DESC";
            DataTable dt = dp.GetDataTable(sql);

            string newMa;
            if (dt.Rows.Count > 0)
            {
                string lastMa = dt.Rows[0]["MaDonVi"].ToString(); // Ví dụ "LT05"
                int num = int.Parse(lastMa.Substring(2)); // lấy phần số
                num++;
                newMa = "DV" + num.ToString("D2"); // LT06
            }
            else
            {
                newMa = "DV01";
            }

            txtMaDonVi.Text = newMa;
            txtMaDonVi.Enabled = false;

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTenDonVi.Text=="")
            {
                MessageBox.Show("Vui lòng nhập tên Đơn vị thuốc!");
                txtTenDonVi.Focus();
                return;
            }
            string sqlCheck = "SELECT * FROM tDonVi WHERE MaDonVi = '" + txtMaDonVi.Text + "'";
            DataTable dt = dp.GetDataTable(sqlCheck);
            if(dt.Rows.Count > 0)
            {
                string sqlUpdate = "UPDATE tDonVi SET TenDonVi = N'" + txtTenDonVi.Text + "' WHERE MaDonVi='" + txtMaDonVi.Text + "'";
                dp.ExecuteNonQuery(sqlUpdate);
                MessageBox.Show("Đã sửa đơn vị thuốc !");
            } 
            else
            {
                string sqlInsert = "INSERT INTO tDonVi VALUES('" + txtMaDonVi.Text + "',N'" + txtTenDonVi.Text + "')";
                dp.ExecuteNonQuery(sqlInsert);
                MessageBox.Show("Đã thêm đơn vị thuốc mới!");
            }
            LoadData();
            ResetValue();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa loại thuốc này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                string strDel = "DELETE FROM tDonVi WHERE MaDonVi ='" + txtMaDonVi.Text + "'";
                dp.ExecuteNonQuery(strDel);
                MessageBox.Show("Đã xóa đơn bị thuốc !");
                LoadData();
                ResetValue();
                btnThem.Enabled = true;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ?", "Thoát", MessageBoxButtons.OK,MessageBoxIcon.Question)==DialogResult.OK)
            {
                Close();
            }
        }

        private void dgvDonVi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                btnLuu.Enabled = false;
                txtMaDonVi.Text=dgvDonVi.CurrentRow.Cells["MaDonVi"].Value.ToString();
                txtTenDonVi.Text = dgvDonVi.CurrentRow.Cells["TenDonVi"].Value.ToString();
            }
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void frmDonVi_Click(object sender, EventArgs e)
        {
            LoadData();
            ResetValue() ;
        }
    }
}
