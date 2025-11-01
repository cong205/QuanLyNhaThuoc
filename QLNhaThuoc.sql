-- Tạo database
CREATE DATABASE QLNhaThuoc;
GO

USE QLNhaThuoc;
GO

-- =========================
-- 1. Bảng LOẠI THUỐC
-- =========================
CREATE TABLE tLoaiThuoc (
    MaLoaiThuoc NVARCHAR(30) PRIMARY KEY,
    TenLoaiThuoc NVARCHAR(100) NOT NULL
);
GO

-- =========================
-- 2. Bảng ĐƠN VỊ TÍNH
-- =========================
CREATE TABLE tDonVi (
    MaDonVi NVARCHAR(30) PRIMARY KEY,
    TenDonVi NVARCHAR(50) NOT NULL
);
GO

-- =========================
-- 3. Bảng THUỐC
-- =========================
CREATE TABLE tThuoc (
    MaThuoc NVARCHAR(30) PRIMARY KEY,
    TenThuoc NVARCHAR(100) NOT NULL,
    MaLoaiThuoc NVARCHAR(30) REFERENCES tLoaiThuoc(MaLoaiThuoc),
    MaDonVi NVARCHAR(30) REFERENCES tDonVi(MaDonVi),
	TrangThai NVARCHAR(20),
    CongDung NVARCHAR(255),
    GiaNhap DECIMAL(18,2),
    GiaBan DECIMAL(18,2),
	Anh NVARCHAR(50)
);
GO

-- =========================
-- 4. Bảng LÔ THUỐC
-- =========================
CREATE TABLE tLoThuoc (
    MaLo NVARCHAR(30) PRIMARY KEY,
    MaThuoc NVARCHAR(30) REFERENCES tThuoc(MaThuoc),
    NgaySanXuat DATE,
    HanSuDung DATE,
    SoLuongTon INT
);
GO

-- =========================
-- 5. Bảng NHÀ CUNG CẤP
-- =========================
CREATE TABLE tNhaCungCap (
    MaNCC NVARCHAR(30) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    SoDienThoai NVARCHAR(20)
);
GO

-- =========================
-- 6. Bảng NHÂN VIÊN
-- =========================
CREATE TABLE tNhanVien (
    MaNV NVARCHAR(30) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(5),
    NgaySinh DATE,
    DiaChi NVARCHAR(200),
    SoDienThoai NVARCHAR(20),
    ChucVu NVARCHAR(50),
    Password NVARCHAR(50) NOT NULL
);
GO

-- =========================
-- 7. Bảng KHÁCH HÀNG
-- =========================
CREATE TABLE tKhachHang (
    MaKH NVARCHAR(30) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(5),
    NgaySinh DATE,
    DiaChi NVARCHAR(200),
    SoDienThoai NVARCHAR(20)
);
GO

-- =========================
-- 8. Bảng HÓA ĐƠN NHẬP
-- =========================
CREATE TABLE tHoaDonNhap (
    MaHDN NVARCHAR(30) PRIMARY KEY,
    MaNCC NVARCHAR(30) REFERENCES tNhaCungCap(MaNCC),
    MaNV NVARCHAR(30) REFERENCES tNhanVien(MaNV),
    NgayNhap DATE,
    TongTien DECIMAL(18,2)
);
GO

-- =========================
-- 9. Bảng CHI TIẾT HÓA ĐƠN NHẬP
-- =========================
CREATE TABLE tChiTietHDN (
    MaHDN NVARCHAR(30),
    MaLo NVARCHAR(30),
    SoLuongNhap INT,
    ThanhTien DECIMAL(18,2),
    PRIMARY KEY (MaHDN, MaLo),
    FOREIGN KEY (MaHDN) REFERENCES tHoaDonNhap(MaHDN),
    FOREIGN KEY (MaLo) REFERENCES tLoThuoc(MaLo)
);
GO

-- =========================
-- 10. Bảng HÓA ĐƠN BÁN
-- =========================
CREATE TABLE tHoaDonBan (
    MaHDB NVARCHAR(30) PRIMARY KEY,
    MaKH NVARCHAR(30) REFERENCES tKhachHang(MaKH),
    MaNV NVARCHAR(30) REFERENCES tNhanVien(MaNV),
    NgayBan DATE,
    TongTien DECIMAL(18,2)
);
GO

