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
    public partial class frmDangNhap : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        string chucVu = "";
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtTenDangNhap.Text;
            string pass = txtMatKhau.Text;
            if (user == "" || pass == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = $"SELECT * FROM tNhanVien WHERE MaNV = '{user}' AND Password = '{pass}'";
            DataTable dt = dp.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                chucVu = dt.Rows[0]["ChucVu"].ToString();
                frmMain f = new frmMain();
                f.setChucVu(chucVu);
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show(
        "Bạn có muốn thoát không?",
        "Xác nhận thoát",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (r == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}

