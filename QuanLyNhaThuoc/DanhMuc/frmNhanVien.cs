using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class frmNhanVien : Form
    {
        Classes.DataProcesser dp = new Classes.DataProcesser();
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            DataTable dt = dp.GetDataTable("select * from tNhanVien");
            dgvNV.DataSource = dt;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