-- =========================
-- 11. Bảng CHI TIẾT HÓA ĐƠN BÁN
-- =========================
CREATE TABLE tChiTietHDB (
    MaHDB NVARCHAR(30),
    MaLo NVARCHAR(30),
    SoLuongBan INT,
	GiamGia FLOAT,
    ThanhTien DECIMAL(18,2),
    PRIMARY KEY (MaHDB, MaLo),
    FOREIGN KEY (MaHDB) REFERENCES tHoaDonBan(MaHDB),
    FOREIGN KEY (MaLo) REFERENCES tLoThuoc(MaLo)
);
USE QLNhaThuoc;
GO

-- =====================================
-- 1. LOẠI THUỐC
-- =====================================
INSERT INTO tLoaiThuoc VALUES
('LT01', N'Kháng sinh'),
('LT02', N'Giảm đau'),
('LT03', N'Hạ sốt'),
('LT04', N'Vitamin'),
('LT05', N'Tiêu hóa'),
('LT06', N'Tim mạch'),
('LT07', N'Hô hấp'),
('LT08', N'Chống dị ứng'),
('LT09', N'Dưỡng não'),
('LT10', N'Kháng viêm');
GO

-- =====================================
-- 2. ĐƠN VỊ TÍNH
-- =====================================
INSERT INTO tDonVi VALUES
('DV01', N'Viên'),
('DV02', N'Hộp'),
('DV03', N'Vỉ'),
('DV04', N'Chai'),
('DV05', N'Gói'),
('DV06', N'Ống'),
('DV07', N'Bình'),
('DV08', N'Hũ'),
('DV09', N'Tuýp'),
('DV10', N'Lọ');
GO

-- =====================================
-- 3. THUỐC
-- =====================================
INSERT INTO tThuoc VALUES
('T001', N'Amoxicillin', 'LT01', 'DV03', N'Còn hàng', N'Kháng sinh điều trị nhiễm khuẩn', 2500, 4000,N'amoxicillin.jpg'),
('T002', N'Paracetamol', 'LT03', 'DV01', N'Còn hàng', N'Hạ sốt, giảm đau', 2000, 3500,N'paracetamol.jpg'),
('T003', N'Vitamin C', 'LT04', 'DV02', N'Còn hàng', N'Tăng sức đề kháng', 10000, 15000,N'vitamin_c.jpg'),
('T004', N'Metoprolol', 'LT06', 'DV01', N'Còn hàng', N'Điều trị tim mạch', 5000, 8000,N'metoprolol.jpg'),
('T005', N'Omeprazole', 'LT05', 'DV01', N'Còn hàng', N'Chống viêm loét dạ dày', 3000, 5000,N'omeprazole.jpg'),
('T006', N'Loratadine', 'LT08', 'DV01', N'Còn hàng', N'Chống dị ứng', 2500, 4000,N'loratadine.jpg'),
('T007', N'Ginkgo Biloba', 'LT09', 'DV02', N'Còn hàng', N'Tăng cường tuần hoàn não', 15000, 20000,N'ginkgo_biloba.jpg'),
('T008', N'Ibuprofen', 'LT02', 'DV01', N'Còn hàng', N'Giảm đau, hạ sốt', 3000, 5000,N'ibuprofen.jpg'),
('T009', N'Salbutamol', 'LT07', 'DV06', N'Còn hàng', N'Giãn phế quản', 8000, 12000,N'salbutamol.jpg'),
('T010', N'Prednisolone', 'LT10', 'DV01', N'Còn hàng', N'Kháng viêm mạnh', 4000, 6500,N'prednisolone.jpg');
GO

-- =====================================
-- 4. LÔ THUỐC
-- =====================================
INSERT INTO tLoThuoc VALUES
('L001', 'T001', '2024-06-01', '2026-06-01', 500),
('L002', 'T002', '2024-05-15', '2026-05-15', 300),
('L003', 'T003', '2024-03-20', '2026-03-20', 200),
('L004', 'T004', '2024-07-10', '2026-07-10', 150),
('L005', 'T005', '2024-02-12', '2026-02-12', 400),
('L006', 'T006', '2024-04-18', '2026-04-18', 350),
('L007', 'T007', '2024-01-25', '2026-01-25', 250),
('L008', 'T008', '2024-09-01', '2026-09-01', 600),
('L009', 'T009', '2024-10-12', '2026-10-12', 180),
('L010', 'T010', '2024-08-05', '2026-08-05', 220);
GO

