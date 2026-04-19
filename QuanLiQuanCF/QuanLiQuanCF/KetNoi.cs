using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCF
{
    internal class KetNoi
    {
        private string strCon = @"Data Source=LAPTOP-S5R9KSSF;Initial Catalog=QuanLyQuanCF;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private SqlConnection con;

        public KetNoi()
        {
            con = new SqlConnection(strCon);
        }

        // 2. Hàm mở kết nối
        private void MoKetNoi()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        // 3. Hàm đóng kết nối
        private void DongKetNoi()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        // 4. Hàm lấy dữ liệu (Dùng cho DataGridView, ComboBox)
        public DataTable LayDuLieu(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message);
            }
            return dt;
        }

        // 5. Hàm thực thi lệnh (Dùng cho Thêm, Sửa, Xóa)
        public bool ThucThi(string sql)
        {
            try
            {
                MoKetNoi();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                DongKetNoi(); // Nhớ đóng kết nối sau khi thực thi
                return true;
            }
            catch (Exception ex)
            {
                // Dòng này cực kỳ quan trọng để biết tại sao không phản hồi
                MessageBox.Show("Lỗi SQL: " + ex.Message);
                return false;
            }
        }
    }
}

