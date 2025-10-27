using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyNhaThuoc.Classes
{

    internal class DataProcesser
    {
        string strConnect = @"Data Source=DESKTOP-SBAL8UV\SQLEXPRESS;Initial Catalog=QLNhaThuoc;Integrated Security=True";
        SqlConnection sqlConnect = null;

        // Mở kết nối
        public void OpenConnect()
        {
            if (sqlConnect == null)
                sqlConnect = new SqlConnection(strConnect);
            if (sqlConnect.State == ConnectionState.Closed)
                sqlConnect.Open();
        }

        // Đóng kết nối
        public void CloseConnect()
        {
            if (sqlConnect != null && sqlConnect.State == ConnectionState.Open)
                sqlConnect.Close();
        }

        // Thực thi câu lệnh SELECT
        public DataTable GetDataTable(string strSQL)
        {
            OpenConnect();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConnect);
            DataTable dt = new DataTable();
            sqlAdapter.Fill(dt);
            CloseConnect();
            return dt;
        }

        // Thực thi câu lệnh INSERT, UPDATE, DELETE
        public void ExecuteNonQuery(string strSQL)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand(strSQL, sqlConnect);
            cmd.ExecuteNonQuery();
            CloseConnect();
            cmd.Dispose();
        }

    }
}
