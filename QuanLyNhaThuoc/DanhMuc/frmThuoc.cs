using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QuanLyNhaThuoc.DanhMuc
{
    public partial class frmThuoc : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        private readonly string imagesFolder = Path.Combine(Application.StartupPath, "Images");


        public frmThuoc()
        {
            InitializeComponent();
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            ResetValue();

            string sql = "SELECT TOP 1 MaThuoc FROM tThuoc ORDER BY MaThuoc DESC";
            DataTable dt = dp.GetDataTable(sql);
            string newMa;

            if (dt.Rows.Count > 0)
            {
                string lastMa = dt.Rows[0]["MaThuoc"].ToString(); // VD: T005
                int num = int.Parse(lastMa.Substring(1));
                num++;
                newMa = "T" + num.ToString("D3");
            }
            else newMa = "T001";

            txtMaThuoc.Text = newMa;
            txtMaThuoc.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTenThuoc.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên thuốc!");
                txtTenThuoc.Focus();
                return;
            }
            if (cboLoaiThuoc.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại thuốc!");
                return;
            }
            if (cboDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị!");
                return;
            }

            string trangThai = rdoConHang.Checked ? "Còn hàng" : "Hết hàng";

            string sqlCheck = $"SELECT * FROM tThuoc WHERE MaThuoc='{txtMaThuoc.Text}'";
            DataTable dt = dp.GetDataTable(sqlCheck);

            if (dt.Rows.Count > 0)
            {
                string sqlUpdate = $@"
                    UPDATE tThuoc SET 
                        TenThuoc = N'{txtTenThuoc.Text}',
                        MaLoaiThuoc = '{cboLoaiThuoc.SelectedValue}',
                        MaDonVi = '{cboDonVi.SelectedValue}',
                        TrangThai = N'{trangThai}',
                        CongDung = N'{txtCongDung.Text}',
                        GiaNhap = {txtGiaNhap.Text},
                        GiaBan = {txtGiaBan.Text}
                    WHERE MaThuoc = '{txtMaThuoc.Text}'";
                dp.ExecuteNonQuery(sqlUpdate);
                MessageBox.Show("Đã cập nhật thuốc!");
            }
            else
            {
                string sqlInsert = $@"
                    INSERT INTO tThuoc (MaThuoc, TenThuoc, MaLoaiThuoc, MaDonVi, TrangThai, CongDung, GiaNhap, GiaBan)
                    VALUES ('{txtMaThuoc.Text}', N'{txtTenThuoc.Text}', '{cboLoaiThuoc.SelectedValue}', 
                            '{cboDonVi.SelectedValue}', N'{trangThai}', N'{txtCongDung.Text}', 
                            {txtGiaNhap.Text}, {txtGiaBan.Text})";
                dp.ExecuteNonQuery(sqlInsert);
                MessageBox.Show("Đã thêm thuốc mới!");
            }

            LoadData();
            ResetValue();
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thuốc này không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = $"DELETE FROM tThuoc WHERE MaThuoc='{txtMaThuoc.Text}'";
                dp.ExecuteNonQuery(sql);
                MessageBox.Show("Đã xóa thuốc!");
                LoadData();
                ResetValue();
            }
        }
        private void HienThiAnhThuoc(string tenFileAnh)
        {
            try
            {
                // Ảnh sẽ được lưu và hiển thị trong bin/Debug/Images
                string duongDanAnh = Path.Combine(imagesFolder, tenFileAnh);

                // Kiểm tra tồn tại
                if (!File.Exists(duongDanAnh))
                {
                    if (picAnh.Image != null)
                    {
                        picAnh.Image.Dispose();
                        picAnh.Image = null;
                    }
                    return;
                }

                // Giải phóng ảnh cũ
                if (picAnh.Image != null)
                {
                    picAnh.Image.Dispose();
                    picAnh.Image = null;
                }

                // Đọc ảnh bằng MemoryStream để tránh lỗi file bị khóa
                byte[] imageBytes = File.ReadAllBytes(duongDanAnh);
                using (var stream = new MemoryStream(imageBytes))
                {
                    picAnh.Image = Image.FromStream(stream);
                }
                picAnh.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị ảnh: " + ex.Message);
                if (picAnh.Image != null)
                {
                    picAnh.Image.Dispose();
                    picAnh.Image = null;
                }
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Lấy danh sách mã thuốc
            string sql = "SELECT MaThuoc, TenThuoc FROM tThuoc";
            DataTable dt = dp.GetDataTable(sql);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có thuốc nào trong danh sách!");
                return;
            }

            // ====== Tạo form tìm kiếm ======
            Form frmSelect = new Form();
            frmSelect.Text = "🔍 Tìm kiếm thuốc";
            frmSelect.Size = new Size(350, 180);
            frmSelect.StartPosition = FormStartPosition.CenterParent;
            frmSelect.FormBorderStyle = FormBorderStyle.FixedDialog;
            frmSelect.MaximizeBox = false;
            frmSelect.MinimizeBox = false;

            // ====== Label hướng dẫn ======
            Label lbl = new Label();
            lbl.Text = "Nhập hoặc chọn mã thuốc:";
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lbl.Location = new Point(20, 20);

            // ====== ComboBox danh sách thuốc ======
            ComboBox cbo = new ComboBox();
            cbo.DropDownStyle = ComboBoxStyle.DropDown; // Cho phép nhập + chọn
            cbo.Width = 280;
            cbo.Location = new Point(20, 50);

            // Thêm cột hiển thị đẹp hơn: Mã - Tên thuốc
            dt.Columns.Add("HienThi", typeof(string), "MaThuoc + ' - ' + TenThuoc");
            cbo.DataSource = dt;
            cbo.DisplayMember = "HienThi";
            cbo.ValueMember = "MaThuoc";

            // ====== Nút Tìm ======
            Button btnOK = new Button();
            btnOK.Text = "Tìm";
            btnOK.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnOK.BackColor = Color.LightSkyBlue;
            btnOK.Width = 80;
            btnOK.Height = 30;
            btnOK.Location = new Point(120, 90);
            btnOK.DialogResult = DialogResult.OK;

            // Thêm control vào form
            frmSelect.Controls.Add(lbl);
            frmSelect.Controls.Add(cbo);
            frmSelect.Controls.Add(btnOK);
            frmSelect.AcceptButton = btnOK;

            // ====== Hiển thị form ======
            if (frmSelect.ShowDialog() == DialogResult.OK)
            {
                string maCanTim = cbo.SelectedValue?.ToString() ?? cbo.Text.Trim();

                if (string.IsNullOrEmpty(maCanTim))
                {
                    MessageBox.Show("Vui lòng nhập hoặc chọn mã thuốc!");
                    return;
                }

                // ====== Tìm thuốc ======
                string sqlThuoc = $"SELECT * FROM tThuoc WHERE MaThuoc = '{maCanTim}'";
                DataTable dtThuoc = dp.GetDataTable(sqlThuoc);

                if (dtThuoc.Rows.Count > 0)
                {
                    DataRow r = dtThuoc.Rows[0];
                    txtMaThuoc.Text = r["MaThuoc"].ToString();
                    txtTenThuoc.Text = r["TenThuoc"].ToString();
                    cboLoaiThuoc.SelectedValue = r["MaLoaiThuoc"].ToString();
                    cboDonVi.SelectedValue = r["MaDonVi"].ToString();
                    txtCongDung.Text = r["CongDung"].ToString();
                    txtGiaNhap.Text = r["GiaNhap"].ToString();
                    txtGiaBan.Text = r["GiaBan"].ToString();

                    string tt = r["TrangThai"].ToString();
                    if (tt == "Còn hàng") rdoConHang.Checked = true;
                    else rdoHetHang.Checked = true;

                    // ====== Lấy thông tin lô thuốc ======
                    string sqlLo = $"SELECT TOP 1 * FROM tLoThuoc WHERE MaThuoc = '{maCanTim}' ORDER BY NgaySanXuat DESC";
                    DataTable dtLo = dp.GetDataTable(sqlLo);

                    if (dtLo.Rows.Count > 0)
                    {
                        txtSoLuongTon.Text = dtLo.Rows[0]["SoLuongTon"].ToString();
                        if (dtLo.Rows[0]["NgaySanXuat"] != DBNull.Value)
                            dtpNgaySX.Value = Convert.ToDateTime(dtLo.Rows[0]["NgaySanXuat"]);
                        if (dtLo.Rows[0]["HanSuDung"] != DBNull.Value)
                            dtpHanSD.Value = Convert.ToDateTime(dtLo.Rows[0]["HanSuDung"]);
                    }
                    else
                    {
                        txtSoLuongTon.Text = "0";
                        dtpNgaySX.Value = DateTime.Now;
                        dtpHanSD.Value = DateTime.Now;
                    }

                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    // ✅ Hiển thị ảnh thuốc
                    if (dtThuoc.Rows[0]["Anh"] != DBNull.Value)
                    {
                        string tenFileAnh = dtThuoc.Rows[0]["Anh"].ToString();
                        HienThiAnhThuoc(tenFileAnh);
                    }
                    else
                    {
                        picAnh.Image = null;
                    }
                    dgvThuoc.DataSource = dtThuoc;
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy thuốc có mã '{maCanTim}'!");
                }

            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu đang hiển thị trong DataGridView
                System.Data.DataTable dt = (System.Data.DataTable)dgvThuoc.DataSource;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!");
                    return;
                }

                // ====== Tạo ứng dụng Excel ======
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true; // Mở Excel ngay trên màn hình

                Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Name = "Danh sách thuốc";

                // ====== Tiêu đề ======
                worksheet.Cells[1, 1] = "DANH SÁCH THUỐC";
                Excel.Range titleRange = worksheet.Range["A1", "H1"];
                titleRange.Merge();
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // ====== Ghi tiêu đề cột ======
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[3, i + 1] = dgvThuoc.Columns[i].HeaderText;
                    Excel.Range header = worksheet.Cells[3, i + 1];
                    header.Font.Bold = true;
                    header.Interior.Color = Color.LightGray;
                    header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    header.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }

                // ====== Ghi dữ liệu ======
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 4, j + 1] = dt.Rows[i][j].ToString();
                        Excel.Range cell = worksheet.Cells[i + 4, j + 1];
                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }

                // ====== Tự căn chỉnh kích thước cột ======
                worksheet.Columns.AutoFit();

                // ====== Cuộn lên đầu trang ======
                worksheet.Activate();
                worksheet.Application.ActiveWindow.SplitRow = 0;
                worksheet.Application.ActiveWindow.SplitColumn = 0;
                worksheet.Application.ActiveWindow.FreezePanes = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void dgvThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaThuoc.Text = dgvThuoc.CurrentRow.Cells["MaThuoc"].Value.ToString();
                txtTenThuoc.Text = dgvThuoc.CurrentRow.Cells["TenThuoc"].Value.ToString();
                cboLoaiThuoc.SelectedValue = dgvThuoc.CurrentRow.Cells["MaLoaiThuoc"].Value.ToString();
                cboDonVi.SelectedValue = dgvThuoc.CurrentRow.Cells["MaDonVi"].Value.ToString();
                txtCongDung.Text = dgvThuoc.CurrentRow.Cells["CongDung"].Value.ToString();
                txtGiaNhap.Text = dgvThuoc.CurrentRow.Cells["GiaNhap"].Value.ToString();
                txtGiaBan.Text = dgvThuoc.CurrentRow.Cells["GiaBan"].Value.ToString();

                string sqlAnh = $"SELECT Anh FROM tThuoc WHERE MaThuoc = '{txtMaThuoc.Text}'";
                DataTable dtAnh = dp.GetDataTable(sqlAnh);

                if (dtAnh.Rows.Count > 0 && dtAnh.Rows[0]["Anh"] != DBNull.Value)
                {
                    HienThiAnhThuoc(dtAnh.Rows[0]["Anh"].ToString());
                }
                else
                {
                    if (picAnh.Image != null)
                    {
                        picAnh.Image.Dispose();
                        picAnh.Image = null;
                    }
                }

                string tt = dgvThuoc.CurrentRow.Cells["TrangThai"].Value.ToString();
                if (tt == "Còn hàng") rdoConHang.Checked = true;
                else rdoHetHang.Checked = true;

                string maThuoc = txtMaThuoc.Text;
                string sqlLo = $"SELECT TOP 1 * FROM tLoThuoc WHERE MaThuoc = '{maThuoc}' ORDER BY NgaySanXuat DESC";
                DataTable dtLo = dp.GetDataTable(sqlLo);

                if (dtLo.Rows.Count > 0)
                {
                    txtSoLuongTon.Text = dtLo.Rows[0]["SoLuongTon"].ToString();
                    if (dtLo.Rows[0]["NgaySanXuat"] != DBNull.Value)
                        dtpNgaySX.Value = Convert.ToDateTime(dtLo.Rows[0]["NgaySanXuat"]);
                    if (dtLo.Rows[0]["HanSuDung"] != DBNull.Value)
                        dtpHanSD.Value = Convert.ToDateTime(dtLo.Rows[0]["HanSuDung"]);
                }
                else
                {
                    txtSoLuongTon.Text = "0";
                    dtpNgaySX.Value = DateTime.Now;
                    dtpHanSD.Value = DateTime.Now;
                }

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo thư mục chứa ảnh tồn tại
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Chọn ảnh thuốc";
                    ofd.Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = Path.GetFileName(ofd.FileName);
                        string destPath = Path.Combine(imagesFolder, fileName); // ảnh lưu ở bin/Debug/Images

                        // Giải phóng ảnh cũ nếu có (tránh lỗi "file bị khóa")
                        if (picAnh.Image != null)
                        {
                            picAnh.Image.Dispose();
                            picAnh.Image = null;
                        }

                        // Nếu file đã tồn tại, ghi đè lên
                        File.Copy(ofd.FileName, destPath, true);

                        // Đọc file ảnh một cách an toàn (tránh bị lock)
                        byte[] imageBytes;
                        using (FileStream fs = new FileStream(destPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var ms = new MemoryStream())
                            {
                                fs.CopyTo(ms);
                                imageBytes = ms.ToArray();
                            }
                        }

                        // Hiển thị ảnh trong PictureBox
                        using (var stream = new MemoryStream(imageBytes))
                        {
                            picAnh.Image = Image.FromStream(stream);
                        }
                        picAnh.SizeMode = PictureBoxSizeMode.Zoom;

                        // Lưu tên ảnh vào CSDL
                        string sql = $"UPDATE tThuoc SET Anh = N'{fileName}' WHERE MaThuoc = '{txtMaThuoc.Text}'";
                        dp.ExecuteNonQuery(sql);

                        MessageBox.Show("Đã cập nhật ảnh cho thuốc!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm ảnh: " + ex.Message);
            }
        }


        private void frmThuoc_Load(object sender, EventArgs e)
        {
            MessageBox.Show(imagesFolder);
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);
            LoadData();
            LoadComboBox();
        }
        void LoadComboBox()
        {
            // Load loại thuốc
            cboLoaiThuoc.DataSource = dp.GetDataTable("SELECT MaLoaiThuoc, TenLoaiThuoc FROM tLoaiThuoc");
            cboLoaiThuoc.DisplayMember = "TenLoaiThuoc";
            cboLoaiThuoc.ValueMember = "MaLoaiThuoc";

            // Load đơn vị
            cboDonVi.DataSource = dp.GetDataTable("SELECT MaDonVi, TenDonVi FROM tDonVi");
            cboDonVi.DisplayMember = "TenDonVi";
            cboDonVi.ValueMember = "MaDonVi";
        }
        void LoadData()
        {
            string sql = @"SELECT MaThuoc, TenThuoc, MaLoaiThuoc, MaDonVi, TrangThai, 
                                  CongDung, GiaNhap, GiaBan
                           FROM tThuoc";
            DataTable dt = dp.GetDataTable(sql);
            dgvThuoc.DataSource = dt;

            dgvThuoc.Columns["MaThuoc"].HeaderText = "Mã thuốc";
            dgvThuoc.Columns["TenThuoc"].HeaderText = "Tên thuốc";
            dgvThuoc.Columns["MaLoaiThuoc"].HeaderText = "Loại thuốc";
            dgvThuoc.Columns["MaDonVi"].HeaderText = "Đơn vị";
            dgvThuoc.Columns["TrangThai"].HeaderText = "Trạng thái";
            dgvThuoc.Columns["CongDung"].HeaderText = "Công dụng";
            dgvThuoc.Columns["GiaNhap"].HeaderText = "Giá nhập";
            dgvThuoc.Columns["GiaBan"].HeaderText = "Giá bán";

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
        }
        void ResetValue()
        {
            txtMaThuoc.Text = "";
            txtTenThuoc.Text = "";
            txtSoLuongTon.Text = "";
            txtCongDung.Text = "";
            txtGiaNhap.Text = "";
            txtGiaBan.Text = "";
            rdoConHang.Checked = true;
            cboLoaiThuoc.SelectedIndex = -1;
            cboDonVi.SelectedIndex = -1;
            if (picAnh.Image != null)
            {
                picAnh.Image.Dispose();
                picAnh.Image = null;
            }
        }

        private void frmThuoc_Click(object sender, EventArgs e)
        {
            ResetValue();
            LoadData();
            btnThem.Enabled = true;
        }
    }
}
