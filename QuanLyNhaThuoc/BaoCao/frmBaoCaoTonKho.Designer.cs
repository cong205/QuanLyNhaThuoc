namespace QuanLyNhaThuoc.BaoCao
{
    partial class frmBaoCaoTonKho
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHuyBo = new System.Windows.Forms.Button();
            this.txtTenNhanVien = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboMaNhanVien = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExcel = new System.Windows.Forms.Button();
            this.dtpTimeStart = new System.Windows.Forms.DateTimePicker();
            this.btnXemBaoCao = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvBaoCaoTonKho = new System.Windows.Forms.DataGridView();
            this.dtpTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCaoTonKho)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHuyBo
            // 
            this.btnHuyBo.Location = new System.Drawing.Point(638, 155);
            this.btnHuyBo.Name = "btnHuyBo";
            this.btnHuyBo.Size = new System.Drawing.Size(103, 46);
            this.btnHuyBo.TabIndex = 16;
            this.btnHuyBo.Text = "Hủy bỏ";
            this.btnHuyBo.UseVisualStyleBackColor = true;
            this.btnHuyBo.Click += new System.EventHandler(this.btnHuyBo_Click);
            // 
            // txtTenNhanVien
            // 
            this.txtTenNhanVien.Enabled = false;
            this.txtTenNhanVien.Location = new System.Drawing.Point(472, 92);
            this.txtTenNhanVien.Name = "txtTenNhanVien";
            this.txtTenNhanVien.Size = new System.Drawing.Size(200, 22);
            this.txtTenNhanVien.TabIndex = 15;
            this.txtTenNhanVien.TextChanged += new System.EventHandler(this.txtTenNhanVien_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(367, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Tên nhân viên:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(367, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Mã nhân viên:";
            // 
            // cboMaNhanVien
            // 
            this.cboMaNhanVien.Enabled = false;
            this.cboMaNhanVien.FormattingEnabled = true;
            this.cboMaNhanVien.Location = new System.Drawing.Point(472, 55);
            this.cboMaNhanVien.Name = "cboMaNhanVien";
            this.cboMaNhanVien.Size = new System.Drawing.Size(200, 24);
            this.cboMaNhanVien.TabIndex = 12;
            this.cboMaNhanVien.SelectedIndexChanged += new System.EventHandler(this.cboMaNhanVien_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(367, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Người lập:";
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayLap.Location = new System.Drawing.Point(60, 165);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(200, 22);
            this.dtpNgayLap.TabIndex = 10;
            this.dtpNgayLap.ValueChanged += new System.EventHandler(this.dtpNgayLap_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ngày lập:";
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(370, 155);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(103, 46);
            this.btnExcel.TabIndex = 8;
            this.btnExcel.Text = "xuất excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // dtpTimeStart
            // 
            this.dtpTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTimeStart.Location = new System.Drawing.Point(60, 57);
            this.dtpTimeStart.Name = "dtpTimeStart";
            this.dtpTimeStart.Size = new System.Drawing.Size(200, 22);
            this.dtpTimeStart.TabIndex = 6;
            this.dtpTimeStart.ValueChanged += new System.EventHandler(this.dtpTimeStart_ValueChanged);
            // 
            // btnXemBaoCao
            // 
            this.btnXemBaoCao.Location = new System.Drawing.Point(504, 155);
            this.btnXemBaoCao.Name = "btnXemBaoCao";
            this.btnXemBaoCao.Size = new System.Drawing.Size(103, 46);
            this.btnXemBaoCao.TabIndex = 2;
            this.btnXemBaoCao.Text = "Xem báo cáo";
            this.btnXemBaoCao.UseVisualStyleBackColor = true;
            this.btnXemBaoCao.Click += new System.EventHandler(this.btnXemBaoCao_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "từ:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "đến:";
            // 
            // dgvBaoCaoTonKho
            // 
            this.dgvBaoCaoTonKho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoCaoTonKho.Location = new System.Drawing.Point(12, 238);
            this.dgvBaoCaoTonKho.Name = "dgvBaoCaoTonKho";
            this.dgvBaoCaoTonKho.RowHeadersWidth = 51;
            this.dgvBaoCaoTonKho.RowTemplate.Height = 24;
            this.dgvBaoCaoTonKho.Size = new System.Drawing.Size(776, 205);
            this.dgvBaoCaoTonKho.TabIndex = 9;
            this.dgvBaoCaoTonKho.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBaoCaoTonKho_CellContentClick);
            // 
            // dtpTimeEnd
            // 
            this.dtpTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTimeEnd.Location = new System.Drawing.Point(60, 93);
            this.dtpTimeEnd.Name = "dtpTimeEnd";
            this.dtpTimeEnd.Size = new System.Drawing.Size(200, 22);
            this.dtpTimeEnd.TabIndex = 7;
            this.dtpTimeEnd.ValueChanged += new System.EventHandler(this.dtpTimeEnd_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thời gian:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnHuyBo);
            this.groupBox1.Controls.Add(this.txtTenNhanVien);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboMaNhanVien);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpNgayLap);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.dtpTimeEnd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpTimeStart);
            this.groupBox1.Controls.Add(this.btnXemBaoCao);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 224);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // frmBaoCaoTonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvBaoCaoTonKho);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmBaoCaoTonKho";
            this.Text = "Báo cáo tồn kho";
            this.Load += new System.EventHandler(this.frmBaoCaoTonKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCaoTonKho)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHuyBo;
        private System.Windows.Forms.TextBox txtTenNhanVien;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboMaNhanVien;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.DateTimePicker dtpTimeStart;
        private System.Windows.Forms.Button btnXemBaoCao;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvBaoCaoTonKho;
        private System.Windows.Forms.DateTimePicker dtpTimeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}