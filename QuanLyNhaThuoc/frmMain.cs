using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.DanhMuc;
using QuanLyNhaThuoc.HoaDon;

namespace QuanLyNhaThuoc
{
    public partial class frmMain : Form
    {
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
            hdb.ShowDialog();

        }

        private void mnuHoaDonNhap_Click(object sender, EventArgs e)
        {
            frmHoaDonNhap hdn = new frmHoaDonNhap();
            hdn.ShowDialog();

        }
    }
}
