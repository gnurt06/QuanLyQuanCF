using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCF
{
    public partial class frmHoaDon : Form
    {
        KetNoi kn = new KetNoi();
        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadDSHoaDon();
        }

        public frmHoaDon()
        {
            InitializeComponent();
        }
        private void LoadDSHoaDon()
        {
            // Câu lệnh SQL nâng cao để lấy đủ thông tin
            string sql = @"SELECT h.MaHD, h.MaBan, h.GioVao, h.GioRa, n.HoTen AS NhanVienLap, h.TongTien 
                   FROM HoaDon h 
                   LEFT JOIN NhanVien n ON h.MaNV = n.MaNV 
                   ORDER BY h.GioVao DESC";

            DataTable dt = kn.LayDuLieu(sql);
            dgvChiTietHoaDon.DataSource = dt;

            // Định dạng cột
            if (dgvChiTietHoaDon.Columns.Count > 0)
            {
                dgvChiTietHoaDon.Columns["MaHD"].HeaderText = "Mã HD";
                dgvChiTietHoaDon.Columns["MaBan"].HeaderText = "Bàn";
                dgvChiTietHoaDon.Columns["GioVao"].HeaderText = "Giờ Vào";
                dgvChiTietHoaDon.Columns["GioRa"].HeaderText = "Giờ Ra";
                dgvChiTietHoaDon.Columns["NhanVienLap"].HeaderText = "Người Lập";
                dgvChiTietHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";

                // Fix lỗi bôi đen mới hiện chữ
                dgvChiTietHoaDon.DefaultCellStyle.ForeColor = Color.Black;
                dgvChiTietHoaDon.DefaultCellStyle.BackColor = Color.White;
                dgvChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiemHoaDon_Click(object sender, EventArgs e)
        {
            string tuNgay = dtpTuNgay.Value.ToString("yyyy-MM-dd");
            string denNgay = dtpDenNgay.Value.ToString("yyyy-MM-dd");
            string maHD = txtTimKiemTheoMaHoaDon.Text;

            string sql = $"SELECT * FROM HoaDon WHERE NgayLap BETWEEN '{tuNgay} 00:00:00' AND '{denNgay} 23:59:59'";
            if (!string.IsNullOrEmpty(maHD))
            {
                sql += $" AND MaHD LIKE '%{maHD}%'";
            }

            dgvChiTietHoaDon.DataSource = kn.LayDuLieu(sql);
        }

        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            string maHD = dgvChiTietHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();

            // Xóa chi tiết trước (Bắt buộc do ràng buộc khóa ngoại)
            kn.ThucThi($"DELETE FROM ChiTietHoaDon WHERE MaHD = '{maHD}'");

            // Xóa hóa đơn chính
            if (kn.ThucThi($"DELETE FROM HoaDon WHERE MaHD = '{maHD}'"))
            {
                MessageBox.Show("Đã xóa hóa đơn!");
                btnTimKiemHoaDon_Click(sender, e); // Gọi lại hàm tìm kiếm để refresh bảng
            }
            if (dgvChiTietHoaDon.CurrentRow != null) ;
        }

        private void dgvChiTietHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}