-- =====================================
-- 5. NHÀ CUNG CẤP
-- =====================================
INSERT INTO tNhaCungCap VALUES
('NCC01', N'Công ty Dược Trung ương', N'Hà Nội', '0912000111'),
('NCC02', N'Công ty Dược Hậu Giang', N'Cần Thơ', '0912000112'),
('NCC03', N'Công ty Dược Pymepharco', N'Phú Yên', '0912000113'),
('NCC04', N'Công ty Dược Sanofi', N'Hồ Chí Minh', '0912000114'),
('NCC05', N'Công ty Dược OPC', N'Hồ Chí Minh', '0912000115'),
('NCC06', N'Công ty Traphaco', N'Hà Nội', '0912000116'),
('NCC07', N'Công ty Domesco', N'Đồng Tháp', '0912000117'),
('NCC08', N'Công ty US Pharma', N'Hồ Chí Minh', '0912000118'),
('NCC09', N'Công ty Mekophar', N'Hà Nội', '0912000119'),
('NCC10', N'Công ty SPM', N'Hồ Chí Minh', '0912000120');
GO

-- =====================================
-- 6. NHÂN VIÊN
-- =====================================
INSERT INTO tNhanVien VALUES
('NV01', N'Nguyễn Văn A', N'Nam', '1990-01-01', N'Hà Nội', '0911111111', N'Quản lý', 'NV01'),
('NV02', N'Trần Thị B', N'Nữ', '1992-02-02', N'Hồ Chí Minh', '0911111112', N'Nhân viên', 'NV02'),
('NV03', N'Lê Văn C', N'Nam', '1995-03-03', N'Đà Nẵng', '0911111113', N'Nhân viên', 'NV03'),
('NV04', N'Phạm Thị D', N'Nữ', '1998-04-04', N'Cần Thơ', '0911111114', N'Nhân viên', 'NV04'),
('NV05', N'Hoàng Văn E', N'Nam', '1993-05-05', N'Hà Nội', '0911111115', N'Nhân viên ', 'NV05'),
('NV06', N'Đỗ Thị F', N'Nữ', '1996-06-06', N'Hải Phòng', '0911111116', N'Nhân viên ', 'NV06'),
('NV07', N'Nguyễn Văn G', N'Nam', '1991-07-07', N'Nghệ An', '0911111117', N'Nhân viên', 'NV07'),
('NV08', N'Trần Thị H', N'Nữ', '1994-08-08', N'Huế', '0911111118', N'Nhân viên', 'NV08'),
('NV09', N'Lý Văn I', N'Nam', '1997-09-09', N'Hà Nội', '0911111119', N'Nhân viên', 'NV09'),
('NV10', N'Phạm Thị K', N'Nữ', '1999-10-10', N'Đà Nẵng', '0911111120', N'Quản lý', 'NV10');
GO


-- =====================================
-- 7. KHÁCH HÀNG
-- =====================================
INSERT INTO tKhachHang VALUES
('KH01', N'Nguyễn Minh Tâm', N'Nam', '1990-03-12', N'Hà Nội', '0912000211'),
('KH02', N'Lê Thị Thu', N'Nữ', '1988-06-05', N'Hải Dương', '0912000212'),
('KH03', N'Phạm Văn Quang', N'Nam', '1985-02-25', N'Hà Nội', '0912000213'),
('KH04', N'Đỗ Thị Mai', N'Nữ', '1995-09-14', N'Hà Nam', '0912000214'),
('KH05', N'Trần Văn Nam', N'Nam', '1992-10-01', N'Đà Nẵng', '0912000215'),
('KH06', N'Lưu Thị Lan', N'Nữ', '1998-11-10', N'Hà Nội', '0912000216'),
('KH07', N'Vũ Văn Dũng', N'Nam', '1987-08-20', N'Nam Định', '0912000217'),
('KH08', N'Ngô Thị Hoa', N'Nữ', '1993-12-02', N'Hà Nội', '0912000218'),
('KH09', N'Phan Văn Tú', N'Nam', '1991-04-22', N'Hải Phòng', '0912000219'),
('KH10', N'Hoàng Thị Yến', N'Nữ', '1996-07-30', N'Hà Nội', '0912000220');
GO

