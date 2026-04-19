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
    public partial class frmKhoHang : Form
    {
        // 1. Viết hàm định nghĩa LoadDataKho
        private void LoadDataKho()
        {
            try
            {
                // Lấy các thông tin từ bảng SanPham trong SQL
                string sql = "SELECT MaSP, TenSP, LoaiSP, MaNCC, Gia FROM SanPham";

                DataTable dt = kn.LayDuLieu(sql);

                // Gán dữ liệu vào DataGridView (Đảm bảo tên đúng là dgvKhoHang)
                dgvKhoHang.DataSource = dt;

                // Tùy chỉnh giao diện cột cho đẹp
                if (dgvKhoHang.Columns.Count > 0)
                {
                    dgvKhoHang.Columns["MaSP"].HeaderText = "Mã Sản Phẩm";
                    dgvKhoHang.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                    dgvKhoHang.Columns["LoaiSP"].HeaderText = "Loại";
                    dgvKhoHang.Columns["MaNCC"].HeaderText = "Mã NCC";
                    dgvKhoHang.Columns["Gia"].HeaderText = "Đơn Giá";

                    dgvKhoHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị kho hàng: " + ex.Message);
            }
        }

        // Bước 2: Gọi hàm LoadDataKho vào sự kiện Load của Form
        private void frmKhoHang_Load(object sender, EventArgs e)
        {
            LoadDataKho();
        }
        KetNoi kn = new KetNoi();

        public frmKhoHang()
        {
            InitializeComponent();
        }

        // Bước 3: Sau khi Thêm/Sửa/Xóa thành công, gọi lại LoadDataKho() để cập nhật bảng
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            string sql = string.Format(
                "INSERT INTO SanPham (MaSP, TenSP, LoaiSP, MaNCC, Gia) VALUES ('{0}', N'{1}', N'{2}', '{3}', {4})",
                txtMaSP.Text, txtTenSP.Text, txtLoaiSP.Text, txtNCC.Text, txtGia.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Thêm sản phẩm thành công!");
                LoadDataKho(); // <--- Cập nhật lại Grid ngay lập tức
            }
        }
    }
}
