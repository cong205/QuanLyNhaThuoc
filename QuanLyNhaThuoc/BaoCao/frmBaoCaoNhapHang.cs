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
    public partial class frmBaoCaoNhapHang : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();

        private string maNhanVien = "";
        private string tenNhanVien = "";
        public frmBaoCaoNhapHang()
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

        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            if (dtpTimeStart.Value > dtpTimeEnd.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2️⃣ Câu SQL chi tiết theo thuốc
            string sql = $@"
        SELECT 
            t.MaThuoc AS [Mã thuốc],
            t.TenThuoc AS [Tên thuốc],
            d.TenDonVi AS [Đơn vị],
            ISNULL(SUM(ct.SoLuongNhap), 0) AS [Số lượng nhập],
            ISNULL(SUM(ct.ThanhTien), 0) AS [Tổng tiền nhập]
        FROM tThuoc t
        JOIN tDonVi d ON t.MaDonVi = d.MaDonVi
        JOIN tLoThuoc l ON t.MaThuoc = l.MaThuoc
        LEFT JOIN tChiTietHDN ct ON l.MaLo = ct.MaLo
        LEFT JOIN tHoaDonNhap hdn 
            ON ct.MaHDN = hdn.MaHDN 
            AND hdn.NgayNhap BETWEEN '{dtpTimeStart.Value:yyyy-MM-dd}' AND '{dtpTimeEnd.Value:yyyy-MM-dd}'
        GROUP BY t.MaThuoc, t.TenThuoc, d.TenDonVi
        ORDER BY t.TenThuoc";

            // 3️⃣ Lấy dữ liệu và hiển thị
            DataTable dt = dp.GetDataTable(sql);
            dgvBaoCaoNhapHang.DataSource = dt;

            // 4️⃣ Kiểm tra nếu không có dữ liệu
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu nhập hàng trong khoảng thời gian này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 5️⃣ Căn chỉnh hiển thị
            dgvBaoCaoNhapHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCaoNhapHang.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void frmBaoCaoNhapHang_Load(object sender, EventArgs e)
        {
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

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            dgvBaoCaoNhapHang.DataSource = null;
            txtTenNhanVien.Clear();
            cboMaNhanVien.SelectedIndex = -1;
            dtpTimeStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimeEnd.Value = DateTime.Now;
            dtpNgayLap.Value = DateTime.Now;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvBaoCaoNhapHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = wb.ActiveSheet;
                ws.Name = "BaoCaoNhapHang";

                // ====== Tiêu đề ======
                ws.Cells[1, 1] = "BÁO CÁO NHẬP HÀNG";
                ws.Cells[2, 1] = $"Từ ngày: {dtpTimeStart.Value:dd/MM/yyyy} - Đến ngày: {dtpTimeEnd.Value:dd/MM/yyyy}";
                ws.Cells[3, 1] = $"Người lập: {txtTenNhanVien.Text}";
                ws.Cells[4, 1] = $"Ngày lập: {dtpNgayLap.Value:dd/MM/yyyy}";

                // ====== Header ======
                for (int i = 0; i < dgvBaoCaoNhapHang.Columns.Count; i++)
                {
                    ws.Cells[6, i + 1] = dgvBaoCaoNhapHang.Columns[i].HeaderText;
                    ws.Cells[6, i + 1].Font.Bold = true;
                    ws.Cells[6, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                }

                // ====== Dữ liệu ======
                for (int i = 0; i < dgvBaoCaoNhapHang.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvBaoCaoNhapHang.Columns.Count; j++)
                    {
                        ws.Cells[i + 7, j + 1] = dgvBaoCaoNhapHang.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // ====== Tính tổng ======
                int tongSoLuong = 0;
                decimal tongTien = 0;

                foreach (DataGridViewRow row in dgvBaoCaoNhapHang.Rows)
                {
                    if (row.Cells["Số lượng nhập"].Value != null &&
                        int.TryParse(row.Cells["Số lượng nhập"].Value.ToString(), out int sl))
                        tongSoLuong += sl;

                    if (row.Cells["Tổng tiền nhập"].Value != null &&
                        decimal.TryParse(row.Cells["Tổng tiền nhập"].Value.ToString(), out decimal tien))
                        tongTien += tien;
                }

                int dongTong = dgvBaoCaoNhapHang.Rows.Count + 8;
                ws.Cells[dongTong, dgvBaoCaoNhapHang.Columns.Count - 1] = "TỔNG CỘNG:";
                ws.Cells[dongTong, dgvBaoCaoNhapHang.Columns.Count] = $"{tongTien:N0} VNĐ";
                ws.Cells[dongTong, dgvBaoCaoNhapHang.Columns.Count - 1].Font.Bold = true;
                ws.Cells[dongTong, dgvBaoCaoNhapHang.Columns.Count].Font.Bold = true;

                // ====== Căn chỉnh đẹp ======
                ws.Columns.AutoFit();
                ws.Rows[1].Font.Size = 16;
                ws.Rows[1].Font.Bold = true;
                ws.Rows[1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