-- =====================================
-- 8. HÓA ĐƠN NHẬP
-- =====================================
INSERT INTO tHoaDonNhap VALUES
('HDN20112025001', 'NCC01', 'NV05', '2025-11-20', 500000),
('HDN20112025002', 'NCC02', 'NV05', '2025-11-20', 650000),
('HDN20112025003', 'NCC03', 'NV05', '2025-11-20', 400000),
('HDN20112025004', 'NCC04', 'NV05', '2025-11-20', 800000),
('HDN20112025005', 'NCC05', 'NV05', '2025-11-20', 750000),
('HDN20112025006', 'NCC06', 'NV05', '2025-11-20', 600000),
('HDN20112025007', 'NCC07', 'NV05', '2025-11-20', 900000),
('HDN20112025008', 'NCC08', 'NV05', '2025-11-20', 300000),
('HDN20112025009', 'NCC09', 'NV05', '2025-11-20', 550000),
('HDN20112025010', 'NCC10', 'NV05', '2025-11-20', 700000);
GO

-- =====================================
-- 9. CHI TIẾT HÓA ĐƠN NHẬP
-- =====================================
INSERT INTO tChiTietHDN (MaHDN, MaLo, SoLuongNhap, ThanhTien) VALUES
('HDN20112025001', 'L001', 100, 250000),
('HDN20112025002', 'L002', 100, 200000),
('HDN20112025003', 'L003', 50, 500000),
('HDN20112025004', 'L004', 80, 400000),
('HDN20112025005', 'L005', 60, 180000),
('HDN20112025006', 'L006', 70, 175000),
('HDN20112025007', 'L007', 90, 1350000),
('HDN20112025008', 'L008', 120, 360000),
('HDN20112025009', 'L009', 100, 800000),
('HDN20112025010', 'L010', 75, 300000);
GO

-- =====================================
-- 10. HÓA ĐƠN BÁN
-- =====================================
INSERT INTO tHoaDonBan VALUES
('HDB20112025001', 'KH01', 'NV03', '2025-11-20', 80000),
('HDB20112025002', 'KH02', 'NV03', '2025-11-20', 120000),
('HDB20112025003', 'KH03', 'NV06', '2025-11-20', 95000),
('HDB20112025004', 'KH04', 'NV03', '2025-11-20', 60000),
('HDB20112025005', 'KH05', 'NV06', '2025-11-20', 115000),
('HDB20112025006', 'KH06', 'NV03', '2025-11-20', 70000),
('HDB20112025007', 'KH07', 'NV03', '2025-11-20', 98000),
('HDB20112025008', 'KH08', 'NV06', '2025-11-20', 64000),
('HDB20112025009', 'KH09', 'NV03', '2025-11-20', 56000),
('HDB20112025010', 'KH10', 'NV06', '2025-11-20', 102000);
GO

-- =====================================
-- 11. CHI TIẾT HÓA ĐƠN BÁN
-- =====================================
INSERT INTO tChiTietHDB (MaHDB, MaLo, SoLuongBan, GiamGia, ThanhTien) VALUES
('HDB20112025001', 'L001', 10, 0.05, 38000),
('HDB20112025002', 'L002', 20, 0, 70000),
('HDB20112025003', 'L003', 5, 0.1, 67500),
('HDB20112025004', 'L004', 8, 0, 64000),
('HDB20112025005', 'L005', 6, 0.05, 28500),
('HDB20112025006', 'L006', 10, 0, 40000),
('HDB20112025007', 'L007', 4, 0.1, 72000),
('HDB20112025008', 'L008', 12, 0.05, 57000),
('HDB20112025009', 'L009', 6, 0, 72000),
('HDB20112025010', 'L010', 7, 0.05, 43175);
GO
