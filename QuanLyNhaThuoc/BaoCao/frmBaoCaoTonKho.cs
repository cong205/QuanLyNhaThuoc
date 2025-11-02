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

namespace QuanLyNhaThuoc.BaoCao
{
    public partial class frmBaoCaoTonKho : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();

        private string maNhanVien = "";
        private string tenNhanVien = "";
        public frmBaoCaoTonKho()
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
        private void frmBaoCaoTonKho_Load(object sender, EventArgs e)
        {
            // Load danh sách nhân viên
            string sqlNV = "SELECT MaNV, TenNV FROM tNhanVien";
            DataTable dtNV = dp.GetDataTable(sqlNV);
            cboMaNhanVien.DataSource = dtNV;
            cboMaNhanVien.DisplayMember = "MaNV";
            cboMaNhanVien.ValueMember = "MaNV";
            cboMaNhanVien.SelectedIndex = -1;
            // 🔹 Nếu có nhân viên đăng nhập thì gán sẵn
            if (!string.IsNullOrEmpty(maNhanVien))
            {
                cboMaNhanVien.SelectedValue = maNhanVien;
                txtTenNhanVien.Text = tenNhanVien;
            }
            else
            {
                cboMaNhanVien.SelectedIndex = -1;
                txtTenNhanVien.Clear();
            }
            // Mặc định ngày lập hôm nay
            dtpNgayLap.Value = DateTime.Now;
        }

        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNhanVien.SelectedIndex >= 0)
            {
                string maNV = cboMaNhanVien.SelectedValue.ToString();
                string sql = $"SELECT TenNV FROM tNhanVien WHERE MaNV = '{maNV}'";
                DataTable dt = dp.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                    txtTenNhanVien.Text = dt.Rows[0]["TenNV"].ToString();
            }
        }

        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            if (dtpTimeStart.Value > dtpTimeEnd.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = $@"
                SELECT 
                    t.MaThuoc AS [Mã thuốc],
                    t.TenThuoc AS [Tên thuốc],
                    d.TenDonVi AS [Đơn vị],
                    ISNULL(SUM(n.SoLuongNhap), 0) AS [Số lượng nhập],
                    ISNULL(SUM(b.SoLuongBan), 0) AS [Số lượng bán],
                    (ISNULL(SUM(n.SoLuongNhap), 0) - ISNULL(SUM(b.SoLuongBan), 0)) AS [Tồn kho]
                FROM tThuoc t
                LEFT JOIN tDonVi d ON t.MaDonVi = d.MaDonVi
                LEFT JOIN tLoThuoc l ON t.MaThuoc = l.MaThuoc
                LEFT JOIN tChiTietHDN n ON l.MaLo = n.MaLo
                LEFT JOIN tHoaDonNhap hdn ON n.MaHDN = hdn.MaHDN AND hdn.NgayNhap BETWEEN '{dtpTimeStart.Value:yyyy-MM-dd}' AND '{dtpTimeEnd.Value:yyyy-MM-dd}'
                LEFT JOIN tChiTietHDB b ON l.MaLo = b.MaLo
                LEFT JOIN tHoaDonBan hdb ON b.MaHDB = hdb.MaHDB AND hdb.NgayBan BETWEEN '{dtpTimeStart.Value:yyyy-MM-dd}' AND '{dtpTimeEnd.Value:yyyy-MM-dd}'
                GROUP BY t.MaThuoc, t.TenThuoc, d.TenDonVi
                ORDER BY t.TenThuoc";

            DataTable dt = dp.GetDataTable(sql);
            dgvBaoCaoTonKho.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvBaoCaoTonKho.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = wb.ActiveSheet;
                ws.Name = "BaoCaoTonKho";

                // Tiêu đề
                ws.Cells[1, 1] = "BÁO CÁO TỒN KHO NHÀ THUỐC";
                ws.Cells[2, 1] = $"Từ ngày: {dtpTimeStart.Value:dd/MM/yyyy} - Đến ngày: {dtpTimeEnd.Value:dd/MM/yyyy}";
                ws.Cells[3, 1] = $"Người lập: {txtTenNhanVien.Text}";
                ws.Cells[4, 1] = $"Ngày lập: {dtpNgayLap.Value:dd/MM/yyyy}";

                // Header
                for (int i = 0; i < dgvBaoCaoTonKho.Columns.Count; i++)
                {
                    ws.Cells[6, i + 1] = dgvBaoCaoTonKho.Columns[i].HeaderText;
                }

                // Dữ liệu
                for (int i = 0; i < dgvBaoCaoTonKho.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvBaoCaoTonKho.Columns.Count; j++)
                    {
                        ws.Cells[i + 7, j + 1] = dgvBaoCaoTonKho.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Tính tổng tồn
                int tongTon = 0;
                foreach (DataGridViewRow row in dgvBaoCaoTonKho.Rows)
                {
                    if (row.Cells["Tồn kho"].Value != null &&
                        int.TryParse(row.Cells["Tồn kho"].Value.ToString(), out int ton))
                    {
                        tongTon += ton;
                    }
                }

                // Ghi tổng
                int dongTong = dgvBaoCaoTonKho.Rows.Count + 8;
                ws.Cells[dongTong, dgvBaoCaoTonKho.Columns.Count - 1] = "TỔNG TỒN:";
                ws.Cells[dongTong, dgvBaoCaoTonKho.Columns.Count] = tongTon;
                ws.Cells[dongTong, dgvBaoCaoTonKho.Columns.Count - 1].Font.Bold = true;
                ws.Cells[dongTong, dgvBaoCaoTonKho.Columns.Count].Font.Bold = true;

                // Tự động căn chỉnh
                ws.Columns.AutoFit();
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            dgvBaoCaoTonKho.DataSource = null;
            txtTenNhanVien.Clear();
            cboMaNhanVien.SelectedIndex = -1;
            dtpTimeStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimeEnd.Value = DateTime.Now;
            dtpNgayLap.Value = DateTime.Now;
        }
        private void dtpTimeStart_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dtpTimeEnd_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dtpNgayLap_ValueChanged(object sender, EventArgs e)
        {
        }

        private void txtTenNhanVien_TextChanged(object sender, EventArgs e)
        {
        }

        private void dgvBaoCaoTonKho_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}

