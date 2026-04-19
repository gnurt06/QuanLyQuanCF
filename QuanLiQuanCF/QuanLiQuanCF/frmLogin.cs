using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCF
{
    public partial class frmLogin : Form
    {
        KetNoi kn = new KetNoi();

        string strConn = @"Data Source=LAPTOP-S5R9KSSF;Initial Catalog=QuanLyQuanCF;Integrated Security=True;";

        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // 1. Khai báo chuỗi SQL (Lấy cả MaNV, HoTen và LoaiTK như yêu cầu của bạn)
            string sql = string.Format(
                "SELECT d.MaNV, n.HoTen, d.LoaiTK " +
                "FROM DangNhap d JOIN NhanVien n ON d.MaNV = n.MaNV " +
                "WHERE sTaiKhoan = '{0}' AND sMatKhau = '{1}'",
                txtTenTK.Text, txtMatKhau.Text
            );

            // 2. Thực hiện lấy dữ liệu
            DataTable dt = kn.LayDuLieu(sql);

            // 3. Kiểm tra xem có dòng nào khớp không
            if (dt != null && dt.Rows.Count > 0)
            {
                // Đăng nhập thành công -> Lưu thông tin vào Session
                Session.MaNV = dt.Rows[0]["MaNV"].ToString();
                Session.TenNV = dt.Rows[0]["HoTen"].ToString();
                Session.LoaiTK = dt.Rows[0]["LoaiTK"].ToString();

                MessageBox.Show("Chào mừng " + Session.TenNV + " (" + Session.LoaiTK + ") quay trở lại!");

                // Mở Form Main
                frmMain f = new frmMain();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK) Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
