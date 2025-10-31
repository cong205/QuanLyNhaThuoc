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
    public partial class frmBaoCaoDoanhThu : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();

        public frmBaoCaoDoanhThu()
        {
            InitializeComponent();
        }

        private void frmBaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            // Load danh sách nhân viên
            string sql = "SELECT MaNV, TenNV FROM tNhanVien";
            DataTable dtNV = dp.GetDataTable(sql);
            cboMaNhanVien.DataSource = dtNV;
            cboMaNhanVien.DisplayMember = "MaNV";
            cboMaNhanVien.ValueMember = "MaNV";
            cboMaNhanVien.SelectedIndex = -1;

            // Mặc định ngày lập là hôm nay
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
                    h.MaHDB AS [Mã hóa đơn],
                    n.TenNV AS [Nhân viên],
                    h.NgayBan AS [Ngày bán],
                    h.TongTien AS [Tổng tiền]
                FROM tHoaDonBan h
                JOIN tNhanVien n ON h.MaNV = n.MaNV
                WHERE h.NgayBan BETWEEN '{dtpTimeStart.Value:yyyy-MM-dd}' AND '{dtpTimeEnd.Value:yyyy-MM-dd}'
                ORDER BY h.NgayBan";

            DataTable dt = dp.GetDataTable(sql);
            dgvBaoCaoDoanhThu.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvBaoCaoDoanhThu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = wb.ActiveSheet;
                ws.Name = "BaoCaoDoanhThu";

                // Tiêu đề
                ws.Cells[1, 1] = "BÁO CÁO DOANH THU NHÀ THUỐC";
                ws.Cells[2, 1] = $"Từ ngày: {dtpTimeStart.Value:dd/MM/yyyy} - Đến ngày: {dtpTimeEnd.Value:dd/MM/yyyy}";
                ws.Cells[3, 1] = $"Người lập: {txtTenNhanVien.Text}";
                ws.Cells[4, 1] = $"Ngày lập: {dtpNgayLap.Value:dd/MM/yyyy}";

                // Header
                for (int i = 0; i < dgvBaoCaoDoanhThu.Columns.Count; i++)
                {
                    ws.Cells[6, i + 1] = dgvBaoCaoDoanhThu.Columns[i].HeaderText;
                }

                // Dữ liệu
                for (int i = 0; i < dgvBaoCaoDoanhThu.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvBaoCaoDoanhThu.Columns.Count; j++)
                    {
                        ws.Cells[i + 7, j + 1] = dgvBaoCaoDoanhThu.Rows[i].Cells[j].Value?.ToString();
                    }
                }
                // Tính tổng doanh thu
                decimal tongDoanhThu = 0;
                foreach (DataGridViewRow row in dgvBaoCaoDoanhThu.Rows)
                {
                    if (row.Cells["Tổng tiền"].Value != null &&
                        decimal.TryParse(row.Cells["Tổng tiền"].Value.ToString(), out decimal giaTri))
                    {
                        tongDoanhThu += giaTri;
                    }
                }

                // Ghi tổng doanh thu cuối bảng
                int dongTong = dgvBaoCaoDoanhThu.Rows.Count + 8;
                ws.Cells[dongTong, dgvBaoCaoDoanhThu.Columns.Count - 1] = "TỔNG DOANH THU:";
                ws.Cells[dongTong, dgvBaoCaoDoanhThu.Columns.Count - 1].Font.Bold = true;

                ws.Cells[dongTong, dgvBaoCaoDoanhThu.Columns.Count] = tongDoanhThu;
                ws.Cells[dongTong, dgvBaoCaoDoanhThu.Columns.Count].Font.Bold = true;
                ws.Cells[dongTong, dgvBaoCaoDoanhThu.Columns.Count].NumberFormat = "#,##0 \"VNĐ\"";

                // Tự động căn chỉnh cột
                ws.Columns.AutoFit();
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void dtpTimeStart_ValueChanged(object sender, EventArgs e) { }
        private void dtpTimeEnd_ValueChanged(object sender, EventArgs e) { }
        private void dtpNgayLap_ValueChanged(object sender, EventArgs e) { }
        private void txtTenNhanVien_TextChanged(object sender, EventArgs e) { }
        private void dgvBaoCaoDoanhThu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu trong DataGridView
            dgvBaoCaoDoanhThu.DataSource = null;

            // Xóa textbox tên nhân viên
            txtTenNhanVien.Clear();

            // Reset combobox nhân viên
            cboMaNhanVien.SelectedIndex = -1;

            // Đặt lại ngày bắt đầu và kết thúc về mặc định (tháng hiện tại)
            dtpTimeStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimeEnd.Value = DateTime.Now;

            // Đặt lại ngày lập là hôm nay
            dtpNgayLap.Value = DateTime.Now;
        }
    }
}
